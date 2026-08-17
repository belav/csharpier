## 1. Parser dependency spike (gating)

- [ ] 1.1 Add the `System.Management.Automation` reference to a throwaway/test target and confirm `Parser.ParseInput` returns a `ScriptBlockAst`, token stream, and `ParseError[]` for representative `.ps1`/`.psm1`/`.psd1` samples
- [ ] 1.2 Measure the packaged-size and cold-start delta the dependency adds to the CLI/global tool; record the number in design.md's Open Questions and decide full SDK vs parser-only vs separate reference
- [ ] 1.3 Confirm the dependency's supported target frameworks and settle the `netstandard2.0` strategy (compile PowerShell support out of that TFM vs target-conditional unit); write the decision into `CSharpier.Core.csproj` conditions

## 2. Formatter wiring (plug into the shared engine)

- [ ] 2.1 Add `PowerShell` to the `Formatter` enum in `Src/CSharpier.Core/PrinterOptions.cs`
- [ ] 2.2 Map `ps1`, `psm1`, `psd1` (case-insensitive) to `Formatter.PowerShell` in `PrinterOptions.GetFormatter`
- [ ] 2.3 Default the PowerShell indent size to 4 in `PrinterOptions` (parallel to the XML special-case)
- [ ] 2.4 Add a `Formatter.PowerShell` arm to `CodeFormatter.FormatAsync` in `Src/CSharpier.Core/CodeFormatter.cs`

## 3. PowerShell front-end

- [ ] 3.1 Create `Src/CSharpier.Core/PowerShell/PowerShellFormatter.cs` with public `Format` and `FormatAsync` entry points mirroring `XmlFormatter` (parse → build Doc → `DocPrinter.Print`), returning original source + warning on parse errors
- [ ] 3.2 Add the new public entry point(s) to `Src/CSharpier.Core/PublicAPI.Unshipped.txt`
- [ ] 3.3 Implement AST-walking node printers under `Src/CSharpier.Core/PowerShell/` for the core constructs (script blocks, pipelines, commands + parameters/arguments, assignments, if/switch/loops, function definitions, hashtables/arrays, param blocks)
- [ ] 3.4 Emit string literals, here-strings, and unhandled/unknown nodes verbatim from their source `Extent` so contents round-trip exactly
- [ ] 3.5 Reattach comments and comment-based help from the token stream by source offset (parallel to the C# trivia handling)
- [ ] 3.6 Apply end-of-line normalization and indent style/size via the existing `PrinterOptions`/`DocPrinter` path

## 4. Validation

- [ ] 4.1 Create `PowerShellFormattingValidator` implementing `IFormattingValidator`: re-parse the formatted output, assert no new `ParseError`s, and compare a trivia-independent normalization of input vs output `ScriptBlockAst` for structural equivalence
- [ ] 4.2 Add a `Formatter.PowerShell` branch to `FormattingEngine.ValidateFormatting` in `Src/CSharpier.Cli/FormattingEngine.cs`

## 5. Tests

- [ ] 5.1 Add a PowerShell subclass of the `FormattingTests` sample-pair harness under `Src/CSharpier.Tests/` and seed input/expected sample files for the core constructs
- [ ] 5.2 Add an idempotency corpus asserting that formatting already-formatted output is byte-identical
- [ ] 5.3 Add a test asserting unparseable PowerShell is returned unchanged with a warning and no mangled output
- [ ] 5.4 Add `PowerShellFormattingValidator` unit tests (equivalent output passes; a meaning-changing edit fails)
- [ ] 5.5 Add CLI integration coverage: `.ps1`/`.psm1`/`.psd1` files are discovered, formatted, `--check`-reported, and warned-on when unsupported (netstandard2.0 host)
- [ ] 5.6 Add comment / comment-based-help / here-string fidelity tests

## 6. Secondary surfaces (may split into a follow-up change)

- [ ] 6.1 Surface PowerShell in the Playground language handling
- [ ] 6.2 Update documentation to list the new supported file types
- [ ] 6.3 Update editor-extension file-type lists as needed
- [ ] 6.4 Decide and, if chosen, implement an experimental/opt-in gate for the first release

## 7. Verification

- [ ] 7.1 Run the full test suite and confirm C#/XML behavior is unchanged
- [ ] 7.2 Run `openspec validate add-powershell-formatting` and confirm the change is consistent
