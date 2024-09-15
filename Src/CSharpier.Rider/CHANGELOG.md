<!-- Keep a Changelog guide -> https://keepachangelog.com -->

# csharpier-rider Changelog

## [1.8.2]
- Possible fix for issue with OSX not being able to run dotnet tool list command
- Better handling of error when validating custom install of csharpier

## [1.8.0]
- Use dotnet tool list to find both local and global installs of csharpier.

## [1.7.4]
- Only use CSharpier Server for 0.29.0+
- Add option to bypass csharpier server.

## [1.7.3]
- Fix issue with csharpier server not working when localhost resolved to IPv6 ::1

## [1.7.2]
- Better error message when csharpier server fails to start

## [1.7.1]
- Fix unicode issue with csharpier 0.28.0+

## [1.7.0]
- Fix deprecated function warning
- Use CSharpier Http Server for 0.28.0+
- Log version of CSharpier used to format a given file

## [1.6.4]
- Better support for format on save - now works when files are saved via building/running the project.

## [1.6.3]
- Fix issue with occasional exception "java.lang.IllegalStateException: |E| Wrong thread RdOptionalProperty"

## [1.6.2]
- Fix issues with lookup of '.NET CLI executable path', csharpier will now wait until rider is ready with the information.
- No more falling back to `PATH`

## [1.6.1]
- Delay lookup of '.NET CLI executable path' until it is needed
- Fall back to looking for dotnet on PATH if '.NET CLI executable path' is not available

## [1.6.0]
- Better support for dotnet commands.
  - Uses the Rider setting for '.NET CLI executable path' for running dotnet commands
  - If unable to run dotnet commands, show an error message

## [1.5.3]
- Add experimental support for CSharpier server

## [1.5.2]
- Support for a CSharpier version format of 0.X.Y+HASH in dotnet-tools.json

## [1.5.1]
- Ensure rider plugin always uses \n as a line separator

## [1.5.0]
- Improved error handling and reporting around csharpier failing to install or run

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