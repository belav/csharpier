# CSharpier

This is a prettier plugin for csharp. It uses a simple .net core console application to use Roslyn to generate the syntax tree. It is in an early state, but can generate correct code for some basic cases.

## Known Issues
- Strips out all comments
- Strips out empty lines in some areas, always adds empty lines in other areas
- Formatting is a little rough
- Doesn't currently handle a large number of node types

## Development
See ./prettier-plugin-csharpier/README.md