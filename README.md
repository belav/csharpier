## CSharpier
CSharpier is an opinionated code formatter for c#. It uses Roslyn to parse your code and re-prints it using its own rules. The printing process was ported from [prettier](https://github.com/prettier/prettier) but has evolved over time.

### Work In Progress
CSharpier is currently in alpha and I'd be hesitant to recommend using it on production code.
  - It has been tested against a large number of public repositories to validate that the changes it makes do not lose any syntax but there is a possibility it will break your code.
  - The rules it uses to format code are not yet finalized and some of the results are ugly.
  - The rules it uses to format your code will change over time.
  - There are currently no options exposed for formatting.
  - The options for what files it should format are limited.

I'm currently using CSharpier to format some small projects I'm working on and I've only run into a few of critical bugs since releasing it in alpha.
  - One bug dealt with file encoding, and would save some files with an incorrect encoding - this resulted in files that would not compile.
  - Support for formatting a Record type's primary constructor was missing. CSharpier reported the missing type and did not format the file.
  - CSharpier had a case where it would insert an extra new line on each format of a file.
  - If you kill the CSharpier process while it is formatting a file, there is a chance it will leave a file half written. This will be fixed shortly.

I encourage you to try it out on your own code and report any bugs, code that doesn't format well, or opinions on how you think the formatting should change. If you can live with the fact that the formatting will be changing over time, it is reasonably safe to use.

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
```console
dotnet tool install -g csharpier
```

## Usage
### Basic Usage
Run csharpier from the directory you wish to format.
```console
# the first time running csharpier it is normally not possible to review all the changes it makes
# and catch any instances of code being lost 
dotnet csharpier --validate

# after a project has already been csharpiered, you can choose
# to skip the --validate flag to speed up formatting
dotnet csharpier
```

### Formatting Single File
```console
dotnet chsarpier [PathToFile]
```

### More Information
CSharpier currently excludes .g.cs and .cshtml.cs files.

By default csharpier will validate any files it formats by comparing the resulting syntax tree to the original.
Formatting will take longer, but csharpier will validate the formatted syntax tree against the original and warn if it believes it introduced breaking changes. 

```console
Usage:
  dotnet-csharpier [options] [<directory>]

Arguments:
  <directory>    A path to a directory containing files to format. If a path is not specified the current directory is used

Options:
  -f, --fast    Skip comparing syntax tree of formatted file to original file to validate changes.
  --version         Show version information
  -?, -h, --help    Show help and usage information

```

### \<directory\>
Currently CSharpier only supports being passed a directory to recursively scan for .cs files.
If a directory is not supplied, it will use the current directory.

### --fast
CSharpier validates the changes it makes to a file. 
It does this by comparing the syntax tree before and after formatting, but ignoring any whitespace trivia in the syntax tree.
If a file fails validation, CSharpier will output the lines that differ. If this happens it indicates a bug in CSharpier's code.  
This validation may be skipped by passing the --fast argument. Validation appears to increase the formatting time by ~50%.

An example of CSharpier finding a file that failed validation.
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

## IDE Support
### Rider
1. Open Settings
2. Tools - File Watchers
3. Add New File Watcher
    * File Type: C# File
    * Program: dotnet
    * Arguments: csharpier $FilePath$
    * Output paths to refresh: $FilePath$
    * Advanced Options - Auto-save edited files...: This should probably be off otherwise if you pause while coding csharpier will reformat the file as is.

## Development
See [Development Readme](Src/README.md)

