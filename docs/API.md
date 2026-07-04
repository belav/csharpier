---
hide_table_of_contents: true
---
CSharpier can be used to programmatically format code.

This requires adding the [CSharpier.Core](https://www.nuget.org/packages/CSharpier.Core/) nuget package to your project.
```bash
dotnet add package CSharpier.Core
```

### Formatting C#

```csharp

var unformattedCode = "public class ClassName   { }";

var formattedCode = CSharpFormatter.Format(unformattedCode).Code;
var asyncFormattedCode = await CSharpFormatter.FormatAsync(unformattedCode).Code;

var options = new CodeFormatterOptions { Width = 60 };
var narrowerCode = CSharpFormatter.Format(unformattedCode, options);
var asyncNarrowerCode = await CSharpFormatter.FormatAsync(unformattedCode, options);

var codeWithErrorDiagnostics = "public class ClassName   {";
var result = CSharpFormatter.Format(codeWithErrorDiagnostics);
if (result.ErrorDiagnostics.Any())
{
    // result.Code will still be the unformatted code, it is not possible to format code that can't compile
    // result.ErrorDiagnostics will contain all the errors from attempting to format the code
}
```

### Formatting XML
```csharp
var unformattedXml = "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><LangVersion>4</LangVersion></PropertyGroup></Project>
var formattedXml = XmlFormatter.Format(unformattedXml).Code;
```
