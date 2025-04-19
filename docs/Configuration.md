---
hide_table_of_contents: true
---

CSharpier has support for a configuration file. You can use any of the following files
- A ```.csharpierrc``` file in JSON or YAML.
- A ```.csharpierrc.json``` or ```.csharpierrc.yaml``` file.
- A ```.editorconfig``` file. See [EditorConfig](#editorconfig) section below.

The configuration file will be resolved based on the location of the file being formatted.
- If a `.csharpierrc` file exists somewhere at or above the given file, that will be used.
- Otherwise if an `.editorconfig` file exists somewhere at or above the given file, that will be used. Respecting editorconfig inheritance.
### Configuration Options
JSON
```json
{
    "printWidth": 100,
    "useTabs": false,
    "indentSize": 4,
    "endOfLine": "auto"
}
```
YAML
```yaml
printWidth: 100
useTabs: false
indentSize: 4
endOfLine: auto
```

#### Print Width
Specify at what point the printer will wrap content. This is not a hard limit. Some lines will be shorter or longer.

Default `100`
#### Use Tabs
Indent lines with tabs instead of spaces.

Default `false`
#### Indent Size
Specify the number of spaces used per indentation level.

Default for C# `4`\
Default for XML `2`

#### End of Line

Valid options:

- "auto" - Maintain existing line endings (mixed values within one file are normalised by looking at what's used after the first line)
- "lf" – Line Feed only (\n), common on Linux and macOS as well as inside git repos
- "crlf" - Carriage Return + Line Feed characters (\r\n), common on Windows

Default `auto`

### Configuration Overrides ###
Overrides allows you to specify different configuration options based on glob patterns. This can be used to format non-standard extensions, or to change options based on file path. Top level options will apply to `**/*.{cs,csx}`

```json
{
    "overrides": [
        {
           "files": ["*.cst"],
           "formatter": "csharp",
           "indentSize": 2,
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
      indentSize: 2
      useTabs: true
      printWidth: 10
      endOfLine: "LF"
```

### EditorConfig
CSharpier supports configuration via an `.editorconfig` file. A `.csharpierrc*` file in the same directory will take priority.

```ini
[*.{cs,csx}]
# Non-configurable behaviors
charset = utf-8
insert_final_newline = true
trim_trailing_whitespace = true
dotnet_sort_system_directives_first = true
dotnet_separate_import_directive_groups = false

# Configurable behaviors
# end_of_line = lf - there is no 'auto' with an .editorconfig
indent_style = space
indent_size = 4
max_line_length = 100

[*.{csproj,props,targets,xml,config}]
# Configurable behaviors
indent_style = space
indent_size = 2
max_line_length = 100
```

Formatting non-standard file extensions using csharpier can be accomplished with the `csharpier_formatter` option
```ini
[*.cst]
csharpier_formatter = csharp
indent_style = space
indent_size = 2
max_line_length = 80
```

