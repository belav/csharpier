CSharpier has support for a configuration file. You can use any of the following files
- A ```.csharpierrc``` file in JSON or YAML.
- A ```.csharpierrc.json``` or ```.csharpierrc.yaml``` file.

### Configuration Options
JSON
```json
{
   "printWidth": 100
}
```
YAML
```json
printWidth: 100
```

#### Print Width
Specify at what point the printer will wrap content. This is not a hard limit. Some lines will be shorter or longer.

Default 100
