## Context

`CommandLineFormatter.FormatPhysicalFiles` currently formats a directory like this (`Src/CSharpier.Cli/CommandLineFormatter.cs:331-351`):

```csharp
var tasks = new List<Task>();
await foreach (var file in EnumerateNonignoredFiles(directoryOrFilePath).WithCancellation(cancellationToken))
{
    var relativePath = file.Replace(directoryOrFilePath, originalDirectoryOrFile);
    tasks.Add(FormatFile(file, relativePath));
}
try { await Task.WhenAll(tasks).WaitAsync(cancellationToken); }
catch (OperationCanceledException ex) { if (ex.CancellationToken != cancellationToken) throw; }
```

Every discovered file becomes a live `Task` immediately; the `List<Task>` holds them all until the directory finishes. There is no cap on how many format/validate operations run at once. Formatting is CPU-bound (parsing + printing + optional syntax-tree validation), so on a large repo this oversubscribes the thread pool and inflates peak memory. File ordering is already non-deterministic, so nothing downstream depends on order.

This change assumes `refactor-command-line-formatter` has landed, so the enumeration is available as `NonIgnoredFileEnumerator` and the per-file work is the pipeline invocation.

## Goals / Non-Goals

**Goals:**
- Cap concurrent per-file formatting at a bounded degree of parallelism defaulting to `Environment.ProcessorCount`.
- Keep peak memory and thread-pool pressure bounded regardless of directory size.
- Preserve every observable result: output, counters, exit codes, and cancellation semantics.
- Prove the change with a before/after benchmark on a large directory (peak memory + wall-clock).

**Non-Goals:**
- No change to formatting output or to the set of files formatted.
- No reordering guarantees (order is already unspecified).
- No changes to the formatting cache, writers, validators, or the per-file pipeline itself.
- No change to stdin / single-file paths (they format one file; concurrency is irrelevant).

## Decisions

### Decision 1: `Parallel.ForEachAsync` over the async file stream

Replace the collect-all-tasks-then-`WhenAll` pattern with `Parallel.ForEachAsync(EnumerateNonignoredFiles(dir), parallelOptions, async (file, ct) => await FormatFile(...))`, where `parallelOptions.MaxDegreeOfParallelism` is the configured bound and `parallelOptions.CancellationToken` is the run token.

**Why:** It consumes the existing `IAsyncEnumerable<string>` directly (discovery stays streaming — it does not need to finish before formatting starts), it bounds in-flight work to `MaxDegreeOfParallelism`, and it does not accumulate a `List<Task>` of every file. It is the standard BCL primitive for exactly this shape.

**Alternatives considered:**
- *`Channel<string>` + N worker loops* — equivalent behavior, more code; only preferable if we need custom backpressure beyond what `Parallel.ForEachAsync` gives. Rejected as unnecessary.
- *`SemaphoreSlim` gating around the existing `Task.WhenAll`* — still allocates a task per file up front and keeps the `List<Task>`; only limits execution, not the allocation/memory footprint. Rejected.

### Decision 2: Default degree of parallelism = `Environment.ProcessorCount`

Formatting is CPU-bound, so `ProcessorCount` is the natural default. Keep it in one named constant/helper so it is easy to tune.

**Why:** Matches the workload; avoids both oversubscription (today) and under-utilization.

### Decision 3: Preserve cancellation semantics exactly

`Parallel.ForEachAsync` throws `OperationCanceledException` when its token cancels. Wrap the call so a cancellation carrying the run's token is swallowed (as today) and any other cancellation rethrows — mirroring the current `catch (OperationCanceledException ex) { if (ex.CancellationToken != cancellationToken) throw; }`. Note that `Parallel.ForEachAsync` may surface cancellation as a plain `OperationCanceledException` whose `CancellationToken` is the linked token; the wrapper must compare against the run token by checking `ct.IsCancellationRequested` rather than only reference-equality if reference-equality proves unreliable — to be confirmed during implementation with a test.

**Why:** Cancellation behavior is observable and covered; it must not regress.

## Risks / Trade-offs

- **Cancellation-token identity mismatch** → `Parallel.ForEachAsync` may wrap the token, so the existing `ex.CancellationToken != cancellationToken` reference check could behave differently. Mitigation: add a test that cancels mid-run and assert the same outward behavior; adjust the catch to key off `cancellationToken.IsCancellationRequested`.
- **Throughput regression on small directories** → A bound below the previous effective concurrency could slow tiny runs. Mitigation: default to `ProcessorCount` (≥ typical small-dir parallelism benefit) and benchmark; small dirs are dominated by startup anyway.
- **Exception aggregation differences** → `Task.WhenAll` surfaces the first exception; `Parallel.ForEachAsync` cancels remaining iterations and throws (possibly aggregated). In practice per-file exceptions are caught inside the pipeline and converted to counters, so exceptions rarely escape `FormatFile`. Mitigation: confirm the pipeline still swallows per-file format exceptions into `ExceptionsFormatting` (it does today) so nothing new escapes to the loop.
- **Interlocked counters under different scheduling** → Counters already use `Interlocked` and are order-independent; bounded concurrency does not affect correctness. No mitigation needed beyond keeping `Interlocked`.

## Migration Plan

Internal execution change; no runtime migration. Rollback is `git revert`. Verification gates: `dotnet test` (CLI suite, esp. `CommandLineFormatterTests`) green and unmodified for correctness, plus a before/after benchmark on a large directory recording peak working set and wall-clock.

## Open Questions

- **Expose a CLI flag?** Should max degree of parallelism be user-configurable (e.g. `--parallelism <n>` / env var) or stay an internal default? Recommendation: ship the internal `ProcessorCount` default first; add a flag only if users report a need. If a flag is added it touches `CommandLineOptions` and its parsing/help text.
- **Interaction with the MSBuild-version check and per-path cache** — unchanged (both run once per path argument, outside the file loop), but confirm no shared state is touched concurrently by the bounded workers that was previously safe only due to timing.
