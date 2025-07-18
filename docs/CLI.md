---
hide_table_of_contents: true
---
Use the `dotnet csharpier` command to run CSharpier from the command line.

In practice, it will look something like:
```shell
dotnet csharpier format .
```
This command will format all c# files in the current directory and its children.

You may want to set up an [ignore file](Ignore.md) or [configuration file](Configuration.md).

## Commands
### Format
`dotnet csharpier format [<directoryOrFile\>]`

If a list of paths is supplied
- if the path points to an existing file, CSharpier will format that file
- if the path points to an existing directory, CSharpier will recursively format the contents of that directory

If a list of paths is not supplied, then stdin is read as a file, formatted and written to stdout
- When no path is specified via `stdin-path`
  - No ignore files are considered.
  - The current directory is considered when locating options
  - The file is assumed to be c# unless the first non-whitespace character is `<` in which case it is assumed to be xml.


#### Options
- `--config-path`

    If your configuration file lives in a location that CSharpier would not normally resolve it (such as in a config folder)
    you can pass the path for the configuration file to CSharpier.
    ```bash
    dotnet csharpier format . --config-path "./config/.csharpierrc"
    
    # also supports any name for the config file
    dotnet csharpier format . --config-path "./config/csharpier.yaml"

    # allows passing in a path to an editorconfig
    dotnet csharpier format . --config-path "./config/.editorconfig"
    ```

- `--ignore-path`

  If your ignore file lives in a location that CSharpier would not normally resolve it (such as in a config folder)
  you can pass the path for the ignore file to CSharpier.
    ```bash
    dotnet csharpier format . --ignore-path "./config/.csharpierignore"
    ```

- `--stdin-path`

  If you are piping input to csharpier and want to specify a path to be used for resolving options and determining if a file is ignored
    ```bash
    "public class Class { }" | dotnet csharpier format --stdin-path "MyFile.cs"
    ```

- `--log-format`

  Log output format
  - Console (default) - Formats messages in a human readable way for console interaction.
  - MsBuild - Formats messages in standard error/warning format for MSBuild.

- `--log-level`

    Changes the level of logging output. Valid options are:
  - None
  - Error
  - Warning
  - Information (default)
  - Debug


- `--no-cache`

    This option can be used to bypass the cache that is normally used to speed up formatting files.  
    By default the following are used as cache keys and a file is only formatted if one of them has changed.
    * CSharpier Version
    * CSharpier Options
    * Content of the file
    <br/><br/>
    The cache is stored at `[LocalApplicationData]/CSharpier/.formattingCache`.


- `--include-generated`

    By default CSharpier ignores any files generated by the SDK or that begin with a comment that contains `<autogenerated` or `<auto-generated`. This option can be used to format those files.


- `--skip-validation`

    CSharpier validates the changes it makes to a file.
    For C# it does this by comparing the syntax tree before and after formatting, but ignoring any whitespace trivia in the syntax tree.
    If a file fails validation CSharpier will output an error message. If this happens it indicates a bug in CSharpier's code.  
    This validation may be skipped by passing the --skip-validation argument.
    <br/><br/>
    An example of CSharpier finding a file that failed validation.
    ```
    \src\[Snip]\AbstractReferenceFinder_GlobalSuppressions.cs       - failed formatting validation
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

- `--write-stdout`

    By default CSharpier will format files in place. This option allows you to write the formatting results to stdout.
    <br/><br/>
    If you pipe input to CSharpier it will also write the formatting results to stdout.
    <br/><br/>
    *TestFile.cs*
    ```csharp
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


- `--no-msbuild-check`

    Bypass the check to determine if a csproj files references a different version of CSharpier.MsBuild.


- `--skip-write`

    Skip writing changes. Generally used for testing to ensure csharpier doesn't throw any errors or cause syntax tree validation failures.


- `--compilation-errors-as-warnings`

    Treat compilation errors from files as warnings instead of errors.

### Check
`dotnet csharpier check [<directoryOrFile\>]`

Used to check if your files are already formatted. Outputs any files that have not already been formatted.
This will return exit code 1 if there are unformatted files which is useful for CI pipelines.

#### Options
See the `format` command for descriptions of these options
- `--include-generated`
- `--no-msbuild-check`
- `--compilation-errors-as-warnings`
- `--config-path`
- `--log-format`
- `--log-level`

### Server
`dotnet csharpier server`

Running csharpier to format a single file is slow because of the overhead of starting up dotnet.
This option starts up an http server with an endpoint for formatting files. This is mainly used by IDE plugins
to drastically improve formatting time.

#### Options
- `--server-port`

    Specify the port that CSharpier should start on. Defaults to a random unused port.

### Pipe Files
`dotnet csharpier pipe-files`

Running csharpier to format a single file is slow because of the overhead of starting up dotnet.
This option keeps csharpier running so that multiple files can be formatted by piping input to the running process. This is mainly used by IDE plugins
to drastically improve formatting time.  
The input is a '\u0003' delimited list of file names followed by file contents.  
The results are written to stdout delimited by \u0003.  
For an example of implementing this in code see [this example](https://github.com/belav/csharpier/blob/main/Src/CSharpier.VSCode/src/CSharpierProcessPipeMultipleFiles.ts)
```bash
$ [FullPathToFile]\u0003[FileContents]\u0003[FullPathToFile]\u0003[FileContents]\u0003 | csharpier --pipe-multiple-files
public class ClassName
{
    public string Field;
}
\u0003
public class ClassName
{
    public string Field;
}
```
