CSharpier has support for a configuration file. You can use any of the following files
- A ```.csharpierrc``` file in JSON or YAML.
- A ```.csharpierrc.json``` or ```.csharpierrc.yaml``` file.

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
```json
printWidth: 100
useTabs: false
tabWidth: 4
endOfLine: auto
```

#### Print Width
Specify at what point the printer will wrap content. This is not a hard limit. Some lines will be shorter or longer.

Default 100
#### Use Tabs
Indent lines with tabs instead of spaces.

Default false
#### Tab Width
Specify the number of spaces used per indentation level.

Default 4
#### End of Line
Specify what type of line endings will be printed in files.
Options
- "auto" - Detects which type of line ending to used based on the first one it encounters in the file **Default**
- "lf" - Line feed only (\n)
- "crlf" Carriage return and line feed (\r\n)
