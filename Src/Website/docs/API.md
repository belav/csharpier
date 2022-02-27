---
hide_table_of_contents: true
---
CSharpier can be used to programmatically format code.

This requires adding the [CSharpier.Core](https://www.nuget.org/packages/CSharpier.Core/) nuget package to your project.
```bash
dotnet add package CSharpier.Core
```

```csharp

var unformattedCode = "public class ClassName   { }"

var formattedCode = CodeFormatter.Format(unformattedCode);
var asyncFormattedCode = await CodeFormatter.FormatAsync(unformattedCode);

var options = new CodeFormatterOptions { Width = 60 };

var narrowerCode = CodeFormatter.Format(unformattedCode, options);
var asyncNarrowerCode = await CodeFormatter.FormatAsync(unformattedCode, options);

```
