# CSharpier
This is a WIP [Prettier](https://github.com/prettier/prettier) port to .net for formatting c# code.
  - It is NOT production ready.
  - It is opinionated and will eventually offer a limited set of style options.
  - It can generate mostly correct code for c# <= 8.0 with a goal of supporting 9.0 before release.
  - It formats a solution of ~11,000 source files in ~35 seconds
  - It will support comparing the syntax tree of the formatted code to the original code to detect when code is lost. From my testing, this roughly doubles the time it takes to run. Generally this will be used the first time CSharpier is run on a large code base.

The remaining issues before I'm ready to call it an alpha
- ~~Leading/Trailing Trivia - The approach to printing these is implemented but a number of nodes are not yet printing them and they are lost. Each print function needs to be reviewed.~~
  - ~~Comments~~
  - ~~Preprocessor Directives - new lines around these are lost sometimes and should probably be preserved~~
  - ~~New lines - the goal is to preserve new lines in some areas. For example the lines that appear between properties will be preserved.~~
- Multiline comments aren't properly supported yet - WIP
- ~~CSharpier will happily format invalid code, which results in very strange results~~
- ~~CSharpier needs a mechanism to detect when formatting resulted in a lose of code~~
- CSharpier is being tested against a number of large open source repositories to find places where it is losing source code during formatted.
  - ~~[Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)~~
  - ~~[Roslyn](https://github.com/dotnet/roslyn)~~
  - [Asp.Net Core](https://github.com/dotnet/aspnetcore) In Progress
  - [.NET runtime](https://github.com/dotnet/runtime)
  - TBD
- There is no CLI tool yet

Try it out at [Playground](https://csharpier.bnt-studios.com)

## Syntax Tree Validation
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
See [Development Readme](./CSharpier/README.md)

# Prettier-Plugin-CSharpier

**This is most likely abandoned due to performance issues with the approach**

This is a prettier plugin for csharp. It uses a simple .net core console application to use Roslyn to generate the syntax tree. 
It can generate mostly correct code for c# <= 8.0. The remaining major issues
- Comments are a WIP, most of them are lost on formatting.
- Strips out preprocessor statements
- Loses some new lines from the original code that would be desirable to keep.
- Formatting (mostly when to line break/indent longer code) is not finalized and probably needs a lot of work.
- It is slow, and a major reason it is slow is because it has to call a console app that serializes json. That console app has to initialize roslyn for each file. And then has to serialize each file into json.

Compare [AllInOne](./prettier-plugin-csharpier/Samples/AllInOne.cs) to [AllInOne.Formatted](./prettier-plugin-csharpier/Samples/AllInOne.Formatted.cs) to get a sense of what is missing.

## Development
See [Development Readme](./prettier-plugin-csharpier/README.md)
