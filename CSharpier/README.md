## Development
There are a couple different ways of running CSharpier while developing it.

### Using the playground
From your IDE run the following in the /CSharpier/CSharpier.Playground directory
```bash
dotnet watch run
```
- This should open the playground website at https://localhost:5001
- Any changes made in code will trigger a recompile, and the website will automatically reload
- The playground includes tabs to view the AST and generated doc tree, which help troubleshooting issues.

### Unit Tests
CSharpier.Core.Tests contains a number of different unit tests.
- DocPrinterTests - are used for testing converting Doc's into source code. Playing around with them can help to understand how Doc's are formatted
- TestFiles
  - Each directory here roughly corresponds to a different node type.
  - The unit tests are auto generated based on the files found within the directory. There is a powershell file that will regenerate them when CSharpier.Core.Tests is rebuilt
  - The testing files are .cst to easily exclude them from being compiled with the project
  - The testing files are formatted to [file].actual.cst so they can be compared against the original [file].cst
  - In a situation where the original file is not the expected output, [file].expected.cst can be created and it will be compared instead
  - All tests also product .json and .doctree.txt files to aid in troubleshooting.
- Samples
  - Mainly used for testing the AllInOne.cs file
  - Scratch.cs is also a dumping ground for a quick test, but has mostly been replaced by using the Playground

### CSharpier.CLI
This is a WIP that is mainly used for running CSharpier against a large repository I have access to. I'm working on a method that will hopefully determine if CSharpier loses any code after formatting. To use this on your own repo.
- modify the path in Program.cs
- modify Options to include TestRun = true
- check the result.TestRunFailed to see if CSharpier thinks that it lost something from the source it shouldn't have

### Helpful Information
- https://github.com/prettier/prettier/blob/main/commands.md is useful for understanding how formatting with the Doc classes works. CSharpier hasn't implemented all of the prettier Doc types, and it has added a couple new ones.
- https://sharplab.io/ is useful for understanding the AST that is produced from some C# code
