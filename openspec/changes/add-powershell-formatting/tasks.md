## 1. Parser dependency (gating)

- [x] 1.1 Reference `System.Management.Automation` and confirm `Parser.ParseInput` returns AST, tokens, and errors
- [x] 1.2 Measure the packaged-size delta the dependency adds to the CLI tool and CSharpier.Core nupkg; record it in design.md
- [x] 1.3 Compile PowerShell support out of `netstandard2.0`

## 2. Formatter wiring

- [x] 2.1 Add `PowerShell` to the `Formatter` enum
- [x] 2.2 Map `ps1`, `psm1`, `psd1` in `PrinterOptions.GetFormatter`
- [x] 2.3 Default indent size 4
- [x] 2.4 `Formatter.PowerShell` arm in `CodeFormatter.FormatAsync`

## 3. Foundation (one agent, first)

- [x] 3.1 Unknown AST types print verbatim source instead of the .NET type name
- [x] 3.2 Comment attachment (leading/trailing/dangling) with the unprinted-comment verbatim fallback (design Decision 6)
- [x] 3.3 Leaf printers print `Extent.Text` (constants, strings, variables, types, command parameters)
- [x] 3.4 Generator orders derived AST types before base types in the `Node.Print` switch
- [x] 3.5 `PowerShellFormattingValidator` (design Decision 9) with unit tests
- [x] 3.6 `FormattingEngine.ValidateFormatting` branch for PowerShell
- [x] 3.7 Test harness: use the PowerShell validator for the `powershell` folder and assert a second format is unchanged
- [x] 3.8 Fix `Random.test`; remove `TODO 1894` items that this phase resolves
- [x] 3.9 Line-ending and tab handling for multi-line comments and verbatim text

## 4. Printers (sequential agents, each adds test files per construct)

- [x] 4.1 Statements: named blocks, `if`, `switch`, `for`, `foreach` (incl. `-parallel`), `while`, `do/while`, `do/until`, `try/catch [types]/finally`, `trap`, `break`/`continue` with labels, `return`, `throw`, `exit`, loop labels, `data`, `using`, blank-line and `;` handling
- [x] 4.2 Pipelines and commands: `|` breaking, `&&`/`||` chains, background `&`, invocation operators `&`/`.`, command parameters (`-Name:value`), redirections, splatting, `--%`, backtick-continued arguments
- [x] 4.3 Expressions: binary, unary, ternary, casts, attributed expressions, member access and method calls (static, instance, null-conditional, generic), index, array literal, `@()`, `$()`, parens, hashtable, script block expression, `$using:`, expandable strings, assignments with all operators
- [x] 4.4 Declarations: `param` blocks, parameters with attributes and defaults, attributes with named arguments, functions/filters (both parameter forms), classes, enums, properties, methods, constructors with `: base()`
- [x] 4.5 Explicit verbatim printers for `configuration`, dynamic keywords, workflow block statements, error nodes

## 5. Real-code testing

- [x] 5.1 Download the public repositories listed in design Decision 11 outside the repo and report AST node frequency
- [x] 5.2 Format the set with the dev build: zero exceptions, zero validation failures, second run reports no changes
- [x] 5.3 Read a sample of formatted output for ugly results and fix the printers; add a test file for each fix

## 6. CLI, docs, editors

- [x] 6.1 CLI tests: `.ps1`/`.psm1`/`.psd1` discovered, formatted, `check` reports, parse errors reported, validation failure reported
- [x] 6.2 Docs: supported file types, configuration notes
- [x] 6.3 Editor extensions (VS Code, Rider, Visual Studio): add PowerShell file types where each extension lists them
- [x] 6.4 Playground

## 7. Verification

- [x] 7.1 Full test suite passes; C#/XML output unchanged
- [x] 7.2 `dotnet csharpier format .` leaves the repo formatted
- [x] 7.3 `openspec validate add-powershell-formatting`

## 8. Findings from the downloaded repositories

- [x] 8.1 (5.1) 4,535 files from 8 repositories downloaded; all 59 reachable AST types occur except `BaseCtorInvokeMemberExpressionAst`, `BlockStatementAst`, and the two error node types
- [x] 8.2 Format files whose only parse errors are resolution errors (`ResourceNotDefined`, `ModuleNotFoundDuringParse`, `TypeNotFound`, `RequiresModuleInvalid`, `WorkflowNotSupportedInPowerShellCore`, `DscResourceMissingSetMethod`); 170 of the downloaded files hit these because a module or type is not installed on the machine
- [x] 8.3 Hand-written tests for constructs the downloaded files lack: `--%`, `: base(...)` constructors, mixed line endings, `&&`/`||`, `data`, `trap`, attributed expressions
- [x] 8.4 Confirm UTF-16 and BOM files round-trip through the CLI (15 UTF-16 files, 1,064 UTF-8 BOM files in the downloaded set)

## 9. Review: blocking fixes

Source: review report from 2026-11-12 and the Copilot review on PR 1894.

- [x] 9.1 Non-UTF-8 PowerShell files without a BOM are left unchanged with a warning
- [x] 9.2 Comment among catch types no longer prints `catch catch`
- [x] 9.3 Background `&` kept when an ancestor falls back to verbatim
- [x] 9.4 `--%` followed by `|` is stable and keeps the argument text
- [x] 9.5 Function names (and other names) print from source text
- [x] 9.6 Blank lines that decide script help vs function help are kept
- [x] 9.7 `data` header comment is not duplicated
- [x] 9.8 Depth guard in the printer, validator, and `AstSyntaxWriter`
- [x] 9.9 PowerShell files that fail validation are not written
- [x] 9.10 (Copilot) `PropertyMember` does not pick a variable name out of a comment
- [x] 9.11 (Copilot) `SwitchStatement` does not read flags out of a comment; audit other raw-text scans
- [x] 9.12 (Copilot) `AstSyntaxWriter` JSON `MaxDepth` and tuple serialization
- [x] 9.13 Playground returns diagnostics, not HTTP 500, on PowerShell syntax errors

## 10. Review: correctness and robustness

- [ ] 10.1 Add `ErrorLoadingAssembly` to the non-blocking parse errors so `using assembly System.Windows.Forms` does not stop formatting
- [ ] 10.2 `CommentAttacher.Attach` is quadratic (20,000 commented statements: 15.9 s). Use a binary search over the sorted children or one merged walk; stop re-sorting in `GetStatementsAndTraps`
- [ ] 10.3 Not idempotent: array argument containing a block comment (`ArrayLiteral.cs:17-31` picks the separator from source layout)
- [ ] 10.4 Verbatim fallback covers too much for common comment positions: comment or help between a function name and `{`; `switch ($a) { # c`; `try { } # c` before `catch`; `catch { } # c` before `finally`; `$a.b( # c`; comment inside `: base(...)`
- [ ] 10.5 DSC warning path: `FormattingEngine.cs:349-353` returns before `WriteResult`, so stdin and `pipe-files` output is empty on Linux/macOS (from code reading; confirm on Linux)
- [ ] 10.6 `csharpier server` drops the DSC warning (`CSharpierServiceImplementation.cs:85-104` reads `WarningMessage` only when `Code` is empty)
- [ ] 10.7 Run the validator for PowerShell in server and pipe modes
- [ ] 10.8 Signed scripts (`# SIG # Begin signature block`): skip with a warning or document that formatting invalidates the signature
- [ ] 10.9 Line endings inside here-strings and multi-line strings are rewritten to the configured end of line; decide whether to keep them, and correct `docs/Configuration.md`, which says here-string contents are left as written
- [ ] 10.10 Input that overflows the stack inside the PowerShell parser itself (20,000-term `1 + 1 + ...`)
- [ ] 10.11 Pass the cancellation token to `PowerShellFormatter.FormatAsync`
- [ ] 10.12 Replace the `"DscClassCache"` message match in `PowerShellParseErrors.IsDscUnavailable` with something less brittle

## 11. Review: decisions needed (packaging and rollout)

- [ ] 11.1 Tool package size: 28.4 MB nupkg, 86.1 MB installed; 14.9 MB of the nupkg is `System.Management.Automation` and its 19 packages. Options: accept, trim the runtime folders, or ship PowerShell as a separate package
- [ ] 11.2 `CSharpier.Core` gains the `System.Management.Automation` dependency with no public PowerShell API. Expose an API, or move PowerShell into an assembly only the CLI references
- [ ] 11.3 Existing `csharpier check .` runs and CSharpier.MsBuild start picking up `.ps1/.psm1/.psd1`. Opt-in for one release, or an upgrade note with a `.csharpierignore` example and a CHANGELOG entry
- [ ] 11.4 VS Code: registering a formatter for `powershell` conflicts with the PowerShell extension and activates CSharpier in PowerShell-only folders. Decide whether to register, gate on CLI version, or add a setting
- [ ] 11.5 Rider and Visual Studio: format-on-save now covers PowerShell with no per-language switch; neither extension was built. Confirm the language id `"PowerShell"`, the Rider 2,000 ms timeout against a cold parser load, and whether Visual Studio needs a VSCT placement
- [ ] 11.6 `csharpier-ignore` comments for PowerShell (range ignore at least)
- [ ] 11.7 Parsing `configuration` creates an `InitialSessionState` (330 ms against 90 ms per file) and output for DSC files differs by OS
- [ ] 11.8 Not verified: whether parsing `using module` or DSC can run code from the repository being formatted; whether `System.Management.Automation` sends telemetry when used as a parser; trimming/AOT
- [ ] 11.9 Read inheritance from compilation symbols in the generator instead of the hand-kept `BaseTypes` table (`PowershellNodeGenerator.cs:79-112`); `StringConstantExpression` in the table has no printer

## 12. Review: formatting style

The reviewer would fix 12.1-12.3 before release. Examples for each are in the review report.

- [ ] 12.1 Double indent after a binary operator or comma when the operand has its own delimiters (`-f @(`, `task Name Dep, {`, `-Property @{...}, @{...}`); expected unchanged
- [ ] 12.2 Bare array as the right operand inside `if (`: first element at one indent, the rest at +4
- [ ] 12.3 Breaks before a short right operand of a comparison and inside index brackets; only `-and`/`-or`/`+` chains should be break points, never break inside `[ ]`
- [ ] 12.4 One-line `if`/`else` always expands (`$x = if ($a) { 1 } else { 2 }` becomes 5 lines; `if ($x) { return }` becomes 3)
- [ ] 12.5 A single argument is broken even when the command line stays over the width
- [ ] 12.6 Arrays always go one element per line (byte tables, `CmdletsToExport` lists, `-f $a, $b, $c`); consider filling
- [ ] 12.7 One-line hashtable rows in multi-line `@( )` test-case tables expand
- [ ] 12.8 Pipelines have three layouts; a pipeline inside `( )` continues at the outer pipeline's indent
- [ ] 12.9 .NET calls hug whichever argument is last; method chains are never broken; `[ValidateScript({ ... })]` is not hugged
- [ ] 12.10 Param blocks: blank line forced between all parameters once one has an attribute; `[type]` on its own line is joined to `[type]$Name`
- [ ] 12.11 Line-tied comments move off their line (`# cspell: disable-line` before `{`, `if (...) # note`, comments around `|` and binary operators, comment between the last attribute and `param(`)
- [ ] 12.12 Block comment bodies keep tabs when the `<#` line is re-indented with spaces
- [ ] 12.13 Smaller choices: switch flags and `data -supportedCommand` keep their case while keywords are lowercased; hashtable `=` alignment removed (document it); redirection order changes on expressions; `static hidden` becomes `hidden static`; attribute/type order on properties; DSC blocks fully verbatim; named blocks expand inside a function but not at script level
- [ ] 12.14 `for ($i = 0; $i -lt $n)` gains a trailing `;`

## 13. Review: tests

- [ ] 13.1 `Should_Format_When_A_Dsc_Resource_Cannot_Be_Resolved` passes whether DSC parsed or the warning path ran; make it assert one or the other per platform
- [ ] 13.2 Missing tests: the DSC warning through stdin/pipe/server; `.csharpierignore` for `.ps1`; `printWidth` other than 100; the Playground controller
- [ ] 13.3 Run the real-code check (format, validate, re-check) and a comment fuzzer in CI
- [ ] 13.4 `PowerShellPrinterCoverageTests` only checks a class name exists
- [ ] 13.5 `UsingStatement.test` depends on `MyModule`, `./Local.psm1`, and `Foo` not resolving on the test machine
- [ ] 13.6 Run the formatting sample tests on Linux (only the non-file-writing PowerShell tests were run in a container)

## 14. Review: minor

- [ ] 14.1 Move the unrelated C# style edits (`Token.cs`, `TupleExpression.cs`, `CSharpFormatter.cs`, `StringDoc.cs`, `DocListBuilder.cs`, `ConsoleLogger.cs`, `GlobMatcher.cs`, `NodePrinterGenerator.sbntxt`) to their own commit
- [ ] 14.2 `PowershellNodeGenerator` naming vs `PowerShell` elsewhere; it duplicates `GetContent` from `NodePrinterGenerator`
- [ ] 14.3 `FunctionMember.cs:4-10` walks the method with `FindAll`; `node.Body.Parent` is the node
- [ ] 14.4 `Pipeline.cs:39` index math depends on exactly five `Add` calls per iteration
- [ ] 14.5 `ErrorStatement.cs` and `ErrorExpression.cs` cannot be reached
- [ ] 14.6 Validator: text comparison past depth 500 gives false failures; `catch { continue; }` on property getters hides errors
- [ ] 14.7 Playground "Show Doc" is empty for PowerShell (`DocTree` never set)
- [ ] 14.8 Docs: `Configuration.md` says top-level options apply to `**/*.{cs,csx}` and implies a public PowerShell API; `Nuget/README.md`, `Nuget/Build.props` description, `Src/Website/src/pages/index.js`, and the pre-commit sample are not updated; `design.md` still mentions 7.4.6; package sizes not re-measured on 7.6.10
- [ ] 14.9 Decide whether `openspec/changes/add-powershell-formatting/` belongs in the PR
