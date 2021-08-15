
### Command Line Options
```console
Usage:
  dotnet-csharpier [options] [<directoryOrFile>]

Arguments:
  <directoryOrFile>    One or more paths to a directory containing files to format or a file to format. If a path is not specified the current directory is used

Options:
  --check           Check that files are formatted. Will not write any changes.
  --fast            Skip comparing syntax tree of formatted file to original file to validate changes.
  --skip-write      Skip writing changes. Generally used for testing to ensure csharpier doesn't throw any errors or cause syntax tree validation failures.
  --write-stdout    Write the results of formatting any files to stdout.
  --version         Show version information
  -?, -h, --help    Show help and usage information


```

### \[<directoryOrFile\>]
 
If a list of paths is supplied
- if the path points to an existing file, CSharpier will format that file
- if the path points to an existing directory, CSharpier will recursively format the contents of that directory

If a list of paths is not supplied, then stdin is read.

### --check
Used to check if your files are already formatted. Outputs any files that have not already been formatted.
This will return exit code 1 if there are unformatted files which is useful for CI pipelines.

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

### --write-stdout
By default CSharpier will format files in place. This option allows you to write the formatting results to stdout.

If you pipe input to CSharpier it will also write the formatting results to stdout.

*TestFile.cs*
```c#
public class ClassName
{
    public string Field;
}
```
*shell*
```bash
$ cat TestFile.cs | dotnet csharpier
public class ClassName
{
    public string Field;
}
```