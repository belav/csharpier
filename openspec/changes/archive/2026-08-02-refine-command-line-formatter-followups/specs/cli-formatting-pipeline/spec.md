## MODIFIED Requirements

### Requirement: Standard input formatting

The CLI SHALL format code supplied on standard input and write the result to standard output, resolving configuration relative to the supplied path.

#### Scenario: Content piped without a path
- **WHEN** standard-in contents are provided and the single directory-or-file argument is an existing directory
- **THEN** the system SHALL treat the input as a single file located in that directory, inferring `.xml` when the trimmed content starts with `<` and `.cs` otherwise, and write the formatted result to standard output

#### Scenario: Content piped with a supplied file path
- **WHEN** standard-in contents are provided and `--stdin-path` supplies a file path whose directory does not exist on disk
- **THEN** the system SHALL resolve configuration as if the file were located at the supplied path — using the configuration and ignore files that apply to the nearest existing ancestor directory — and SHALL apply ignore-file rules to the supplied path

#### Scenario: Stdin file is ignored or generated
- **WHEN** the stdin file path is a generated code file (and `--include-generated` is not set), or is ignored by configuration when a path was supplied
- **THEN** the system SHALL NOT format it and SHALL produce no formatted output for it
