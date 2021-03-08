## Development
There are a couple different ways of running CSharpier while developing it.

### Using the playground
From your IDE run the following in the /CSharpier/CSharpier.Playground directory
```bash
dotnet watch run
```
- This should open the playground website at https://localhost:5001
- Paste the code you want to format there and click format.
- The playground includes tabs to view the AST and generated doc tree, which help troubleshooting issues.

### Unit Tests
CSharpier.Core.Tests contains a number of different unit tests.
- DocPrinterTests - are used for testing converting Doc's into source code. Playing around with them can help to understand how Doc's are formatted
- TestFiles
  - Each directory here roughly corresponds to a different node type.
  - Originally tests were very granular, but more recently there is a single file per node type.
  - The testing files are .cst to easily exclude them from being compiled with the project
  - The testing files are formatted to [file].actual.cst so they can be compared against the original [file].cst
  - In a situation where the original file is not the expected output, [file].expected.cst can be created and it will be compared instead
- Samples
  - Mainly used for testing the AllInOne.cs file
  - Scratch.cs is also a dumping ground for a quick test, but has mostly been replaced by using the Playground

### Helpful Information
- https://github.com/prettier/prettier/blob/main/commands.md is useful for understanding how formatting with the Doc classes works. CSharpier hasn't implemented all of the prettier Doc types, and it has added a couple new ones.
- https://dev.to/fvictorio/how-to-write-a-plugin-for-prettier-6gi is also useful for understanding how formatting works.
- https://sharplab.io/ is useful for understanding the AST that is produced from some C# code
- https://www.linqpad.net/ can also show the AST, but is less forgiving with errors in your code.
