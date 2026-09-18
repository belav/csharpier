## Context

CSharpier's formatting engine has had roughly thirty perf-focused PRs — a pooled `ValueListBuilder`, cached `Indent` and `StringDoc` instances, a value-type `PrintCommand`, non-allocating `Any` overloads for Roslyn's struct lists. The obvious wins are already taken.

An audit found that two of those optimizations no longer do anything, and that a handful of hot paths recompute values that are already known. Measured baseline from the repo's own BenchmarkDotNet suite:

| Benchmark | Mean | Allocated |
| --- | --- | --- |
| `Default_CodeFormatter_Complex` | 128.5 ms | 47.5 MB |
| `Default_CodeFormatter_Tests` | 59.4 ms | 27.6 MB |

A single format pass allocates 27–47 MB and triggers ~2 Gen0 collections. That is the budget these changes draw down.

Two findings dominate, and both share a root cause: **the optimization was written, was never asserted by a test, and stopped working without anyone noticing.**

- `DocListBuilder` rents from `ArrayPool<Doc>.Shared` in its constructor; only 3 of 24 sites return the array. Measured at exactly 152 bytes per construction — a `Doc[16]`, the pool's minimum bucket — which is worse than both a right-sized `Doc[n]` (64 B end-to-end) and a `List<Doc>` (96–112 B).
- `CompareFullSpan` uses `==` on `ReadOnlySpan<char>`, which is documented as "point at the same memory", not content equality. Production always passes two distinct strings, so it returns `false` every time. The benchmark passes `(code, code)` — the same instance — which is the only shape where reference comparison succeeds, so the benchmark reports the optimization working while the CLI gets nothing.

## Goals / Non-Goals

**Goals:**
- Restore the two broken optimizations and prove they work with tests, not benchmarks.
- Remove per-node and per-file work that recomputes a known value.
- Keep formatted output byte-identical. The existing expected-output corpus is the contract.
- Leave behind invariants that fail the build if any of this silently regresses again.

**Non-Goals:**
- The long tail of small allocations (LINQ over Roslyn struct lists, `Select`+`Join`, `Guid` group IDs, per-node singletons). Those are tracked separately in `reduce-printer-allocation-churn`.
- The correctness bugs surfaced by the same audit. Tracked in `fix-audit-correctness-bugs`.
- Changing `Concat.Contents` from `IList<Doc>` to `Doc[]`, reordering the doc-type dispatch chains, or replacing the command stack. All three are plausible but not obviously net-positive; they need their own benchmark run before anyone commits to them.
- Any change to CLI-observable behavior. `cli-formatting-pipeline` requirements are untouched.

## Decisions

### Fix `DocListBuilder` at the call sites, not by removing the pool

`using var` at the 21 leaking sites, rather than switching them to `List<Doc>` or a plain array.

Measured end-to-end, including the `Concat` that consumes the buffer:

| Approach | 2 docs | 5 docs |
| --- | --- | --- |
| current (rent, never return) | 216 B | 240 B |
| `using var` (rent + return) | **64 B** | **88 B** |
| `new List<Doc>()` | 112 B | 200 B |
| `new List<Doc>(count)` | 96 B | 120 B |
| `new Doc[count]` | 64 B | 88 B |

*Alternatives considered.* Switching to `List<Doc>` would be an improvement over today (112 B vs 216 B) and would let `Doc.Concat(List<Doc>)` skip the `ToArray()` copy, since `Concat` stores an `IList<Doc>` directly. Rejected on three grounds: it retains a 32-byte wrapper per node; it keeps `Concat.Contents` bimorphic, which is the worst case for devirtualization in the three hottest walk loops; and it aliases the caller's list into the doc tree, which `PropagateBreaks` then mutates in place. A plain `Doc[count]` ties the fixed builder on bytes and is cheaper on CPU, but only works where the final count is known up front — most printers add conditionally. Where the count *is* known, prefer the array; that is called out per-task.

*Longer term.* For buffers this small, the pool's bookkeeping barely pays for itself. A C# 12 `[InlineArray]` stack buffer with pool fallback on growth would give zero allocation and zero pool traffic. Out of scope here — `using var` is the mechanical fix that restores the intended behavior; the inline-array variant can follow once the invariant test exists.

*Why the API is also at fault.* 21 of 24 sites got this wrong, so the fix includes clearing `span` in `Dispose` and adding the source-level invariant test. A `ConcatAndDispose()` helper is worth considering but is not required to close the finding.

### `SequenceEqual` is sound here, not merely faster

Both trees are parsed from the same `CSharpParseOptions` instance, so identical full-span text implies identical subtrees; skipping is safe. Each span is sliced from its own source string, so the position shift between original and formatted file is already handled.

*Alternative considered.* Comparing `node.FullSpan.Length` first as a cheap reject. Unnecessary — `SequenceEqual` already length-checks before comparing, and the vectorized comparison is fast enough that a manual guard adds nothing.

The benchmark change matters as much as the fix: leaving `(code, code)` in place would let a future regression to reference comparison pass unnoticed all over again.

### Compute printed width eagerly, not lazily

`StringDoc.PrintedWidth` is computed in the constructor rather than memoized on first read.

Lazy memoization (`printedWidth = -1`, fill on first access) is a benign race in principle — an idempotent `int` store — but the ~250 static `StringDoc` singletons are shared across the threads that format files in parallel, and eager computation removes the question entirely. Every `StringDoc` is measured at least once when printed, so nothing is computed that would not have been.

The `int` occupies existing padding next to the `bool IsDirective`, so `StringDoc` does not grow.

*Alternative considered.* An ASCII fast path inside `GetPrintedWidth` using `MemoryExtensions.ContainsAnyExceptInRange`. Measured 1.7× against 18× for memoization, needs a `#if NET8_0_OR_GREATER` guard for the `netstandard2.0` target, and becomes nearly irrelevant once the value is stored. Skipped.

### Guard the preprocessor walk on `ContainsDirectives`

Hoisted into the static `GetSets(SyntaxTree)` so the walker instance — and its `List`, `HashSet` and two `SymbolContext` allocations — is never constructed for a directive-free file.

`ContainsDirectives` is a green-node flag, so the guard is a bit read. It is true for `#region`, `#nullable` and `#pragma` as well as `#if`, so those files still walk; that is correct and conservative.

### `Token.cs` scans before it allocates

Three sites, all the same shape — allocate the buffer, then discover there was nothing to put in it. Each gets a cheap pre-scan.

The leading-trivia guard needs both `includeInitialNewLines` and `context.State.NextTriviaNeedsLine` in its condition: with the former set an `EndOfLineTrivia` does emit a doc, and the latter injects a `HardLine` into an otherwise-empty list. Getting that condition wrong changes output, which is why this task carries the byte-exactness check explicitly.

### Share the lambda body `Doc` across both `IfBreak` branches

`ParenthesizedLambdaExpression` already does exactly this, so the pattern is established and the sibling branch in the same method already prints its body once.

*Risk acknowledged.* `PrintBody` mutates `CSharpPrintingContext.State` — `SkipNextLeadingTrivia`, `TrailingComma`, `ReorderedModifiers` — so calling it once instead of twice is a behavioral change, not merely a caching one. And `PropagateBreaks` rewrites `Concat.Contents` in place under `ForceFlat`, so a shared node reached at two different `forceFlat` depths could in principle differ. Both are narrow, and `PropagateBreaks` already guards against revisiting shared `Group`s — but this is the one task in the change that cannot be validated by inspection.

## Risks / Trade-offs

- **Sharing the lambda body changes context-mutation counts** → Land it as its own commit, separate from the mechanical items, and run the full `dotnet test` suite. If any expected-output test moves, revert that task alone; the rest of the change stands without it.
- **`Token.cs` fast paths can silently change trivia output** → The pre-scan conditions are the risky part, not the allocation removal. Write the guard to bail into the existing slow path whenever it is not certain, so a mistake costs performance rather than correctness.
- **Restoring `CompareFullSpan` changes how much the comparer skips** → It is the only item that changes what work is *skipped* rather than how fast the same work runs. If the skip is unsound in some case, validation stops catching a real formatting bug. Mitigated by the parse-options argument above, plus scenarios covering shifted offsets and differing text.
- **Gating the XML `AST` is a Core API behavior change** → No in-repo consumer reads `CodeFormatterResult.AST` on the XML path without setting `IncludeAST`, and it matches what `CSharpFormatter` already does. Called out as **BREAKING** in the proposal for external consumers.
- **`using var` on a `ref struct` relies on scope-exit ordering** → `using var x = …; return F(ref x);` lowers to `try { return F(ref x); } finally { x.Dispose(); }`, so the return value is computed before disposal. Verified against `Doc.Concat(ref DocListBuilder)`, whose three arms all copy out (`NullDoc` singleton, `contents[0]`, or `ToArray()`).

## Migration Plan

Land in this order, each independently revertible:

1. Invariant tests first, asserting the *current* broken behavior fails. This proves the tests have teeth before any fix makes them pass.
2. `CompareFullSpan` + benchmark inputs — one word plus one benchmark line.
3. XML `AST` gating — one line.
4. `DocListBuilder` — `Dispose` clears `span`, then the 21 `using var` sites.
5. `StringDoc.PrintedWidth`, then the two read sites.
6. `PreprocessorSymbols` guard.
7. `Token.cs` fast paths, then the lambda body sharing — the two behavioral-risk items, last and separate.

Rollback is per-commit. Nothing here shares state with anything else in the list except steps 4 and 5, which touch different files.

## Open Questions

- Should `DocListBuilder` gain a `ConcatAndDispose()` so the correct call is also the shortest one? It would prevent recurrence better than a test, but it changes an API that 24 sites use. Deferred — the invariant test closes the finding either way.
- Is the `[InlineArray]` stack-buffer variant worth pursuing once the pool traffic is measurable again? Depends on what the benchmark shows after step 4; not worth deciding in advance.

## Measured Results

Both runs on the same machine (AMD Ryzen 9 7900X, 24 logical cores, .NET 11.0.0-rc.1, BenchmarkDotNet 0.16.0-preview.1), `--filter "*CSharpBenchmarks.Default_CodeFormatter*"`. "Before" is this branch's merge base with the change's source files restored to `HEAD`.

| Benchmark | Mean before | Mean after | Allocated before | Allocated after |
| --- | --- | --- | --- | --- |
| `Default_CodeFormatter_Tests` | 73.50 ms | 61.11 ms | 27.53 MB | 23.55 MB |
| `Default_CodeFormatter_Complex` | 139.97 ms | 134.58 ms | 42.30 MB | 39.29 MB |

Allocated bytes per format dropped 14.5% and 7.1%; Gen0/Gen1 collections dropped with them (2000 → 1625 and 3000 → 2750 Gen0 per 1000 ops).

The plan's recorded baseline of 59.4 ms / 27.6 MB and 128.5 ms / 47.5 MB was taken on different hardware, so only the same-machine pair above is comparable. Allocation figures are close enough between the two machines to line up; the timings are not.

`Default_SyntaxNodeComparer` is not compared before/after, because this change deliberately alters its inputs — the old number was measured against a single shared string instance, which is the one shape where the broken reference comparison succeeded.
