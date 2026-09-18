## Context

This is the tail of the performance audit. `optimize-formatter-hot-paths` takes the items with measurements behind them; `fix-audit-correctness-bugs` takes the defects. What remains is a long list of individually-small allocation and repeated-work costs.

The organizing insight is that they are not a long list of distinct problems. Three antipatterns recur across dozens of sites, so the work is mostly *adding a handful of shared helpers and then applying them*, not editing forty files independently.

Baseline to draw against: 27.6 MB per `Default_CodeFormatter_Tests`, 47.5 MB per `Default_CodeFormatter_Complex`, ~2 Gen0 collections per format.

A caution that shapes everything below: this codebase has already had roughly thirty perf PRs. Many obvious-looking wins here are genuinely small, and the report they come from marks them as such. The value is in the aggregate and in the shared helpers, not in any single line.

## Goals / Non-Goals

**Goals:**
- Add the small number of helpers that let ~35 boxing sites and ~12 join sites be fixed mechanically.
- Remove per-node work that is computed and discarded.
- Stop the CLI and server repeating per-file and per-request work that can be hoisted or cached.
- Keep output byte-identical.

**Non-Goals:**
- Anything requiring a benchmark to justify. Three candidates are explicitly deferred: `Concat.Contents` from `IList<Doc>` to `Doc[]` (the build-time `ToArray()` copy is not free), reordering the doc-type dispatch chains or adding a `DocKind` tag, and replacing `Stack<PrintCommand>` to remove `DocFitter`'s O(k²) rescan (`k` is usually small; the quadratic term only bites on long single-line constructs).
- Refactors the audit recommends *against*: unifying `DocPrinter.ProcessNextCommand` with `DocFitter.Fits`, unifying the two vendored glob engines, and collapsing the tiny one-file-per-syntax-kind printers.
- Touching generated output directly. `SyntaxNodeComparer.generated.cs` (4,121 lines) and `SyntaxNodeJsonWriter.generated.cs` (9,435 lines) are emitted by `CSharpier.Generators`; fix the generator.

## Decisions

### Add helpers first, then apply them

The three recurring shapes and their fixes:

| Shape | Sites | Fix |
| --- | --- | --- |
| `.Any(predicate)` on a Roslyn struct list | ~16 | `AnyComment()` / `AnyDirective()` / `AnyCommentOrDirective()` on `SyntaxTriviaExtensions` |
| `Select(...)` then `Doc.Join` | ~12 | `Doc.Join(Doc, ReadOnlySpan<Doc>)` taking a static method group |
| `.Skip(n)` used as indexing | ~5 | indexer, or `GetRange(1, Count - 1)` |

Roslyn's own no-argument `Any()`, `Count`, `First()`, `Last()` and indexers do **not** allocate — only the predicate and projection overloads fall through to `System.Linq`. Any conversion must check which overload actually binds; converting a non-allocating call into a hand-rolled loop is churn with no benefit and makes the code worse.

*Why helpers rather than inlining loops at each site.* Inlining ~35 `foreach` loops trades one allocation each for a readability cost at every site. A named `trivia.AnyComment()` reads better than the lambda it replaces, and the `Doc.Join` overload keeps call sites at one line. This is the difference between the change improving the code and merely making it faster.

### Fix the generated comparer in the generator, with a hand-written helper

The generator currently string-concatenates `.OrderBy(o => o.Text).ToArray()` onto a property name, emitted at 30 sites. Replacing that with a call to a hand-written `CompareModifierLists(originalNode.Modifiers, formattedNode.Modifiers, …)` fixes three things at once:

- The boxing and the `OrderedEnumerable` disappear.
- The helper can fast-path `Count <= 1` and `!ReorderedModifiers`. `Modifiers.cs:117` only sets that flag when the printer *actually* changed the order, so for every already-formatted file both lists are known to be in the same order and the sort is pure waste.
- Passing `SyntaxTokenList` instead of `SyntaxToken[]` gives `CompareLists<T>` a JIT-specialized instantiation rather than degrading to `__Canon` interface dispatch.

It also removes a per-call delegate allocation as a side effect: the generated code passes the *instance* method group `CompareModifierToken`, and C# 11's method-group caching applies only to **static** method groups. The file already handles this correctly for `CompareFunc` — this one site just never got the same treatment.

`AllSeparatorsButLast` gets the same treatment: a `CompareSeparators` helper indexing `GetSeparator(i)` directly. Safe because the generator emits the node-list comparison immediately before, which already returns `NotEqual` on a count mismatch.

### Use the existing counter for group IDs, not `Guid`

Five sites call `Guid.NewGuid()` — a CSPRNG call plus a 36-char string — to mint a doc group ID. `RightHandSide.cs:20` is the one that matters: `Layout.Fluid` is the *default* arm of `DetermineLayout`, so it fires on ordinary assignments, not an edge case. `SwitchExpression` burns two per arm.

`Doc.GroupWithNewId` already implements the cheap version with `Interlocked.Increment` and has **zero callers**. Expose the counter as `Doc.NextGroupId()` — needed because two sites want the ID before the group's contents exist, so `GroupWithNewId` cannot be used directly — and use it everywhere.

*Alternative considered.* Moving `GroupId` from `string` to `int` throughout would remove string hashing from `GroupModeMap` entirely, in both the print and fit loops. Deferred: it touches `DocSerializer` and its tests, and GUID/`"LambdaArguments #1"` IDs are self-describing when reading a serialized doc tree or sitting in a debugger. The counter captures most of the win at none of that cost.

Uniqueness only has to hold within a single print, so a monotonic counter is sufficient.

### Hoist and lazify rather than restructure

Several items are one-line moves that the audit found by asking "is this value used on every path?":

- `SeparatedSyntaxList` computes `list[x].GetTrailingTrivia()` — an O(depth) walk down the rightmost spine — for every element, but reads it only inside `x >= list.SeparatorCount` guards. Move it into the last-element branch.
- Three printers allocate a `StringBuilder` unconditionally that is written only inside a `csharpier-ignore` region. `StringBuilder? x = null` with `??=` at the append sites.
- `Section` eagerly builds `noDirectoryMatcher`, reachable only via `--config-path <dir>/.editorconfig`. Make it lazy.
- `Element.cs` allocates `attrGroupId` per element, consumed only by branches guarded on a `shouldHugContent` that is never true. Delete the dead branches and the ID with them.

### CLI: normalize once, cache the parse, order the cheap check first

Three independent problems, each a hoist rather than a redesign:

- `IgnoreRule.IsMatch` re-runs `NormalisePath()` per rule, on a path `IgnoreList.IsIgnored` has already normalized, and the cheap directory-only early-out sits *after* that work. Normalize at the boundary, move the early-out first, and hoist the per-rule constants (`patternBeforeFirstWildcard`, `Pattern.Contains('/')`) into the constructor.
- `.editorconfig` results are memoized per directory, but individual file *parses* are not — so each nested config re-parses every ancestor, O(k²) in nesting depth. Memoize the parse keyed by full path.
- Server mode builds `OptionsProvider` as a local, so all four of its caches start empty per request and ~300 ignore regexes are recompiled via `Reflection.Emit`. `SharedFunc` does not help — it removes its key in a `finally`, deduping only concurrent callers.

*On the server cache.* Blind caching is wrong: config files change while the server lives. Key the parsed artifacts on `(path, lastWriteTimeUtc, length)` so an edit is picked up on the next request. Even a stamp-check-per-request avoids all the regex compilation.

*On reordering the ignore check.* Resolving printer options before the ignore check means unsupported files stop paying the O(300 rules) test. This is the one item that could change observable behavior: today a file that is both ignored and unsupported is silently skipped, and a naive swap would start warning about it. The reorder must be guarded on `!warnForUnsupported`, or re-check ignored-ness before warning.

### Consolidate only where the duplicate is genuinely identical

The audit distinguished real duplication from things that merely look alike. Only the verified-identical cases are in scope: the 4-site optional-initializer tail, the 3-site self-nesting `OptionalBraces` switch, the `TrimEnd` copy in `DocPrinter`, and the two hand-inlined copies of `ExplicitInterfaceSpecifier.Print`.

*One trap that constrains any list-printing helper.* `Token.Print` is stateful — it reads and clears `context.State.TrailingComma` and reacts to `SkipNextLeadingTrivia`. A helper must take the `SyntaxToken`s and print them in the same left-to-right order. A helper taking *pre-printed* `Doc` parameters invites a call-site reordering that silently breaks byte-exactness in files with a trailing comment before a closing delimiter. This is why the delimited-list consolidation the audit floated is **not** in scope.

## Risks / Trade-offs

- **Volume.** This is the largest of the three changes by site count, and every site is a chance to introduce a subtle output difference. Mitigation: land it in the task-group order below, running the full suite between groups, so a regression is bisectable to a group rather than to the whole change.
- **Converting a non-allocating call by mistake** → Roslyn's no-arg `Any()` and `First()` are already free. Every conversion must confirm which overload binds; when in doubt, leave it. A wrong conversion costs readability and gains nothing.
- **`MembersWithForcedLines` has a dead `else if` label** → `EndRegionDirectiveTrivia` appears in *both* the `printExtraNewLines` and `triviaContainsEndIfOrRegion` lists, and because they are `else if`-chained the second is unreachable. Rewriting the chain as a `switch` is a compile error, and "fixing" the precedence changes behavior. Preserve the current precedence and treat the dead label as a separate question.
- **Reordering the CLI ignore check can surface new warnings** → Guarded explicitly, and called out as its own task with its own test.
- **Server caching can serve stale config** → Stamp-keyed, not blind. Needs a test that editing a config file between requests is picked up.
- **Aggregate benefit may disappoint** → Stated plainly: many of these are small, and the codebase is already well optimized. If the post-change benchmark shows little movement, that is information, not failure — the shared helpers and the removed duplication still stand on their own.

## Migration Plan

No migration; all internal. Land in dependency order so the mechanical work has its helpers available:

1. Shared helpers (`SyntaxTriviaExtensions`, `Doc.Join` overload, `StringBuilder.TrimEnd`, `Doc.NextGroupId`).
2. Apply the helpers across their call sites.
3. Generator changes (comparer modifier lists and separators).
4. Printer hoists and lazy allocations.
5. Doc-printer items (`DocFitter` builder, indent cache).
6. XML parser scans.
7. CLI and server.
8. Duplication extraction, dead-code removal, and the file rename.

## Open Questions

- Is the `Doc.Join(Doc, ReadOnlySpan<Doc>)` overload the right shape, or should it take the Roslyn list plus a print delegate directly (`Doc.Join<T>(Doc, SyntaxList<T>, Func<T, CSharpPrintingContext, Doc>, CSharpPrintingContext)`)? The latter removes the closure at more sites but adds a generic instantiation per node type. Decide when writing the helper, with the call sites in front of you.
- Should `Doc.GroupWithNewId` be deleted or kept once `NextGroupId` exists? It has no callers today but is the more ergonomic API where the contents *are* available up front.
- `RawNodeReader`'s per-character `StringBuilder` loops could collapse into one shared "read until terminator, normalizing newlines" helper. Worth doing as part of item 6, or does it deserve its own pass alongside the parser's other rough edges?
