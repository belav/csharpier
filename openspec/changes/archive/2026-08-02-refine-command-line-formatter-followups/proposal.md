## Why

The `refactor-command-line-formatter` change deliberately preserved several pre-existing quirks verbatim and noted them as follow-ups. This change addresses four of them: a fragile path rewrite that can corrupt displayed paths, inconsistent use of the `IFileSystem` abstraction, duplicated per-file setup between the stdin and physical-file paths, and an unexamined stdin ancestor-walk whose necessity was never verified. None of these is intended to change user-facing behavior; each makes the code more correct, more consistent, or smaller.

## What Changes

- **Prefix-safe path rewrite** — replace the global-substring `file.Replace(directoryPath, originalPath)` in `FormattingEngine.FormatDirectory` with a prefix-only rewrite, so a directory name that recurs as a nested path segment no longer produces a corrupted display path.
- **Consistent `IFileSystem` usage** — replace the remaining static `System.IO.Path` calls in `CommandLineFormatter` (`Path.Combine`, `Path.IsPathRooted`, `Path.DirectorySeparatorChar`) with the injected `fileSystem.Path.*`, closing a test-isolation seam. Production behavior is unchanged because the real `IFileSystem` wraps `System.IO`.
- **Unify stdin per-file formatting into `FormattingEngine`** — extract a shared per-file core on the engine so the stdin path and the physical-file path stop duplicating the generated/ignore gate, printer-options resolution, formatter guard, issue-logger construction, and dispatch to `PerformFormattingSteps`. `CommandLineFormatter.FormatStandardInput` shrinks to path/extension inference plus engine construction.
- **Investigate and resolve the stdin ancestor-walk** — add characterization tests for `--stdin-path` pointing at a non-existent directory, then determine empirically whether the explicit walk-up loop is load-bearing (config/ignore resolution already walk up with existence guards). If equivalent, remove the loop; otherwise keep it, guard against unbounded looping, and document why.

No new CLI options, no public API changes; `CommandLineFormatter` and `FormattingEngine` are `internal`.

## Capabilities

### New Capabilities
<!-- None. This is an internal-quality follow-up; all observable behavior is preserved. -->

### Modified Capabilities
- `cli-formatting-pipeline`: The "Content piped with a supplied file path" scenario currently prescribes the ancestor-walk *mechanism* ("SHALL walk up to the nearest existing ancestor directory"). It is restated in terms of the observable outcome — configuration and ignore rules are resolved as if the file were at the supplied path even when its parent directory does not exist on disk — so the requirement holds whether or not item 3 removes the explicit loop.

## Impact

- **Code**: `Src/CSharpier.Cli/FormattingEngine.cs` (prefix-safe rewrite; new shared per-file core; new stdin entry method) and `Src/CSharpier.Cli/CommandLineFormatter.cs` (`fileSystem.Path` usage; slimmed `FormatStandardInput`; ancestor-walk resolution).
- **Tests**: `Src/CSharpier.Tests/CommandLineFormatterTests.cs` must stay green unchanged as the behavior-preservation gate. New tests are added for the recurring-nested-segment path rewrite and for stdin config/ignore resolution with a non-existent `--stdin-path` parent.
- **APIs**: No public API changes. `PublicAPI.Shipped.txt` / `PublicAPI.Unshipped.txt` are unaffected.
- **Dependencies**: None added or removed.
- **Depends on**: `refactor-command-line-formatter` (builds on its extracted `FormattingEngine` and orchestrator structure). Independent of `bound-formatting-concurrency`.
