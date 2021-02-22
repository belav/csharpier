## CSharpier
CSharpier is an opinionated code formatter for c#. It uses Roslyn to parse your code and re-prints it using its own rules. The printing process was ported from [prettier](https://github.com/prettier/prettier) but has evolved over time.

### Work In Progress
CSharpier is currently in alpha and I do not recommend using it on production code.
  - It has been tested against a large number of public repositories to validate that the changes it makes do not lose any syntax but there is a possibility it will break your code.
  - The rules it uses to format code are not yet finalized and some of the results are ugly.
  - There are currently no options exposed for formatting.
  - The options for what files it should format are limited.

I encourage you to try it out on your own code and report any bugs, code that doesn't format well, or opinions on how you think the formatting should change.

### Features
  - It is fast. It can format a solution of ~11,000 files in ~30 seconds.
  - It supports validating the syntax of the code it produces to ensure the only changes made were whitespace and line breaks.
  - It formats c# <= 9.0

Try it out at [Playground](https://csharpier.bnt-studios.com)

### Before
```c#
public class ClassName {
    public string ShortPropertyName {
        get;
        set; 
    }

    public void LongUglyMethod(string longParameter1, string longParameter2, string longParameter3) { 
        this.LongUglyMethod("1234567890", "abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
    }
}
```

### After
```c#
public class ClassName
{
    public string ShortPropertyName { get; set; }

    public void LongUglyMethod(
        string longParameter1,
        string longParameter2,
        string longParameter3)
    {
        this.LongUglyMethod(
            "1234567890",
            "abcdefghijklmnopqrstuvwxyz",
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
    }
}
```

## Installation
CSharpier is available at TODO
```
dotnet tool install -g csharpier
```

## Usage
TODO
show examples
show command line arguments

### Syntax Tree Validation
CSharpier supports validating the changes it made to a file. It does this by comparing the syntax tree before and after formatting, but ignoring any whitespace trivia in the syntax tree. If a file fails validation, CSharpier will output the lines that differ.
For example
```
\src\[Snip]\AbstractReferenceFinder_GlobalSuppressions.cs       - failed syntax tree validation
    Original: Around Line 280
            }

            if (prefix.Span[^2] is < 'A' or > 'Z')
            {
                return false;
            }

            if (prefix.Span[^1] is not ':')
    Formatted: Around Line 330
            }

            if (prefix.Span[^2] is )
            {
                return false;
            }

            if (prefix.Span[^1] is not ':')
```

## Development
See [Development Readme](Src/README.md)

