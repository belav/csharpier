# 0.29.0
## Breaking Changes
### The formatting command will now exit with an error code of 1 if one of the target files cannot be compiled [#1131](https://github.com/belav/csharpier/issues/1131)
Prior to 0.29.0 if csharpier encountered a file that could not be compiled it would treat it as a warning and exit with a code of 0.  
As of 0.29.0 a file that cannot be compiled is now treated as an error and csharpier will exit with code 1

## What's Changed
### Enforce trailing commas in object and collection initializer [#668](https://github.com/belav/csharpier/issues/668)
CSharpier will now add trailing commas automatically where appropriate. It will collapse to a single line and remove the trailing comma in cases where everything fits on one line.
```c#
// input
public enum SomeEnum
{
    Value1,
    Value2
}

string[] someArray = new string[]
{
    someLongValue_____________________________________________,
    someLongValue_____________________________________________
};

string[] someArray = new string[]
{
    someValue,
    someValue,
};

// 0.29.0
public enum SomeEnum
{
    Value1,
    Value2,
}

string[] someArray = new string[]
{
    someLongValue_____________________________________________,
    someLongValue_____________________________________________,
}

string[] someArray = new string[] { someValue, someValue };
```
Many thanks go to @dawust for the contribution.

### Support for formatting custom file extensions [#1220](https://github.com/belav/csharpier/issues/1220)
Prior to 0.29.0 csharpier would only format files with an extension of .cs or .csx. It is now possible to configure csharpier to format other files extensions, and to specify configuration options per file extension.
See https://csharpier.com/docs/Configuration#configuration-overrides for more details.

### Invalid blank line being added with lambda returning collection expression [#1306](https://github.com/belav/csharpier/issues/1306)
```c#
// input & expected output
CallMethod(_ =>
    [
        LongValue________________________________________________,
        LongValue________________________________________________,
    ]
);

// 0.28.2
CallMethod(_ =>

    [
        LongValue________________________________________________,
        LongValue________________________________________________,
    ]
);

```
### Switch expressions do not break consistently with other lambdas [#1282](https://github.com/belav/csharpier/issues/1282)
Prior to 0.29.0 csharpier would break before the `=>` in switch expression arms. It now breaks after them to be consistent with other lambda expressions.
```c#
// 0.28.2
return someEnum switch
{
    Value1 => someOtherValue,
    Value2
    or Value3
        => someValue________________________________________________________________________,
    Value4
        => someValue_____________________________________________________________________________,
};

// 0.29.0
return someEnum switch
{
    Value1 => someOtherValue,
    Value2 or Value3 =>
        someValue________________________________________________________________________,
    Value4 =>
        someValue_____________________________________________________________________________,
};

```
### Formatting of empty collection initializer for huge type [#1268](https://github.com/belav/csharpier/issues/1268)
Empty collection expression initializers formatting was including a break plus indentation resulting in poor formatting.
```c#
// 0.28.2
var someObject = new List<(
    int Field1__________________________________,
    int Field2__________________________________
)>
{
    };

// 0.29.0
var someObject = new List<(
    int Field1__________________________________,
    int Field2__________________________________
)>
{ };

```

Thanks go to @Rudomitori for the contribution

### Switch expression single line broken when preceded by comment [#1262](https://github.com/belav/csharpier/issues/1262)
Improved formatting for short expression arms that have a leading comment.
```c#
// 0.28.2
return someValue switch
{
    // comment
    Some.One
        => 1,
    Some.Two => 2,
};

return someValue switch
{
    Some.One => 1,
    // comment
    Some.Two
        => 2,
};

// 0.29.0
return someValue switch
{
    // comment
    Some.One => 1,
    Some.Two => 2,
};

return someValue switch
{
    Some.One => 1,
    // comment
    Some.Two => 2,
};

```
### Incorrect formatting of ternary expression with a comment after an interpolated string [#1258](https://github.com/belav/csharpier/issues/1258)
Fixed bug with comments on a ternary expression that resulted in invalid code.
```c#
// input & expected output
public string TrailingComment = someCondition
    ? $"empty" // trailing comment
    : someString;

// 0.28.2
public string TrailingComment = someCondition ? $"empty" // trailing comment : someString;

```
### Formatting for indexer parameters should mostly be the same as for method parameters. [#1255](https://github.com/belav/csharpier/issues/1255)
Improved formatting of indexed properties that contained attributes.
```c#
// input & expected output
public class ClassName
{
    public string this[
        [SomeAttribute] int a________________________________,
        [SomeAttribute] int b________________________________
    ] => someValue;
}

// 0.28.2
public class ClassName
{
    public string this[[SomeAttribute] int a________________________________, [SomeAttribute]
        int b________________________________] => someValue;
}

```

### Do not overwrite `CSharpier_Check` when already set. [#1314](https://github.com/belav/csharpier/pull/1314)
Fixed a bug with csharpier.msbuild where it would overwrite the `CSharpier_Check` value in some cases.

Thanks go to @PetSerAl for the contribution

### The CLI has contradictory message about directoryOrFile being required [#1296](https://github.com/belav/csharpier/issues/1296)
The help text for the cli has been improved to better indicate when `directoryOrFile` is required.

Thanks go to @marcinjahn for the contribution

### Fullwidth unicode characters should be accounted for in print width [#260](https://github.com/belav/csharpier/issues/260)
CSharpier now considers full width unicode characters such as `가` to be 2 spaces wide when determining how to format code.

**Full Changelog**: https://github.com/belav/csharpier/compare/0.28.2...0.29.0
# 0.28.2
## What's Changed
### Pipe to `dotnet csharpier` fails when subdirectory is inaccessible [#1240](https://github.com/belav/csharpier/pull/1240)
When running the following CSharpier would look for config files in subdirectories of the `pwd`. This could lead to exceptions if some of those directories were inaccessible.
```
echo "namespace Foo { public class Bar { public string Baz {get;set;}}}" | dotnet csharpier
```

Thanks go to @jamesfoster for reporting the issue.

**Full Changelog**: https://github.com/belav/csharpier/compare/0.28.1...0.28.2
# 0.28.1
## What's Changed
### Third party .editorconfig leading to: Error Failure parsing editorconfig files [#1227](https://github.com/belav/csharpier/issues/1227)
When CSharpier encountered an invalid `.editorconfig` file, it would throw an exception and not format files. These files could appear in 3rd party code (for example within node_modules). CSharpier now ignores invalid lines in `.editorconfigs`


Thanks go to @K0Te for reporting the issue

**Full Changelog**: https://github.com/belav/csharpier/compare/0.28.0...0.28.1
# 0.28.0
## What's Changed
### Fix dedented method call if there is a long chain [#1154](https://github.com/belav/csharpier/issues/1154)
In some cases of method chains, the first invocation would end up dedented.

```c#
// 0.27.3
o.Property.CallMethod(
    someParameter_____________________________,
    someParameter_____________________________
)
    .CallMethod()
    .CallMethod();

// 0.28.0
o.Property.CallMethod(
        someParameter_____________________________,
        someParameter_____________________________
    )
    .CallMethod()
    .CallMethod();
```
### Extra newline in switch case statement with curly braces [#1192](https://github.com/belav/csharpier/issues/1192
If a case statement started with a block it would get an extra new line
```c#
// 0.27.3
switch (someValue)
{
    case 0:
    {
        // dedented because the only statement is a block
        break;
    }

    case 1:

        {
            // indented because there are two statements, a block then a break
        }
        break;
}

// 0.28.0
// 0.27.3
switch (someValue)
{
    case 0:
    {
        // dedented because the only statement is a block
        break;
    }

    case 1:
        {
            // indented because there are two statements, a block then a break
        }
        break;
}
```

Thanks go to @emberTrev for reporting the bug.

### Handle more editorconfig glob patterns. [#1214](https://github.com/belav/csharpier/issues/1214)
The editorconfig parsing was not handling glob patterns that contained braces.
```editorconfig
# worked in 0.27.3
[*.cs]
indent_size = 4
tab_width = 4

# did not work in 0.27.3
[*.{cs,csx}]
indent_size = 4
tab_width = 4

# did not work in 0.27.3
[*.{cs}]
indent_size = 4
tab_width = 4
```

Thanks go to @kada-v for reporting the bug 

### Ignore-start combined with regions throws exception [#1197](https://github.com/belav/csharpier/issues/1197)
The following code would throw an exception, it is now working as expected.
```c#
class ClassName
{
    #region Region
    // csharpier-ignore-start
    public string   Field;
    // csharpier-ignore-end
    #endregion
}
```
Thanks go to @davidescapolan01 for reporting the bug

### Cannot format project containing editorconfig [#1194](https://github.com/belav/csharpier/issues/1194)
On some OSs the following would cause an exception.
```bash
dotnet new console -n foo
cd foo
dotnet new editorconfig
dotnet csharpier ./
```

Thanks go to @hashitaku for contributing the fix.

### Expose IncludeGenerated in CodeFormatterOptions [#1215](https://github.com/belav/csharpier/issues/1215)
`CodeFormatterOptions.IncludeGenerated` is now available for the SDK.

### Returning errors + status from csharpier http server [#1191](https://github.com/belav/csharpier/pull/1191)
Improved the http server that CSharpier will soon use to facilitate formatting by plugins. The formatting request now returns errors and a status for each file formatted.
This allows the plugin to provide more information to the user when they attempt to format a file. The plugins will be updated to use the http server option for CSharpier 0.28.0+


**Full Changelog**: https://github.com/belav/csharpier/compare/0.27.3...0.28.0
# 0.27.3
## What's Changed
### Add more options to CodeFormatterOptions [#1172](https://github.com/belav/csharpier/issues/1172)
The API for CSharpier was only exposing `CodeFormatterOptions.PrintWidth`. It is now in sync with the CLI and exposes all of the available options
```c#
public class CodeFormatterOptions
{
    public int Width { get; init; } = 100;
    public IndentStyle IndentStyle { get; init; } = IndentStyle.Spaces;
    public int IndentSize { get; init; } = 4;
    public EndOfLine EndOfLine { get; init; } = EndOfLine.Auto;
}
```

Thanks go to @Phault for the contribution
### Extra indent when call method on RawStringLiteral [#1169](https://github.com/belav/csharpier/issues/1169)
When a raw string literal was the first argument to a method call, it was getting an extra indent.

```c#
// input & expected output
CallMethod(
    """
    SomeRawString
    """.CallMethod()
);

// 0.27.2
CallMethod(
    """
        SomeRawString
        """.CallMethod()
);

```

Thanks go to @Rudomitori for reporting the bug.
### Using aliases sorting is not always the same depending on the input order [#1168](https://github.com/belav/csharpier/issues/1168)
Using aliases were not sorting properly, resulting differing outputs and unstable formatting.

Inputs of

```c#
using A = string;
using B = string;
using C = string;
using D = string;
```
And
```c#
using D = string;
using C = string;
using B = string;
using A = string;
```
Now always result in properly sorted output of
```c#
using A = string;
using B = string;
using C = string;
using D = string;
```

Thanks go to @Araxor for reporting the bug.
### Spread (in collection expression) are not formatted [#1167](https://github.com/belav/csharpier/issues/1167)
The spread element was unformatted, and left as is. It is now formatted as follows.
```c#
int[] someArray = [.. someOtherArray];
int[] someOtherArray = [.. value1, .. value2, .. value3];

int[] someOtherArray =
[
    .. value1________________________________,
    .. value2________________________________,
    .. value3________________________________
];
```

Thanks go to @jods4 for reporting the bug.
### Fix empty line before collection expression in attribute [#1164](https://github.com/belav/csharpier/pull/1160)
A collection expression in an attribute resulted in an extra line before the collection expression.
```c#
// input & expected output
[SomeAttribute(
    [
        someValue_______________________________________________,
        someValue_______________________________________________,

    ]
)]
class ClassName { }

// 0.27.2
[SomeAttribute(

    [
        someValue_______________________________________________,
        someValue_______________________________________________,
    ]
)]
class ClassName { }

```

Thanks go to @Rudomitori for reporting the bug.
### using static System.* usings not ordered before other static usings like using System.* ones [#1162](https://github.com/belav/csharpier/issues/1162)
Static usings were not following the rule that `System.*` should be sorted to the top.
```c#
// input & expected output
using static System;
using static System.Web;
using static AWord;
using static ZWord;

// 0.27.2
using static AWord;
using static System;
using static System.Web;
using static ZWord;
```

### Remove hash from version [#1144](https://github.com/belav/csharpier/issues/1144)
When `.net8` support was added, CSharpier started including a commit hash in the version number output. This was due to a [breaking change](https://github.com/dotnet/sdk/issues/34568) in the sdk.
```bash
> dotnet csharpier --version
0.27.2+b456544aad8957d0e2026afe1a37544bb74552ba
```

CSharpier no longer includes the commit hash
```bash
> dotnet csharpier --version
0.27.3
```

**Full Changelog**: https://github.com/belav/csharpier/compare/0.27.2...0.27.3
# 0.27.2
## What's Changed
### Orphan variable since 0.27.1 [#1153](https://github.com/belav/csharpier/issues/1153)
0.27.1 introduced the following formatting regression, resulting in short variables being orphaned on a line
```c#
// 0.27.1
o
    .Property.CallMethod(
        someParameter_____________________________,
        someParameter_____________________________
    )
    .CallMethod()
    .CallMethod();

// 0.27.2
o.Property.CallMethod(
    someParameter_____________________________,
    someParameter_____________________________
)
    .CallMethod()
    .CallMethod();
```

Thanks go to @aurnoi1 for reporting the bug

### Better support for CSharp Script [#1141](https://github.com/belav/csharpier/issues/1141)
Version 0.27.1 parsed `.csx` files as if they were C#, so it could only format simple ones. It now parses them as CSharpScript files so it can format them properly.

Thanks go to @Eptagone for reporting the bug.

**Full Changelog**: https://github.com/belav/csharpier/compare/0.27.1...0.27.2
# 0.27.1
## What's Changed
### Support for CSharp Script [#1141](https://github.com/belav/csharpier/issues/1141)
Previously CSharpier would only format files matching `*.cs` which prevented it from formatting C# script files. It now formats `*.{cs,csx}`

Thanks go to @Eptagone for the suggestion
### Weird formatting of invocation chain [#1130](https://github.com/belav/csharpier/issues/1130)
Invocation chains that started with an identifier <= 4 characters were causing a strange break in the first method call. There were other edge cases cleaned up while working on the fix.

```c#
// 0.27.0
var something________________________________________ = x.SomeProperty.CallMethod(
    longParameter_____________,
    longParameter_____________
)
    .CallMethod();

// 0.27.1
var something________________________________________ = x
    .SomeProperty.CallMethod(longParameter_____________, longParameter_____________)
    .CallMethod();
```

```c#
// 0.27.0
var someLongValue_________________ = memberAccessExpression[
    elementAccessExpression
].theMember______________________________();

// 0.27.1
var someLongValue_________________ = memberAccessExpression[elementAccessExpression]
    .theMember______________________________();
```

```c#
// 0.27.0
someThing_______________________
    ?.Property
    .CallMethod__________________()
    .CallMethod__________________();

// 0.27.1
someThing_______________________
    ?.Property.CallMethod__________________()
    .CallMethod__________________();
```

Thanks go to @Rudomitori for reporting the issue
### "Failed syntax tree validation" for raw string literals [#1129](https://github.com/belav/csharpier/issues/1129)
When an interpolated raw string changed indentation due to CSharpier formatting, CSharpier was incorrectly reporting it as failing syntax tree validation.
```c#
// input
CallMethod(CallMethod(
   $$"""
   SomeString
   """, someValue));

// output
CallMethod(
    CallMethod(
        $$"""
        SomeString
        """,
        someValue
    )
);
```
Thanks go to @Rudomitori for reporting the issue

### Adding experimental support using HTTP for the extensions to communicate with CSharpier [#1137](https://github.com/belav/csharpier/pull/1137)
The GRPC support added in 0.27.0 increased the size of the nuget package significantly and has been removed.

CSharpier can now start a kestrel web server to support communication with the extensions once they are all updated.

**Full Changelog**: https://github.com/belav/csharpier/compare/0.27.0...0.27.1
# 0.27.0
## What's Changed
### Improve formatting of lambda expressions [#1066](https://github.com/belav/csharpier/pull/1066)
Many thanks go to @Rudomitori for contributing a number of improvements to the formatting of lambda expressions.

Some examples of the improvements.
```c#
// input
var affectedRows = await _dbContext.SomeEntities
    .ExecuteUpdateAsync(
        x => 
            x.SetProperty(x => x.Name, x => command.NewName)
                .SetProperty(x => x.Title, x => command.NewTItle)
                .SetProperty(x => x.Count, x => x.Command.NewCount)
    );

// 0.27.0
var affectedRows = await _dbContext.SomeEntities
    .ExecuteUpdateAsync(x =>
        x.SetProperty(x => x.Name, x => command.NewName)
            .SetProperty(x => x.Title, x => command.NewTItle)
            .SetProperty(x => x.Count, x => x.Command.NewCount)
    );
```

```c#
// input
builder.Entity<IdentityUserToken<string>>(b =>
{
    b.HasKey(
        l =>
            new
            {
                l.UserId,
                l.LoginProvider,
                l.Name
            }
    );
    b.ToTable("AspNetUserTokens");
});

// 0.27.0
builder.Entity<IdentityUserToken<string>>(b =>
{
    b.HasKey(l => new
    {
        l.UserId,
        l.LoginProvider,
        l.Name
    });
    b.ToTable("AspNetUserTokens");
});
```

```c#
// input
table.PrimaryKey(
    "PK_AspNetUserTokens",
    x =>
        new
        {
            x.UserId,
            x.LoginProvider,
            x.Name
        }
);

// 0.27.0
table.PrimaryKey(
    "PK_AspNetUserTokens",
    x => new
    {
        x.UserId,
        x.LoginProvider,
        x.Name
    }
);
```

### `readonly ref` is changed to `ref readonly` causing error CS9190 [#1123](https://github.com/belav/csharpier/issues/1123)
CSharpier was sorting modifiers in all places they occurred. Resulting the following change that led to code that would not compile.
```c#
// input
void Method(ref readonly int someParameter) { }

// 0.26.7
void Method(readonly ref int someParameter) { }

// 0.27.0
void Method(ref readonly int someParameter) { }
```
Thanks go to @aurnoi1 for reporting the bug
### #if at the end of collection expression gets eaten [#1119](https://github.com/belav/csharpier/issues/1119)
When a collection expression contained a directive immediately before the closing bracket, that directive was not included in the output.

```c#
// input
int[] someArray =
[
    1
#if DEBUG
    ,
    2
#endif
];

// 0.26.7
int[] someArray = [1];

// 0.27.0
int[] someArray =
[
    1
#if DEBUG
    ,
    2
#endif
];
```

Thanks go to @Meowtimer for reporting the bug
### CSharpier.MsBuild - Set Fallback for dotnetcore3.1 or net5.0 applications [#1111](https://github.com/belav/csharpier/pull/1111)
CSharpier.MsBuild made an assumption that the project being built would be built using net6-net8 and failed when the project was built with earlier versions of dotnet.

It now falls back to trying to use `net8`

Thanks go to @samtrion for the contribution
### Allow empty/blank lines in object initializers [#1110](https://github.com/belav/csharpier/issues/1110)
Large object initializers now retain single empty lines between initializers.

```c#
vvar someObject = new SomeObject
{
    NoLineAllowedAboveHere = 1,

    ThisLineIsOkay = 2,

    // comment
    AndThisLine = 3,
    DontAddLines = 4,
};
```

Thanks go to @Qtax for the suggestion
### Add option to allow formatting auto generated files. [#1055](https://github.com/belav/csharpier/issues/1055
By default CSharpier will not format files that were generated by the SDK, or files that begin with `<autogenerated />` comments.

Passing the option `--include-generated` to the CLI will cause those files to be formatted.

### Format raw string literals indentation [#975](https://github.com/belav/csharpier/issues/975)
CSharpier now adjusts the indentation of raw string literals if the end delimiter is indented.

```c#
// input
var someString = """
            Indent based on previous line
            """;

var doNotIndentIfEndDelimiterIsAtZero = """
Keep This
    Where It
Is
""";

// 0.26.7
var someString = """
            Indent based on previous line
            """;

var doNotIndentIfEndDelimiterIsAtZero = """
Keep This
    Where It
Is
""";

// 0.27.0
var someString = """
    Indent based on previous line
    """;

var doNotIndentIfEndDelimiterIsAtZero = """
Keep This
    Where It
Is
""";
```

Thanks go to @jods4 for reporting the issue
### Incorrect indentation on a multi-line statement split by comments [#968](https://github.com/belav/csharpier/issues/968
CSharpier was not properly indenting an invocation chain when it was being split by comments.
```c#
// input
var someValue =
    // Some Comment
    CallSomeMethod()
        // Another Comment
        .CallSomeMethod();

// 0.26.7
var someValue =
// Some Comment
CallSomeMethod()
    // Another Comment
    .CallSomeMethod();

// 0.27.0
var someValue =
    // Some Comment
    CallSomeMethod()
        // Another Comment
        .CallSomeMethod();
```

Thanks go to @tyrrrz for reporting the issue
### Adding experimental support for GRPC for the extensions to communicate with CSharpier [#944](https://github.com/belav/csharpier/pull/944)
Currently the extensions for CSharpier send data to a running instance of CSharpier by piping stdin/stdout back and forth. This approach has proved problematic and hard to extend.

As of 0.27.0, CSharpier can run a GRPC server to allow communication with the extensions once they are all updated.

**Full Changelog**: https://github.com/belav/csharpier/compare/0.26.7...0.27.0
# 0.26.7
## What's Changed
### Keep Field.Method() on the same line when breaking long method chain [#1010](https://github.com/belav/csharpier/issues/1010)
0.26.0 introduced changes that broke long invocation chains on fields/properties as well as methods. That change has been reverted after community feedback.

```c#
// 0.26.0
var loggerConfiguration = new LoggerConfiguration()
    .Enrich
    .FromLogContext()
    .Enrich
    .WithProperty("key", "value")
    .Enrich
    .WithProperty("key", "value")
    .Enrich
    .WithProperty("key", "value")
    .Enrich
    .WithProperty("key", "value")
    .WriteTo
    .Console(outputTemplate: "template");

// 0.26.7
var loggerConfiguration = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("key", "value")
    .Enrich.WithProperty("key", "value")
    .Enrich.WithProperty("key", "value")
    .Enrich.WithProperty("key", "value")
    .WriteTo.Console(outputTemplate: "template");
```

**Full Changelog**: https://github.com/belav/csharpier/compare/0.26.6...0.26.7
# 0.26.6
## What's Changed
### CSharpier incorrectly reports problems with differing line endings as "The file did not end with a single newline"[#1067](https://github.com/belav/csharpier/issues/1067)
If CSharpier was validating that a file was formatted, and that file contained only `\n` but CSharpier was configured to use `\r\n`, then it would report the problem as `The file did not end with a single newline`

CSharpier added support for reading line ending configuration from an `.editorconfig` which could contain `end_of_line = crlf` so some users were unknowingly configuring CSharpier to use `\r\n`

CSharpier now correctly reports the problem as `The file contained different line endings than formatting it would result in.`

**Full Changelog**: https://github.com/belav/csharpier/compare/0.26.5...0.26.6
# 0.26.5
## What's Changed
### 0.26.4 sorts `NSubstitute` before `Newtonsoft.Json` [#1061](https://github.com/belav/csharpier/issues/1061)
The using sorting in `0.26.4` was taking into account case.

```c#
// 0.26.4
using System;
using NSubstitute;
using Newtonsoft.Json;

// 0.26.5
using System;
using Newtonsoft.Json;
using NSubstitute;
```

Thanks go to @loraderon for contributing the fix.

### Extra newline added when using a collection expression with { get; } [#1063](https://github.com/belav/csharpier/issues/1063)
A collection expression in a property initializer was including an extra new line.
```c#
// 0.26.4
public class ClassName
{
    public List<DayOfWeek> DaysOfWeek { get; } =

        [
            DayOfWeek.Sunday,
            // snip
            DayOfWeek.Saturday
        ];    
}

// 0.26.5
public class ClassName
{
    public List<DayOfWeek> DaysOfWeek { get; } =
        [
            DayOfWeek.Sunday,
            // snip
            DayOfWeek.Saturday
        ];    
}
```
Thanks go to @SapiensAnatis for contributing the fix.

### Comments at the end of a collection expression should be indented [#1059](https://github.com/belav/csharpier/issues/1059)
When the close bracket on a collection expression had a leading comment, it had the same indentation as the bracket.
```c#
// 0.26.4
host.AddSection(
    name: "Kontakt Libraries (Third Party)",
    tags: Tags.SamplesUsed,
    tasks:
    [
    // TODO: Add any used third party instruments below as you discover them.
    ]
);

// 0.26.5
host.AddSection(
    name: "Kontakt Libraries (Third Party)",
    tags: Tags.SamplesUsed,
    tasks:
    [
        // TODO: Add any used third party instruments below as you discover them.
    ]
);
```

Thanks go to @fgimian for reporting the problem

**Full Changelog**: https://github.com/belav/csharpier/compare/0.26.4...0.26.5
# 0.26.4
## What's Changed
### Spacing bugs related to C#12 collection expressions [#1049](https://github.com/belav/csharpier/issues/1049) [#1047](https://github.com/belav/csharpier/issues/1047)
There were a number of cases where CSharpier was including extra blank lines, an extra space, or not formatting contents of collection expressions.
```c#
// 0.26.3
var a = new A { B =  [1, 2, 3] };

List<string> items = [// My item
    "Hello",];

items.AddRange(

    [
        LongValue________________________________________________,
        LongValue________________________________________________
    ]
);

items =  [];
items ??=  [];

class SomeClass
{
    public SomeValue SomeProperty =>

        [
            LongValue________________________________________________,
            LongValue________________________________________________
        ];

    public SomeValue Method() =>

        [
            LongValue________________________________________________,
            LongValue________________________________________________
        ];
}

// 0.26.4
var a = new A { B = [1, 2, 3] };

List<string> items =
[
    // My item
    "Hello",
];

items.AddRange(
    [
        LongValue________________________________________________,
        LongValue________________________________________________
    ]
);

items = [];
items ??= [];

class SomeClass
{
    public SomeValue SomeProperty =>
        [
            LongValue________________________________________________,
            LongValue________________________________________________
        ];

    public SomeValue Method() =>
        [
            LongValue________________________________________________,
            LongValue________________________________________________
        ];
}


```


Thanks go to @fgimian and @JoshWoodArup for reporting the issues
### Usings sorting differs based on system culture [#1051](https://github.com/belav/csharpier/issues/1051)
The sorting of Usings was done in a culture specific manner, resulting in unexpected behavior.\
In Czech (cs-CZ) the `ch` is a "single letter" which is placed between `h` and `i`, which resulted in the following sorting behavior.
```c#
// 0.26.3
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Channel;

// 0.26.4
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
```

Thanks go to @davidkudera for the contribution

**Full Changelog**: https://github.com/belav/csharpier/compare/0.26.3...0.26.4
# 0.26.3
## What's Changed
### C#12 Collection expressions are prefixed with two spaces [#1009](https://github.com/belav/csharpier/issues/1009)
```c#
// 0.26.0
List<int> ids =  [];

// 0.26.3
List<int> ids = [];
```

Thanks go to @Jackenmen for reporting the problem.
### CSharpier inserts extra spaces around the contents of collection expressions [#1002](https://github.com/belav/csharpier/issues/1002)
```c#
// 0.26.0
List<int> ids = [ ];
List<int> ids = [ 1, 2, 3 ];

// 0.26.3
List<int> ids = [];
List<int> ids = [1, 2, 3];
```

Thanks go to @golavr for reporting the problem.
### Configuration files not respected for stdin [#1028](https://github.com/belav/csharpier/issues/1028)
When piping a file to csharpier via stdin, CSharpier uses the working directory to locate any configuration files. This was broken with `0.26.0`.

Thanks go to @kikniknik for reporting the problem.

### Modify CSharpier.MSBuild to use NETCoreSdkVersion to detect which sdk to use for running CSharpier [#1022](https://github.com/belav/csharpier/issues/1022) [#1027](https://github.com/belav/csharpier/issues/1027)
Previously CSharpier.MSBuild was using `targetFramework` to determine which version of CSharpier to run. This was problematic when there were multiple target frameworks, or the project was targeting a superset such as `net8.0-windows`

It now makes use of `NETCoreSdkVersion` to determine which version of CSharpier to run.

Thanks go to @Tyrrrz for the suggestion and to @Cjewett for the contribution to make it work
### CSharpierIgnore not respected when recursively finding .editorconfig
When looking for `.editorconfig` files, CSharpier looks for them recursively in the current directory. This logic was not taking into account any files or directories ignored by a `.csharpierignore`.

Thanks go to @sebastieng84 for the contribution.
### Optimize editorconfig lookups when piping files [#1039](https://github.com/belav/csharpier/pull/1039)
CSharpier now only looks for an `.editorconfig` for the file being piped to CSharpier. Under normal usage it recursively looks for all possible `.editorconfig` files for the given directory. 

**Full Changelog**: https://github.com/belav/csharpier/compare/0.26.2...0.26.3
# 0.26.2
## What's Changed
### CSharpier.MsBuild does not support DotNet 8 [#1012](https://github.com/belav/csharpier/issues/1012)
When using CSharpier.MsBuild in a setting where the project targeted net8.0 and only the net8 sdk was installed, CSharpier.MsBuild would attempt to run the net7.0 version of csharpier which failed.

Thanks go to @aditnryn for the fix
### Global System using directives should be sorted first [#1003](https://github.com/belav/csharpier/issues/1003)
Global using were not sorting `System` to the top, which was inconsistent with regular using.

```c#
// 0.26.1
global using ZWord;
global using AWord;
global using System.Web;
global using System;

// 0.26.2
global using System;
global using System.Web;
global using AWord;
global using ZWord;
```

Thanks go to @vipentti for the fix

**Full Changelog**: https://github.com/belav/csharpier/compare/0.26.1...0.26.2
# 0.26.1
## What's Changed
### Editorconfig with duplicated sections was freezing IDE's [#989](https://github.com/belav/csharpier/issues/989)
CSharpier was unable to parse an `.editorconfig` file that contained duplicate sections and would crash. This would result in a hung IDE.
```
[*]
insert_final_newline = true

[*]
spelling_languages = en-us
```

Thanks go to @echoix for helping track this down.

### A .csharpierrc file anywhere above a file now takes priority over any .editorconfig [#987](https://github.com/belav/csharpier/issues/987)
Given the following setup
```
/src/.editorconfig
/src/ProjectName/.editorconfig
/src/.csharpierrc
```

Originally with 0.26.0, the `/src/ProjectName/.editorconfig` file would be used for determining the configuration options for a file within `src/ProjectName`. This resulted in the existing options within `.csharpierrc` being ignored.

With 0.26.1, if a `.csharpierrc` exists anywhere above a given file, it will be used to determine the configuration options.

Thanks go to @parched for reporting the issue.

**Full Changelog**: https://github.com/belav/csharpier/compare/0.26.0...0.26.1
# 0.26.0
## What's Changed
### EditorConfig Support
CSharpier will now read configuration options from an `.editorconfig`. See https://csharpier.com/docs/Configuration for more details.

### Net8 Support
CSharpier now supports the .net8 sdk. It still supports net6 and net7.

### Sorting of using directives [#661](https://github.com/belav/csharpier/issues/661)
CSharpier now sorts using statements. It follows the following rules
```c#
global using System.Linq; // sort global first
using System; // sort anything in System
using NonSystem; // sort anything non-system
using static Static; // sort static
using Alias = Z; // sort alias
using SomeAlias = A;
#if DEBUG // finally any usings in #if's
using Z; // contents are not sorted as of now
using A;
#endif
```
### Remove line before the content of a bracketless if/else statement [#979](https://github.com/belav/csharpier/issues/979)
```c#
// input
if (true)

    CallMethod();
else if (false)

    CallMethod();
else

    CallMethod();

for (; ; )

    CallMethod();

while (true)

    CallMethod();

// 0.26.0
if (true)
    CallMethod();
else if (false)
    CallMethod();
else
    CallMethod();

for (; ; )
    CallMethod();

while (true)
    CallMethod();
```

Thanks go to @Infinite-3D for reporting
### Support C# 12 primary constructors on structs [#969](https://github.com/belav/csharpier/issues/969)
CSharpier now supports primary constructors on structs
```c#
public struct NamedItem2(
    string name1,
    string name2
)
{
    public string Name1 => name1;
    public string Name2 => name1;
}
```
### Support C# 12 collection expressions [#964](https://github.com/belav/csharpier/issues/964
CSharpier now supports collection expressions
```c#
int[] a =  [ 1, 2, 3, 4, 5, 6, 7, 8 ];

Span<int> b =  [ 'a', 'b', 'c', 'd', 'e', 'f', 'h', 'i' ];

string[] c =
[
    "________________________",
    "________________________",
    "________________________",
    "________________________"
];

int[][] d =
[
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
];
```

Thanks go to @meenzen for reporting
### MSBuild - when a file fails to compile csharpier interferes with getting you clickable links to the compilation errors. [#957](https://github.com/belav/csharpier/issues/957)
Build errors will now display properly when using CSharpier.MSBuild
### Format element access properly in long invocation chains [#956](https://github.com/belav/csharpier/issues/956)
```c#
// 0.25.0
var x = someLongNameField.CallMethod____________________________________().AccessArray[
    1
].Property_______________;

// 0.26.0
var x = someLongNameField
    .CallMethod____________________________________()
    .AccessArray[1]
    .Property_______________;
```
### Improvements to visible whitespace in console output. [#953](https://github.com/belav/csharpier/issues/953)
When using `cshapier --check` whitespace is now only visible in the following situations

When an otherwise empty line contains whitespace
```
----------------------------- Expected: Around Line 4 -----------------------------
    private string field1;

    private string field2;
----------------------------- Actual: Around Line 4 -----------------------------
    private string field1;
····
    private string field2;
```

When a line has extra trailing whitespace
```
----------------------------- Expected: Around Line 3 -----------------------------
{
    private string field1;
}
----------------------------- Actual: Around Line 3 -----------------------------
{
    private string field1;····
}
```
### MSBuild is not encoding using UTF8 [#947](https://github.com/belav/csharpier/issues/947)
When CSharpier.MSBuild ran into a failed csharpier check, it was not encoding the std-error output with UTF8. This resulted in messages such as
```
----------------------------- Expected: Around Line 3 -----------------------------
{
┬╖┬╖┬╖┬╖private┬╖string┬╖field1;
}
----------------------------- Actual: Around Line 3 -----------------------------
{
┬╖┬╖┬╖┬╖private┬╖string┬╖field1;┬╖┬╖┬╖┬╖
}
```
Thanks go to @Tyrrrz for reporting
### Comment inside raw string literal is lost when file is formatted. [#937](https://github.com/belav/csharpier/issues/937)
```c#
// input
var rawLiteralWithExpressionThatWeDontFormat = new StringContent(
    // this comment shouldn't go away
    $$"""
      {
          "params": "{{searchFilter switch
{
    SearchFilter.Video => "EgIQAQ%3D%3D",
    _ => null
}}}"
      }
      """
);

// 0.25.0
var rawLiteralWithExpressionThatWeDontFormat = new StringContent(
    $$"""
      {
          "params": "{{searchFilter switch
{
    SearchFilter.Video => "EgIQAQ%3D%3D",
    _ => null
}}}"
      }
      """
);
```

Thanks go to @Tyrrrz for reporting
### Allow line endings to be configurable [#935](https://github.com/belav/csharpier/issues/935)
CSharpier now supports the following options for line endings. The default is `auto`
- "auto" - Maintain existing line endings (mixed values within one file are normalised by looking at what's used after the first line)
- "lf" – Line Feed only (\n), common on Linux and macOS as well as inside git repos
- "crlf" - Carriage Return + Line Feed characters (\r\n), common on Windows

Thanks go to @phuhl for the feature request
### Avoid breaking only around binary expression but not binary expression itself [#924](https://github.com/belav/csharpier/issues/924)
```c#
// 0.25.0
if (
    someLongStatement == true || someOtherStatement________________________________ == false
)

// 0.26.0
if (someLongStatement == true || someOtherStatement________________________________ == false)
```

Thanks go to @Nixxen for reporting
### Nested loops without brackets should not be indented [#867](https://github.com/belav/csharpier/issues/867)
```c#
// 0.25.0
foreach (var subsequence in sequence)
    foreach (var item in subsequence)
        item.DoSomething();

// 0.26.0
foreach (var subsequence in sequence)
foreach (var item in subsequence)
    item.DoSomething();
```
Thanks go to @Rudomitori for the contribution
**Full Changelog**: https://github.com/belav/csharpier/compare/0.25.0...0.26.0
# 0.25.0
## Breaking Changes
### Improve if directive formatting [#404](https://github.com/belav/csharpier/issues/404)
The `preprocessorSymbolSets` configuration option is no longer supported. 
CSharpier can now parse and format the full range of `#if` preprocessor statements so it is no longer required.
```
// 0.24.2 - supported some basic versions of #if
#if DEBUG
// some code 
#endif

// 0.25.0 - supports the full range of #if including nested statements
// would require the use of the preprocessorSymbolSets configuration option previously 
#if (DEBUG && !NET48) || MONO
// some code
#if NET6_0
// some other code
#endif 
#endif
```

## What's Changed
### Sort Modifiers [#725](https://github.com/belav/csharpier/issues/725)
CSharpier will now sort modifiers according to the defaults for [IDE0036](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0036#csharp_preferred_modifier_order)
```c#
// input
public override async Task Method1() { } 
async public override Task Method2() { }

// output
public override async Task Method1() { } 
public override async Task Method2() { }
```

Thanks go to @glmnet for the contribution

### Support c# 12 features [#883](https://github.com/belav/csharpier/issues/883)
CSharpier now supports formatting 
[Primary Constructors](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12#primary-constructors),
[Alias any typ](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12#alias-any-type), and
[Default lambda parameters](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12#default-lambda-parameters)

### Support for log levels [#875](https://github.com/belav/csharpier/issues/875)
CSharpier now supports `--loglevel` with the CLI and `CSharpier_LogLevel` for MSBuild. This changes the level of logging output. Valid options are:
- None
- Error
- Warning
- Information (default)
- Debug

Thanks go to @samtrion for the suggestion

### CSharpier removes blank line before unsafe block [#917](https://github.com/belav/csharpier/issues/917)
CSharpier was not honoring lines that appeared before `unsafe`
```c#
// input
var x = 1;

unsafe
{
    // should retain empty line
}

// 0.24.2
var x = 1;
unsafe
{
    // should retain empty line
}

// 0.25.0
var x = 1;

unsafe
{
    // should retain empty line
}
```

Thanks go to @fgimian for reporting the bug

### Adding ability to bypass CSharpier when using CSharpier.MsBuild [#914](https://github.com/belav/csharpier/issues/914)
In some instances it is desirable to completely bypass CSharpier.MsBuild, this can now be done with the `CSharpier_Bypass` property.
```bash
dotnet publish -c release -o /app --no-restore /p:CSharpier_Bypass=true
```

Thanks go to @OneCyrus for the suggestion

### Strong Name Sign Assemblies [#911](https://github.com/belav/csharpier/issues/911)
CSharpier is now strong name signed so that it can be used in packages that are strong name signed.

Thanks go to @TwentyFourMinutes for the suggestions and to @goelhardik for strong name signing [Ignore](https://github.com/goelhardik/ignore)

### Don't format files in obj folders [#910](https://github.com/belav/csharpier/pull/910)
CSharpier will no longer format `cs` files that are in an `obj` folder.

### CSharpier.MsBuild runs once for each framework, can it be more efficient. [#900](https://github.com/belav/csharpier/issues/900)
When CSharpier.MsBuild was in a csproj that had multiple target frameworks, it would run once for each target framework. It will now run just a single time. 

### CSharpier.MsBuild returns exit code 1 when ManagePackageVersionsCentrally is set to true [#898](https://github.com/belav/csharpier/issues/898)
CSharpier.MsBuild was not running correctly when used in a project that had centrally managed package version.

Thanks go to @adc-cjewett for reporting the bug

### Multiline comments always indented with spaces when formatting with tabs [#891](https://github.com/belav/csharpier/issues/891)
With `useTabs: true`, CSharpier was formatting multiline comments with a space instead of a tab.
```c#
// input
public class Foo
{
	/**
	 * comment
	 */
	public class Bar { }
}

// 0.24.1
public class Foo
{
 /**
  * comment
  */
	public class Bar { }
}

// 0.25.0
public class Foo
{
	/**
	 * comment
	 */
	public class Bar { }
}
```

Thanks go to @MonstraG for reporting the bug.

### File scoped namespaces should be followed by a blank line [#861](https://github.com/belav/csharpier/issues/861)
CSharpier now adds an empty line after file scoped namespaces if there is not already one
```c#
// input
namespace Namespace;
using System;

// 0.25.0
namespace Namespace;

using System;
```

**Full Changelog**: https://github.com/belav/csharpier/compare/0.24.2...0.25.0
# 0.24.2
## What's Changed
### csharpier-ignore comments force CRLF line endings [#884](https://github.com/belav/csharpier/issues/884)
In a case where 
- a file on windows (which defaults to CRLF) contained only LF
- the file contained `// csharpier-ignore` on a multi-line statement
- the file was formatted in multiple passes due to preprocessor symbols (such as an `#if DEBUG`)

CSharpier would end up formatting the file with `CRLF` on the `// csharpier-ignore` statement but `LF` in the rest of the file. The file would then fail the formatting check.

Thanks go to @pingzing for the bug report and detailed reproduction steps.

**Full Changelog**: https://github.com/belav/csharpier/compare/0.24.1...0.24.2


# 0.24.1
## What's Changed
### 0.24.0 Regression csharpier-ignore causes blank lines between statements to be removed. [#879](https://github.com/belav/csharpier/issues/879)
```c#
// input & expected output

// csharpier-ignore
public string Example
{
  get
     {
       if (_example is not null)
         return _example;

       var number = Random.Shared.Next();

       return _example = number.ToString();
     }
}

// 0.24.0

// csharpier-ignore
public string Example
{
  get
     {
       if (_example is not null)
         return _example;
       var number = Random.Shared.Next();
       return _example = number.ToString();
     }
}
```

Thanks go to @Pentadome for reporting the regression bug.

**Full Changelog**: https://github.com/belav/csharpier/compare/0.24.0...0.24.1


# 0.24.0
## What's Changed
### Formatting named list patterns loses code and causes compilation error [#876](https://github.com/belav/csharpier/issues/876)
```c#
// input & expected output
return list switch
{
    [var elem] => elem * elem,
    [] => 0,
    [..] elems => elems.Sum(e => e + e),
};

// 0.23.0
return list switch
{
    [var elem] => elem * elem,
    [] => 0,
    [..] => elems.Sum(e => e + e),
};

```

Thanks go to @Dragemil for reporting the bug

### CSharpier.MSBuild does not support usernames or project paths with spaces [#872](https://github.com/belav/csharpier/issues/872)
CSharpier.MSBuild would throw an exception when building a project if the username had a space, or if the project path had a space.

Thanks go to @ooo2003003v2 for reporting the bug.

### #pragma with long line introduces extra line break [#865](https://github.com/belav/csharpier/issues/865)
```c#
// input & expected output
if (
    e is
#pragma warning disable CS0618
    BadHttpRequestException
#pragma warning restore CS0618
    {
        Message: "______________________________________________________________________________________________________________"
    }
) { }

// 0.23.0
if (
    e is
#pragma warning disable CS0618
    BadHttpRequestException
#pragma warning restore CS0618

    {
        Message: "______________________________________________________________________________________________________________"
    }
) { }
```

Thanks go to @Denton-L for reporting the bug

### Better support for ignore on method attributes [#848](https://github.com/belav/csharpier/issues/848)
```c#
// input
public class AttributesAndMethods
{
    // csharpier-ignore - only the first attribute
    [Attribute          ]
    [Attribute          ]
    public void MethodThatShouldFormat()     { }

    [Attribute]
    // csharpier-ignore - only the second attribute
    [Attribute         ]
    public void MethodThatShouldFormat()     { }

    [Attribute  ]
    [Attribute  ]
    // csharpier-ignore - just the method
    public void MethodThatShouldNotFormat(           ) { }
}

// 0.23.0
public class AttributesAndMethods
{
    // csharpier-ignore - only the first attribute
    [Attribute          ]
    [Attribute          ]
    public void MethodThatShouldFormat()     { }

    [Attribute]
    // csharpier-ignore - only the second attribute
    [Attribute]
    public void MethodThatShouldFormat() { }

    [Attribute]
    [Attribute]
    // csharpier-ignore - just the method
    public void MethodThatShouldNotFormat() { }
}

// 0.24.0
public class AttributesAndMethods
{
    // csharpier-ignore - only the first attribute
    [Attribute          ]
    [Attribute]
    public void MethodThatShouldFormat() { }

    [Attribute]
    // csharpier-ignore - only the second attribute
    [Attribute]
    public void MethodThatShouldFormat() { }

    [Attribute]
    [Attribute]
    // csharpier-ignore - just the method
    public void MethodThatShouldNotFormat() { }
}
```

Thanks go to @Billuc for reporting the bug

### Ranged ignore applies some formatting when multiple statements are on a line [#846](https://github.com/belav/csharpier/issues/846)
```c#
// input & expected output
void MethodName()
{
    // csharpier-ignore-start
    var packet = new List<byte>();
    packet.Add(0x0f); packet.Add(0x00);
    packet.Add(0x00); packet.Add(0x00);
    // csharpier-ignore-end
}

// 0.23.0
void MethodName()
{
    // csharpier-ignore-start
    var packet = new List<byte>();
    packet.Add(0x0f);
packet.Add(0x00);
    packet.Add(0x00);
packet.Add(0x00);
    // csharpier-ignore-end
}
```

Thanks go to @Billuc for reporting the bug

### Support scoped variables (better handling of unrecognized syntax nodes) [#839](https://github.com/belav/csharpier/issues/839)
Scoped variables are a language proposal. CSharpier has some support for printing unrecognized syntax nodes but the validation logic didn't account for them and would throw an exception
```c#
scoped Span<byte> span;
```
Thanks go to @Dragemil for reporting the bug
### Unrecognized syntax nodes lose comments [#869](https://github.com/belav/csharpier/issues/869)
CSharpier now supports printing commends on unrecognized nodes.
```c#
// comment on unrecognized node
scoped Span<byte> span;
```

**Full Changelog**: https://github.com/belav/csharpier/compare/0.23.0...0.24.0


# 0.23.0
## Breaking Changes
### Make compile errors public when using CSharpier.Core [#799](https://github.com/belav/csharpier/issues/799)
Previously `CodeFormatter.Format(unformattedCode)` and its overloads returned only the formatted code. It now returns a result object.
```c#
public class CodeFormatterResult
{
    public string Code { get; }
    public IEnumerable<Diagnostic> CompilationErrors { get; }
}
```

This is a breaking change. There were also a number of types that should not have been `public` that were made `internal`.

Thanks go to @verdverm for the suggestion
## What's Changed
### Allow comment-description suffix on csharpier-ignore comments [#835](https://github.com/belav/csharpier/issues/835)
It is now possible to include a suffix on `csharpier-ignore` comments. The description must be seperated from the comment by at least one - character.
```c#
// csharpier-ignore - class copied as-is from another project
public class Unformatted     { 
        private string     unformatted;
}

// csharpier-ignore-start -- class copied as-is from another project
public class Unformatted1     { }
public class Unformatted2     { }
// csharpier-ignore-end
```
Thanks go to @strepto for the suggestion

### Fix formatting for open generics [#832](https://github.com/belav/csharpier/issues/832)
```c#
// 0.22.1
typeof(AnExceptionallyLongAndElaborateClassNameToMakeAnExampleRegardingOpenGenerics<
    ,
>).MakeGenericType(typeof(string), typeof(int));

// 0.23.0
typeof(AnExceptionallyLongAndElaborateClassNameToMakeAnExampleRegardingOpenGenerics<,>).MakeGenericType(
    typeof(string),
    typeof(int)
);
```


Thanks go to @jonstodle for reporting the issue

### #region should be indented based on context [#812](https://github.com/belav/csharpier/issues/812)
Previously the preceding whitespace was left as is on `#region` and `#endregion` which resulted undesired formatting.
```c#
// 0.22.1
public class ClassName
{
            #region Ugly methods
    public int LongUglyMethod()
    {    
        return 42;
    }
            #endregion
}

// 0.23.0
public class ClassName
{
    #region Ugly methods
    public int LongUglyMethod()
    {    
        return 42;
    }
    #endregion
}
```
Thanks go to @jods4 for reporting the issue
### Return statement followed by linq query syntax not indenting correctly [#811](https://github.com/belav/csharpier/issues/811)
```c#
// 0.22.1
return from i in Enumerable.Range(0, 10)
let i2 = i * i
where i2 < 100
select new { Square = i2, Root = i };

// 0.23.0
return from i in Enumerable.Range(0, 10)
    let i2 = i * i
    where i2 < 100
    select new { Square = i2, Root = i };
```

Thanks go to @jods4 for reporting the issue

### Array and dictionary initializers should break in some cases to improve readability [#809](https://github.com/belav/csharpier/issues/809)
```c#
// 0.22.1
var dictionaryInitializer = new Dictionary<int, string> { { 1, "" }, { 2, "a" }, { 3, "b" } };
int[,,] cube = { { { 111, 112 }, { 121, 122 } }, { { 211, 212 }, { 221, 222 } } };
int[][] jagged = { { 111 }, { 121, 122 } };

// 0.23.0
var dictionaryInitializer = new Dictionary<int, string>
{
    { 1, "" },
    { 2, "a" },
    { 3, "b" }
};
int[,,] cube =
{
    {
        { 111, 112 },
        { 121, 122 }
    },
    {
        { 211, 212 },
        { 221, 222 }
    }
};
int[][] jagged =
{
    { 111 },
    { 121, 122 }
};
```
### List initializer inside object initializer breaks poorly [#802](https://github.com/belav/csharpier/issues/802)
```c#
// 0.22.1
var someObject = new SomeObject { SomeArray = new SomeOtherObject[]
    {
        new SomeOtherObject { SomeProperty = 1 },
        new SomeOtherObject()
    }.CallMethod().CallMethod() };

// 0.23.0
var someObject = new SomeObject
{
    SomeArray = new SomeOtherObject[]
    {
        new SomeOtherObject { SomeProperty = 1 },
        new SomeOtherObject()
    }
        .CallMethod()
        .CallMethod()
};

```

Thanks go to @shocklateboy92 for reporting the issue
### Allow passing --config-path to cli [#777](https://github.com/belav/csharpier/issues/777)
It is now possible to pass `--config-path` to the cli for cases where it is not in the root or you want to bypass the auto location and speed up formatting requests.
```bash
dotnet csharpier . --config-path "./config/.csharpierrc"
```

Thanks go to @bdovaz for the suggestion

### Allow blank lines in query syntax [#754](https://github.com/belav/csharpier/issues/754)
It is now possible to add blank lines in query syntax expressions which can aid in readability
```c#
var result = await (
    from post in dbContext.Posts
    join blog in dbContext.Blogs on post.BlogId equals blog.Id
    
    let count = dbContext.Posts.Count(p => p.Name == post.Name)
    
    where post.Id == 1
    select new 
    {
         Post = post,
         Blog = blog,
         SamePostNameCount = count
    }
)
    .AsNoTracking()
    .FirstAsync();
```

Thanks go to @TwentyFourMinutes for the suggestion

### #if causes line after it to break when it contains an if [#666](https://github.com/belav/csharpier/issues/666)
```c#
// 0.22.1
class ClassName
{
    public void MethodName()
    {
#if !IF_STATEMENT_HERE_SHOULD_NOT_BREAK_INVOCATION_AFTER_ENDIF
        if (true)
        {
            return;
        }
#endif
        SomeObject
            .CallMethod()
            .CallOtherMethod(shouldNotBreak);
    }
}

// 0.23.0
class ClassName
{
    public void MethodName()
    {
#if !IF_STATEMENT_HERE_SHOULD_NOT_BREAK_INVOCATION_AFTER_ENDIF
        if (true)
        {
            return;
        }
#endif
        SomeObject.CallMethod().CallOtherMethod(shouldNotBreak);
    }
}

```

**Full Changelog**: https://github.com/belav/csharpier/compare/0.22.0...0.23.0

# 0.22.1
## What's Changed
### Fix for CSharpier.MsBuild so it selects a compatible framework if the project does not target net6 or net7 [#797](https://github.com/belav/csharpier/pull/797)
This fix auto selects `net7.0` for projects that do not target `net6.0` or `net7.0`. This means the `CSharpier_FrameworkVersion` property is only required if a project is targeting < `net6.0` and `net7.0` is not installed.

Thanks go to @samtrion for submitting the fix.

# 0.22.0
## Breaking Changes
### Support only UTF8 and UTF8-BOM files [#787](https://github.com/belav/csharpier/pull/787)
Previously UTF.Unknown was used to try to determine file encodings. 
This was problematic because if a file was too small it would not properly detect the encoding.
```c#
public enum MeetingLocation
{
  Café,
  Restaurant
}
```
This file saved as UTF8 would be detected as SBCSCodePageEncoding and result in CSharpier trying to parse the following file
```c#
public enum MeetingLocation
{
  CafÄ‚Â©,
  Restaurant
}
```

CSharpier now only supports UTF8 & UTF8-BOM files. This is consistent with the IDE plugins, which stream files to CSharpier as UTF8.

Thanks go to @Meligy for reporting the problem.

### CSharpier.MSBuild support for .NET 7 [#773](https://github.com/belav/csharpier/issues/773)
CSharpier.MSBuild now multi-targets net6.0 and net7.0. As a side effect of multi-targeting, the `CSharpier_FrameworkVersion` property is now required for projects that do not target `net6.0` or `net7.0`. See https://csharpier.com/docs/MsBuild#target-frameworks

Thanks go to @OneCyrus for reporting it

## What's Changed
### Fix for CSharpier.MsBuild "Specified condition "$(CSharpier_Check)" evaluates to "" instead of a boolean" [#788](https://github.com/belav/csharpier/pull/788)
When projects referencing CSharpier.MsBuild were reloaded, they would get the error "Specified condition "$(CSharpier_Check)" evaluates to "" instead of a boolean" and fail to load.


Thanks go to @samtrion for submitting the fix.

### List Pattern support for subpattern within a slice [#779](https://github.com/belav/csharpier/issues/779)

CSharpier did not have proper support for the new c# 11 slice pattern. When a slice contained a pattern, that pattern would be lost.

```c#
// input
var someValue = someString is [var firstCharacter, .. var rest];

// 0.21.0
var someValue = someString is [var firstCharacter, ..];

// 0.22.0
var someValue = someString is [var firstCharacter, .. var rest];
```

Thanks go to @domn1995 for reporting it

### Fix for comments within expressions in interpolated strings [#774](https://github.com/belav/csharpier/issues/774)
When an interpolated string contained a comment within an expression, CSharpier was inserting a line break that resulted in invalid code.

```c#
// input
var trailingComment = $"{someValue /* Comment shouldn't cause new line */}";

// 0.21.0
var trailingComment = $"{someValue /* Comment shouldn't cause new line */
    }";

// 0.22.0
var trailingComment = $"{someValue /* Comment shouldn't cause new line */}";
```

Thanks go to @IT-CASADO for reporting it

### Always put generic type constraints onto a new line [#527](https://github.com/belav/csharpier/issues/527)
```c#
// 0.21.0
public class SimpleGeneric<T> where T : new() { }

// 0.22.0 
public class SimpleGeneric<T>
    where T : new() { }
```
### Always put constructor initializers on their own line [#526](https://github.com/belav/csharpier/issues/526)
```c#
// 0.21.0
public Initializers() : this(true) { }

public Initializers(string value) : base(value) { }

// 0.22.0
public Initializers()
    : this(true) { }

public Initializers(string value)
    : base(value) { }
```

**Full Changelog**: https://github.com/belav/csharpier/compare/0.21.0...0.22.0


# 0.21.0
## What's Changed
### Support file scoped types [#748](https://github.com/belav/csharpier/issues/748)
CSharpier now supports a file scoped type
```c#
file class FileScopedClass
{
    // implementation
}
```

### Csharpier removes empty lines in ignored blocks of code [#742](https://github.com/belav/csharpier/issues/742)
In some instances csharpier was removing empty lines in `csharpier-ignore` blocks of code
```c#
// input
public class KeepLines1
{
    // csharpier-ignore-start
    private string    first;

    private string    second;
    // csharpier-ignore-end
}

// 0.20.0
public class KeepLines1
{
    // csharpier-ignore-start
    private string    first;private string    second;
    // csharpier-ignore-end
}
```

Thanks go to @MonstraG for reporting it

### Await + LINQ query syntax indents incorrectly [#740](https://github.com/belav/csharpier/issues/740)
```c#
// 0.20.0
var result = await from thing in Things
from otherThing in OtherThings
from finalThing in SomethingAsync(thing, otherThing)
select finalThing;

// 0.21.0
var result = await
    from thing in Things
    from otherThing in OtherThings
    from finalThing in SomethingAsync(thing, otherThing)
    select finalThing;
```
Thanks go to @domn1995 for reporting it.

### Break anonymous object creation when there are more than two properties [#753](https://github.com/belav/csharpier/issues/753)
Object initializers break when they have more than two properties. For example
```c#
var x = new Thing
{
    Post = post,
    Blog = blog,
    SamePostNameCount = count
};
```

Anonymous object initializers were not included in this logic prior to 0.21.0
```c#
// 0.20.0
var result =
    from post in Posts
    select new { Post = post, Blog = blog, SamePostNameCount = count };

// 0.21.0
var result =
    from post in Posts
    select new
    {
        Post = post,
        Blog = blog,
        SamePostNameCount = count
    };
```

Thanks go to @TwentyFourMinutes for reporting it.

### Support net7 [#756](https://github.com/belav/csharpier/pull/756)
The CSharpier dotnet tool now works with net6 or net7.

### Fix for ignoring subfolders in node_modules [#762](https://github.com/belav/csharpier/pull/762)
CSharpier was not properly ignoring .cs files when they were in a subfolder of node_modules

Thanks go to @snebjorn for reporting the bug.

**Full Changelog**: https://github.com/belav/csharpier/compare/0.20.0...0.21.0


# 0.20.0
## What's Changed
### Improve Tuple formatting [#735](https://github.com/belav/csharpier/issues/#735)
Tuples would break poorly in some cases
```c#
// 0.19.2

public async Task<(ILookup<string, int> someLookup, ILookup<int, string> reverseLookup, ILookup<
        string,
        ClassName
    > thirdLookup)> CreateLookups()
{
    return (null, null);
}

public void TuplesAsInput(
    (int myInt, string myString, ClassName myClassNameInstance, Dictionary<
        int,
        string
    > wordList) inputArgs
)
{
    // do something
}

// 0.20.0
public async Task<(
    ILookup<string, int> someLookup,
    ILookup<int, string> reverseLookup,
    ILookup<string, ClassName> thirdLookup
)> CreateLookups()
{
  return (null, null);
}

public void TuplesAsInput(
    (
        int myInt,
        string myString,
        ClassName myClassNameInstance,
        Dictionary<int, string> wordList
    ) inputArgs
 )
 {
   // do something
 }
```

Thanks go to @BenjaBobs for reporting the bug.

**Full Changelog**: https://github.com/belav/csharpier/compare/0.19.2...0.20.0


# 0.19.2
## What's Changed
### .NET Tool Crashes When Run Concurrently [#728](https://github.com/belav/csharpier/issues/733)

Fixed another edge case with running csharpier concurrently.

Thanks go to @Kurt-von-Laven for reporting the bug.

**Full Changelog**: https://github.com/belav/csharpier/compare/0.19.1...0.19.2

# 0.19.1
## What's Changed
### CSharpier crashes when run multiple times simultaneously [#728](https://github.com/belav/csharpier/issues/728)

The new caching for CSharpier didn't properly handle multiple CSharpier processes formatting at the same time. This is most common when using CSharpier.MsBuild in a solution with multiple projects.

Thanks go to @pingzing for reporting the bug.

**Full Changelog**: https://github.com/belav/csharpier/compare/0.19.0...0.19.1


# 0.19.0
## What's Changed
### Adding a cache to speed up formatting. [#692](https://github.com/belav/csharpier/issues/692)
CSharpier now caches information about files that it has formatted to speed up subsequent runs.  
By default the following are used as cache keys and a file is only formatted if one of them has changed.

- CSharpier Version
- CSharpier Options
- Content of the file

The cache is stored at [LocalApplicationData]/CSharpier/.formattingCache.

### Ignore node_modules [#699](https://github.com/belav/csharpier/issues/699)

CSharpier now ignores any files within a node_modules folder.

Thanks go to @RichiCoder1 for the suggestion and @SubjectAlpha for the implementation.

### Extra space before curly brace in array initializer [#693](https://github.com/belav/csharpier/issues/693)

```c#
// 0.18.0
public class ClassName
{
    public int[] SomeArray { get; set; } =  { 1, 2, 3 };
}
// 0.19.0
public class MyClass
{
    public int[] SomeArray { get; set; } = { 1, 2, 3 };
}

```

Thanks go to @TiraelSedai for reporting the bug.

**Full Changelog**: https://github.com/belav/csharpier/compare/0.18.0...0.19.0


# 0.18.0
## What's Changed
### Initial C# 11 support [#686](https://github.com/belav/csharpier/pull/686)
CSharpier can format the following c# 11 features
- Raw string literals
- Generic attributes
- Static abstract members in interfaces
- Newlines in string interpolation expressions **CSharpier will leave existing new lines within expressions and not add new ones**
- List Patterns
- UTF8 string literals
- Unsigned right shift operator
- Checked operator
- Generic math

### use relative file path in CommandLineFormatter [#680](https://github.com/belav/csharpier/pull/680)
CSharpier now outputs relative or absolute file paths so that they are clickable in terminals.
```
dotnet csharpier .

# csharpier 0.17.0
Error Invalid.cs - Failed to compile so was not formatted.

# csharpier 0.18.0
Error ./Invalid.cs - Failed to compile so was not formatted.

dotnet csharpier c:/src

# csharpier 0.17.0
Error Invalid.cs - Failed to compile so was not formatted.

# csharpier 0.18.0
Error c:/src/Invalid.cs - Failed to compile so was not formatted.
```

Thanks go to @dlech

### Invalid code for comments inside expressions in verbatim interpolated strings [#679](https://github.com/belav/csharpier/issues/679)
```c#
// input
var someValue =
    $@"
    {
        // comment
        "hi"
    }
    ";
// 0.17.0
var someValue =
    $@"
    {
        // comment "hi"}
    ";
// 0.18.0
var someValue =
    $@"
    {
        // comment
        "hi"
    }
    ";
```
Thanks go to @ivan-razorenov
### CSharpier ranged ignore [#678](https://github.com/belav/csharpier/issues/678)
CSharpier now has the ability to ignore a range of statements or members. See [Ignore](https://csharpier.com/docs/Ignore) for more details
```c#
// csharpier-ignore-start
var unformatted =        true;
var unformatted =        true;
// csharpier-ignore-end
```
Thanks go to @pingzing

**Full Changelog**: https://github.com/belav/csharpier/compare/0.17.0...0.18.0


# 0.17.0
## What's Changed

- MSBuild Task target too late? Breakpoints are not hit [#674](https://github.com/belav/csharpier/issues/674)
- Excessive indent level with lambda as the only method call argument [#669](https://github.com/belav/csharpier/issues/669)
- Empty (or malformed) .csproj file will cause csharpier to fail. [#665](https://github.com/belav/csharpier/issues/665)
- #endif retains extra blank lines [#660](https://github.com/belav/csharpier/issues/660)
- Option for indentation  [#645](https://github.com/belav/csharpier/issues/645)
- Small bug with formatting LINQ queries with multiple orderby fields [#643](https://github.com/belav/csharpier/issues/643)
- Consistently Indent By 4 Spaces [#617](https://github.com/belav/csharpier/issues/617)
- Conditional access edge cases [#603](https://github.com/belav/csharpier/issues/603)
- Improve formatting for casting [#407](https://github.com/belav/csharpier/issues/407)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.16.0...0.17.0

# 0.16.0
## What's Changed

- fix: ignore file detection when directory contains period [#634](https://github.com/belav/csharpier/pull/634)
- Format switch statement consistently with other code. [#624](https://github.com/belav/csharpier/pull/624)
- CodeFormatter should accept SyntaxTree [#621](https://github.com/belav/csharpier/issues/621)
- Add support for netstandard 2.0 to CSharpier.Core [#619](https://github.com/belav/csharpier/pull/619)
- Indent c style multiline comments correctly when they switch indentation. [#606](https://github.com/belav/csharpier/issues/606)
- Member access should break [#600](https://github.com/belav/csharpier/issues/600)
- SuppressNullableWarningExpression ( !. ) does not break consistenly [#596](https://github.com/belav/csharpier/issues/596)
- Turn CSharpier.com into a proper website. [#505](https://github.com/belav/csharpier/issues/505)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.15.1...0.16.0

# 0.15.1
## What's Changed
- Set CSharpier.MsBuild as DevelopmentDependency. [#607](https://github.com/belav/csharpier/pull/607)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.15.0...0.15.1
 
# 0.15.0
## Breaking Changes
- `CSharpier.MsBuild` now requires .NET6 [#565](https://github.com/belav/csharpier/issues/565)

## What's Changed
- .csharpierignore causes csharpier to be significantly slower [#594](https://github.com/belav/csharpier/issues/594)
- Support for // csharpier-ignore [#581](https://github.com/belav/csharpier/issues/581)
- Multiline comments are not properly indented. [#580](https://github.com/belav/csharpier/issues/580)
- Generics + ObjectCreationExpression should break consistently [#578](https://github.com/belav/csharpier/issues/578)
- Extra blank lines should be removed at the end of a method [#575](https://github.com/belav/csharpier/issues/575)
- Null conditional operator does not break consistently [#561](https://github.com/belav/csharpier/issues/561)
- Enum members should follow the rules for new lines [#553](https://github.com/belav/csharpier/issues/553)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.14.0...0.15.0

# 0.14.0
## What's Changed
- File with no preprocessor symbols formats twice. [#555](https://github.com/belav/csharpier/issues/555)
- A namespace with `assembly` attribute and `using` causes two newlines [#551](https://github.com/belav/csharpier/issues/551)
- Wrapping arithmetic expressions not stacked/chopped down [#547](https://github.com/belav/csharpier/issues/547)
- Use UTF8 for piping in/out to CLI to support unicode characters  [#545](https://github.com/belav/csharpier/issues/545)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.13.0...0.14.0

# 0.13.0
## What's Changed
- Incorrect indentation in Method chain inside Initializer [#529](https://github.com/belav/csharpier/issues/529)
- Allow empty lines before comments in enums [#524](https://github.com/belav/csharpier/issues/524)
- For with empty statement should have space. [#523](https://github.com/belav/csharpier/issues/523)
- Empty lines are not respected with break and continue [#520](https://github.com/belav/csharpier/issues/520)
- Extra Whitespace in empty anonymous initializer [#519](https://github.com/belav/csharpier/issues/519)
- Class that ends with comment does not retain extra line before comment [#513](https://github.com/belav/csharpier/issues/513)
- Join Clause with Type losing Type [#508](https://github.com/belav/csharpier/issues/508)
- Give cli bad path, you get an exception [#506](https://github.com/belav/csharpier/issues/506)
- Double blank lines appearing in top level statements [#501](https://github.com/belav/csharpier/issues/501)
- VisualStudio Extension [#499](https://github.com/belav/csharpier/issues/499)
- Rider Plugin [#498](https://github.com/belav/csharpier/issues/498)
- CSharpier.MSBuild may have mismatched version with CLI [#490](https://github.com/belav/csharpier/issues/490)
- Break object initializers if there are 3 or more properties [#446](https://github.com/belav/csharpier/issues/446)
- Force lines before and after some members. [#285](https://github.com/belav/csharpier/issues/285)
- Formatting may conflict with StyleCopAnalayzers [#13](https://github.com/belav/csharpier/issues/13)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.12.0...0.13.0

# 0.12.0
## Breaking Changes
- If a file that fails to compile is piped to csharpier, csharpier now writes an error message on std error and return a 1 exit code.  

## What's Changed
- Nested Initializers should break [#487](https://github.com/belav/csharpier/issues/487)
- Adding --pipe-multiple-files and other changes to support vscode extension [#283](https://github.com/belav/csharpier/pull/495)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.11.1...0.12.0

# 0.11.2 (CSharpier.MSBuild only)
## What's Changed
- CSharpier.MSBuild 0.11.1 is not published correctly [#481](https://github.com/belav/csharpier/issues/481)

# 0.11.1
## What's Changed
- base should merge in an invocation chain [#473](https://github.com/belav/csharpier/issues/473)
- File with multiple newlines at the end keeps them [#464](https://github.com/belav/csharpier/issues/464)
- Extra space in generic with omitted types [#463](https://github.com/belav/csharpier/issues/463)
- Object creation still uses SpaceBrace [#462](https://github.com/belav/csharpier/issues/462)
- Empty Initializer gets double whitespace [#461](https://github.com/belav/csharpier/issues/461)
- Support C# 10 and .Net 6 [#448](https://github.com/belav/csharpier/issues/448)
- Always break nested Conditionals  [#434](https://github.com/belav/csharpier/issues/434)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.10.0...0.11.1

# 0.10.0
## What's Changed
- try-finally without catch clause is formatted strangely. [#454](https://github.com/belav/csharpier/issues/454)
- Nested FixedStatements should break [#438](https://github.com/belav/csharpier/issues/438)
- Disabled text validation fails with trailing whitespace [#428](https://github.com/belav/csharpier/issues/428)
- Vertically Align Curly Braces [#423](https://github.com/belav/csharpier/issues/423)
- Crash On Empty Config File [#421](https://github.com/belav/csharpier/issues/421)
- Conditional in Arguments should indent. [#419](https://github.com/belav/csharpier/issues/419)
- Chained assignments formatting can be improved [#417](https://github.com/belav/csharpier/issues/417)
- Improve ConditionalExpression in ReturnStatement formatting [#416](https://github.com/belav/csharpier/issues/416)
- Pattern Matching edge cases [#413](https://github.com/belav/csharpier/issues/413)
- Implement proper logging. [#406](https://github.com/belav/csharpier/issues/406)
- (finally) Improve formatting of InvocationExpressions [#7](https://github.com/belav/csharpier/issues/7)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.9...0.10.0

# 0.9.9
## Breaking Changes
- Require the directoryOrFile argument when not piping into to stdin [#381](https://github.com/belav/csharpier/issues/381)

## What's Changed
- SwitchExpression + Pattern edge cause causes extra line and poor formatting [#408](https://github.com/belav/csharpier/issues/408)
- NewLines not retained before lock statement [#401](https://github.com/belav/csharpier/issues/401)
- Better error handling when given a csproj or sln file [#398](https://github.com/belav/csharpier/issues/398)
- CSharpierignore not taken into account when supplying . as the directory [#392](https://github.com/belav/csharpier/issues/392)
- SwitchStatement with When breaks even with body of switch [#387](https://github.com/belav/csharpier/issues/387)
- Respect new lines between case statements [#383](https://github.com/belav/csharpier/issues/383)
- Line breaks in "when" clause in SwitchExpression [#382](https://github.com/belav/csharpier/issues/382)
- Block loses extra lines [#378](https://github.com/belav/csharpier/issues/378)
- RecordDeclaration should format consistently with ConstructorDeclaration [#371](https://github.com/belav/csharpier/issues/371)
- Conditional breaking without indentation [#345](https://github.com/belav/csharpier/issues/345)
- Improve formatting of pattern matching (IsPatternExpression, BinaryPattern, etc) [#154](https://github.com/belav/csharpier/issues/154)
- Code in IfDirective can't currently be formatted [#15](https://github.com/belav/csharpier/issues/15)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.8...0.9.9

# 0.9.8
## What's Changed
- Remove all configuration options except for print width. [#358](https://github.com/belav/csharpier/issues/358)
- Array Rank not breaking [#353](https://github.com/belav/csharpier/issues/353)
- SwitchStatement should SpaceBrace [#352](https://github.com/belav/csharpier/issues/352)
- ObjectInitializer keeping brace on same line [#336](https://github.com/belav/csharpier/issues/336)
- ObjectInitializer in CollectionInitializer breaking [#335](https://github.com/belav/csharpier/issues/335)
- Better format do-while [#317](https://github.com/belav/csharpier/issues/317)
- Blocks inside of other blocks are getting an extra new line. [#316](https://github.com/belav/csharpier/issues/316)
- Implicit Object Creation breaking  [#302](https://github.com/belav/csharpier/issues/302)
- ForEachVariableStatement not breaking/indenting consistently with regular ForEachStatement [#300](https://github.com/belav/csharpier/issues/300)
- MethodDeclaration with Constraints not breaking before brace [#299](https://github.com/belav/csharpier/issues/299)
- Constructor with Base edge cases [#298](https://github.com/belav/csharpier/issues/298)
- Nested Generics in Variable Declaration [#295](https://github.com/belav/csharpier/issues/295)
- #pragma or trailing comment causes breaking in Object Initializer [#252](https://github.com/belav/csharpier/issues/252)
- Verbatim string with mismatched line endings triggers "failed syntax tree validation" [#244](https://github.com/belav/csharpier/issues/244)
- SwitchExpression formatting. [#237](https://github.com/belav/csharpier/issues/237)
- Empty Method should keep braces on same line [#133](https://github.com/belav/csharpier/issues/133)
- Improving formatting for edge cases of ForStatement [#112](https://github.com/belav/csharpier/issues/112)
- ConditionalExpression indentation [#83](https://github.com/belav/csharpier/issues/83)
- BinaryExpression Grouping [#37](https://github.com/belav/csharpier/issues/37)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.7...0.9.8

# 0.9.7
## What's Changed
- New overload for `Doc.GroupWithId()` [#334](https://github.com/belav/csharpier/issues/334)
- Improve formatting by grouping parenthesized expressions and indenting them if they break. [#328](https://github.com/belav/csharpier/issues/328)
- Improve formatting of IsPattern in IfStatement [#327](https://github.com/belav/csharpier/issues/327)
- Improve formatting of the condition in a do-while [#326](https://github.com/belav/csharpier/issues/326)
- Always break statements without braces. [#303](https://github.com/belav/csharpier/issues/303)
- Empty Line being added with Array Type [#301](https://github.com/belav/csharpier/issues/301)
- Implicit Array Initializer does not format braces consistently with other statements. [#297](https://github.com/belav/csharpier/issues/297)
- Format checked like a invocation expression with an argument list [#270](https://github.com/belav/csharpier/issues/270)
- Attribute should newline before close paren [#257](https://github.com/belav/csharpier/issues/257)
- Tuple with VariableDeclaration [#251](https://github.com/belav/csharpier/issues/251)
- Record - does not format consistently with a class. [#233](https://github.com/belav/csharpier/issues/233)
- CSharpier.Playground should only publish with new released version [#224](https://github.com/belav/csharpier/issues/224)
- Attributes on parameters [#204](https://github.com/belav/csharpier/issues/204)
- Improve Lambda Formatting - indent expression body and break in a way to minimize new lines. [#176](https://github.com/belav/csharpier/issues/176)
- Format ObjectCreationExpression with Initializer consistently [#113](https://github.com/belav/csharpier/issues/113)
- Improve formatting of long Catch Clauses [#86](https://github.com/belav/csharpier/issues/86)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.6...0.9.7

# 0.9.6
## What's Changed
- Add options to write the formatted file to stdout and accept a file from stdin [#282](https://github.com/belav/csharpier/issues/282)
- Implement ConditionalGroup doc type [#278](https://github.com/belav/csharpier/issues/278)
- Optimize some hot paths to speed up formatting. [#277](https://github.com/belav/csharpier/issues/277)
- Implement Align Doc Type [#276](https://github.com/belav/csharpier/issues/276)
- Improve formatting of ClassDeclaration with BaseList + Constraints [#275](https://github.com/belav/csharpier/issues/275)
- Switch tests to width 100 so they line up with default option [#256](https://github.com/belav/csharpier/issues/256)
- Improving formatting of generics + variable declarations. [#240](https://github.com/belav/csharpier/pull/240)
- Improve Forrmatting of Field with lambda and generics [#236](https://github.com/belav/csharpier/issues/236)
- Improve Formatting of object initialiser syntax [#234](https://github.com/belav/csharpier/issues/234)
- Improve formatting of generic methods and constructors [#94](https://github.com/belav/csharpier/issues/94)
- Improve formatting of field with generics [#47](https://github.com/belav/csharpier/issues/47)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.5...0.9.6

# 0.9.5
## What's Changed
- CSharpier.MSBuild does not work with dotnet watch run [#228](https://github.com/belav/csharpier/issues/228)
- Leading comments interfering with breaking InitializerExpression [#217](https://github.com/belav/csharpier/issues/217)
- \#endregion is getting indented more on each format [#216](https://github.com/belav/csharpier/issues/216)
- Some files getting extra new lines on each format [#215](https://github.com/belav/csharpier/issues/215)
- File that fails check should give some indication of the formatting that was missing. [#182](https://github.com/belav/csharpier/issues/182)
- Missing nodes in SyntaxNodeComparer need better reporting. [#160](https://github.com/belav/csharpier/issues/160)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.4...0.9.5

# 0.9.4
## What's Changed
- Interpolated verbatim string is not handling line endings [#221](https://github.com/belav/csharpier/issues/221)
- CLI Support for multiple targets [#220](https://github.com/belav/csharpier/issues/220)
- Add support for nuget package that inject msbuild step to run csharpier [#218](https://github.com/belav/csharpier/issues/218)
- Loops without braces [#202](https://github.com/belav/csharpier/issues/202)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.3...0.9.4

# 0.9.3
## What's Changed
- RecordDeclaration with Generics produces invalid code [#201](https://github.com/belav/csharpier/issues/201)
- Formatting of auto implemented properties with access modifiers [#188](https://github.com/belav/csharpier/issues/188)
- Verbatim string literals take into account EndOfLine configuration [#183](https://github.com/belav/csharpier/issues/183)
- CSharpierIgnore & CSharpierRC from parent directories should be respected. [#181](https://github.com/belav/csharpier/issues/181)
- Break apart readme [#172](https://github.com/belav/csharpier/issues/172)
- PatternMatching includes extra spaces [#167](https://github.com/belav/csharpier/issues/167)
- Re-add async file reads [#127](https://github.com/belav/csharpier/issues/127)
- Dictionary Initializer inserts extra new line [#103](https://github.com/belav/csharpier/issues/103)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.2...0.9.3

#0.9.2
## What's Changed
- Support "auto" for endOfLine [#147](https://github.com/belav/csharpier/issues/147)
- Long Parameter Attribute should break after ending brace [#174](https://github.com/belav/csharpier/issues/174)
- Attribute on parameter should have space [#104](https://github.com/belav/csharpier/issues/104)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.1...0.9.2

# 0.9.1
## What's Changed
- Add support for .csharpierignore [#159](https://github.com/belav/csharpier/issues/159)
- Break before BinaryOperator [#152](https://github.com/belav/csharpier/issues/152)
- LeadingComments affect breaking [#149](https://github.com/belav/csharpier/issues/149)
- Ignore generated files by default [#140](https://github.com/belav/csharpier/issues/140)
- Return with BinaryExpression [#137](https://github.com/belav/csharpier/issues/137)
- IsPattern breaking inside of IfStatement [#130](https://github.com/belav/csharpier/issues/130)
- SpaceBrace causing breaking when it shouldn't [#100](https://github.com/belav/csharpier/issues/100)
- Implement Formatting Options with Configuration File [#10](https://github.com/belav/csharpier/issues/10)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.0...0.9.1

















