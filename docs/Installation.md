---
hide_table_of_contents: true
---

CSharpier is implemented as a dotnet tool and can be installed with the following commands.

```bash
# if you don't yet have a .config/dotnet-tools.json file
dotnet new tool-manifest

dotnet tool install csharpier
```

This will act as a local dotnet tool for the directory these commands are run from. This ensures the project gets the correct version of CSharpier.

Dotnet tools can also be installed globally with the following command.

```bash
dotnet tool install -g csharpier
```

To update an existing installation of CSharpier, run the dotnet update command.

For local installations run the following command from the install directory.

```bash
dotnet tool update csharpier
```

For global installations run the following command from any location.

```bash
dotnet tool update csharpier -g
```
