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

## Open Questions

- **Exact dependency and its size:** full `System.Management.Automation` (or `Microsoft.PowerShell.SDK`) vs a trimmed parser-only reference — and the measured packaged-size impact on the global tool. This gates Decision 2.
- **Coverage boundary for v1:** which constructs are explicitly in-scope vs deferred (e.g., DSC configurations, dynamic keywords, class syntax, workflow).
- **Experimental gate:** ship on by extension immediately, or behind an opt-in flag until node-printer coverage is broad enough to avoid churn in users' scripts?
- **Playground/editor/docs rollout:** whether these secondary surfaces land in the same change or a follow-up.
