## Development
The two main ways to work with CSharpier locally are the playground or with formatting tests.

### Using the playground
From your IDE run the following in the /CSharpier/CSharpier.Playground directory
```bash
dotnet watch run
cd ClientApp
npm run start
```
- This should open the playground website at http://localhost:5000
- Paste the code you want to format there and click format.
- The playground includes options to view the AST and generated doc tree, which help troubleshooting issues.

### Formatting Tests
The main way CSharpier is tested is with the files at src/CSharpier.Tests/FormattingTests/TestFiles. Any changes to formatting should include appropriate test coverage using these files.

A source generator is used to generate an nunit test for each file. That test
- Uses CSharpier to format each [FileName].cst to a new file [FileName].actual.cst
- If a file [FileName].expected.cst exists, it is compared to [FileName].actual.cst instead
- If the files differ, a diff tool will automatically open to allow you to compare the files.

### Other Tests
Most areas of CSharpier are covered by tests. Some to take note of

- /Scripts/TestCli.ps1 - Full end to end style tests, only used for cases that can't be covered another way.
- /Src/CSharpier.Tests/CommandLineFormatterTests - Integration tests that are close to end to end.
- /Src/CSharpier.Tests/DocPrinterTests - used to test the doc types directly. Can be useful to understand how the different doc types work.
- /Scripts/CreateTestingPR.ps1 - used to test the formatting changes in your branch against a large repo. Useful for finding edge cases you may have missed. (this script may require changes to work correctly)

### Helpful Information
- https://github.com/prettier/prettier/blob/main/commands.md is useful for understanding how formatting with the Doc classes works. CSharpier hasn't implemented all of the prettier Doc types, and it has added a couple new ones.
- https://dev.to/fvictorio/how-to-write-a-plugin-for-prettier-6gi is also useful for understanding how formatting works.
- https://sharplab.io/ is useful for understanding the AST that is produced from some C# code
- https://www.linqpad.net/ can also show the AST, but is less forgiving with errors in your code.
- https://www.sapehin.com/blog/csharp-via-roslynapi-the-big-picture/ gives an overview of all the different syntax nodes in c#

### Issues
If you plan to contribute by working on an issue, you can assign it to yourself by adding a comment `.assign`
