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
    "endOfLine": "auto",

    "newLineBeforeOpenBrace": 0,
    "newLineBeforeElse": true,
    "newLineBeforeCatch": true,
    "newLineBeforeFinally": true,
    "newLineBeforeMembersInObjectInitializers": null
}
```
YAML
```yaml
printWidth: 100
useTabs: false
tabWidth: 4
endOfLine: auto

newLineBeforeOpenBrace: 0
newLineBeforeElse: true
newLineBeforeCatch: true
newLineBeforeFinally: true
newLineBeforeMembersInObjectInitializers: null
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

#### New line before open brace
_Available only in CSharpierConfig_

See [editorconfig](
https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/csharp-formatting-options#csharp_new_line_before_open_brace) page to more information.
Values are mapped in `BraceNewLine` and it's a flag value. Valid options:
`all`, `none`, `accessors`, `anonymous_methods`, `anonymous_types`, `control_blocks`, `events`, `indexers`, `lambdas`, `local_functions`, `methods`, `object_collection_array_initializers`, `properties`, `types`

All values is mapped into `BraceNewLine`.

Default `65535` `all`

#### New line before else
_Available only in CSharpierConfig_

Place `else` statements on a new line.

Default `true`

#### New line before catch
_Available only in CSharpierConfig_

Place `catch` statements on a new line.

Default `true`

#### New line before finally
_Available only in CSharpierConfig_

Place `finally` statements on a new line.

Default `true`

#### Avoid inline inizializer
_Available only in CSharpierConfig_

Avoid initializers for objects or arrays should be placed on the same line if they contain fewer than three elements or Collections are not complex element.

Valid options:

- `true`: Avoid initializers on a single line
  ```csharp
  var objectInitializer = new ObjectName
  {
      A = 1,
      B = 2
  };
  ```
- `false`: Permit initializers on a single line
  ```csharp
  var objectInitializer = new ObjectName { A = 1, B = 2 };
  ```

Default `false`

#### New line before members in object initializers
_Available only in CSharpierConfig_

Require members of object initializers to be on separate lines or not.

Valid options:

- `true`: Require members of object initializers to be on separate lines
  ```csharp
  var objectInitializer = new ObjectName
  {
      A = 1,
      B = 2
  };
  ```
- `false`: Require members of object initializers to be on the same line
  ```csharp
  var objectInitializer = new ObjectName 
  {
      A = 1, B = 2
  };
  ```
- `null`: Use default behaviour of CSharpier, inline for objects or arrays should be placed on the same line if they contain fewer than three elements or Collections are not complex element.

Default `null`

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
