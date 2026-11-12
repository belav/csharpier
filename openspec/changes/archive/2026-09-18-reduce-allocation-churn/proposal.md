## Why

A performance audit found a long tail of allocation and repeated-work overhead across the syntax printers, the generated tree comparer, the XML parser and the CLI. Individually most items are small. They are grouped here because the same few antipatterns recur across dozens of sites, so a handful of shared helpers fixes them in bulk rather than one file at a time.

Three recurring shapes account for most of it:

- **LINQ over Roslyn's struct lists** (~35 sites). `SyntaxTriviaList`, `SyntaxTokenList`, `SyntaxList<T>` and `SeparatedSyntaxList<T>` provide non-allocating instance methods only for the *no-argument* forms. Every `.Any(predicate)`, `.Select(...)`, `.Skip(n)` and `.All(...)` falls through to `System.Linq`, boxing the struct and allocating an iterator. The repo already established the fix — `ListExtensions` has hand-rolled `Any` overloads for two of the four types — it was simply never extended.
- **Per-node work that is thrown away.** `SeparatedSyntaxList` walks `GetTrailingTrivia()` for every element but reads it only for the last. `MembersWithForcedLines` builds an immutable hash set per member to test seven flags. The generated comparer LINQ-sorts modifier lists on every node, twice, even when nothing was reordered.
- **Config and ignore work repeated per file.** The ignore matcher re-normalizes and re-splits the path once per rule per file (~300 rules × 50k files); `.editorconfig` ancestors are re-parsed once per nested config; and server mode rebuilds the entire options world — recompiling ~300 regexes via `Reflection.Emit` — on every format request.

The measured baseline is 27.6 MB allocated for `Default_CodeFormatter_Tests` and 47.5 MB for `Default_CodeFormatter_Complex`, at ~2 Gen0 collections per format.

## What Changes

**Shared helpers that fix many sites at once**
- Add `AnyComment()` / `AnyDirective()` / `AnyCommentOrDirective()` to `SyntaxTriviaExtensions`, and a `Doc.Join(Doc separator, ReadOnlySpan<Doc>)` overload taking a static method group, replacing the `Select(...)` + `Doc.Join` shape at its call sites.
- Replace `.Skip(n)` used purely as indexing with an indexer or `GetRange`.

**Generated comparer** (one generator change, ~30 and ~32 call sites respectively)
- Route modifier-list comparison through a hand-written helper that fast-paths `Count <= 1` and the not-reordered case, instead of emitting `OrderBy(o => o.Text).ToArray()` per node per side.
- Replace `AllSeparatorsButLast`'s per-node array with direct `GetSeparator(i)` indexing.

**Printers**
- Stop minting group IDs with `Guid.NewGuid()` at 5 sites; use the existing `Interlocked` counter, which `Doc.GroupWithNewId` already implements and which currently has zero callers.
- Make the `csharpier-ignore` scratch `StringBuilder` lazy in three printers where it is allocated unconditionally but written only inside an ignore region.
- Hoist `GetTrailingTrivia()` into the last-element branch of `SeparatedSyntaxList`.
- Replace the per-member `ToImmutableHashSet()` in `MembersWithForcedLines` with a single pass setting named flags.
- Add early returns and remove closures in `Modifiers`.

**Doc printer**
- Drop the `StringBuilder` in `DocFitter` that every measured string is copied into and whose text is never read — only its length and trailing-whitespace count are used.
- Key the indent cache off the `Indent` instance rather than its whitespace string, whose hash cost grows with nesting depth.

**XML parser**
- Replace the `Substring`-per-character terminator scans in `RawNodeReader` with `IndexOf` plus a single bounded `Append`.

**CLI**
- Normalize the ignore path once per file instead of once per rule, hoist per-rule constants into the constructor, and replace `path.Split('/').Any(lambda)` with a span-based segment loop.
- Memoize individual `.editorconfig` parses; make `Section.noDirectoryMatcher` lazy.
- Cache `OptionsProvider` per directory in server mode, keyed on file stamps so config edits are still picked up.
- Resolve printer options before the ignore check, so unsupported files stop paying the most expensive per-file test.

**Duplication**
- Extract `InitializerExpression.PrintOptionalWithLine` (4 byte-identical sites), `OptionalBraces.PrintWithSelfNesting` (3 identical sites), a `StringBuilder.TrimEnd` overload (removing a character-for-character copy in `DocPrinter`), and route two hand-inlined copies through the existing `ExplicitInterfaceSpecifier.Print`.
- Rename `Cli/EditorConfig/CSharpierConfigParser.cs` to `EditorConfigFileParser.cs` — an accidental `R100` rename in commit `93edc84a` left the file name disagreeing with the type it declares.
- Delete dead code: `Doc.GroupWithNewId` is superseded rather than deleted; `EqualsIgnoreCase`, `StringExtensions.IndexOf(char)` and the `shouldHugContent` branches in `Xml/Element.cs` have no callers or are unreachable.

Output must remain byte-identical throughout.

## Capabilities

### New Capabilities
<!-- None. Every item here is an internal optimization or refactor with no observable behavior change. The performance invariants worth asserting are established by `optimize-formatter-hot-paths` in the `formatter-performance-invariants` capability; items in this change that warrant assertions extend that spec rather than introducing a new one. -->

### Modified Capabilities
- `cli-formatting-pipeline`: Adds a requirement that the per-file filters may be evaluated in any order chosen for performance, but that the outcome reported for a file must not depend on that order. This does not change behavior — it pins down behavior that is currently implicit, so that resolving printer options before the ignore check cannot start emitting "unsupported file type" warnings for files that are both ignored and unsupported.

## Impact

**Core — shared helpers**
- `CSharp/SyntaxTriviaExtensions.cs`, `Utilities/ListExtensions.cs`, `DocTypes/Doc.cs`, `Utilities/StringBuilderExtensions.cs`

**Core — generated comparer**
- `CSharpier.Generators/SyntaxNodeComparerGenerator.cs:245`, `:247`, `:273`
- `CSharp/SyntaxNodeComparer.cs:216-239`, `:528-529`

**Core — printers**
- `SyntaxPrinter/` — `Modifiers.cs`, `SeparatedSyntaxList.cs`, `MembersWithForcedLines.cs`, `RightHandSide.cs`, `AttributeLists.cs`, `ConstraintClauses.cs`, `CSharpierIgnore.cs`, `ArgumentListLikeSyntax.cs`
- `SyntaxNodePrinters/` — `SwitchExpression.cs`, `SwitchStatement.cs`, `TypeParameter.cs`, `InvocationExpression.cs`, `BinaryExpression.cs`, `BasePropertyDeclaration.cs`, `ArrayType.cs`, `ArrayRankSpecifier.cs`, `InterpolatedStringExpression.cs`, `RecursivePattern.cs`, `ThrowStatement.cs`, `YieldStatement.cs`, `GotoStatement.cs`, and the four creation-expression printers

**Core — doc printer and XML**
- `DocPrinter/DocFitter.cs`, `DocPrinter/DocPrinter.cs`, `DocPrinter/Indent.cs`, `Utilities/StackExtensions.cs`
- `Xml/RawNodeReader.cs`, `Xml/XNodePrinters/ElementChildren.cs`, `Xml/XNodePrinters/Element.cs`, `Xml/XNodePrinters/Node.cs`

**CLI**
- `DotIgnore/IgnoreRule.cs`, `DotIgnore/StringExtensions.cs`, `DotIgnore/IgnoreList.cs`
- `EditorConfig/EditorConfigLocator.cs`, `EditorConfig/Section.cs`, `EditorConfig/CSharpierConfigParser.cs` (rename)
- `Server/CSharpierServiceImplementation.cs`, `Server/ServerFormatter.cs`, `FormattingEngine.cs:128-147`

**Not in scope.** Three items the audit identified as plausible but not obviously net-positive, each needing its own benchmark run before anyone commits: changing `Concat.Contents` from `IList<Doc>` to `Doc[]`, reordering the doc-type dispatch chains or adding a `DocKind` tag, and replacing the `Stack<PrintCommand>` to remove `DocFitter`'s O(k²) rescan. Also out of scope: unifying `DocPrinter.ProcessNextCommand` with `DocFitter.Fits` (only 4 of 13 arms genuinely match), unifying the two vendored glob engines, and collapsing the one-file-per-syntax-kind printers.
