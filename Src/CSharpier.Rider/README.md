TODO clean this up

- https://plugins.jetbrains.com/docs/intellij/welcome.html
- https://developerlife.com/2021/03/13/ij-idea-plugin-advanced/

# csharpier-rider

TODO keep this?
![Build](https://github.com/belav/csharpier-rider/workflows/Build/badge.svg)
[![Version](https://img.shields.io/jetbrains/plugin/v/PLUGIN_ID.svg)](https://plugins.jetbrains.com/plugin/PLUGIN_ID)
[![Downloads](https://img.shields.io/jetbrains/plugin/d/PLUGIN_ID.svg)](https://plugins.jetbrains.com/plugin/PLUGIN_ID)

## Template ToDo list
- [ ] Verify the [plugin ID](/src/main/resources/META-INF/plugin.xml) and [sources package](/src/main/kotlin).
- [ ] Review the [Legal Agreements](https://plugins.jetbrains.com/docs/marketplace/legal-agreements.html).
- [ ] [Publish a plugin manually](https://plugins.jetbrains.com/docs/intellij/publishing-plugin.html?from=IJPluginTemplate)
for the first time.
- [ ] Set the Plugin ID in the above README badges.
- [ ] Set the [Deployment Token](https://plugins.jetbrains.com/docs/marketplace/plugin-upload.html).

<!-- Plugin description -->
[CSharpier](https://github.com/belav/csharpier) is an opinionated code formatter for c#. 
It uses Roslyn to parse your code and re-prints it using its own rules. 
The printing process was ported from [prettier](https://prettier.io/) but has evolved over time.
<!-- Plugin description end -->

## Installation

- Using IDE built-in plugin system:
  
  <kbd>Settings/Preferences</kbd> > <kbd>Plugins</kbd> > <kbd>Marketplace</kbd> > <kbd>Search for "CSharpier"</kbd> >
  <kbd>Install Plugin</kbd>
---
Plugin based on the [IntelliJ Platform Plugin Template][template].

