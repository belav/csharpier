---
title: Troubleshooting
hide_table_of_contents: true
---

The editor extensions run the CSharpier dotnet tool to facilitate formatting files. They keep it running to speed up formatting results. The extensions install CSharpier to a custom path so that running commands like `dotnet tool update csharpier` are possible.

## Enabling Debug Logs
VSCode and VS provide as options
\
Rider requires setting the debug log setting

## Finding Logs
### VSCode
Output - CSharpier
### Visual Studio
Output - CSharpier
### Rider
Show Log in Explorer

## Troubleshooting Steps
Run `C:\Users\[UserName]\AppData\Local\CSharpier\[CSharpierVersion]\dotnet-csharpier --version`
or `$Home/.cache/csharpier/[CSharpierVersion]\dotnet-csharpier --version`

This may fail because of a missing dotnet sdk, install it and try again.

If this fails for another reason, delete the folder.

Run `dotnet tool install csharpier --version [CSharpierVersion] --tool-path C:\Users\[UserName]\AppData\Local\CSharpier\[CSharpierVersion]`
or `dotnet tool install csharpier --version [CSharpierVersion] --tool-path $Home/.cache/csharpier/[CSharpierVersion]`

Repeat step 1