# CSharpier

This is a prettier plugin for csharp. It uses a simple .net core console application to use Roslyn to generate the syntax tree. It is in an early state, but can (probably) generate correct code for ~50% of the node types found in the roslyn syntax tree.
Compare [AllInOne](./prettier-plugin-csharpier/Samples/AllInOne.cs) to [AllInOne.Formatted](./prettier-plugin-csharpier/Samples/AllInOne.Formatted.cs) to get a sense of the progress. [Compare In Github](https://github.com/belav/csharpier/compare/master...progress#diff-bc7aecb189c0bc5b4772cbb210c1fab5b5d0e5cffe6972970a58f7a452c72c2e)

## Known Issues
- Strips out all comments
- Strips out preprocessor statements
- Formatting is a little rough

## Development
See [Development Readme](./prettier-plugin-csharpier/README.md)
