---
hide_table_of_contents: true
---

## Installing CSharpier

CSharpier is implemented as a dotnet tool and can be installed with the following commands.

```bash
# if you don't yet have a .config/dotnet-tools.json file
dotnet new tool-manifest

dotnet tool install csharpier
```

This will act as a local dotnet tool for the directory these commands are run from. This ensures the project gets the correct version of CSharpier.

A local install is run with `dotnet csharpier`.

```bash
dotnet csharpier --version
```

Dotnet tools can also be installed globally with the following command.

```bash
dotnet tool install -g csharpier
```

A global install is run with `csharpier`, without the `dotnet` prefix. Running `dotnet csharpier` with only a global install fails with `Could not execute because the specified command or file was not found`. This is a limitation of how dotnet runs global tools, see [dotnet/sdk#14626](https://github.com/dotnet/sdk/issues/14626).

```bash
csharpier --version
```

The rest of the documentation uses `dotnet csharpier`. If CSharpier is installed globally, replace `dotnet csharpier` with `csharpier` in those examples.

## Updating CSharpier

To update an existing installation of CSharpier, run the dotnet update command.

For local installations run the following command from the install directory.

```bash
dotnet tool update csharpier
```

For global installations run the following command from any location.

```bash
dotnet tool update -g csharpier
```
