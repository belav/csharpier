CSharpier has support for a configuration file. You can use any of the following files
- A ```.csharpierrc``` file in JSON or YAML.
- A ```.csharpierrc.json``` or ```.csharpierrc.yaml``` file.

### Configuration Options
JSON
```json
{
    "printWidth": 100,
    "preprocessorSymbolSets": ["", "DEBUG", "DEBUG,CODE_STYLE"]
}
```
YAML
```yaml
printWidth: 100
preprocessorSymbolSets:
    - ""
    - "DEBUG"
    - "DEBUG,CODE_STYLE"
```

#### Print Width
Specify at what point the printer will wrap content. This is not a hard limit. Some lines will be shorter or longer.

Default 100

#### Preprocessor Symbol Sets
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

The long term plan is to improve Csharpier's ability to determine the symbol sets itself and to allow specifying the for individual files.