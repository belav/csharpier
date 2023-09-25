<!-- Keep a Changelog guide -> https://keepachangelog.com -->

# csharpier-rider Changelog

## [1.3.10]
- Read from error stream to prevent the plugin hanging when the csharpier process writes too much to the error stream.

## [1.3.9]
- Wait at most 3 seconds for csharpier to format otherwise consider it hung and restart it.

## [1.3.8]
- Add displayName attribute to CSharpier options window to speed up Settings dialog.

## [1.3.7]
- Support for 2023.1-beta

## [1.3.6]
- Disable "CSharpier must be installed globally" popup for sources cache files and the "\" path

## [1.3.5]
- Attempt to detect and recover from csharpier not installing correctly to custom path
- Disable "CSharpier must be installed globally" popup for decompiled files and the "/" path 

## [1.3.0]
- Adding support for detecting version of CSharpier.MsBuild from csproj

## [1.2.7]
- Fix Null Reference Exception from CSharpierProcessPipeMultipleFiles if it fails to start process

## [1.2.6]
- Remove plugin until property so plugin doesn't need to be updated for each new rider version

## [1.2.5]
- Error output loses characters

## [1.2.4]
- Support for 2022.1

## [1.2.3]
- Change csharpier install path on windows to be consistent with other plugins

## [1.2.2]
- Adding support for macOS

## [1.2.1]
- Fixing error when refactoring to move class to new file
- Fixing occasional error when trying to update action

## [1.2.0]
- Fix bug with dotnet csharpier sometimes outputting .net welcome message
- Support for using local version of csharpier from tool manifest file
- Run csharpier from custom tools location so that the global/local versions of the tool are not locked.
- Provide actions for installing csharpier locally or globally if it is not detected
- Hide "Reformat with CSharpier" action for non csharp files
- Better handling of threads for the long-running formatting process

## [1.0.4]
- Adding support for UTF8 and unicode characters. Requires csharpier 0.14.0

## [1.0.3]
- Add logo

## [1.0.2]
- Fix bug where reformat on save can't find project for file

## [1.0.1]
- Initial Release
- Action for Reformat
- Setting for Reformat on Save
- Support for leaving csharpier >= 0.12.0 running for faster formatting