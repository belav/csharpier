# CSharpier

This is a prettier plugin for csharp. It uses a simple .net core console application to use Roslyn to generate the syntax tree. It is in an early state, but can generate (not always correct) code for most of C# <= 7.3, and correct code for < C#6 not counting comments/preprocessor statements
Compare [AllInOne](./prettier-plugin-csharpier/Samples/AllInOne.cs) to [AllInOne.Formatted](./prettier-plugin-csharpier/Samples/AllInOne.Formatted.cs) to get a sense of the progress. [Compare In Github](https://github.com/belav/csharpier/compare/master...progress#diff-bc7aecb189c0bc5b4772cbb210c1fab5b5d0e5cffe6972970a58f7a452c72c2e)

## Known Issues
- Strips out all comments
- Strips out preprocessor statements
- Still missing full support for some features in C# >= 6 which results in code being lost on formatting.
- Formatting is a little rough and not finalized

## Development
See [Development Readme](./prettier-plugin-csharpier/README.md)
