# formatter-performance-invariants

## Purpose

Machine-checkable invariants the formatting engine must hold: pooled buffers are returned, tree comparison compares content rather than reference identity, debug-only serialization is gated behind its option, and per-node work is not repeated. These exist so that an optimization which stops working fails a test instead of degrading performance without notice.

## Requirements

### Requirement: Pooled scratch buffers are returned

`DocListBuilder` rents its backing array from `ArrayPool<Doc>.Shared`. Every construction site SHALL return that array, so the pool reaches a steady state instead of degrading to a fresh allocation on every rent.

#### Scenario: A disposed builder releases its buffer
- **WHEN** a `DocListBuilder` is disposed
- **THEN** the system SHALL return the rented array to `ArrayPool<Doc>.Shared` and SHALL leave the builder's internal span empty, so that any subsequent read is a bounds failure rather than a silent read of a buffer now owned by another caller

#### Scenario: No construction site leaks its buffer
- **WHEN** the `CSharpier.Core` sources are scanned for `new DocListBuilder(`
- **THEN** every occurrence SHALL be bound by a `using` declaration or otherwise disposed on every return path

#### Scenario: Repeated doc construction does not grow the managed heap
- **WHEN** the same document is printed repeatedly after a warm-up
- **THEN** the bytes allocated per print SHALL NOT include a pooled scratch array per printed node

### Requirement: Full-span comparison compares content

The syntax-tree comparer's whole-subtree fast path SHALL compare the *text* of two spans, not whether they occupy the same memory. The two source strings being compared are always distinct instances, so reference comparison disables the optimization entirely.

#### Scenario: Identical text drawn from different source strings
- **WHEN** two syntax nodes have identical full-span text and their spans are sliced from two different string instances
- **THEN** `CompareFullSpan` SHALL report them as equal

#### Scenario: Identical text at a shifted offset
- **WHEN** the formatted node's full span begins at a different offset than the original node's, but the text over both spans is identical
- **THEN** `CompareFullSpan` SHALL report them as equal

#### Scenario: Differing text
- **WHEN** the text over the two spans differs in any character
- **THEN** `CompareFullSpan` SHALL report them as not equal

#### Scenario: The benchmark exercises the production input shape
- **WHEN** the `SyntaxNodeComparer` benchmark is constructed
- **THEN** it SHALL be given two distinct string instances, so that a comparison which only succeeds on reference identity does not appear to pass

### Requirement: Debug-only serialization is gated behind its option

Serializing a parsed tree to JSON is diagnostic output for the Playground. It SHALL be produced only when the caller asks for it, on every formatter.

#### Scenario: XML formatted without requesting the AST
- **WHEN** XML is formatted and `PrinterOptions.IncludeAST` is false
- **THEN** the system SHALL NOT serialize the parsed node tree and `CodeFormatterResult.AST` SHALL be empty

#### Scenario: XML formatted with the AST requested
- **WHEN** XML is formatted and `PrinterOptions.IncludeAST` is true
- **THEN** `CodeFormatterResult.AST` SHALL contain the serialized node tree

### Requirement: Printed width is computed once per string doc

A `StringDoc` is immutable and is shared across the document — every occurrence of a given keyword or punctuator is the same instance. Its printed width SHALL be an intrinsic property computed once, not re-derived at each measurement.

#### Scenario: Width is exposed as a stored property
- **WHEN** a `StringDoc` is constructed
- **THEN** its printed width SHALL be computed once and exposed as a property, and both the printer and the fitter SHALL read that property rather than rescanning the string

#### Scenario: Width accounts for wide characters
- **WHEN** a `StringDoc` holds text containing East Asian wide characters
- **THEN** its printed width SHALL count those characters as two columns and all others as one, matching the previous per-character calculation exactly

### Requirement: Preprocessor symbol discovery skips directive-free files

Discovering `#if` symbol sets walks every node, token and trivia in the file. Files with no preprocessor directives SHALL not pay for that walk.

#### Scenario: File with no directives
- **WHEN** a file's syntax root reports that it contains no directives
- **THEN** the system SHALL return an empty symbol-set list without constructing the walker and without traversing the tree

#### Scenario: File with directives
- **WHEN** a file contains `#if`/`#elif`/`#else` directives
- **THEN** the system SHALL produce the same symbol sets it produced before this change

### Requirement: Argument bodies are printed once

Printing a syntax subtree mutates the printing context and is proportional to subtree size. A single argument's body SHALL be printed once and the resulting `Doc` shared between conditional branches, rather than printed once per branch.

#### Scenario: Invocation with a single simple-lambda argument
- **WHEN** an invocation's argument list is a single simple-lambda argument
- **THEN** the system SHALL invoke the lambda-body printer exactly once and use the resulting `Doc` in both the broken and flat branches of the conditional

#### Scenario: Nested single-lambda arguments
- **WHEN** single-lambda arguments are nested to depth *n*
- **THEN** the number of lambda-body print invocations SHALL be linear in *n*, not exponential

### Requirement: Formatted output is unchanged

Every optimization in this capability is internal. None SHALL alter the bytes CSharpier produces.

#### Scenario: Existing formatting corpus
- **WHEN** the full formatting test suite is run after these changes
- **THEN** every expected-output comparison SHALL pass unchanged, including the cases covering trailing commas, `csharpier-ignore` regions, raw string literals and preprocessor directives
