---
title: What is CSharpier?
id: About
hide_table_of_contents: true
---

CSharpier is an opinionated code formatter for c#. It uses Roslyn to parse your code and re-prints it using its own rules.
The printing process was ported from [prettier](https://github.com/prettier/prettier) but has evolved over time.  

CSharpier provides a few basic options that affect formatting and follows the [Option Philosophy](https://prettier.io/docs/en/option-philosophy.html) of prettier. Option requests are out of scope for CSharpier, they will be closed without discussion.

### Quick Start
Install CSharpier in a project with the following command.
```bash
dotnet tool install csharpier
```
Then format the contents of the project
```bash
dotnet csharpier .
```

See [Install a local tool](https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools#install-a-local-tool) and [CLI Usage](https://csharpier.com/docs/CLI) for more information

CSharpier can also format [on save in your editor](https://csharpier.com/docs/Editors), as a [pre-commit hook](https://csharpier.com/docs/Pre-commit), as [part of your build](https://csharpier.com/docs/MSBuild) or even [programatically](https://csharpier.com/docs/API). Then you can ensure code was formatted with a [CI/CD tool](https://csharpier.com/docs/ContinuousIntegration).

---

[Try it out](https://playground.csharpier.com)

---

### Before
```csharp
public class ClassName {
    public void CallMethod() { 
        var shuffle = shuffle.Skip(26).LogQuery("Bottom Half").InterleaveSequenceWith(shuffle.Take(26).LogQuery("Top Half"), shuffle.Skip(26).LogQuery("Bottom Half")).LogQuery("Shuffle").ToArray();
    }
}
```

### After
```csharp
public class ClassName
{
    public void CallMethod()
    {
        var shuffle = shuffle
            .Skip(26)
            .LogQuery("Bottom Half")
            .InterleaveSequenceWith(
                shuffle.Take(26).LogQuery("Top Half"),
                shuffle.Skip(26).LogQuery("Bottom Half")
            )
            .LogQuery("Shuffle")
            .ToArray();
    }
}


```
