## [1.5.1]
- Fixed bug with being unable to setup CSharpier when trying to use a global version of csharpier >= 0.26.0
- Fixed bug with being unable to setup CSharpier with username containing space

## [1.5.0]
- Improved error handling and reporting around csharpier failing to install or run

## [1.3.6]
- Fix bug where 2nd instance of VSCode was not able to format code

## [1.3.5]
- Attempt to detect and recover from csharpier not installing correctly to custom path

## [1.3.0]
- Adding support for detecting version of CSharpier.MsBuild from csproj

## [1.2.4]
- Remove error output display. It used to lose characters.

## [1.2.3]
- Proper fix for unsaved documents not formatting.

## [1.2.2]
- Quick fix for unsaved documents not formatting.

## [1.2.1]
- Fix bug for usernames that contain a space not being able to format files

## [1.2.0]
- Fix bug with dotnet csharpier sometimes outputting .net welcome message
- Run csharpier from custom tools location so that the global/local versions of the tool are not locked.

## [1.1.0]

- Support local version of csharpier
- Support multiple versions of csharpier in the same workspace
- Auto close csharpier process so that it can be updated while vscode is running

## [1.0.4]

- Update readme

## [1.0.3]

- New logo

## [1.0.2]

- Fix bug with long files sometimes losing content

## [1.0.1]

- Fix bug with ignored files

## [1.0.0]

- Initial release
