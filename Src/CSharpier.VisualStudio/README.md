This extension adds support for [CSharpier](https://github.com/belav/csharpier), an opinionated code formatter for c#.
It uses Roslyn to parse your code and re-prints it using its own rules.
The printing process was ported from [prettier](https://prettier.io/) but has evolved over time.

To use it:
- Install csharpier globally with `dotnet tool install -g csharpier`
- Use the `Reformat with CSharpier` right click context menu action.
- Optionally configure a keyboard shortcut for `EditorContextMenus.CodeWindow.ReformatWithCSharpier`
- Optionally configure `Reformat with CSharpier on Save` under Tools | Options | CSharpier | General

Please report any [issues](https://github.com/belav/csharpier/issues)

### Troubleshooting
CSharpier will log messages and errors to Output | Show output from: CSharpier  
Debug logging can be turned on under Tools | Options | CSharpier | Log Debug Messages  
The extension installs CSharpier to `C:\Users\{CurrentUser}\AppData\Local\CSharpier`. Closing the extension and deleting this folder can fix issues with bad installs.
