## 1. Baseline safety net

- [x] 1.1 Run `dotnet test` for the CLI test suite (esp. `CommandLineFormatterTests`) and record the green baseline before any change.
- [x] 1.2 Skim `CommandLineFormatterTests` to confirm which behaviors are covered end-to-end (stdin, check mode, ignore/generated, validation, exit codes) so gaps are known before refactoring.

## 2. Introduce the `FormattingEngine` worker (one new type)

- [x] 2.1 Add an internal `FormattingEngine` class (in `Src/CSharpier.Cli/FormattingEngine.cs`) holding the per-path shared state as constructor fields: `IFormattedFileWriter`, `OptionsProvider`, `IFormattingCache`, `CommandLineOptions`, `IFileSystem`, `ILogger`, `CommandLineFormatterResult`.
- [x] 2.2 Move the body of `PerformFormattingSteps` onto `FormattingEngine` as a method taking only `(FileToFormatInfo, FileIssueLogger, PrinterOptions, CancellationToken)`, reading the shared inputs from fields. Keep the sequence a single linear method, behavior-verbatim (empty→cache→encoding→format→syntax→warning/failure→validation→check-diff→write, same `Interlocked` counters and short-circuits).
- [x] 2.3 Pull the validation block into a private `ValidateFormatting(...)` method — behavior-verbatim, preserving the empty `else`/`TODO log error?` branch and the existing `"/n"` message text.
- [x] 2.4 Move `EnumerateNonignoredFiles` onto `FormattingEngine` as a private recursive method using the `OptionsProvider`/`IFileSystem` fields; preserve recursion, ignore-directory skipping, and streaming/cancellation semantics.
- [x] 2.5 Merge the per-file dispatch (the old local `FormatFile` + `FormatPhysicalFile`) into a single `FormattingEngine.FormatPhysicalFile(actual, original, warnForUnsupported, ct)`: generated/ignored gate → printer options → build `FileToFormatInfo`/`FileIssueLogger` → debug log → run the sequence; unsupported-file warning preserved.
- [x] 2.6 Add `FormattingEngine.FormatDirectory(directoryOrFilePath, originalDirectoryOrFile, ct)` = enumerate + `Task.WhenAll` fan-out with the same `OperationCanceledException` (token-mismatch rethrow) handling.

## 3. Slim `CommandLineFormatter` to a thin orchestrator

Make `CommandLineFormatter` an instance class with a primary constructor holding the ambient run state (`CommandLineOptions`, `IFileSystem`, `IConsole`, `ILogger`, `CancellationToken`) plus the `CommandLineFormatterResult` field, keeping a `public static Task<int> Format(...)` entry point with the **unchanged** signature (the test harness and both prod callers depend on it). This removes the 6–8 parameter threading from the orchestrator methods too — they read the ambient state from fields.

- [x] 3.1 Extract the stdin setup into `FormatStandardInput()` (temp path + `.xml`/`.cs` inference, ancestor-walking, options provider, ignore/generated gate), ending in a `new FormattingEngine(StdOut writer, optionsProvider, NullCache, ...)` `.PerformFormattingSteps(...)` call. Behavior identical.
- [x] 3.2 Extract physical handling into `FormatPhysicalFiles()` (writer selection + path loop) and `FormatPhysicalPath(index, writer)` (per-path options/cache setup + file-vs-directory dispatch via `FormattingEngine`), plus a small `SelectWriter()`. Preserve the missing-path and MSBuild-mismatch early `return 1` before the summary log, and the per-path `ResolveAsync`.
- [x] 3.3 Reduce the run entry (`Format` → private `FormatAsync`) to: timer → dispatch stdin/physical → summary log → `ReturnExitCode`, preserving the `InvalidIgnoreFileException` catch verbatim. `ReturnExitCode` unchanged.

## 4. Verification

- [x] 4.1 Build the solution with no new warnings.
- [x] 4.2 Run `dotnet test` for the CLI suite; confirm `CommandLineFormatterTests` passes with zero test modifications.
- [x] 4.3 Diff-review the moved sequence and dispatch against the original lines to confirm order, short-circuits, counter increments, and message text are byte-for-byte equivalent.
- [x] 4.4 Run CSharpier on the changed files (dogfood formatting) so the new code conforms to the repo's own style.
- [x] 4.5 Note the intentionally-preserved quirks (`TODO log error?`, `"/n"` typo, stdin ancestor-walk) in the PR description as follow-ups, not fixed here.

## PR follow-up notes (intentionally preserved, not fixed here)

Ported verbatim to keep the refactor behavior-neutral; each is a candidate for a separate follow-up:

- **`TODO log error?`** — in `FormattingEngine.ValidateFormatting`, the `else` branch (a non-C#/non-XML formatter reaching validation) is still an empty block with the original comment.
- **`"/n"` typo** — the failed-validation message in `ValidateFormatting` still uses the literal `"/n"` rather than a newline `"\n"`, exactly as before.
- **stdin ancestor-walk** — in `CommandLineFormatter.FormatStandardInput`, a non-existent `--stdin-path` directory still walks up to the nearest existing ancestor for config resolution (unchanged).
