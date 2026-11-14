## Why

A performance audit of the formatting engine found that **two existing optimizations silently do not work**, and that several hot paths recompute values that are already known.

- `DocListBuilder` rents a pooled array in its constructor but only 3 of 24 construction sites ever return it. Because nothing refills the pool, every `Rent` allocates fresh — rounded up to the pool's 16-element minimum bucket. Measured at **152 bytes per construction**, which makes the pooled builder *worse* than both a plain `Doc[n]` and the `List<Doc>` it was introduced to replace.
- `SyntaxNodeComparer.CompareFullSpan` compares two `ReadOnlySpan<char>` with `==`, which is memory identity, not content equality. Since the two source strings are always distinct instances, it returns `false` 100% of the time and the whole-subtree skip never fires. The commit that introduced it describes it as "basically a fast `SequenceEqual`" targeting up to 30% of CLI comparison time. It went unnoticed because the benchmark passes the same string instance for both arguments — the one input shape where `==` can succeed.

Both are things the codebase already intends to do. Neither has a test asserting it works, which is why both regressed invisibly. Alongside them, four hot paths recompute known values: printed width is re-derived per measurement, every file walks its whole tree looking for `#if` symbols it usually does not have, `Token.cs` allocates before knowing it needs to, and single-lambda arguments print their body twice — exponentially when nested.

## What Changes

- **Return pooled buffers.** Convert the 21 `DocListBuilder` sites that never dispose to `using var`, and clear `span` in `Dispose` so use-after-dispose is unreachable by construction rather than only by convention.
- **Restore the full-span skip.** `CompareFullSpan` uses `SequenceEqual`. The benchmark is corrected to pass two distinct-but-equal strings so the fast path is actually exercised.
- **Gate XML AST serialization.** `XmlFormatter` currently runs a reflection-based `JsonSerializer.Serialize` over the parsed tree on every `.csproj`/`.xml` file and discards the result. It is gated on `printerOptions.IncludeAST`, matching `CSharpFormatter`. **BREAKING** for any consumer of `CodeFormatterResult.AST` on the XML path that did not set `IncludeAST` — no in-repo consumer does.
- **Memoize printed width** on `StringDoc`, computed once in the constructor. The `int` lands in existing padding, so object size is unchanged.
- **Skip the preprocessor walk** for files with no directives, guarded on Roslyn's O(1) `SyntaxNode.ContainsDirectives`.
- **Add fast paths to `Token.cs`** so the per-token and per-line paths scan before allocating, rather than allocating and then discovering there was nothing to store.
- **Print single-lambda argument bodies once**, hoisting the shared `Doc` across both `IfBreak` branches — matching what the sibling parenthesized-lambda branch already does.
- **Add regression tests** for the invariants above, so a silently-disabled optimization fails the build instead of the benchmark.

Output must remain byte-identical. Every change here is an internal optimization except the XML `AST` gating.

## Capabilities

### New Capabilities
- `formatter-performance-invariants`: Machine-checkable invariants the formatting engine must hold — pooled buffers are returned, doc-tree comparison compares content rather than reference identity, debug-only serialization is gated behind its option, and per-node work is not repeated. Exists so that an optimization which stops working fails a test rather than degrading silently.

### Modified Capabilities
<!-- None. This change preserves all CLI-observable behavior; `cli-formatting-pipeline` requirements are unchanged. The one behavioral change (XML `AST` gating) is on the Core API surface, which has no existing spec, and is captured as a requirement in the new capability above. -->

## Impact

**Core — doc printing**
- `Utilities/DocListBuilder.cs` (`Dispose` clears `span`)
- 21 call sites across `CSharp/SyntaxPrinter/` and `Xml/XNodePrinters/Node.cs`
- `DocTypes/StringDoc.cs`, `DocPrinter/DocPrinter.cs:276`, `DocPrinter/DocFitter.cs:61`

**Core — C# pipeline**
- `CSharp/SyntaxNodeComparer.cs:383`
- `CSharp/PreprocessorSymbols.cs:31`, entry via `CSharp/CSharpFormatter.cs:160`
- `CSharp/SyntaxPrinter/Token.cs:58`, `:258`, `:425`
- `CSharp/SyntaxPrinter/ArgumentListLikeSyntax.cs:35-36`

**Core — XML pipeline**
- `Xml/XmlFormatter.cs:52`

**Tests and benchmarks**
- `CSharpier.Benchmarks/Program.cs:79` — the comparer benchmark's inputs
- New allocation and invariant tests in `CSharpier.Tests`

**Risk.** `Token.cs` and `ArgumentListLikeSyntax` both touch code that mutates `CSharpPrintingContext.State`, so print ordering is load-bearing; those two require a full `dotnet test` run rather than a filtered subset. The remaining items are mechanical.
