<!-- Keep a Changelog guide -> https://keepachangelog.com -->

# csharpier-rider Changelog

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