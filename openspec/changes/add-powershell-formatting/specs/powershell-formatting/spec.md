## ADDED Requirements

### Requirement: PowerShell file detection

The system SHALL recognize PowerShell source files by extension and route them to the PowerShell formatter.

#### Scenario: Recognized PowerShell extensions
- **WHEN** a file path ends in `.ps1`, `.psm1`, or `.psd1` (case-insensitively)
- **THEN** the system SHALL resolve its formatter to PowerShell

#### Scenario: Non-PowerShell extensions are unaffected
- **WHEN** a file path ends in an extension already mapped to another formatter (for example `.cs`, `.csx`, `.xml`) or in an unrecognized extension
- **THEN** the system SHALL resolve its formatter to that existing formatter or to Unknown, and SHALL NOT route it to the PowerShell formatter

### Requirement: PowerShell formatting entry point

The system SHALL expose a public PowerShell formatting entry point that accepts source text and formatting options and returns a formatter result, mirroring the existing C# and XML entry points.

#### Scenario: Format valid PowerShell source
- **WHEN** valid PowerShell source is passed to the PowerShell formatter
- **THEN** the system SHALL return a result whose formatted code is the re-printed PowerShell and whose error diagnostics are empty

#### Scenario: Dispatch through the shared formatter
- **WHEN** the shared code formatter is invoked with options whose formatter is PowerShell
- **THEN** the system SHALL delegate to the PowerShell formatter and return its result

### Requirement: Opinionated re-printing through the shared engine

The system SHALL re-print PowerShell by building a Doc tree with the shared Doc primitives and printing it through the shared DocPrinter, so that indentation, line-width, indent style, and end-of-line handling follow the same configured options as other languages.

#### Scenario: Indentation and line width applied
- **WHEN** PowerShell source with inconsistent indentation is formatted with the configured print width and indent size
- **THEN** the system SHALL emit output indented according to the configured indent style and size, wrapping constructs that exceed the configured print width where the formatter supports breaking

#### Scenario: End-of-line normalization
- **WHEN** PowerShell source is formatted
- **THEN** the system SHALL produce line endings according to the configured end-of-line option, defaulting to the source's detected line ending when the option is Auto

#### Scenario: Idempotent formatting
- **WHEN** already-formatted PowerShell output is formatted a second time with the same options
- **THEN** the system SHALL produce byte-identical output

### Requirement: Unparseable input is left unchanged

The system SHALL NOT alter PowerShell source that cannot be parsed, and SHALL report the condition rather than emit corrupted output.

#### Scenario: Syntactically invalid PowerShell
- **WHEN** PowerShell source contains parse errors
- **THEN** the system SHALL return the original source unchanged together with a warning (or error diagnostics) indicating the input could not be formatted, and SHALL NOT write mangled output

### Requirement: Formatting preserves program meaning

The system SHALL validate that formatting a PowerShell file does not change the program it represents, consistent with the validation performed for other languages, so that formatting is safe to apply automatically.

#### Scenario: Post-format validation on change
- **WHEN** the CLI formats a PowerShell file whose formatted output differs from its input and validation is not skipped
- **THEN** the system SHALL re-parse the output and compare it against the input, and SHALL report a validation failure (rather than write the file) if the two are not equivalent

#### Scenario: Comments and here-strings are preserved
- **WHEN** PowerShell source containing comments, comment-based help, or here-strings is formatted
- **THEN** the formatted output SHALL retain those comments and preserve here-string contents verbatim
