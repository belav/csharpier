# CSharpier

This is a prettier plugin for csharp. It uses a simple .net core console application to use Roslyn to generate the syntax tree. It is in an early state, but can (probably) generate correct code for ~50% of the node types found in the roslyn syntax tree.
Compare [AllInOne](./prettier-plugin-csharpier/Samples/AllInOne.cs) to [AllInOne.Formatted](./prettier-plugin-csharpier/Samples/AllInOne.Formatted.cs) to get a sense of the progress.

## Known Issues
- Strips out all comments
- Strips out preprocessor statements
- Formatting is a little rough

## Development
See [Development Readme](./prettier-plugin-csharpier/README.md)
