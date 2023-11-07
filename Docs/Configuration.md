---
hide_table_of_contents: true
---

CSharpier has support for a configuration file. You can use any of the following files
- A ```.csharpierrc``` file in JSON or YAML.
- A ```.csharpierrc.json``` or ```.csharpierrc.yaml``` file.
- A ```.editorconfig``` file. See EditorConfig section below.

The configuration file will be resolved starting from the location of the file being formatted, and searching up the file tree until a config file is (or isn’t) found.

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
```c#
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

### EditorConfig

CSharpier supports configuration via an `.editorconfig` file. A `.csharpierrc*` file in the same directory will take priority.

```ini
[*]
# Non-configurable behaviors
charset = utf-8
insert_final_newline = true
trim_trailing_whitespace = true

# Configurable behaviors
# end_of_line = lf - there is no 'auto' with a .editorconfig
indent_style = space
indent_size = 4
max_line_length = 100

```
