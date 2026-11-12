## Context

CSharpier is built around a language-agnostic printing engine ported from Prettier: a source language is parsed, walked to build a **Doc** tree out of the primitives in `Src/CSharpier.Core/DocTypes/` (`Group`, `IndentDoc`, `LineDoc`, `IfBreak`, `HardLine`, `StringDoc`, …), and that tree is rendered by the shared `DocPrinter` (`Src/CSharpier.Core/DocPrinter/`) which owns width, indentation, and line-break decisions. Two front-ends already sit on this engine:

- **C#** (`CSharp/CSharpFormatter.cs`) — parses with Roslyn, walks `SyntaxNode`s, reattaches comments/trivia, and validates output with `SyntaxNodeComparer`.
- **XML** (`Xml/XmlFormatter.cs`) — deliberately avoids the `System.Xml` DOM for printing and instead uses a hand-written `RawNodeReader` to preserve fidelity, using `System.Xml`'s `XmlReader` only to *validate* that the input is well-formed.

Adding a language is a known, bounded shape. The wiring touch points, all confirmed in the current codebase, are:

1. `Formatter` enum + extension map in `Core/PrinterOptions.cs` (`GetFormatter`).
2. Dispatch arm in `Core/CodeFormatter.cs` (`FormatAsync`).
3. A public `*.Format` / `FormatAsync` entry point (parallel to `CSharpFormatter` / `XmlFormatter`), plus an entry in `Core/PublicAPI.Unshipped.txt`.
4. A validator branch in `Cli/FormattingEngine.cs` (`ValidateFormatting`).
5. Tests under `Src/CSharpier.Tests/` using the existing `FormattingTests` sample-pair harness.

The novel, hard part for PowerShell is **not** the wiring — it is obtaining a faithful parse and re-printing a large, context-sensitive grammar well.

## Goals / Non-Goals

**Goals:**
- Format `.ps1`, `.psm1`, `.psd1` through the existing Doc engine with the same options (width, indent style/size, end-of-line) as other languages.
- Follow the established plug-in pattern so PowerShell is additive and the C#/XML paths are untouched.
- Be safe by default: parse → build Doc → print; leave unparseable input unchanged with a warning; validate that formatting preserves program meaning before writing.
- Cover the constructs found in typical scripts and module manifests, preserving comments, comment-based help, and here-strings verbatim.

**Non-Goals:**
- 100% coverage of every PowerShell construct (DSC, dynamic keywords, exotic edge cases) in the first release.
- New user-facing style options — CSharpier follows Prettier's option philosophy; PowerShell inherits the existing options only.
- Semantic rewriting (alias expansion, casing normalization of cmdlet names, quote-style changes beyond what safe re-printing requires) — these are linting concerns, not formatting.
- Reformatting the *inside* of here-strings or embedded here-doc content.

## Decisions

### Decision 1: Parse with `System.Management.Automation`'s `Parser` — do not hand-roll a parser

PowerShell ships an authoritative, MIT-licensed parser: `System.Management.Automation.Language.Parser.ParseInput(text, out Token[] tokens, out ParseError[] errors)`, which returns a `ScriptBlockAst` **and** the full token stream. This is the same engine PowerShell itself and PSScriptAnalyzer use.

- **Why over a hand-rolled reader (the XML approach):** XML's grammar is small enough that `RawNodeReader` is tractable. PowerShell's grammar (pipelines, script blocks, splatting, subexpressions, format/redirection operators, backtick continuation, here-strings, expandable-string subexpressions, statement-terminating newlines) is not — re-implementing it would be a parser project of its own and a permanent correctness liability. Reuse the real one.
- **Comment/trivia handling:** PowerShell's AST does not contain comments; they arrive as `Token`s of kind `Comment` with source `Extent`s. This mirrors Roslyn trivia reattachment that the C# front-end already does — walk the AST for structure, and splice comments back in by offset from the token stream.
- **Verbatim spans:** string literals, here-strings, and command-argument text are emitted from their source `Extent` (offset range into the original text), not reconstructed, so their contents round-trip exactly.

**Alternatives considered:** (a) hand-rolled tokenizer/reader — rejected, see above; (b) a third-party managed PowerShell grammar — none is authoritative or maintained enough to trust for round-tripping; (c) shelling out to `pwsh` — introduces a runtime dependency and process cost, unacceptable for a bundled formatter.

### Decision 2: Isolate the dependency and target frameworks carefully

`CSharpier.Core` currently multi-targets `net8.0;net9.0;net10.0;netstandard2.0`. The `System.Management.Automation` reference package supports the modern .NET targets but **not** `netstandard2.0`. This is the single biggest structural constraint.

Approach:
- Add the PowerShell parser reference only for the modern TFMs, and either (a) compile the PowerShell front-end out of the `netstandard2.0` build (returning "unsupported on this target" for `.ps1` there), or (b) move the PowerShell front-end into a target-conditional compilation unit. The netstandard2.0 target exists for embedding scenarios; PowerShell support degrading there is acceptable and must be explicit.
- Keep all `System.Management.Automation` types behind the `PowerShell/` namespace so no other part of Core (or consumers who only format C#/XML) references them.
- **Binary size:** Core already carries Roslyn, so it is not a lightweight assembly, but the PowerShell SDK surface is large. Measure the packaged-size delta early; if it is unacceptable for the global tool, fall back to shipping PowerShell support as a separately-referenced piece. This measurement gates the approach and is tracked as an open question.

### Decision 3: Mirror the XML wiring exactly

Add `Formatter.PowerShell`; map `ps1`/`psm1`/`psd1` in `GetFormatter`; add a `Formatter.PowerShell` arm in `CodeFormatter.FormatAsync`; add `PowerShellFormatter.Format`/`FormatAsync` with the same signature shape as `XmlFormatter`; register the public entry in `PublicAPI.Unshipped.txt`. Default indent size is 4 (PowerShell convention), unlike XML's 2 — set via the same `PrinterOptions` mechanism that already special-cases XML.

### Decision 4: Validate by re-parse and structural comparison

Add `PowerShellFormattingValidator` (implementing the existing `IFormattingValidator`) and a `Formatter.PowerShell` branch in `FormattingEngine.ValidateFormatting`. Start with the lighter `XmlFormattingValidator` shape: re-parse the formatted output, assert it has no new `ParseError`s, and compare a trivia-independent normalization of the two `ScriptBlockAst`s for structural equivalence. This catches the catastrophic failure mode (formatting changed what the script *does*) without requiring a full C#-style node comparer up front.

### Decision 5: Scope the first release to a parseable subset, format-what-you-can

Everything that parses gets formatted; anything with parse errors is returned unchanged with a warning, exactly like `XmlFormatter`'s invalid-input path. Within parseable input, cover the common statement/expression/pipeline constructs first and expand node-printer coverage incrementally, with any unhandled node falling back to verbatim source-extent emission so output is never corrupted — only sub-optimally formatted.

## Risks / Trade-offs

- **netstandard2.0 cannot carry the parser** → Compile PowerShell support out of that target and document that `.ps1` formatting requires a modern-.NET host; the CLI/tool ships on modern .NET so end users are unaffected.
- **PowerShell SDK binary-size / cold-start cost** → Measure the packaged delta before committing; if unacceptable, ship the PowerShell front-end behind a separate reference rather than folding it into the default Core package.
- **Context-sensitive newlines and continuation** (statement-terminating newlines, backtick continuation, `|` at line start/end, splatting) are where most formatting bugs will live → Lean on source `Extent`s for anything ambiguous, add a large idempotency corpus, and treat "output re-parses to an equivalent AST" as a hard gate in tests.
- **Comment/here-string fidelity** → Reattach comments by token extent and emit here-strings/string literals verbatim from spans; cover with targeted sample pairs.
- **Formatting-quality maturity at first release** → Consider gating behind an experimental/opt-in signal initially (as CSharpier has done for maturing features) so early adopters opt in while the node-printer coverage broadens.

## Status after the POC (2026-11-11)

Done: `Formatter.PowerShell` enum + extension map, `CodeFormatter` dispatch, `System.Management.Automation` 7.4.6 referenced for non-netstandard2.0 targets with `PowerShell/**` compiled out of netstandard2.0, `PowerShellFormatter.FormatAsync` (parse errors returned as diagnostics, source unchanged), a source generator that builds `Node.Print` from the file names in `PowerShell/AstPrinters/`, printers for 17 AST types, a `powershell` folder in the formatting test harness, Playground support.

Not done or wrong in the POC:
- Unknown AST types print their .NET type name instead of source text (corrupts output).
- Comments are only kept between statements; comments inside expressions, param blocks, hashtables, and between `}` and `else` are dropped.
- Leaf printers rebuild text from parsed values (`Value.ToString()`, `"$" + VariablePath`, quotes re-added), which changes number literals, escaped quotes, `${}` variables, splatting, and scoped variables.
- `Pipeline` joins elements with nothing; `InvokeMemberExpression` always prints `::` and `()`; `catch` drops type filters; named blocks, param blocks, and conditions are verbatim.
- No validator, no CLI validation branch, harness uses the C# comparer for PowerShell, no idempotency check.
- `Random.test` fails.
- Docs, editor extensions, public API decision, package size measurement.

### Decision 6: Comment handling

Comments are attached to AST nodes before printing, the way Prettier does it for ASTs without trivia. For each comment token: find the deepest node whose extent contains it (enclosing), and among that node's children the one ending before it (preceding) and the one starting after it (following). A comment on the same line as the end of the preceding node is a trailing comment of that node. Otherwise it is a leading comment of the following node. With neither, it is a dangling comment of the enclosing node. `Node.Print` prints leading and trailing comments around every node; printers of block-like nodes print their own dangling comments.

As implemented: `Node.Print` only prints leading and trailing comments by itself for statements that sit directly in a block and for param blocks, because a `#` comment printed in the middle of a line swallows the code after it. For any other node the comments stay unprinted unless the parent printer calls `context.PrintLeadingComments(child)` / `context.PrintTrailingComments(child)` at a place where a line break is safe.

Safety net: `PrintContext` tracks which comments have been printed. After a node is printed, if any comment inside its extent was not printed, the node is printed verbatim from source instead. The validator also compares the comment list of input and output.

### Decision 7: Leaves and unknown nodes print from source text

Constants, strings, variables, type names, command parameters, and bare words print `Extent.Text`. Any AST type without a printer prints its extent verbatim. `--%` commands and anything containing a here-string keep the here-string lines literal.

### Decision 8: Style rules

These follow the common community style (OTBS, what `Invoke-Formatter` and the VS Code extension produce by default).

- Indent 4 spaces by default; tabs when configured. Print width from options.
- Opening brace on the same line. `} else {`, `} elseif (...) {`, `} catch [Type] {`, `} finally {`, `} while (...)`, `} until (...)` on the closing-brace line.
- Empty block is `{ }`. Empty hashtable `@{}`, empty array `@()`.
- One statement per line; `;` between statements is removed. Blank lines between statements: at most one kept, none at the start or end of a block.
- Keywords and operators are lowercased (`If` -> `if`, `-EQ` -> `-eq`). Command names, parameter names, type names, member names, and variable names keep their source casing. No alias expansion. Quote style is never changed.
- One space around binary and assignment operators, after `,`, and after keywords before `(`. No space after unary `!`/`-`/`+`/`++`/`--`/casts; one space after `-not`, `-bnot`, `-join`, `-split` in unary form.
- Range `..` and member access `.`/`::`/`?.` have no surrounding spaces. Index `[...]` has no inner spaces.
- Pipelines: on one line when they fit; otherwise break after each `|` with the continuation indented one level. A pipeline that was multi-line in the source at `|` stays broken. Same rule for `&&`/`||` chains.
- Backtick continuations are removed when the construct can break without them (after `|`, operators, `,`, inside parentheses/braces). A command whose arguments used backtick continuation keeps backticks, one argument per line, indented one level.
- Binary expressions break after the operator, continuation indented.
- Hashtables: `@{ a = 1; b = 2 }` on one line when the source had it on one line and it fits; otherwise one entry per line without `;`. A hashtable that had a newline after `@{` in the source stays expanded.
- Arrays and argument lists break after `,` one element per line when they do not fit.
- Script block expressions: `{ $_.Name }` on one line when the source was on one line, it has a single statement, and it fits; otherwise expanded like a statement block.
- `param(...)`: empty is `param()`. On one line when the source was on one line, no parameter has attributes on separate lines, and it fits. Otherwise one parameter per line, each attribute on its own line above the parameter, a blank line between parameters when any parameter has attributes. A blank line follows the param block.
- Attributes: `[Parameter(Mandatory = $true, Position = 0)]`; type constraints directly followed by the variable (`[string]$Name`).
- Functions: `function Name {`; `function Name($a, $b) {` keeps the parenthesized parameter form. Named blocks (`begin`/`process`/`end`/`clean`/`dynamicparam`) each start on their own line with a blank line between them.
- `switch`: `switch -Regex ($x) {`, each clause `pattern { ... }` on its own line; clause bodies follow script-block rules.
- Classes/enums: members one per line, blank line between methods, attributes above members.
- File ends with exactly one newline. Leading `#requires`, comment-based help, and `using` lines keep their order.
- Here-string contents and closing markers are never indented or changed.

Rules added while writing the expression and command printers:

- `@( )` and `$( )` stay expanded when the source had a line break after the opening delimiter or they hold more than one statement; otherwise they are on one line when they fit.
- A command line cannot wrap without a backtick, so commands are never broken between arguments. An array literal that is a command argument stays on one line unless the source had it on several lines. Hashtables, script blocks, `@( )`, `$( )`, and parenthesized expressions used as arguments can still break inside their delimiters.
- A command that used backtick continuation keeps the source grouping of arguments per line; only the continuation lines are indented.
- Arguments of a command that touch in the source (`Write-Host("hi")`) stay touching.
- File redirection has one space after the operator (`2> $null`); merging redirection is unchanged (`2>&1`). Redirections stay where they were among the arguments.
- `-Name: value` becomes `-Name:value`.
- Method call arguments break one per line inside the parentheses. When the last argument is a script block, hashtable, `@( )`, or `$( )`, or any argument is a multi-line string, the arguments stay on the line of the parentheses and that argument breaks by itself.
- After a first operand that is a hashtable, script block, `@( )`, or `$( )`, a binary operator and its right operand stay on the line of the closing delimiter. The same applies to `( )` when a single short operand follows. The format operator `-f` never breaks after the operator.
- A parenthesized binary expression, ternary, or array literal breaks after `(` and before `)`. Other parenthesized content (commands, pipelines) stays next to the parentheses.
- A ternary breaks after `?` and after `:`, because a line that starts with `?` is a new command.
- Unary `-` and `+` keep a source space between operator and operand so `- -$x` does not turn into `--$x`.
- A pipeline whose elements span lines but whose `|` were all on the same line as the neighbouring elements keeps that layout (`$x | ForEach-Object {` ... `} | Sort-Object`).
- All of these decisions use the node types or the source layout that the output keeps, never whether something happened to fit, so a second format gives the same result.

Rules added while writing the statement and declaration printers:

- A statement block always has its statements on their own lines. The exceptions are a `switch` clause body and a script block expression, which stay on one line when the source had them on one line with a single statement. A script block expression with a `param()` and one statement also stays on one line.
- A comment between `}` and `else`/`elseif`/`catch`/`finally`/`while`/`until` stays where it is: a comment on the line of `}` stays after the brace, a comment on its own line stays on its own line, and the keyword starts the next line.
- A comment between the header of a statement and its `{` (`if ($a) # why`) moves to the first line inside the block.
- The lines of a `<# #>` comment after the first are written exactly as in the source. Only the first line follows the indentation of the code.
- `param(...)` breaks when the source had it on more than one line, when it holds a comment, or when it does not fit. When it breaks, attributes go on their own lines, the type constraints directly in front of the name stay with the name, and parameters are separated by a blank line when any parameter has an attribute. A type constraint that is followed by an attribute keeps its place in the order and gets its own line.
- Class members: attributes on their own lines, then `hidden`, then `static`, then the type. A blank line goes before and after every method. Enum members go one per line.
- `function f()` keeps its empty parentheses because the tree differs from `function f`.
- `switch` flags, the name and `-SupportedCommand` of a `data` statement, and labels keep the casing of the source.
- `for` prints `for (init; condition; step)` and `for (;;)`.
- `using` statements come first, followed by a blank line. A `using` with an alias is printed as written.
- A blank line separates the param block from the code after it and each named block from the next.
- Arguments of attributes, method calls, and `base(...)` go one per line when one of them has a comment next to it.

### Decision 9: Validator compares ASTs and comments

`PowerShellFormattingValidator` lives in `Src/CSharpier.Core/PowerShell/`, parses both texts, walks both trees in parallel, and fails on the first difference in node type, child count, or leaf data (constant values, string values and quote type, variable paths, operator token kinds, command element text, parameter names, type names, member names, static flag, redirection data). Comments are compared as an ordered list with per-line leading whitespace trimmed. The formatting test harness uses it for the `powershell` folder and also formats the output a second time and asserts it is unchanged.

### Decision 10: Resolved open questions

- Dependency: keep `System.Management.Automation`; record the measured package size delta here. If it is large, that is reported to the maintainer, not solved in this change.
  - Measured 2026-11-11, Release pack of this branch against the published 1.4.0 packages:
    - `csharpier` tool nupkg: 13.5 MB -> 28.1 MB. Unpacked `tools/net10.0/any`: 19.9 MB -> 42.5 MB; `tools/net11.0/any`: 19.6 MB -> 42.2 MB. 53 new files per target framework, 22.5 MB.
    - Largest additions: `runtimes/win/lib/net8.0/System.Management.Automation.dll` (7.6 MB), `runtimes/unix/lib/net8.0/System.Management.Automation.dll` (7.1 MB), `System.Diagnostics.EventLog.Messages.dll`, `Newtonsoft.Json.dll`, `System.Configuration.ConfigurationManager.dll`, `System.DirectoryServices.dll`, `Microsoft.ApplicationInsights.dll`, `System.Security.Cryptography.Pkcs.dll`, `System.Management.dll`, `Microsoft.Management.Infrastructure` (per runtime), native `libpsl-native` and `pwrshplugin` binaries, `Microsoft.PowerShell.CoreCLR.Eventing.dll`, `System.CodeDom.dll`.
    - `CSharpier.Core` nupkg: 526 KB -> 558 KB. The `net10.0` and `net11.0` dependency groups gain `System.Management.Automation` 7.4.6, so consumers restore that package and its dependencies. The `netstandard2.0` group is unchanged.
    - `CSharpier.MsBuild` copies the CLI build output into `tools/csharpier`, so it grows by the same files.
    - The packed tool installed into a temporary tool path formatted a `.ps1` file on Windows. Linux and macOS were not tried.
- Block comments: a multi-line `<# #>` comment that starts its line moves as a whole. Lines after the first that begin with the first line's source indentation are printed at the new indentation; other lines are kept as written. Trailing whitespace on re-indented lines is removed.
- `csharpier-ignore` comments are not supported in PowerShell files.
- Coverage: every `Ast` subclass gets a printer or an explicit verbatim printer. DSC `configuration`, dynamic keywords, and `workflow` blocks are verbatim.
- No experimental gate: PowerShell files are formatted by default like XML was in 1.0.
- Public API: no new public type. The public `CSharpFormatter`/`XmlFormatter` equivalents are deferred until the output is stable.
- Playground is done. Docs and editor-extension lists are part of this change.

### Decision 11: Testing against real code

A set of public repositories (PowerShell/PowerShell, Pester, PSScriptAnalyzer, dbatools, PSReadLine, SqlServerDsc, fleschutz/PowerShell, vscode-powershell) is downloaded outside the repo. The dev build formats a copy of it; the run must report zero exceptions, zero validation failures, and a second run must report every file as already formatted.

### Decision 12: Resolution errors do not block formatting

The parser reports errors that depend on what is installed on the machine, not on syntax: `ResourceNotDefined`, `ModuleNotFoundDuringParse`, `TypeNotFound`, `RequiresModuleInvalid`, `WorkflowNotSupportedInPowerShellCore`, `DscResourceMissingSetMethod`. The AST is still complete for these. `CustomAttributeTypeNotFound` was added to the list while testing classes whose attributes come from another module. A file whose errors all have one of these `ErrorId` values is formatted; the validator ignores the same ids. Any other error leaves the file unchanged and is reported.

## Open Questions (resolved in Decision 10)

- **Exact dependency and its size:** full `System.Management.Automation` (or `Microsoft.PowerShell.SDK`) vs a trimmed parser-only reference — and the measured packaged-size impact on the global tool. This gates Decision 2.
- **Coverage boundary for v1:** which constructs are explicitly in-scope vs deferred (e.g., DSC configurations, dynamic keywords, class syntax, workflow).
- **Experimental gate:** ship on by extension immediately, or behind an opt-in flag until node-printer coverage is broad enough to avoid churn in users' scripts?
- **Playground/editor/docs rollout:** whether these secondary surfaces land in the same change or a follow-up.
