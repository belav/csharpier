This extension makes use of the dotnet tool [CSharpier](https://github.com/belav/csharpier) to format your code and is versioned independently.

CSharpier is an opinionated code formatter for c#. It uses Roslyn to parse your code and re-prints it using its own rules. The printing process was ported from [prettier](https://prettier.io/) but has evolved over time.

## CSharpier Version
The extension determines which version of csharpier is needed to format a given file by looking for a dotnet manifest file. If one is not found it looks for a globally installed version of CSharpier.

## Usage

- Use the `Reformat with CSharpier` right click context menu action.
- Optionally configure a keyboard shortcut for `EditorContextMenus.CodeWindow.ReformatWithCSharpier`
- Optionally configure `Reformat with CSharpier on Save` under Tools | Options | CSharpier | General
  - This option can be configured at the solution level or at the global level. 

### Troubleshooting
CSharpier will log messages and errors to Output | Show output from: CSharpier

Debug logging can be turned on under Tools | Options | CSharpier | Log Debug Messages

The extension installs CSharpier to `C:\Users\{CurrentUser}\AppData\Local\CSharpier`. Closing the extension and deleting this folder can fix issues with bad installs.

Please report any [issues](https://github.com/belav/csharpier/issues)
