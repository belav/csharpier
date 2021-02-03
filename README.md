# CSharpier
This is a work in progress .net port of the core printing part of prettier. The Prettier-Plugin-Sharpier below has been halfway converted into c#, with some basics needed from prettier converted into c#. Because it all runs in .net it avoids the overhead of serializing the AST to json. A small file formats in ~70ms vs 250ms. It should also be possible to format files in parallel.

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

Try it out at [Playground](https://csharpier.bnt-studios.com)

Compare [AllInOne](./prettier-plugin-csharpier/Samples/AllInOne.cs) to [AllInOne.Formatted](./prettier-plugin-csharpier/Samples/AllInOne.Formatted.cs) to get a sense of what is missing.

## Development
See [Development Readme](./prettier-plugin-csharpier/README.md)
