---
title: Troubleshooting
hide_table_of_contents: true
---

The editor extensions run CSharpier to facilitate format files.
CSharpier is kept running to speed up formatting results.
The extensions install CSharpier to a custom path to avoid commands like `dotnet tool update -g csharpier` failing because a CSharpier is running.

When the extension is unable to format files, it is generally a problem with being unable to install or execute the extension. You can use the information from the logs it outputs to understand the failure and then attempt the troubleshooting steps below.

## Enabling Debug Logs
### VisualStudio
- Navigate to `Tools - Options - CSharpier`
- Change `Log Debug Messages` to `true`

### VSCode
- Navigate to `File - Preferences - Settings - Extensions - CSharpier`
- Check `Enable debug logs`
- Restart VSCode

### Rider
- Execute the action `Debug Log Settings`
- Add an entry for `#com.intellij.csharpier.CSharpierLogger`
- Restart Rider

## Locating Logs
### Visual Studio
- Navigate to `View - Output`
- Change the `Show output from:` dropdown to `CSharpier`

### VSCode
- Navigate to `View - Output`
- Change the dropdown to `CSharpier`

### Rider
- Execute the action `Show Log in Explorer`
- Look for lines that contain `#c.i.c.CSharpierLogger`

## Troubleshooting Steps
The following can help track down issues with the extension being unable to install/run CSharpier

1. Validate the following command can run in the directory of your project<br/>
   `dotnet csharpier --version`
2. If the extension was able to install CSharpier it should exist at a path such as<br/>
   `C:\Users\[UserName]\AppData\Local\CSharpier\[CSharpierVersion]` or<br/>
   `$HOME/.cache/csharpier/[CSharpierVersion]`
3. Assuming the directory above exists, attempt to run the following in that directory<br/>
   `dotnet-csharpier --version`
4. If the installation appears to be corrupt, delete the directory and install CSharpier there yourself<br/>
   `dotnet tool install csharpier --version [CSharpierVersion] --tool-path [PathFromStep2]`
5. Repeat step 3 to validate the install
