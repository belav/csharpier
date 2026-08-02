## ADDED Requirements

These requirements characterize the **existing** observable behavior of the CLI formatting flow. They are recorded so the refactor has an explicit acceptance contract: after the refactor, every scenario below MUST still hold. No behavior is being added or changed.

### Requirement: Standard input formatting

The CLI SHALL format code supplied on standard input and write the result to standard output, resolving configuration relative to the supplied path.

#### Scenario: Content piped without a path
- **WHEN** standard-in contents are provided and the single directory-or-file argument is an existing directory
- **THEN** the system SHALL treat the input as a single file located in that directory, inferring `.xml` when the trimmed content starts with `<` and `.cs` otherwise, and write the formatted result to standard output

#### Scenario: Content piped with a supplied file path
- **WHEN** standard-in contents are provided and `--stdin-path` supplies a file path whose directory does not exist on disk
- **THEN** the system SHALL walk up to the nearest existing ancestor directory to resolve configuration, and SHALL apply ignore-file rules to the supplied path

#### Scenario: Stdin file is ignored or generated
- **WHEN** the stdin file path is a generated code file (and `--include-generated` is not set), or is ignored by configuration when a path was supplied
- **THEN** the system SHALL NOT format it and SHALL produce no formatted output for it

### Requirement: Physical file and directory formatting

The CLI SHALL format files and directories passed as arguments, selecting the output destination based on options.

#### Scenario: Output writer selection
- **WHEN** formatting physical files
- **THEN** the system SHALL write to standard output when `--write-stdout` is set, perform no writes when `--check` or `--skip-write` is set, and otherwise write formatted results back to the file system

#### Scenario: Missing path argument
- **WHEN** an argument is neither an existing file nor an existing directory
- **THEN** the system SHALL write an error naming the original path and return exit code 1

#### Scenario: Directory recursion skips ignored subtrees and generated files
- **WHEN** a directory argument is formatted
- **THEN** the system SHALL enumerate files recursively, skipping directories that are ignored and skipping files that are ignored or are generated code (unless `--include-generated` is set)

#### Scenario: Unsupported single file
- **WHEN** a single file argument resolves to an unknown formatter
- **THEN** the system SHALL emit a console warning that the file is an unsupported file type

#### Scenario: MSBuild version mismatch check
- **WHEN** a directory is formatted and `--no-msbuild-check` is not set
- **THEN** the system SHALL check for mismatched CLI and MSBuild versions and return exit code 1 when a mismatch is detected

### Requirement: Per-file formatting sequence

For each file to be formatted, the CLI SHALL perform an ordered sequence of steps, any of which may stop processing of that file early.

#### Scenario: Empty file
- **WHEN** the file contents are empty
- **THEN** the system SHALL skip the file without counting it and without writing output

#### Scenario: Cache hit
- **WHEN** the formatting cache reports the file can be skipped
- **THEN** the system SHALL count the file as processed and as cached, and SHALL NOT re-format it

#### Scenario: Encoding could not be detected
- **WHEN** the file's encoding cannot be detected
- **THEN** the system SHALL emit a warning naming the default encoding used and continue formatting

#### Scenario: Formatting throws
- **WHEN** the formatter throws a non-cancellation exception
- **THEN** the system SHALL log the error against the file and increment the formatting-exceptions count, without writing output

#### Scenario: Syntax errors
- **WHEN** the formatting result contains error diagnostics
- **THEN** the system SHALL report them as an error, or as a warning when `--syntax-errors-as-warnings` is set, increment the failed-compilation count, and not write output

#### Scenario: Formatter warning or failure message
- **WHEN** the formatting result carries a warning message or a failure message
- **THEN** the system SHALL surface it (warning or error respectively) and stop processing the file without writing output

#### Scenario: Validation of formatted output
- **WHEN** validation is not skipped and the formatted output differs from the input for a C#, C# script, or XML file
- **THEN** the system SHALL validate the syntax tree, incrementing the failed-validation count on failure and the validation-exceptions count if validation throws

#### Scenario: Check mode reports unformatted files
- **WHEN** `--check` is set (and not writing to stdout) and the formatted output differs from the input
- **THEN** the system SHALL report the first difference (as an error, or a warning when `--unformatted-as-warnings` is set) and increment the unformatted-files count

#### Scenario: Successful write and cache
- **WHEN** a file is formatted without a short-circuiting condition
- **THEN** the system SHALL write the result via the selected writer and record the result in the formatting cache

### Requirement: Result reporting and exit code

The CLI SHALL report a summary and return an exit code derived from the accumulated result counters.

#### Scenario: Summary log
- **WHEN** formatting completes and `--write-stdout` is not set
- **THEN** the system SHALL log the count of files checked or formatted and the elapsed milliseconds

#### Scenario: Failure exit code
- **WHEN** there were failed compilations (and `--syntax-errors-as-warnings` is not set), or unformatted files in check mode (and `--unformatted-as-warnings` is not set), or any failed validations, formatting exceptions, or validation exceptions
- **THEN** the system SHALL return exit code 1; otherwise it SHALL return exit code 0

#### Scenario: Invalid ignore file
- **WHEN** an `InvalidIgnoreFileException` is thrown (directly or as an inner exception) during formatting
- **THEN** the system SHALL log the ignore-file error and return exit code 1
