## Context

`CommandLineFormatter` is an `internal static` class in `Src/CSharpier.Cli/CommandLineFormatter.cs` (~600 lines) with five methods:

- `Format` — top-level entry: branches on stdin vs. physical files, builds config, times the run, logs the summary, maps result → exit code, and handles the `InvalidIgnoreFileException` case.
- `FormatPhysicalFiles` — loops over path arguments; selects a writer; per path creates an `OptionsProvider` and a formatting cache; contains two local functions (`EnumerateNonignoredFiles`, `FormatFile`); handles the MSBuild version check and directory fan-out via `Task.WhenAll`.
- `FormatPhysicalFile` — builds `FileToFormatInfo` + `FileIssueLogger`, logs, delegates to `PerformFormattingSteps`.
- `ReturnExitCode` — pure function over `CommandLineFormatterResult`.
- `PerformFormattingSteps` — ~180 lines: the ordered per-file sequence (empty → cache → encoding → format → diagnostics → warning/failure → validation → check-diff → write/cache), mutating a shared `CommandLineFormatterResult` via `Interlocked`.

The pain is concentrated in `PerformFormattingSteps` (long, deeply nested, eight parameters) and in the local-function soup inside `FormatPhysicalFiles`. The class is `internal`, exercised end-to-end by `CommandLineFormatterTests`, which is the safety net for a behavior-preserving refactor.

## Goals / Non-Goals

**Goals:**
- Reduce the actual complexity of `CommandLineFormatter`: eliminate the 8–10 parameter threading, separate the two input modes, and un-nest the local-function soup — with *less* total code and *no* new abstraction.
- Give each method a single clear responsibility so `CommandLineFormatter.Format` reads as a thin orchestrator and the per-file sequence stays a readable linear method.
- Preserve behavior exactly: identical output, exit codes, logging text, counters, concurrency, and cancellation semantics. `CommandLineFormatterTests` passes unmodified.

**Non-Goals:**
- No new features, no CLI/option changes, no public API changes.
- No fixing of pre-existing quirks (the `TODO log error?`, the `"/n"` typo in the validation message, stdin ancestor-walking). These are preserved verbatim to keep the diff behavior-neutral. (Noted as follow-ups, not done here.)
- No performance tuning; the directory fan-out / `Interlocked` counting model stays as-is. Bounding the unbounded per-file `Task.WhenAll` fan-out is deferred to the follow-up change `bound-formatting-concurrency`, which builds on this refactor's extracted enumeration.
- No change to `CommandLineFormatterResult`, writers, cache, or validators.

## Decisions

### Decision 1: A single `FormattingEngine` worker class — subtract, don't add

The root causes of the complexity are **parameter threading** (`PerformFormattingSteps` takes 8 params, `FormatPhysicalFile` takes 10 and just forwards them) and **two input modes crammed into one method** (`Format` inlines ~90 lines of stdin setup). The length of the linear per-file sequence is *not* itself the problem — it reads top-to-bottom as guard clauses.

Introduce exactly **one new type**: an `internal` (non-static, non-abstract) `FormattingEngine` class that holds the per-path shared state as constructor fields — `IFormattedFileWriter`, `OptionsProvider`, `IFormattingCache`, `CommandLineOptions`, `IFileSystem`, `ILogger`, `CommandLineFormatterResult`. Because the shared inputs live on `this`, the operations become small methods with no threading and no nested closures:

- `PerformFormattingSteps(FileToFormatInfo, FileIssueLogger, PrinterOptions, CancellationToken)` — the existing per-file sequence, moved verbatim but reading writer/result/options/cache from fields. Kept as **one linear method**; only the deeply-nested validation block is pulled into a private `ValidateFormatting(...)`.
- `FormatPhysicalFile(actual, original, warnForUnsupported, ct)` — the per-physical-file dispatch (the old local `FormatFile` merged with `FormatPhysicalFile`): generated/ignored gate → printer options → build info/logger → debug log → run the sequence.
- `FormatDirectory(dir, originalDir, ct)` — enumerate + `Task.WhenAll` fan-out with identical `OperationCanceledException` handling.
- `EnumerateNonignoredFiles(dir, ct)` — private recursive ignore-aware walk (no longer a closure).

**Why:** This is the textbook refactoring of "a local function capturing N locals" → "a class with fields," plus "an 8-param method" → "a method reading fields." It *removes* more code than it adds (param lists, forwarding, two closures) and introduces **no abstraction** — no interface, no enum, no runner, no per-step classes. The shared `CommandLineFormatterResult` + `Interlocked` counting model is untouched; a fresh `FormattingEngine` is created per path (its fields are per-path), so thread-safety matches today.

**Alternatives considered:**
- *Pipeline of stage classes + `IFormattingStage` + a context object* — rejected: it shatters a readable linear sequence into ~13 files and an interface, and the orchestration complexity just relocates rather than shrinks (net *more* code and indirection to express a fixed, branch-free sequence).
- *A per-file `record` context passed to free-standing static methods* — removes the threading but leaves the directory walk / dispatch as either nested closures or another threaded record; the `FormattingEngine` class subsumes both with one type.
- *Private methods only, no new type* — still forces either 8-param signatures or reintroduced closures; the class is the smaller, cleaner result.

### Decision 2: `CommandLineFormatter` becomes a thin orchestrator

`CommandLineFormatter` becomes an instance class: a primary constructor holds the ambient run state (`CommandLineOptions`, `IFileSystem`, `IConsole`, `ILogger`, `CancellationToken`) plus a `CommandLineFormatterResult` field, and a `public static Task<int> Format(...)` entry point (unchanged signature — the test harness and both prod callers depend on it) constructs the instance and runs it. Holding the ambient state as fields removes the 6–8 parameter threading from the orchestrator methods too.

`FormatAsync` reduces to: start timer → dispatch to `FormatStandardInput` or `FormatPhysicalFiles` → finalize (summary log + exit code) → the `InvalidIgnoreFileException` catch. The stdin setup (temp path/extension inference, ancestor-walking, options-provider creation, ignore/generated gate) moves into `FormatStandardInput`, which ends by constructing a `FormattingEngine` (StdOut writer + `NullCache`) and calling `PerformFormattingSteps`. Physical handling moves into `FormatPhysicalFiles` (writer selection + path loop) and `FormatPhysicalPath` (per-path options/cache setup + file-vs-directory dispatch through a `FormattingEngine`), with a small `SelectWriter`.

**Why:** Separates the two input modes that were tangled together and keeps each top-level method single-purpose, while the missing-path / MSBuild early `return 1` and per-path `ResolveAsync` behavior is preserved exactly.

### Decision 3: Placement and visibility

`FormattingEngine` goes in a new file `Src/CSharpier.Cli/FormattingEngine.cs`, `internal`. `CommandLineFormatter` is an `internal` instance class with a static `Format` entry point. No changes to `PublicAPI.*.txt`.

**Why:** One cohesive worker type alongside the orchestrator, no widened surface area.

## Risks / Trade-offs

- **Behavior drift during extraction** → The moved sequence must reproduce today's exact order and short-circuit points, including subtle ones (empty-file returns *before* incrementing `Files`; cache-skip increments both `Files` and `CachedFiles`; warning/failure messages `return` without writing). Mitigation: move the sequence verbatim (fields replace params, nothing else); rely on the unmodified `CommandLineFormatterTests` as the gate; diff-review against the original lines.
- **Cancellation / exception semantics** → `OperationCanceledException` is rethrown from the format step and the directory `Task.WhenAll` swallows only the matching token. Mitigation: keep the try/catch shapes identical inside the moved methods; do not "clean up" catch clauses.
- **Concurrency of shared state** → Files in a directory are formatted in parallel and mutate a shared `CommandLineFormatterResult`. Mitigation: preserve `Interlocked` usage exactly; a fresh `FormattingEngine` is constructed per path (its fields are per-path), and only the `CommandLineFormatterResult` counter object is shared across files — matching today.
- **A new type where there was none** → `FormattingEngine` adds one class. Mitigation/justification: it is a plain concrete class (no interface/abstraction) that removes more code than it adds (8–10 param signatures, forwarding, two nested closures); it is the standard closure-over-locals → class refactoring, not new machinery.
- **Preserved quirks look like bugs** → Reviewers may flag the `"/n"` typo or `TODO log error?`. Mitigation: call them out in the PR as intentionally preserved follow-ups.

## Migration Plan

Not applicable — internal refactor, no deploy/runtime migration. Rollback is reverting the changed files. The single verification gate is `dotnet test` on `CommandLineFormatterTests` (and the broader CLI test suite) passing unchanged.

## Open Questions

Resolved during implementation:

- The moved `PerformFormattingSteps` keeps its name (recognizable for the behavior-preservation diff-review).
- The worker class is named `FormattingEngine` (not `FileFormatter`) since it handles directory walks, single files, and stdin content — not just a single file.
- Per-path work is a separate `FormatPhysicalPath(index, writer)` method rather than inlined into the loop.
