---
hide_table_of_contents: true
---

CSharpier has support for a configuration file. You can use any of the following files
- A ```.csharpierrc``` file in JSON or YAML.
- A ```.csharpierrc.json``` or ```.csharpierrc.yaml``` file.

The configuration file will be resolved starting from the location of the file being formatted, and searching up the file tree until a config file is (or isn’t) found.

### Configuration Options
JSON
```json
{
    "printWidth": 100,
    "useTabs": false,
    "tabWidth": 4
}
```
YAML
```yaml
printWidth: 100
useTabs: false
tabWidth: 4
```

#### Print Width
Specify at what point the printer will wrap content. This is not a hard limit. Some lines will be shorter or longer.

Default 100
#### Use Tabs
_First available in 0.17.0_

Indent lines with tabs instead of spaces.

Default false
#### Tab Width
_First available in 0.17.0_

Specify the number of spaces used per indentation level.

Default 4

#### Preprocessor Symbol Sets
**Removed in 0.25.0**

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
