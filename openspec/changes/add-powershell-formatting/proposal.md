## Why

CSharpier already formats two languages — C#/C# script and XML — through a shared, language-agnostic Doc/DocPrinter engine ported from Prettier. PowerShell (`.ps1`, `.psm1`, `.psd1`) is a common companion language in .NET repositories (build scripts, tooling, CI helpers, module manifests) and today has no widely-adopted, opinionated, zero-config formatter in the .NET ecosystem. Adding PowerShell support lets teams that already run CSharpier keep their scripts consistently formatted with the same tool, cache, editor integrations, and pre-commit hook they already use.

## What Changes

- Add a new `Formatter.PowerShell` variant and route `.ps1`, `.psm1`, and `.psd1` files to it via extension detection.
- Add a `PowerShellFormatter` that parses PowerShell source, builds a Prettier-style Doc tree with the existing Doc primitives, and prints it through the shared `DocPrinter`.
- Add a public `PowerShellFormatter.Format` / `FormatAsync` entry point mirroring `CSharpFormatter` and `XmlFormatter`, and surface it through `CodeFormatter.FormatAsync` dispatch.
- Add a `PowerShellFormattingValidator` so the CLI's post-format safety check can confirm the formatted output parses to an equivalent tree (parallel to `CSharpFormattingValidator` / `XmlFormattingValidator`).
- Add idempotency/formatting-sample tests and CLI integration coverage for the new file types.
- Scope note: the initial change targets a well-defined, common subset of PowerShell (the constructs found in typical scripts and module manifests) and formats anything it can parse; genuinely unparseable input is left untouched with a warning, matching the XML formatter's "invalid input is not formatted" behavior.

## Capabilities

### New Capabilities
- `powershell-formatting`: Detecting PowerShell files by extension, parsing them, re-printing them through the shared Doc engine with CSharpier's indentation/line-width rules, leaving unparseable input unchanged with a warning, and validating that formatting preserves program meaning.

### Modified Capabilities
<!-- No existing spec's REQUIREMENTS change. The CLI pipeline (cli-formatting-pipeline) already
     dispatches by Formatter and validates per language generically; PowerShell plugs into that
     existing behavior via a new formatter + validator branch without altering the pipeline's contract. -->

## Impact

- **New code**: `Src/CSharpier.Core/PowerShell/` (formatter, node printers, validator, and — depending on the design decision — a raw/token reader). New tests under `Src/CSharpier.Tests/PowerShell/` plus CLI integration cases.
- **Modified code**: `PrinterOptions.GetFormatter` (extension map) and the `Formatter` enum; `CodeFormatter.FormatAsync` (dispatch); `FormattingEngine.ValidateFormatting` (validator branch); `Src/CSharpier.Core/PublicAPI.Unshipped.txt` (new public entry point).
- **Dependencies**: The central open question is how to obtain a PowerShell parser/AST. Options — take a dependency on the PowerShell parsing surface (`System.Management.Automation`), or write a lightweight tokenizer/reader in the spirit of the XML `RawNodeReader`. This trade-off (binary size, licensing, cross-target-framework support, comment/trivia fidelity) is resolved in design.md.
- **Surfaces to update (secondary)**: Playground language handling, documentation (supported file types), and editor extension file-type lists.
- **No breaking changes**: existing C#/XML behavior is unaffected; PowerShell is purely additive.
