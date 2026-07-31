## 1. Prerequisites and baseline

- [ ] 1.1 Confirm `refactor-command-line-formatter` has landed (file enumeration extracted, per-file work invoked via the pipeline); rebase on it.
- [ ] 1.2 Run `dotnet test` for the CLI suite and record the green baseline.
- [ ] 1.3 Capture a before benchmark: format a large directory and record wall-clock and peak working set (for the after comparison).

## 2. Bounded execution

- [ ] 2.1 Add a single named source of the default max degree of parallelism based on `Environment.ProcessorCount`.
- [ ] 2.2 Replace the `List<Task>` + `Task.WhenAll` directory fan-out in `FormatPhysicalFiles` with `Parallel.ForEachAsync` over the file-enumeration stream, using `MaxDegreeOfParallelism` = the default and `CancellationToken` = the run token.
- [ ] 2.3 Keep the per-file relative-path computation and `FormatFile` invocation inside the loop body unchanged.

## 3. Preserve cancellation semantics

- [ ] 3.1 Wrap the `Parallel.ForEachAsync` call so a cancellation carrying the run token is swallowed and any other cancellation rethrows, matching current behavior.
- [ ] 3.2 Add a test that cancels mid-directory-run and asserts the same outward behavior (no throw for the run token); adjust the catch to key off `cancellationToken.IsCancellationRequested` if reference-equality proves unreliable.

## 4. Verification

- [ ] 4.1 Run `dotnet test` for the CLI suite; confirm `CommandLineFormatterTests` passes with no correctness-driven test edits.
- [ ] 4.2 Add/confirm coverage that a large directory formats with identical counters and exit code versus the unbounded baseline.
- [ ] 4.3 Capture the after benchmark; confirm peak working set is bounded and wall-clock is equal-or-better; record before/after in the PR.
- [ ] 4.4 Run CSharpier on the changed files (dogfood formatting).

## 5. Optional: user-facing configuration (only if decided in design open questions)

- [ ] 5.1 If exposing a flag, add the option to `CommandLineOptions` with parsing and help text, defaulting to `Environment.ProcessorCount`.
- [ ] 5.2 Add a test for the flag (custom value honored; default when omitted) and document it.
