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

- [x] 10.1 Add `ErrorLoadingAssembly` to the non-blocking parse errors so `using assembly System.Windows.Forms` does not stop formatting
- [x] 10.2 `CommentAttacher.Attach` is quadratic (20,000 commented statements: 15.9 s). Use a binary search over the sorted children or one merged walk; stop re-sorting in `GetStatementsAndTraps`
- [x] 10.3 Not idempotent: array argument containing a block comment (`ArrayLiteral.cs:17-31` picks the separator from source layout)
- [x] 10.4 Verbatim fallback covers too much for common comment positions: comment or help between a function name and `{`; `switch ($a) { # c`; `try { } # c` before `catch`; `catch { } # c` before `finally`; `$a.b( # c`; comment inside `: base(...)`
- [x] 10.5 DSC warning path: `FormattingEngine.cs:349-353` returns before `WriteResult`, so stdin and `pipe-files` output is empty on Linux/macOS (from code reading; confirm on Linux)
- [x] 10.6 `csharpier server` drops the DSC warning (`CSharpierServiceImplementation.cs:85-104` reads `WarningMessage` only when `Code` is empty)
- [x] 10.7 Run the validator for PowerShell in server and pipe modes
- [x] 10.8 Signed scripts (`# SIG # Begin signature block`): skip with a warning or document that formatting invalidates the signature. Decision: left unchanged with a warning (design Decision 13)
- [x] 10.9 Line endings inside here-strings and multi-line strings are rewritten to the configured end of line; decide whether to keep them, and correct `docs/Configuration.md`, which says here-string contents are left as written. Decision: keep, as for C# verbatim and raw strings; docs corrected
- [x] 10.10 Input that overflows the stack inside the PowerShell parser itself (20,000-term `1 + 1 + ...`). All code now runs on the large-stack thread (design Decision 14)
- [x] 10.11 Pass the cancellation token to `PowerShellFormatter.FormatAsync`
- [x] 10.12 Replace the `"DscClassCache"` message match in `PowerShellParseErrors.IsDscUnavailable` with something less brittle

## 11. Review: decisions needed (packaging and rollout)

- [x] 11.1 Tool package size: 28.4 MB nupkg, 86.1 MB installed; 14.9 MB of the nupkg is `System.Management.Automation` and its 19 packages. Options: accept, trim the runtime folders, or ship PowerShell as a separate package. Decision: accept, covered by the upgrade note
- [x] 11.2 `CSharpier.Core` gains the `System.Management.Automation` dependency with no public PowerShell API. Expose an API, or move PowerShell into an assembly only the CLI references. Decision: leave as is, covered by the upgrade note
- [x] 11.3 Existing `csharpier check .` runs and CSharpier.MsBuild start picking up `.ps1/.psm1/.psd1`. Opt-in for one release, or an upgrade note with a `.csharpierignore` example and a CHANGELOG entry. Decision: upgrade note, no opt-in
- [ ] 11.4 VS Code: registering a formatter for `powershell` conflicts with the PowerShell extension and activates CSharpier in PowerShell-only folders. Decide whether to register, gate on CLI version, or add a setting
- [ ] 11.5 Rider and Visual Studio: format-on-save now covers PowerShell with no per-language switch; neither extension was built. Confirm the language id `"PowerShell"`, the Rider 2,000 ms timeout against a cold parser load, and whether Visual Studio needs a VSCT placement
- [x] 11.6 `csharpier-ignore` comments for PowerShell (range ignore at least)
- [x] 11.7 Parsing `configuration` creates an `InitialSessionState` (330 ms against 90 ms per file) and output for DSC files differs by OS. Decision: keep formatting these files where DSC is installed; the OS difference and the cost are documented in `Configuration.md`
- [x] 11.8 Whether parsing can run code from the repository being formatted, and whether `System.Management.Automation` sends telemetry (trimming/AOT moved to 15.20). Tested on Windows with 7.6.10: formatting scripts that reference a repo-local `.psm1`, `.psd1` (with `ScriptsToProcess`, `NestedModules`, `RequiredAssemblies`) or `.dll` through `using module`, `using assembly`, `#requires` and `Import-DscResource` ran no code from those files (the same files run code when imported in `pwsh`); no network activity in 45 s after parsing, though `Microsoft.ApplicationInsights` is loaded when a file has `using module` or `configuration`. Not tested on Linux/macOS or against modules installed on `PSModulePath`
- [x] 11.9 Read inheritance from compilation symbols in the generator instead of the hand-kept `BaseTypes` table (`PowershellNodeGenerator.cs:79-112`); `StringConstantExpression` in the table has no printer. Done: depth is read from the `System.Management.Automation.Language` type symbols

## 12. Review: formatting style

The reviewer would fix 12.1-12.3 before release. Examples for each are in the review report.

- [x] 12.1 Double indent after a binary operator or comma when the operand has its own delimiters (`-f @(`, `task Name Dep, {`, `-Property @{...}, @{...}`); expected unchanged
- [x] 12.2 Bare array as the right operand inside `if (`: first element at one indent, the rest at +4
- [x] 12.3 Breaks before a short right operand of a comparison and inside index brackets; only `-and`/`-or`/`+` chains should be break points, never break inside `[ ]`. Done: only `-and`/`-or`/`-xor`, `+`/`-`, and repeated-operator chains break; nothing breaks inside `[ ]`; assignments break after `=` when the right side cannot break
- [x] 12.4 One-line `if`/`else` always expands (`$x = if ($a) { 1 } else { 2 }` becomes 5 lines; `if ($x) { return }` becomes 3). Decision: keep expanding these, as C# CSharpier and Prettier do
- [ ] 12.5 A single argument is broken even when the command line stays over the width
- [x] 12.6 Arrays always go one element per line (byte tables, `CmdletsToExport` lists, `-f $a, $b, $c`); consider filling
- [ ] 12.7 One-line hashtable rows in multi-line `@( )` test-case tables expand
- [ ] 12.8 Pipelines have three layouts; a pipeline inside `( )` continues at the outer pipeline's indent
- [ ] 12.9 .NET calls hug whichever argument is last; method chains are never broken; `[ValidateScript({ ... })]` is not hugged
- [ ] 12.10 Param blocks: blank line forced between all parameters once one has an attribute; `[type]` on its own line is joined to `[type]$Name`
- [ ] 12.11 Line-tied comments move off their line (`# cspell: disable-line` before `{`, `if (...) # note`, comments around `|` and binary operators, comment between the last attribute and `param(`)
- [ ] 12.12 Block comment bodies keep tabs when the `<#` line is re-indented with spaces
- [ ] 12.13 Smaller choices: switch flags and `data -supportedCommand` keep their case while keywords are lowercased; hashtable `=` alignment removed (document it); redirection order changes on expressions; `static hidden` becomes `hidden static`; attribute/type order on properties; DSC blocks fully verbatim; named blocks expand inside a function but not at script level
- [ ] 12.14 `for ($i = 0; $i -lt $n)` gains a trailing `;`
- [ ] 12.15 Add and remove backtick continuations in commands. Today `Command.cs` keeps a line break between two arguments only where the source had one (written as `` ` `` and a new line), never adds one, and never joins one, so a command over the print width stays on one line and a short command split with backticks stays split. Break a command that does not fit with one argument (or parameter and its value) per line after a backtick, and join a backtick-split command that fits. A backtick followed by whitespace is not a continuation, so the output must never leave whitespace after a backtick. Related: 12.5

## 13. Review: tests

- [x] 13.1 `Should_Format_When_A_Dsc_Resource_Cannot_Be_Resolved` passes whether DSC parsed or the warning path ran; make it assert one or the other per platform. Decision: leave the test as is
- [x] 13.2 Missing tests: `.csharpierignore` for `.ps1`; `printWidth` other than 100; the Playground controller. (The DSC warning through stdin/pipe/server now has in-process tests; see 15.18 for the process-level gap)
- [x] 13.4 `PowerShellPrinterCoverageTests` only checks a class name exists
- [x] 13.5 `UsingStatement.test` depends on `MyModule`, `./Local.psm1`, and `Foo` not resolving on the test machine
- [x] 13.6 Run the formatting sample tests on Linux (only the non-file-writing PowerShell tests were run in a container)

## 14. Review: minor

- [x] 14.2 `PowershellNodeGenerator` naming vs `PowerShell` elsewhere; it duplicates `GetContent` from `NodePrinterGenerator`
- [x] 14.3 `FunctionMember.cs:4-10` walks the method with `FindAll`; `node.Body.Parent` is the node
- [x] 14.4 `Pipeline.cs:39` index math depends on exactly five `Add` calls per iteration
- [x] 14.5 `ErrorStatement.cs` and `ErrorExpression.cs` cannot be reached. Decision: keep both; they state what happens to error nodes and cost nothing
- [x] 14.6 Validator: text comparison past depth 500 gives false failures; `catch { continue; }` on property getters hides errors. Done: the tree comparison is a loop with an explicit stack, so there is no depth limit or text fallback; property getters are no longer wrapped in `catch { continue; }` (none threw on the 4,538 downloaded files)
- [x] 14.7 Playground "Show Doc" is empty for PowerShell (`DocTree` never set)
- [x] 14.8 Docs: `Configuration.md` says top-level options apply to `**/*.{cs,csx}` and implies a public PowerShell API; `Nuget/README.md`, `Nuget/Build.props` description, `Src/Website/src/pages/index.js`, and the pre-commit sample are not updated; `design.md` still mentions 7.4.6; package sizes not re-measured on 7.6.10. Done; sizes re-measured on 1.4.1 with 7.6.10
- [x] 14.9 Decide whether `openspec/changes/add-powershell-formatting/` belongs in the PR

## 15. Review: follow-ups from the section 10 fixes

- [ ] 15.1 Write the upgrade note decided in 11.1-11.3: PowerShell files are now formatted and checked, a `.csharpierignore` example to exclude them, the package size increase, the new `CSharpier.Core` dependency, and a CHANGELOG entry
- [ ] 15.2 Test seams in product code: `PowerShellFormatter.SimulateDscUnavailable` and `SimulatedPrinterOutput` (`AsyncLocal`, set only by tests). Keep, or replace with an injected parser/printer
- [ ] 15.3 Review `CommandLineOptions.AlwaysValidatePowerShell` (set by `pipe-files`; `--skip-validation` still wins) and the server running the PowerShell validator on every format
- [ ] 15.4 Behaviour change: plain stdin writes the original code on a PowerShell validation failure (it wrote nothing before). Confirm this is wanted and whether C# should match
- [ ] 15.5 Behaviour change: the server returns `Failed` (not `Formatted` with the original text) for PowerShell deep recursion, the DSC warning, and signed scripts. Confirm each editor shows or logs the message acceptably
- [ ] 15.6 Deeply nested PowerShell under 20,000 characters now reports the depth message and the CLI exits 0, where it reported the parser's "too complex" syntax error with exit 1. Decide the exit code
- [ ] 15.7 `Should_Not_Overflow_A_Small_Stack_Of_The_Calling_Thread` kills the test process on regression instead of failing one test. Keep, or run the probe in a child process
- [ ] 15.8 Every PowerShell format now runs on its own large-stack thread (about 0.08 ms per file, 16 MB minimum reserve). Confirm this is acceptable for the server and for parallel file formatting; the 1 GB cap is untested
- [ ] 15.9 `SortedChildren` in `CommentAttacher.cs` handles siblings whose end offsets are out of start order, and no test input has such siblings. Add a test or remove the handling
- [ ] 15.10 `Should_Attach_Many_Comments_In_Linear_Time` asserts under 10 s for 50,000 comments; measured on one machine only. Confirm it is not flaky in CI
- [ ] 15.11 `PowerShellParseErrors.IsDscUnavailable` now matches a `ParserError` located at the first token after `configuration`. Any other exception thrown by the configuration rule gets the DSC warning wording
- [ ] 15.12 Signed scripts are skipped with a warning. The parser's own signature test is looser (ignores case, extra spaces, trailing text); decide whether to match it. No server test for this path
- [ ] 15.13 Changing line endings inside here-strings and multi-line strings changes the string's runtime value. Decided to keep (matches C#); confirm, and note a lone `\r` is kept as written
- [ ] 15.14 Comment style choices from 10.4: a line comment in a function header keeps `{` on its own line; `$a.b(<# c #> 1)` takes three lines; `function f($a) # c` no longer breaks the parameter list; `switch ($a) { # c` moves the comment inside the braces
- [ ] 15.15 Comment positions that still print verbatim: next to an assignment `=` (also dbatools `Invoke-DbaQuery.ps1:507`), after `.` or `::`, hashtable `key <# c #> = value`, inside a `foreach` header, `return <# c #> value`, inside attribute brackets. Overlaps 12.11
- [ ] 15.16 `--stdin-path Script.ps1` with no directory part throws "The path is empty" in `IgnoreFile.FindIgnorePaths`; `./Script.ps1` works. Check whether this also happens for `.cs` on `main`
- [ ] 15.17 `format --write-stdout` on a non-UTF-8 PowerShell file writes nothing and sends its warning to stdout
- [ ] 15.18 Not automated: the DSC warning path on Linux was checked by hand in a container (stdin, pipe-files, server). No process-level test in `CliTests`/`ServerTests`; macOS not tried
- [ ] 15.19 `ErrorLoadingAssembly` was confirmed on Windows only. `CannotLoadAssemblyFromUncPath` and `CannotLoadAssemblyWithUriSchema` are still blocking
- [ ] 15.20 Not verified: trimming/AOT with `System.Management.Automation` in `CSharpier.Core` (relevant to the `nativeaot` branch). Publish the CLI with `PublishAot=true` and format a PowerShell file with the result
