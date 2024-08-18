## [1.8.0]
- Use dotnet tool list to find both local and global installs of csharpier.

## [1.7.4]
- Support for semver
- Only use CSharpier Server on 0.29.0+

## [1.7.3]
- If CSharpier doesn't respond when trying to find a port, then try to find a port in extension
- Add option to bypass csharpier server.

## [1.7.2]
- Fix bad code path when csharpier server failed to start

## [1.7.1]
- Fix issue with csharpier server not working when localhost resolved to IPv6 ::1

## [1.7.0]
- Use CSharpier Server for 0.28.0+

## [1.5.2]
- Experimental support for CSharpier Server

## [1.5.1]
- Fix for occasional NRE

## [1.5.0]
- Fix issues with using 0.26.X
- Better error handling and logging

## [1.4.8]
- Handle version 0.26.0 and higher which include a +[commitHash] in the version number

## [1.4.7]
- Log exception when finding document fails but don't throw it.

## [1.4.6]
- Possible fix for issue with VS2022 not reformatting on save

## [1.4.5]
- Handle error on startup in VS 17.8.0

## [1.4.4]
- Add support for ARM64

## [1.4.3]
- Fix for format on save sometimes not working in VS2022 17.7

## [1.4.2]
- Recompile with VS2022 17.7 preview 4 - fixes the format on save for 17.7 preview 4.

## [1.4.1]
- Fix for bug where CSharpier hangs when file doesn't compile and Format on Save is also installed.

## [1.4.0]
- Add ability to configure "Reformat on save" at the global level

## [1.3.6]
- Disable "CSharpier must be installed globally" popup for some temporary files

## [1.3.5]
- Attempt to detect and recover from csharpier not installing correctly to custom path

## [1.3.0]
- Adding support for detecting version of CSharpier.MsBuild from csproj

## [1.2.3]
- Modify options to be local to the solution that is open.

## [1.2.2]
- Fix bug for usernames that contain a space not being able to format files

## [1.2.1]
- Fix bug that was leaving threads open

## [1.2.0]
- Fix bug with dotnet csharpier sometimes outputting .net welcome message
- Support for using local version of csharpier from tool manifest file
- Run csharpier from custom tools location so that the global/local versions of the tool are not locked.
- Provide actions for installing csharpier locally or globally if it is not detected

## [1.0.1]
- Support for unicode

## [1.0.0]
- Initial Release
- Action for Reformat
- Setting for Reformat on Save
- Support for leaving csharpier >= 0.12.0 running for faster formatting