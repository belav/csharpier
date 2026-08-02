## MODIFIED Requirements

### Requirement: Physical file and directory formatting

The CLI SHALL format files and directories passed as arguments, selecting the output destination based on options. When formatting a directory, the CLI SHALL format the discovered files with a bounded degree of concurrency rather than starting all of them at once.

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

#### Scenario: Bounded concurrency for directory formatting
- **WHEN** a directory containing many files is formatted
- **THEN** the system SHALL format the files concurrently up to a bounded maximum degree of parallelism, and SHALL NOT start all file-formatting operations simultaneously

#### Scenario: Results unchanged under bounded concurrency
- **WHEN** a directory is formatted with bounded concurrency
- **THEN** every per-file outcome — formatted output, result counters (files, cached, unformatted, failures, exceptions), and the final exit code — SHALL be identical to formatting the same directory without a concurrency bound

#### Scenario: Cancellation during directory formatting
- **WHEN** the run's cancellation token is cancelled while a directory is being formatted with bounded concurrency
- **THEN** the system SHALL stop starting new files and SHALL swallow the cancellation for that token exactly as it does today, while any other cancellation propagates
