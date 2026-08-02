## 1. Characterization tests for stdin ancestor-walk (item 3, first)

- [x] 1.1 Add a `CommandLineFormatterTests` case: stdin input with `--stdin-path` whose parent directory does not exist on disk, with a real `.csharpierrc` at an existing ancestor; assert the ancestor's configuration is applied to the formatted output. (Added `Should_Resolve_Config_From_Ancestor_When_StdinFilePath_Directory_Does_Not_Exist` as the safety net for group 3.)
- [x] 1.2 Add a companion case asserting ignore-file resolution for the non-existent-parent stdin path (a supplied path matching an ancestor's ignore rules produces no formatted output). (Added `Should_Resolve_Ignore_From_Ancestor_When_StdinFilePath_Directory_Does_Not_Exist`.)
- [x] 1.3 Run the suite and confirm 1.1–1.2 pass against the current ancestor-walk implementation (they pin today's observable behavior). Both pass against the (restored) ancestor-walk.

## 2. Unify stdin per-file formatting into FormattingEngine (item 7)

- [x] 2.1 Confirm `FileToFormatInfo` exposes its file path (via `Create` and `CreateFromFileSystem`); the shared core reads the path from it rather than a separate parameter. Confirmed: `FileToFormatInfo.Path` is set by both factories.
- [x] 2.2 Add private `FormatCore(...)` to `FormattingEngine`, moving the generated/ignore gate, `GetPrinterOptionsForAsync`, `Formatter is not Unknown` guard, `IncludeGenerated` assignment, `FileIssueLogger` construction, check/format debug log, and `PerformFormattingSteps` dispatch; gate the ignore check on `checkIgnored` and the unsupported-type warning on `warnForUnsupported`. **Signature deviation:** takes `(string filePath, string originalPath, bool checkIgnored, bool warnForUnsupported, Func<CancellationToken, Task<FileToFormatInfo>> getFileToFormatInfo, CancellationToken)` instead of a prebuilt `FileToFormatInfo`. Reason: the physical path must read the file **only after** the gate and supported-formatter check pass (as before), so ignored/generated/unsupported files are never read — and an ignored-but-unreadable file stays silently skipped rather than throwing. A lazy factory preserves this exactly.
- [x] 2.3 Reduce `FormatPhysicalFile(actual, original, warnForUnsupported, ct)` to a thin wrapper calling `FormatCore(actual, original, checkIgnored: true, warnForUnsupported, getInfo: CreateFromFileSystem, ct)`.
- [x] 2.4 Add public `FormatStandardInputFile(FileToFormatInfo, originalPath, checkIgnored, ct)` that calls `FormatCore(info.Path, originalPath, checkIgnored, warnForUnsupported: false, getInfo: () => info, ct)`.
- [x] 2.5 Rewrite `CommandLineFormatter.FormatStandardInput` to keep only path/extension inference, build the in-memory `FileToFormatInfo`, create the `OptionsProvider`, construct the `FormattingEngine` (StdOut writer + `NullCache`), and call `FormatStandardInputFile(info, original, checkIgnored: pathSupplied, ct)`; deleted the duplicated inline gate/logger/printer-options block (~45 lines).
- [x] 2.6 Run the full suite and confirm all green with no test modifications. Full `CSharpier.Tests`: 707 total, 705 passed, 2 pre-existing skips, 0 failed.

## 3. Resolve the stdin ancestor-walk (item 3)

- [x] 3.1 Remove the `while` walk-up loop in `FormatStandardInput`, passing the supplied path's direct (possibly non-existent) parent to `OptionsProvider.Create`; run the full suite. **Result: NOT equivalent** — removal broke 3 tests. `CSharpierConfigParser.FindForDirectoryName` calls `DirectoryInfo.EnumerateFiles`, which throws `DirectoryNotFoundException` for a non-existent directory (both `MockFileSystem` and the real FS), so the loop is load-bearing.
- [x] 3.2 If all tests (esp. 1.1–1.2) pass, keep the removal. If any fail, restore the loop, add a guard that stops at the filesystem root, and add a comment recording the concrete case that requires it. **Restored** the loop with a root-stop guard (`break` when `GetDirectoryName` returns null instead of throwing) and a comment explaining the `CSharpierConfigParser` enumeration requirement.
- [x] 3.3 Confirm the resolved behavior matches the restated `cli-formatting-pipeline` "Content piped with a supplied file path" scenario. Config/ignore resolve as if the file were at the supplied path (via the nearest existing ancestor); verified by 1.1/1.2 and the pre-existing non-existent-path test.

## 4. Prefix-safe path rewrite (item 5)

- [x] 4.1 Replace `file.Replace(directoryPath, originalPath)` in `FormattingEngine.FormatDirectory` with a prefix-only rewrite. Implemented as `originalPath + file[directoryPath.Length..]` — a length-based prefix strip, which reproduces today's exact display strings (including the leading separator) more faithfully than `GetRelativePath` would, satisfying the design's "exact display strings" risk. (`file` is always yielded under `directoryPath`, so the prefix is guaranteed.)
- [x] 4.2 Add a regression test formatting a directory whose name recurs as a nested path segment; assert the displayed/original path is rewritten only at the prefix. Added `Format_Rewrites_Only_The_Directory_Prefix_When_Segment_Recurs`. Note: the double-replace only manifests on non-drive-prefixed (Unix) paths — on Windows `c:\test` the drive prefix prevents recurrence — so this test fails on old code on Linux/CI and passes on both codes on Windows.
- [x] 4.3 Run the suite and confirm the common-case display paths are unchanged (existing tests green) and the new test passes. Full suite: 705 passed / 2 pre-existing skips / 0 failed.

## 5. Consistent fileSystem.Path usage (item 6)

- [x] 5.1 Replace static `Path.Combine`, `Path.IsPathRooted`, and `Path.DirectorySeparatorChar` in `CommandLineFormatter` with `fileSystem.Path.*` equivalents.
- [x] 5.2 Audit `CommandLineFormatter.cs` and `FormattingEngine.cs` for any remaining static `System.IO.Path` calls on run-time paths and convert them. Audited: `CommandLineFormatter` now uses `fileSystem.Path.*` throughout; `FormattingEngine` had no static `Path.*` calls.
- [x] 5.3 Run the suite and confirm green.

## 6. Verification

- [x] 6.1 Run `dotnet test` on `CommandLineFormatterTests` and the broader CLI suite; confirm all pass with no modifications to existing tests. Full `CSharpier.Tests` run: 707 total, 705 passed, 2 pre-existing skips, 0 failed; only new tests added, none modified.
- [x] 6.2 Run `openspec validate refine-command-line-formatter-followups --strict` and resolve any issues. Passes.
