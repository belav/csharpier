---
hide_table_of_contents: true
---

CSharpier has support for a configuration file. You can use any of the following files
- A ```.csharpierrc``` file in JSON or YAML.
- A ```.csharpierrc.json``` or ```.csharpierrc.yaml``` file.
- A ```.editorconfig``` file. See EditorConfig section below.

The configuration file will be resolved based on the location of the file being formatted.
- If a `.csharpierrc` file exists somewhere at or above the given file, that will be used.
- Otherwise if an `.editorconfig` file exists somewhere at or above the given file, that will be used. Respecting editorconfig inheritance.
### Configuration Options
JSON
```json
{
    "printWidth": 100,
    "useTabs": false,
    "tabWidth": 4,
    "endOfLine": "auto"
}
```
YAML
```yaml
printWidth: 100
useTabs: false
tabWidth: 4
endOfLine: auto
```

#### Print Width
Specify at what point the printer will wrap content. This is not a hard limit. Some lines will be shorter or longer.

Default `100`
#### Use Tabs
_First available in 0.17.0_

Indent lines with tabs instead of spaces.

Default `false`
#### Tab Width
_First available in 0.17.0_

Specify the number of spaces used per indentation level.

Default `4`

#### End of Line
_First available in 0.26.0_

Valid options:

- "auto" - Maintain existing line endings (mixed values within one file are normalised by looking at what's used after the first line)
- "lf" – Line Feed only (\n), common on Linux and macOS as well as inside git repos
- "crlf" - Carriage Return + Line Feed characters (\r\n), common on Windows

Default `auto`


#### Preprocessor Symbol Sets
_Removed in 0.25.0_

Currently CSharpier only has basic support for understanding how to format code inside of `#if` directives.
It will attempt to determine which sets of preprocessor symbols are needed for roslyn to parse all the code in each file.

For example in the following code block, the following symbol sets would be needed ["FIRST", "SECOND,THIRD", ""]
```csharp
#if FIRST
// some code
#elif SECOND && THIRD
// some code
#else
// some code
#endif

```

When supplying symbol sets, they will be used for all files being formatted. This will slow down formatting, and determining all symbol sets needed across all files won't be straight forward.

The long term plan is to improve Csharpier's ability to determine the symbol sets itself and to allow specifying them for individual files.

### Configuration Overrides ###
_First available in 0.29.0_
Overrides allows you to specify different configuration options based on glob patterns. This can be used to format non-standard extensions, or to change options based on file path. Top level options will apply to `**/*.{cs,csx}`

```json
{
    "overrides": [
        {
           "files": ["*.cst"],
           "formatter": "csharp",
           "tabWidth": 2,
           "useTabs": true,
           "printWidth": 10,
           "endOfLine": "LF"
        }
    ]
}
```

```yaml
overrides:
    - files: "*.cst"
      formatter: "csharp"
      tabWidth: 2
      useTabs: true
      printWidth: 10
      endOfLine: "LF"
```

### EditorConfig
_First available in 0.26.0_

CSharpier supports configuration via an `.editorconfig` file. A `.csharpierrc*` file in the same directory will take priority.

```ini
[*]
# Non-configurable behaviors
charset = utf-8
insert_final_newline = true
trim_trailing_whitespace = true
dotnet_sort_system_directives_first = true
dotnet_separate_import_directive_groups = false

# Configurable behaviors
# end_of_line = lf - there is no 'auto' with a .editorconfig
indent_style = space
indent_size = 4
max_line_length = 100

```
