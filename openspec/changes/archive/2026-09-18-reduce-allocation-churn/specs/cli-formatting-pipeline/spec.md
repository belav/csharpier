## ADDED Requirements

### Requirement: Per-file filter ordering does not change reported outcomes

The per-file short-circuit checks — generated-code detection, ignore-file matching, and formatter resolution — may be evaluated in whichever order is cheapest, since each is independent of the others' side effects. The outcome the CLI reports for a file SHALL NOT depend on that order.

This exists because the checks are deliberately reordered for performance: resolving printer options costs O(sections in `.editorconfig`) while ignore matching costs O(rules across all ancestor ignore files), so evaluating the cheaper filter first avoids paying the expensive one for files that are discarded anyway. That reorder is only safe while the reported outcome is order-independent, which this requirement pins down.

#### Scenario: File is both ignored and of an unsupported type
- **WHEN** a file matches an ignore rule and also resolves to an unknown formatter
- **THEN** the system SHALL skip it silently, without emitting an unsupported-file-type warning, regardless of which filter is evaluated first

#### Scenario: File is of an unsupported type and is not ignored
- **WHEN** a single file argument resolves to an unknown formatter and no ignore rule matches it
- **THEN** the system SHALL emit the unsupported-file-type warning

#### Scenario: File is ignored and of a supported type
- **WHEN** a file matches an ignore rule and resolves to a known formatter
- **THEN** the system SHALL skip it silently, without formatting it and without counting it

#### Scenario: Generated file that is also ignored
- **WHEN** a file is detected as generated code, `--include-generated` is not set, and the file also matches an ignore rule
- **THEN** the system SHALL skip it silently, producing the same result as when only one of the two conditions holds
