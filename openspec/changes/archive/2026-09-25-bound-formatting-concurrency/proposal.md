## Why

When formatting a directory, the CLI fires one asynchronous task per discovered file all at once and awaits them together (`Task.WhenAll` over an unbounded `List<Task>` in `CommandLineFormatter.FormatPhysicalFiles`). On a large repository this spawns thousands of concurrent formatting operations, oversubscribing the thread pool and holding every task's state in memory until the entire directory completes. Bounding the concurrency caps peak memory and thread-pool pressure and makes throughput predictable on large codebases. This is deferred out of `refactor-command-line-formatter` because it changes concurrency behavior; it should land on top of that structural refactor.

## What Changes

- Replace the unbounded per-file `Task.WhenAll` fan-out with a **bounded-concurrency** execution over the enumerated files (e.g. `Parallel.ForEachAsync` with a `MaxDegreeOfParallelism`, or an equivalent channel-based producer/consumer).
- Default the maximum degree of parallelism to a sensible value based on `Environment.ProcessorCount`.
- Preserve all observable per-file results: identical formatting output, identical result counters, identical exit codes, and the same cancellation semantics (a cancel via the run's token is swallowed as today; other cancellations propagate).
- Streaming file discovery continues to feed the bounded executor (discovery is not required to complete before formatting begins).

## Capabilities

### New Capabilities
<!-- None. This tunes execution of existing behavior. -->

### Modified Capabilities
- `cli-formatting-pipeline`: The directory-formatting requirement is refined to state that files are formatted with **bounded** concurrency rather than all at once, while every per-file outcome, counter, and exit code remains unchanged. (This capability is introduced by `refactor-command-line-formatter`; this change depends on that landing first.)

## Impact

- **Depends on**: `refactor-command-line-formatter` (builds on the extracted file-enumeration and orchestrator structure).
- **Code**: `Src/CSharpier.Cli/CommandLineFormatter.cs` (the directory fan-out block). Possibly a new option on `CommandLineOptions` if a user-facing flag is added (see design — open question).
- **Behavior**: Concurrency degree changes from unbounded to bounded. File *ordering* is already non-deterministic today, so no ordering guarantees change. No formatting-output change.
- **Tests**: Existing `CommandLineFormatterTests` must stay green. New coverage for large-directory behavior / concurrency bound may be added.
- **Performance**: Target is lower peak memory and thread-pool pressure on large repos with equal-or-better wall-clock throughput; validated by a before/after benchmark.
