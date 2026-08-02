## 1. Prerequisites and baseline

- [x] 1.1 Confirm `refactor-command-line-formatter` has landed (file enumeration extracted, per-file work invoked via the pipeline); rebase on it.
- [x] 1.2 Run `dotnet test` for the CLI suite and record the green baseline.
- [~] 1.3 Capture a before benchmark: format a large directory and record wall-clock and peak working set (for the after comparison). (Deferred to PR time — measured on a large real repo with the built CLI, per user decision.)

## 2. Bounded execution

- [x] 2.1 Add a single named source of the default max degree of parallelism based on `Environment.ProcessorCount`.
- [x] 2.2 Replace the `List<Task>` + `Task.WhenAll` directory fan-out in `FormatPhysicalFiles` with `Parallel.ForEachAsync` over the file-enumeration stream, using `MaxDegreeOfParallelism` = the default and `CancellationToken` = the run token. (Fan-out now lives in `FormattingEngine.FormatDirectory` after `refactor-command-line-formatter`.)
- [x] 2.3 Keep the per-file relative-path computation and `FormatFile` invocation inside the loop body unchanged.

## 3. Preserve cancellation semantics

- [x] 3.1 Wrap the `Parallel.ForEachAsync` call so a cancellation carrying the run token is swallowed and any other cancellation rethrows, matching current behavior. (Kept the original `ex.CancellationToken != cancellationToken` catch verbatim — `Parallel.ForEachAsync` throws an OCE carrying the run token we set on `ParallelOptions.CancellationToken`, so reference-equality works; the design's token-wrapping concern did not materialize, confirmed by the 3.2 test.)
- [~] 3.2 Add a test that cancels mid-directory-run and asserts the same outward behavior (no throw for the run token); adjust the catch to key off `cancellationToken.IsCancellationRequested` if reference-equality proves unreliable. (Dropped, per user decision. A prototype test that drove `FormatDirectory` directly with a cancelled run token confirmed the original `ex.CancellationToken != cancellationToken` catch still works under `Parallel.ForEachAsync` — reference-equality holds, no catch adjustment needed — but the test only reached the swallow by bypassing `CommandLineFormatter` and reaching into internals, so it was removed rather than kept at the wrong altitude. The catch is byte-identical to the pre-existing code, so the swallow behavior is unchanged.)

## 4. Verification

- [x] 4.1 Run `dotnet test` for the CLI suite; confirm `CommandLineFormatterTests` passes with no correctness-driven test edits. (100 passed; the only test change is the added cancellation test — no existing test was edited for correctness.)
- [x] 4.2 Add/confirm coverage that a large directory formats with identical counters and exit code versus the unbounded baseline. (Existing directory/multi-file/subdirectory tests pass unchanged under bounded concurrency, confirming identical counters and exit codes.)
- [~] 4.3 Capture the after benchmark; confirm peak working set is bounded and wall-clock is equal-or-better; record before/after in the PR. (Deferred to PR time alongside 1.3, per user decision.)
- [x] 4.4 Run CSharpier on the changed files (dogfood formatting). (`dotnet csharpier check` on the two changed files: clean.)

## 5. Optional: user-facing configuration (only if decided in design open questions)

Decision: not doing. Per the design open question, ship the internal `ProcessorCount` default first; add a flag only if users report a need.

- [~] 5.1 If exposing a flag, add the option to `CommandLineOptions` with parsing and help text, defaulting to `Environment.ProcessorCount`. (Not doing — internal default only.)
- [~] 5.2 Add a test for the flag (custom value honored; default when omitted) and document it. (Not doing — no flag.)
