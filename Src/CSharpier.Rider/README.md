# csharpier-rider

<!-- Plugin description -->
This plugin adds support for [CSharpier](https://github.com/belav/csharpier), an opinionated code formatter for c#. 
It uses Roslyn to parse your code and re-prints it using its own rules. 
The printing process was ported from [prettier](https://prettier.io/) but has evolved over time.

To use it:
- Install csharpier globally with `dotnet tool install -g csharpier`
- Use the `Reformat with CSharpier` action.
- Optionally configure CSharpier to `Run on Save` under Preferences/Settings | Tools | CSharpier

Please report any [issues](https://github.com/belav/csharpier/issues)

## Installation

- Using IDE built-in plugin system:
  
  <kbd>Settings/Preferences</kbd> > <kbd>Plugins</kbd> > <kbd>Marketplace</kbd> > <kbd>Search for "CSharpier"</kbd> >
  <kbd>Install Plugin</kbd>
---

## Troubleshooting

### Viewing CSharpier Logs
- Use the action "Show Log in Explorer"
- Look for entries for "CSharpierLogger"

### Enable Debug Logging for CSharpier
- Use the action "Debug Log Settings"
- Add entry for "#com.intellij.csharpier.CSharpierLogger"
- Restart Rider

<!-- Plugin description end -->

