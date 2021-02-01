# CSharpier

This is a prettier plugin for csharp. It uses a simple .net core console application to use Roslyn to generate the syntax tree. 
It can generate mostly correct code for c# <= 8.0. The remaining major issues
- Comments are a WIP, most of them are lost on formatting.
- Strips out preprocessor statements
- Loses some new lines from the original code that would be desirable to keep.
- Formatting (mostly when to line break/indent longer code) is not finalized and probably needs a lot of work.

Try it out at [Playground](https://csharpier.bnt-studios.com)

Compare [AllInOne](./prettier-plugin-csharpier/Samples/AllInOne.cs) to [AllInOne.Formatted](./prettier-plugin-csharpier/Samples/AllInOne.Formatted.cs) to get a sense of the what is missing. [Compare In Github](https://github.com/belav/csharpier/compare/master...progress#diff-bc7aecb189c0bc5b4772cbb210c1fab5b5d0e5cffe6972970a58f7a452c72c2e)

## Development
See [Development Readme](./prettier-plugin-csharpier/README.md)
