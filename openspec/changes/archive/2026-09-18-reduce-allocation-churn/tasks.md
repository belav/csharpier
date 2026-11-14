## 1. Shared helpers

- [x] 1.1 Add `AnyComment()`, `AnyDirective()` and `AnyCommentOrDirective()` to `CSharp/SyntaxTriviaExtensions.cs`, each a `foreach` over the struct enumerator taking `in SyntaxTriviaList`.
- [x] 1.2 Add a `Doc.Join(Doc separator, ReadOnlySpan<Doc>)` overload backed by `DocListBuilder`. Decide the shape with the call sites in view — a `Doc.Join<T>(Doc, SyntaxList<T>, Func<T, CSharpPrintingContext, Doc>, CSharpPrintingContext)` variant removes the closure at more sites but adds a generic instantiation per node type.
- [x] 1.3 Add `TrimEnd(this StringBuilder, params ReadOnlySpan<char>)` to `Utilities/StringBuilderExtensions.cs`, mirroring the existing `TrimStart`.
- [x] 1.4 Expose the existing `Interlocked` counter as `Doc.NextGroupId()` in `DocTypes/Doc.cs`. Two call sites need the ID before the group's contents exist, so `GroupWithNewId` cannot be reused directly.

## 2. Apply the helpers

- [x] 2.1 Convert the ~16 `.Any(predicate)` sites on Roslyn struct lists to the new trivia helpers. **Before each conversion, confirm the call actually binds to `System.Linq`** — Roslyn's no-arg `Any()`, `Count`, `First()`, `Last()` and indexers are already non-allocating, and converting one of those is churn with no gain. Sites include `Token.cs:446`, `:473`, `SeparatedSyntaxList.cs:122`, `:124`, `Parameter.cs:22`, `RecursivePattern.cs:75`, `SwitchExpression.cs:43`, `CollectionExpression.cs:56`, `BaseTypeDeclaration.cs:168`, `ArrayRankSpecifier.cs:10`, `BasePropertyDeclaration.cs:78`.
- [x] 2.2 Convert the ~12 `Select(...)` + `Doc.Join` sites to the new overload with a static method group: `Modifiers.cs:54`, `:63`, `AttributeLists.cs:31`, `ConstraintClauses.cs:19`, `NamespaceLikePrinter.cs:44`, `SwitchStatement.cs:19`, `SwitchSection.cs:12`, `TryStatement.cs:17`, `QueryBody.cs:12`.
- [x] 2.3 Replace the ~5 `.Skip(n)` used purely as indexing: `InvocationExpression.cs:481` (`groups[1].Skip(1).First()` → `groups[1][1]`), `:103`, `:107`, `BinaryExpression.cs:47` and `:125` (both → `GetRange(1, Count - 1)`).
- [x] 2.4 Replace the 5 `Guid.NewGuid()` group-ID sites with `Doc.NextGroupId()`: `RightHandSide.cs:20`, `SwitchExpression.cs:47`, `:63`, `SwitchStatement.cs:28`, `TypeParameter.cs:12`.
- [x] 2.5 Replace the hand-inlined trailing-trim loop in `DocPrinter.EnsureOutputEndsWithSingleNewLine` with the new `TrimEnd`.
- [x] 2.6 Run the full `dotnet test` suite and confirm output is byte-identical before continuing.

## 3. Generated comparer

- [x] 3.1 Add a hand-written `CompareModifierLists(in SyntaxTokenList original, in SyntaxTokenList formatted, TextSpan, TextSpan)` to `CSharp/SyntaxNodeComparer.cs`. Fast-path `Count <= 1` and `!this.ReorderedModifiers` to a positional compare over the structs; sort into a stack buffer only when reordering actually occurred (modifier lists are ≤6, so an insertion sort is both faster and allocation-free).
- [x] 3.2 Change `CSharpier.Generators/SyntaxNodeComparerGenerator.cs:245-249` to emit a call to that helper instead of appending `.OrderBy(o => o.Text).ToArray()` to the property name. This removes the per-call delegate allocation from passing the *instance* method group `CompareModifierToken` as a side effect.
- [x] 3.3 Add a `CompareSeparators<T>(in SeparatedSyntaxList<T>, in SeparatedSyntaxList<T>)` helper indexing `GetSeparator(i)` for `i` in `[0, Count-2]`, and change the generator at `:273` to emit it. Delete `AllSeparatorsButLast`.
- [x] 3.4 Replace the boxing `OrderBy` in `CompareUsingDirectives` (`SyntaxNodeComparer.cs:528-529`) with a `string[] keys` / `UsingDirectiveSyntax[] nodes` pair sorted via `Array.Sort` with `StringComparer.Ordinal`.
- [x] 3.5 Rebuild and confirm the generated output compiles; run the comparer tests and the `Default_SyntaxNodeComparer` benchmark against its 598 µs / 191 KB baseline.

## 4. Printer hoists and lazy allocation

- [x] 4.1 Hoist `list[x].GetTrailingTrivia()` in `SeparatedSyntaxList.cs:83-85` into the `x >= list.SeparatorCount` branch — it is an O(depth) spine walk computed per element and read only for the last. While there, stop re-indexing `list[x]` at `:83` and `:101` when `member` already holds it.
- [x] 4.2 Make the `csharpier-ignore` scratch `StringBuilder` lazy (`StringBuilder? x = null` with `??=`) at `SeparatedSyntaxList.cs:50`, `MembersWithForcedLines.cs:29` and `CSharpierIgnore.cs:89`.
- [x] 4.3 Replace the per-member `ToImmutableHashSet()` in `MembersWithForcedLines.cs:145-148` with a single pass setting named local flags. **Preserve the existing precedence**: `EndRegionDirectiveTrivia` appears in both the `printExtraNewLines` and `triviaContainsEndIfOrRegion` lists and the second is unreachable because they are `else if`-chained — rewriting as a `switch` is a compile error and "fixing" it changes behavior. Also hoist the up-to-four `member.GetLeadingTrivia()` calls into one local.
- [x] 4.4 In `Modifiers.cs`, move the `Count == 0` guard above the lambda construction in both `PrintSorted` and `PrintSorterWithoutLeadingTrivia`; replace `modifiers.Skip(1).Any(...)` at `:105` with a `for` loop; skip `ToArray()` and `Array.Sort` when the keys are already in order; and switch the `DefaultOrder` comparer from `Array.IndexOf` over `token.Text` to a `switch` on `RawSyntaxKind()`.
- [x] 4.5 Rename the discarded `_` parameter to the supplied context in `RecursivePattern.cs:49`, `:85` and `OrderByClause.cs:15`, so the lambdas stop capturing the enclosing `context` and become cacheable static delegates.
- [x] 4.6 Apply the small per-node items: `Doc.BreakParent` as a `static readonly` singleton (`Doc.cs:22`); `PrintedNode` as a `readonly record struct` (`InvocationExpression.cs:10`); `Doc.Null` instead of `string.Empty` in `ThrowStatement.cs:13`, `YieldStatement.cs:13`, `GotoStatement.cs:11`; right-sized `Doc[]` in `ArrayType.cs:10` and `ImplicitArrayCreationExpression.cs:13`; `ancestor.Span.Contains(node.Span)` or `descendIntoChildren:` in `BinaryExpression.cs:238`.
- [x] 4.7 Run the full `dotnet test` suite.

## 5. Doc printer

- [x] 5.1 Replace `DocFitter`'s `StringBuilder output` with two ints — `outputLength` and `trailingWhitespace` — since only `Length > 0` and the trailing-whitespace count are ever read. Delete the `DocFitterOutput` field at `DocPrinter.cs:23`. Add a comment noting `Fits` never reads the text back.
- [x] 5.2 Replace `Indenter`'s `Dictionary<string, Indent>` with an `Indent.Increased` link populated via `??=`, so lookup stops hashing a whitespace string whose length grows with nesting depth.
- [x] 5.3 Make `PropagateBreaks` push its exit marker only for `ForceFlat` and `Group`, popping other nodes immediately — `OnExit` does nothing for the rest. Add a comment explaining the asymmetry.
- [x] 5.4 Reorder the type-test chains in `DocPrinter.ProcessNextCommand`, `DocFitter.Fits` and `PropagateBreaks.OnEnter` most-common-first (`StringDoc`, `Concat`, `LineDoc`, `Group`, `IndentDoc`, then the rest). Preserve the derived-before-base constraints: `HardLine` before `LineDoc`, `ConditionalGroup` before `Group`. Do **not** add a `DocKind` tag — that needs its own benchmark. Implemented, with one deviation: `DocFitter`'s `case NullDoc:` stays at the front of the switch rather than moving to "the rest". `Doc.Null` is emitted liberally by the printers, and `DocPrinter.ProcessNextCommand` already tests `doc == Doc.Null` ahead of everything else, so demoting it in the fit loop is the one part of the prescribed order that is wrong on its face. See the measurement note below — no timing gain could be demonstrated.
- [x] 5.5 Run the full `dotnet test` suite.

## 6. XML parser

- [x] 6.1 Replace the `Substring`-per-character terminator scans in `RawNodeReader.cs:198`, `:239`, `:276` with `IndexOf(terminator, position, StringComparison.Ordinal)` plus a single `Append(string, int, int)`. Both APIs exist on `netstandard2.0`; avoid `Append(ReadOnlySpan<char>)`, which is netstandard2.1+.
- [x] 6.2 Replace the three `Substring(position, n) == "<!--"`-style prefix tests at `:157`, `:164`, `:171` with `string.CompareOrdinal(...)` or an `AsSpan().StartsWith(...)`.
- [x] 6.3 Have `ReadName` (`:485`) return a `Substring` directly rather than building it one character at a time.
- [x] 6.4 Skip the whole-document `NewlineRegex.Replace` at `:58` when the input contains no `\r`. Drop the meaningless `RegexOptions.Compiled` from the `[GeneratedRegex]` at `:29`.
- [x] 6.5 Gate the three `csharpier-ignore` regexes at `:223-225` behind a cheap `StartsWith(" csharpier-", StringComparison.Ordinal)` prefix test.
- [x] 6.6 Replace the four `Doc` lists per child in `ElementChildren.cs:47-50` with `Doc?` locals, and allocate group IDs only for children that reach the `Doc.Group` at `:99`.
- [x] 6.7 Replace the `List<Doc>` + `All` + `Select` + `string.Join` in `Node.cs:50` with three locals and `string.Concat`.
- [x] 6.8 Run the XML formatting tests and the `CustomParser_Parse` benchmark against its 6,874 µs / 11.8 MB baseline.

## 7. CLI and server

- [x] 7.1 Normalize the ignore path once in `IgnoreList.IsIgnored` and pass it down; delete the `NormalisePath()` call from `IgnoreRule.IsMatch:81`. Move the `PatternFlags.DIRECTORY` early-out to the first line of `IsMatch`.
- [x] 7.2 Hoist `patternBeforeFirstWildcard` and `Pattern.Contains('/')` into the `IgnoreRule` constructor next to the existing `wildcardIndex`.
- [x] 7.3 Replace `path.Split('/').Any(lambda)` at `IgnoreRule.cs:110-117` with a span-based segment loop using `Regex.IsMatch(ReadOnlySpan<char>)`.
- [x] 7.4 Memoize individual `.editorconfig` parses in a `ConcurrentDictionary<string, EditorConfigFile>` keyed by full path, so nested configs stop re-parsing every ancestor.
- [x] 7.5 Make `Section.noDirectoryMatcher` lazy — it is reachable only via `--config-path <dir>/.editorconfig`.
- [x] 7.6 Cache `OptionsProvider` per directory in `Server/CSharpierServiceImplementation.cs`, keyed on `(path, lastWriteTimeUtc, length)` of the underlying config files so edits are still picked up. Add a test that editing a config between requests takes effect.
- [x] 7.7 Stream the server's JSON instead of round-tripping through intermediate strings and byte arrays (`ServerFormatter.cs:74-92`), and replace the fire-and-forget `Task.Run` at `:60` with a direct call wrapped in try/catch that always closes the response stream.
- [x] 7.8 In `FormattingEngine.cs:128-147`, resolve printer options and return early on `Formatter.Unknown` before the ignore check. **Guard on `!warnForUnsupported`** — a naive swap starts emitting "unsupported file type" for files that are both ignored and unsupported. Add a test covering that case.
- [x] 7.9 Use the structured-logging overload for the per-file `LogDebug` at `FormattingEngine.cs:159`, and make `ConsoleLogger.IsEnabled` return `logLevel >= loggingLevel` instead of always `true`.

## 8. Duplication and dead code

- [x] 8.1 Extract `InitializerExpression.PrintOptionalWithLine(InitializerExpressionSyntax?, CSharpPrintingContext)` and call it from the four byte-identical sites: `ArrayCreationExpression.cs:13`, `StackAllocArrayCreationExpression.cs:16`, `ObjectCreationExpression.cs:25`, `ImplicitObjectCreationExpression.cs:18`.
- [x] 8.2 Extract `OptionalBraces.PrintWithSelfNesting<TSelf>` and call it from `WhileStatement.cs:22`, `ForStatement.cs:46`, `CommonForEachStatement.cs:46`. Do **not** fold in the four hand-rolled bypasses (`UsingStatement`, `LockStatement`, `FixedStatement`, `LabeledStatement`) — two of them omit `RemoveInitialDoubleHardLine`, which is a behavior question to settle separately.
- [x] 8.3 Route `BaseMethodDeclaration.cs:156-162` and `BasePropertyDeclaration.cs:53-58` through the existing `ExplicitInterfaceSpecifier.Print`. Use that printer, not `Node.Print` — the latter adds an ignore check and depth guard.
- [x] 8.4 Share the duplicated `lineSeparators` array and the raw-string argument-indent predicate between `Token.cs:43`, `:116-126` and `InterpolatedStringExpression.cs:9`, `:108-116`. Leave the two differing "is the end delimiter indented?" computations alone until it is confirmed they agree.
- [x] 8.5 Rename `Cli/EditorConfig/CSharpierConfigParser.cs` to `EditorConfigFileParser.cs` to match the type it declares — commit `93edc84a` renamed the file without renaming the class.
- [x] 8.6 Delete dead code: `EqualsIgnoreCase` and `StringExtensions.IndexOf(char)` in `Utilities/StringExtensions.cs` (no callers; the latter is unreachable because the instance method always wins), and the `shouldHugContent` local plus its three unreachable branches in `Xml/XNodePrinters/Element.cs:17`. Check whether `attrGroupId` is still referenced after the branches go — if not, drop it and the per-element `GroupFor` call with it.
- [x] 8.7 Decide whether `Doc.GroupWithNewId` is deleted or kept now that `Doc.NextGroupId()` exists. **Deleted** — it had no callers, and every site that needs an id needs it before the contents exist, which is what `NextGroupId()` plus `GroupWithId(..)` covers.

## 9. Verify and measure

- [x] 9.1 Run the full `dotnet test` suite and confirm every expected-output comparison passes unchanged. 784 passed, 0 failed, 5 skipped. Two tests were added (7.6 and 7.8). No expected-output file changed.
- [x] 9.2 Run `dotnet run -c Release --project Src/CSharpier.Benchmarks` and compare against the baseline: `Default_CodeFormatter_Tests` 59.4 ms / 27.6 MB, `Default_CodeFormatter_Complex` 128.5 ms / 47.5 MB, `Default_SyntaxNodeComparer` 598 µs / 191 KB, `CustomParser_Parse` 6,874 µs / 11.8 MB.
- [x] 9.3 Record the measured before/after per task group, so the groups that did not pay off are visible rather than assumed.
- [x] 9.4 If aggregate movement is small, say so in the change notes. Many of these items are individually minor and this codebase is already well optimized — a flat result is information, not a failure, and the shared helpers and removed duplication stand on their own.

### Measurements

Benchmarks were re-measured rather than compared against the numbers quoted above, because BenchmarkDotNet cannot use its normal toolchain on this machine's .NET 11 preview SDK (`GetRuntimeVersion not implemented for NotRecognized`). Everything below ran with `--inProcess`, one benchmark per process, with the before column taken from a `git archive HEAD` copy of the tree built and run the same way in the same session. In-process timings are not comparable to the quoted baselines; allocation numbers are.

| Benchmark | Before | After | Time | Allocated |
| --- | --- | --- | --- | --- |
| `Default_CodeFormatter_Tests` | 65.17 ms / 27.55 MB | 61.10 ms / 26.83 MB | −6.2% | −2.6% |
| `Default_CodeFormatter_Complex` | 136.4 ms / 47.28 MB | 129.8 ms / 42.23 MB | −4.8% | −10.7% |
| `Default_SyntaxNodeComparer` | 654.2 µs / 190.48 KB | 660.2 µs / 189.87 KB | +0.9% | −0.3% |
| `CustomParser_Parse` | 10.02 ms / 11.83 MB | 8.21 ms / 9.54 MB | −18.1% | −19.4% |

Per group:

- **Groups 1–2 (shared helpers and their call sites), 4 (printer hoists), 5 (doc printer)** — these all land on the two `CodeFormatter` benchmarks and were measured together. Net −6% time on the test corpus and −11% allocation on the complex corpus. `DocFitter` dropping its `StringBuilder` (5.1) is the single largest item in this group.
- **Group 3 (generated comparer)** — flat. `Default_SyntaxNodeComparer` moved 190.48 KB → 189.87 KB, which is inside the run to run noise. The `OrderBy(..).ToArray()` per node per side and the `AllSeparatorsButLast` arrays are gone, but the comparer's cost is dominated by walking the two trees, not by those allocations. The change is still worth keeping for the delegate allocation it removes and for `CompareLists` getting a JIT specialized instantiation, but it does not show up as a number.
- **Group 6 (XML parser)** — the clear winner: −18% time and −19% allocation on `CustomParser_Parse`, almost all of it from replacing the `Substring`-per-character terminator scans (6.1).
- **Group 7 (CLI and server)** — not covered by any benchmark in `CSharpier.Benchmarks`. `CliBenchmarks` exercises the CLI but needs a checkout to format and was not run. The wins here are structural (ignore path normalised once per file instead of once per rule per file, `.editorconfig` parses memoized, `OptionsProvider` reused across server requests) and would show up as per-file or per-request cost rather than in these benchmarks.
- **Group 8 (duplication and dead code)** — no performance intent, no measurable effect.

#### Task 5.4, measured separately

5.4 was implemented after the table above was recorded, so it is not reflected in those numbers. It was measured on its own, A/B against a copy of the tree without it, built and run the same way.

**Allocation: unchanged, and unchangeable.** Reordering type tests allocates nothing. Measured allocation matched on both sides to within the GC timing jitter — 41.9–42.2 MB on `Default_CodeFormatter_Complex` and 26.8–27.5 MB on `Default_CodeFormatter_Tests` for both variants.

**Time: no gain could be demonstrated, because the effect is below this machine's noise floor.** Four consecutive runs of the *same* binary on `Default_CodeFormatter_Tests` gave 62.24, 89.20, 64.52 and 81.10 ms — a 43% spread, bimodal between roughly 62 ms and 82 ms on a timescale shorter than a single benchmark run. Against that, a dispatch reorder worth low single digit percent is unmeasurable. Order balanced A/B/B/A quadruples did not help; which mode a run landed in dominated the result.

What the runs did show, before the machine became that noisy, is that the *order within* the reorder matters more than the reorder itself. The prescribed order demotes `DocFitter`'s `case NullDoc:` from first to seventh, and that variant measured consistently slightly slower than no change at all (+1.8%, +1.0%, +0.3% across three paired runs). Keeping `NullDoc` first reversed the sign (−2.2%, −2.8% on the two paired runs not contaminated by a mode switch). Neither result survives the later noise, but the asymmetry is consistent with `Doc.Null` being one of the most frequently visited doc types, so the shipped version keeps it first.

The honest summary is that this item is being kept on the strength of the code reading more sensibly, not on a measured gain. `proposal.md` and `design.md` deferred it as "needing its own benchmark run before anyone commits"; the benchmark was run and it does not settle the question on this hardware. It should be re-measured on a quiet machine before anyone claims a number for it.

#### An earlier measurement changed the implementation

One measurement changed the implementation. The first version of the `Doc.Join` overloads used `DocListBuilder`, which rents from `ArrayPool<Doc>.Shared` and is never disposed anywhere in this codebase, and then copies into a second exact sized array via `Doc.Concat(ref ..)`. That is two allocations where the old `List<Doc>` version had one, and it showed up as `Default_CodeFormatter_Tests` regressing to 71.13 ms with Gen0/Gen1/Gen2 collections at 1666/1333/333 against a baseline of 1000/500/0. The joins know their exact length up front, so they now allocate one right sized `Doc[]` directly. That took the same benchmark to 61.10 ms with the collection counts back at baseline.
