# CSharpier
This is a WIP prettier port to .net for formatting c# code. It is NOT production ready.
It can generate mostly correct code for c# <= 8.0.
It formats a solution of ~11,000 source files in < 30 seconds

The remaining issues
- Leading/Trailing Trivia - A large number of these are currently lost. The approach to printing these is implemented, but changes need to be made to each node types print function as well as some of the helper functions before they will print correctly.
  - Comments
  - Preprocessor Directives
  - New lines - the goal is to preserve new lines in some areas. For example the lines that appear between properties will be preserved.
- Formatting (mostly when to line break/indent longer code) is not finalized.
- A few node types are missing and code for them will be lost.

Try it out at [Playground](https://csharpier.bnt-studios.com)

Compare [AllInOne](./CSharpier/CSharpier.Tests/Samples/AllInOne.cst) to [AllInOne.Formatted](./CSharpier/CSharpier.Tests/prettier-plugin-csharpier/Samples/AllInOne.Formatted.cs) to get a sense of what is missing. [Compare In Github](https://github.com/belav/csharpier/compare/master...progress)

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
