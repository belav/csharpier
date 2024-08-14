This is as non opinionated version of [csharpier](https://github.com/belav/csharpier) tool created to add EditorConfig file style guide, allowing users to define and apply custom styling options.

### Quick Start
Install CSharpier-Config globally using the following command.
```bash
dotnet tool install csharpier-config -g
```
Then format the contents of a directory and its children with the following command.
```bash
dotnet csharpier-config .
```

**This is a work in progress project**

In this branch, I will set up the project with new references in the documentation, as well as handle the deployment and naming of the executable program.

All documentation that you find about csharpier is also valid for csharpier-config, you have only to change command like
All documentation you find about `CSharpier` is also valid for `CSharpier-Config`. You only need to adjust the commands accordingly.

- `dotnet csharpier` -> `dotnet csharpier-config`
- `dotnet-csharpier` -> `dotnet-csharpier-config`

**New feature**

- Support to all [C# formatting options](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/csharp-formatting-options) into `.editorconfig` file

```csharp
#  CSharp formatting rules:
[*.cs]
csharp_new_line_before_open_brace = methods, properties, control_blocks, types
csharp_new_line_before_else = true
csharp_new_line_before_catch = true
csharp_new_line_before_finally = true
csharp_new_line_before_members_in_object_initializers = true
csharp_new_line_before_members_in_anonymous_types = true
csharp_new_line_between_query_expression_clauses = true
```

For more information, please refer to the [documentation](/docs/Configuration.md)
