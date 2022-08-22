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