![CSharpier](./banner.svg)

CSharpier is an opinionated code formatter for c#. It uses Roslyn to parse your code and re-prints it using its own rules. 
The printing process was ported from [prettier](https://github.com/prettier/prettier) but has evolved over time.

CSharpier provides a few basic options that affect formatting and has no plans to add more. It follows the [Option Philosophy](https://prettier.io/docs/en/option-philosophy.html) of prettier.

### Quick Start
Install CSharpier globally using the following command.
```bash
dotnet tool install csharpier -g
```
Then format the contents of a directory and its children with the following command.
```bash
dotnet csharpier .
```

CSharpier can also format [on save in your editor](https://csharpier.com/docs/Editors) or as a [pre-commit hook](https://csharpier.com/docs/Pre-commit). Then you can ensure code was formatted with a [CI/CD tool](https://csharpier.com/docs/ContinuousIntegration).

---

[Read the documentation](https://csharpier.com)    
  
[Try it out](https://playground.csharpier.com)

---

### Before
```c#
public class ClassName {
    public void CallMethod() { 
        this.LongUglyMethod("1234567890", "abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
    }
}
```

### After
```c#
public class ClassName
{
    public void CallMethod()
    {
        this.LongUglyMethod(
            "1234567890",
            "abcdefghijklmnopqrstuvwxyz",
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        );
    }
}
```

## Contributing
See [Development Readme](CONTRIBUTING.md)  

Join Us [![Discord](https://img.shields.io/badge/Discord-chat?label=&logo=discord&logoColor=ffffff&color=7389D8&labelColor=6A7EC2)](https://discord.gg/HfAKGEZQcX)
