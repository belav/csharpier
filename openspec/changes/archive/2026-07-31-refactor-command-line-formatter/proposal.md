## Why

`CommandLineFormatter` has grown into a single ~600-line static class that mixes input-source handling, file discovery, per-directory config resolution, the multi-step formatting pipeline, result accounting, and exit-code logic. The core method `PerformFormattingSteps` alone is ~180 lines and threads eight parameters through a long sequence of conditionally short-circuiting steps, making the control flow hard to follow, hard to reason about, and awkward to extend. This refactor makes the code understandable without changing any observable behavior.

## What Changes

- Introduce **one** new internal type, a `FormattingEngine` class, that holds the per-path shared state (writer, options provider, formatting cache, options, file system, logger, result counters) as fields. The per-file formatting sequence (currently the 8-parameter `PerformFormattingSteps`), the per-file dispatch, and the recursive file walk become small methods on it that read those fields — eliminating the parameter threading and the nested local-function closures. The sequence stays a single **linear** method (only the nested validation block is pulled into a private helper).
- Reduce `CommandLineFormatter` to a **thin orchestrator**: an instance class holding the ambient run state (options, file system, console, logger, cancellation) as fields, with a `public static Task<int> Format(...)` entry point of unchanged signature. `FormatAsync` dispatches to `FormatStandardInput` or `FormatPhysicalFiles`; the stdin setup (path walking, temp file naming, encoding, ignore/generated gate) and the physical per-path setup (writer selection, options-provider creation, cache init, MSBuild version check, directory fan-out) live in their own focused methods.
- **Subtract, don't add**: no pipeline, no stage interface, no per-step classes, no context object — the change is expected to *reduce* total lines in `CommandLineFormatter` while making each method single-purpose.
- **No behavior change**: identical CLI output, exit codes, logging, and result counters. The existing `CommandLineFormatterTests` suite must stay green with no test modifications required for correctness.

## Capabilities

### New Capabilities
<!-- This is an internal structural refactor. No new user-facing behavior is introduced;
     the capability below characterizes the EXISTING observable behavior that the refactor
     MUST preserve, so it can serve as the refactor's acceptance contract. -->
- `cli-formatting-pipeline`: The observable behavior of the CLI file-formatting flow — input-source handling (stdin vs. paths), file discovery with ignore/generated filtering, the per-file formatting/validation/write sequence, result counting, and exit-code determination — that must remain unchanged across the refactor.

### Modified Capabilities
<!-- None. No spec-level requirements change; observable CLI behavior is preserved exactly. -->

## Impact

- **Code**: `Src/CSharpier.Cli/CommandLineFormatter.cs` is slimmed to a thin static orchestrator; one new internal type (`Src/CSharpier.Cli/FormattingEngine.cs`) absorbs the per-file/per-path work. `CommandLineFormatterResult` is unchanged.
- **Tests**: `Src/CSharpier.Tests/CommandLineFormatterTests.cs` continues to pass unchanged; it is the primary safety net verifying behavior preservation.
- **APIs**: No public API changes — `CommandLineFormatter` is `internal`. `PublicAPI.Shipped.txt` / `PublicAPI.Unshipped.txt` are unaffected.
- **Dependencies**: None added or removed.
