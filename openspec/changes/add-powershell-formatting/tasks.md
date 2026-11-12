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
