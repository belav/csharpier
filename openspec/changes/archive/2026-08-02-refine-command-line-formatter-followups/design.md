## Context

After `refactor-command-line-formatter`, the CLI formatting flow lives in two `internal` types: `CommandLineFormatter` (a thin orchestrator holding ambient run state) and `FormattingEngine` (per-path/per-file worker). That refactor was strictly behavior-preserving and explicitly deferred a handful of quirks. This change picks up four of them. All four are confined to `Src/CSharpier.Cli/CommandLineFormatter.cs` and `Src/CSharpier.Cli/FormattingEngine.cs`, with `CommandLineFormatterTests` as the behavior-preservation gate.

Relevant current facts established while scoping this change:
- `FormattingEngine.FormatDirectory` computes each file's display path via `file.Replace(directoryPath, originalPath)` — a **global substring** replace.
- `CommandLineFormatter.FormatStandardInput` re-implements the per-file setup (generated/ignore gate, `GetPrinterOptionsForAsync`, `Formatter` guard, `IncludeGenerated` assignment, `FileIssueLogger`, dispatch to `PerformFormattingSteps`) that `FormattingEngine.FormatPhysicalFile` already performs — the two differ only in that stdin sources contents in-memory and gates the ignore check on whether a path was supplied.
- `CommandLineFormatter.FormatStandardInput` walks up to the nearest existing ancestor directory before creating the `OptionsProvider`. Config/ignore resolution (`IgnoreFile.CreateAsync`, `CSharpierConfigParser.FindForDirectoryName`, `EditorConfigLocator`) already walk up from the supplied directory using `File.Exists`/`DirectoryInfo.Parent`, which tolerate non-existent directories — so the explicit loop appears redundant, but this was never verified.

## Goals / Non-Goals

**Goals:**
- Fix the fragile path rewrite so a directory name recurring as a nested segment cannot corrupt a displayed path.
- Use the injected `IFileSystem` consistently in `CommandLineFormatter`, removing the static `System.IO.Path` calls that bypass the test seam.
- Remove the duplicated per-file setup between the stdin and physical paths by giving `FormattingEngine` sole ownership of the per-file work.
- Empirically settle whether the stdin ancestor-walk is load-bearing, and either remove it or harden and document it — with the observable config/ignore outcome unchanged either way.

**Non-Goals:**
- No new CLI options, no public API changes, no changes to `CommandLineFormatterResult`, writers, cache, or validators.
- No concurrency changes — the unbounded `Task.WhenAll` fan-out remains the concern of `bound-formatting-concurrency`.
- No fixing of the other preserved quirks (`TODO log error?`, the `"/n"` typo in the validation message) — those are separate follow-ups.

## Decisions

### Decision 1: Prefix-only path rewrite (item 5)

`EnumerateNonignoredFiles` only ever yields files rooted under `directoryPath`, so the intended rewrite is "swap the known `directoryPath` prefix for `originalPath`". Replace the global `file.Replace(directoryPath, originalPath)` with a prefix-based computation using the file system's relative-path logic — compute the portion of `file` below `directoryPath` and recombine it onto `originalPath` via `fileSystem.Path`. The common-case output (no recurring segment) must be byte-identical to today, including path separators; a regression test formats a directory whose name recurs as a nested segment and asserts the displayed path is rewritten only at the prefix.

**Alternatives considered:** `string`-level `StartsWith` + `Substring` — works but re-implements separator handling the file-system abstraction already provides; prefer `fileSystem.Path` helpers for correctness and consistency with Decision 2.

### Decision 2: Consistent `fileSystem.Path` usage (item 6)

Replace the static `Path.Combine`, `Path.IsPathRooted`, and `Path.DirectorySeparatorChar` in `CommandLineFormatter` with the injected `fileSystem.Path.*` equivalents, and audit both files so no static `System.IO.Path` call on a run-time path remains. In production the real `IFileSystem` delegates to `System.IO`, so behavior is identical; the benefit is that tests running on a mock file system exercise the same paths. This is purely mechanical and independent of the other items.

### Decision 3: `FormattingEngine` owns per-file formatting; stdin routes through it (item 7)

Extract the shared per-file tail into a private `FormattingEngine` core that both entry points call once they hold a `FileToFormatInfo`:

- private `FormatCore(FileToFormatInfo, string originalPath, bool checkIgnored, bool warnForUnsupported, CancellationToken)` — the gate (skip when generated-and-not-included, or when `checkIgnored` and the path is ignored), then `GetPrinterOptionsForAsync`, the `Formatter is not Unknown` guard, `IncludeGenerated` assignment, `FileIssueLogger` construction, the check/format debug log, and `PerformFormattingSteps`. The unsupported-formatter branch emits the console warning only when `warnForUnsupported`.
- `FormatPhysicalFile(actual, original, warnForUnsupported, ct)` reduces to: build `FileToFormatInfo` from the file system, then `FormatCore(info, original, checkIgnored: true, warnForUnsupported, ct)`.
- new `FormatStandardInputFile(FileToFormatInfo, string originalPath, bool checkIgnored, CancellationToken)` calls `FormatCore(info, originalPath, checkIgnored, warnForUnsupported: false, ct)`. The stdin caller passes `checkIgnored: pathSupplied`, preserving today's "ignore rules apply only when a path was supplied" nuance.

`CommandLineFormatter.FormatStandardInput` then shrinks to: path/extension inference → build the in-memory `FileToFormatInfo` → create the `OptionsProvider` → construct the `FormattingEngine` (StdOut writer + `NullCache`) → call `FormatStandardInputFile`. The current inlined gate/logger/printer-options/engine block is removed.

The gate reads the file path from the `FileToFormatInfo` rather than a separate parameter; a task confirms `FileToFormatInfo` exposes its path (both `Create` and `CreateFromFileSystem` are constructed with one) before the extraction.

**Alternatives considered:** extracting only the shared gate and leaving stdin dispatch in `CommandLineFormatter` — smaller diff, but leaves the printer-options/`Formatter`-guard/logger/dispatch duplication and the two divergent copies that motivated the item. Chosen approach makes the engine the single owner of per-file work, matching how `FormatDirectory` already delegates.

### Decision 4: Investigate-then-resolve the stdin ancestor-walk (item 3)

Sequence the ancestor-walk work as a spike guarded by tests, executed **before** Decision 3 restructures `FormatStandardInput`:

1. Add characterization tests: stdin with `--stdin-path` whose parent directory does not exist on disk, asserting the resolved configuration and ignore outcomes (e.g. a `.csharpierrc` / ignore file at a real ancestor is honored). These pin the observable behavior independent of the mechanism.
2. Try removing the `while` walk-up loop — pass the supplied path's direct (possibly non-existent) parent to `OptionsProvider.Create` — and run the full suite. Because config/ignore resolution already walks up with existence guards, equivalence is the expected outcome.
3. Resolve on the evidence:
   - **Equivalent** → remove the loop; `FormatStandardInput` keeps only the "is the argument a directory" branch for filename inference.
   - **Not equivalent** → keep the loop, add a guard that stops at the filesystem root (so a pathological path cannot loop unbounded), and add a comment recording the concrete case that requires it.

Either outcome satisfies the restated spec scenario, which now describes the observable resolution rather than the walk-up mechanism.

## Risks / Trade-offs

- **Path-rewrite regression for the common case** → The prefix rewrite must reproduce today's exact display strings (separators included) when no segment recurs. Mitigation: keep the existing `CommandLineFormatterTests` green unmodified; add the recurring-segment test as the only new path assertion.
- **Behavior drift while unifying stdin and physical paths** → `FormatCore` must reproduce the stdin path's exact gating (ignore only when a path was supplied), the `warnForUnsupported: false` stdin behavior, and identical counter/short-circuit semantics. Mitigation: move logic verbatim into `FormatCore`, parameterize only the two genuine differences (`checkIgnored`, `warnForUnsupported`), and rely on the unmodified test suite plus the new stdin tests.
- **Removing the ancestor-walk changes an untested edge** → Mitigation: the characterization tests land first and gate the removal; if any fails, keep the loop (hardened) instead.
- **`fileSystem.Path` swap altering paths** → Extremely low; the real implementation delegates to `System.IO.Path`. Mitigation: the existing suite covers the affected stdin/physical path construction.

## Migration Plan

Not applicable — internal refactor, no runtime migration. Rollback is reverting the changed files. The single verification gate is `dotnet test` on `CommandLineFormatterTests` (and the broader CLI suite) passing, together with the two new tests (recurring-segment path rewrite; stdin resolution with a non-existent `--stdin-path` parent).

## Open Questions

- Item 3's resolution (remove vs. harden the loop) is decided by the spike's test evidence during implementation, not up front.
