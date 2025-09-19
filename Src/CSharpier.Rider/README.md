# csharpier-rider

<!-- Plugin description -->

This plugin makes use of the dotnet tool [CSharpier](https://github.com/belav/csharpier) to format your code and is versioned independently.

CSharpier is an opinionated code formatter for c# and xml. \
It provides very few options and provides a deterministic way to enforce formatting of your code. \
The printing process was ported from [prettier](https://prettier.io) but has evolved over time. \

## CSharpier Version

The plugin determines which version of csharpier is needed to format a give file by looking for a dotnet manifest file. If one is not found it looks for a globally installed version of CSharpier.

## XML Formatting
Formatting XML is only available using CSharpier >= 1.0.0

### To format files:

- Install csharpier
  - as a local tool versioned to your project with `dotnet tool install csharpier`
  - globally with `dotnet tool install -g csharpier`
- Use the `Reformat with CSharpier` action.
  - Available when right clicking on a file in the editor
  - Available via "Search Everywhere"
  - Does not have a default keyboard shortcut but can be assigned one.
- Optionally configure CSharpier to `Run on Save` under Preferences/Settings | Tools | CSharpier

Please report any [issues](https://github.com/belav/csharpier/issues)

## Installation

- Using IDE built-in plugin system:

  <kbd>Settings/Preferences</kbd> > <kbd>Plugins</kbd> > <kbd>Marketplace</kbd> > <kbd>Search for "CSharpier"</kbd> >
  <kbd>Install Plugin</kbd>

---

## Troubleshooting

See [Editor Troubleshooting](https://csharpier.com/docs/EditorsTroubleshooting) for more information.

**Note** This plugin does not do any formatting and is versioned separately from CSharpier.

### Viewing CSharpier Logs

- Use the action "Show Log in Explorer"
- Look for entries for "CSharpierLogger"

### Enable Debug Logging for CSharpier

- Use the action "Debug Log Settings"
- Add entry for "#com.intellij.csharpier.CSharpierLogger"
- Restart Rider

### Release Notes

[Change Log](https://github.com/belav/csharpier/blob/main/Src/CSharpier.Rider/CHANGELOG.md)

<!-- Plugin description end -->
