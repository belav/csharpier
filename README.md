# CSharpier
This is a prettier port to .net for formatting c# code.
It can generate mostly correct code for c# <= 8.0. The remaining major issues
- Comments - the approach is being finalized before the work is done to get all nodes to format comments. Comments are lost in a number of places.
- Preprocessor directives - partial support exists for if/else/endif, but csharpier is not able to format code within most if directives due to how roslyn parse that code.
- New Lines - some new lines that should remain are lost - this will be addressed as comment support is added.
- Formatting (mostly when to line break/indent longer code) is not finalized.

Try it out at [Playground](https://csharpier.bnt-studios.com)

Compare [AllInOne](./CSharpier/CSharpier.Tests/Samples/AllInOne.cst) to [AllInOne.Formatted](./CSharpier/CSharpier.Tests/prettier-plugin-csharpier/Samples/AllInOne.Formatted.cs) to get a sense of what is missing. [Compare In Github](https://github.com/belav/csharpier/compare/master...progress)

# Prettier-Plugin-CSharpier

**This is most likely abandoned due to performance issues with the approach**

This is a prettier plugin for csharp. It uses a simple .net core console application to use Roslyn to generate the syntax tree. 
It can generate mostly correct code for c# <= 8.0. The remaining major issues
- Comments are a WIP, most of them are lost on formatting.
- Strips out preprocessor statements
- Loses some new lines from the original code that would be desirable to keep.
- Formatting (mostly when to line break/indent longer code) is not finalized and probably needs a lot of work.
- It is slow, and a major reason it is slow is because it has to call a console app that serializes json. Running it on a very large project took 40+ minutes

Compare [AllInOne](./prettier-plugin-csharpier/Samples/AllInOne.cs) to [AllInOne.Formatted](./prettier-plugin-csharpier/Samples/AllInOne.Formatted.cs) to get a sense of what is missing.

## Development
See [Development Readme](./prettier-plugin-csharpier/README.md)
