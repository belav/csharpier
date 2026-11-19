## ADDED Requirements

### Requirement: Native packages per runtime identifier

The `CSharpier` tool SHALL be published as a pointer package named `CSharpier` together with one NativeAOT package for each of these runtime identifiers: win-x64, win-arm64, linux-x64, linux-arm64, linux-musl-x64, osx-x64, osx-arm64. The win-x64 and win-arm64 packages SHALL NOT be published until plugin versions that can locate the native executable on Windows have been released.

#### Scenario: Install on a listed platform
- **WHEN** a user runs `dotnet tool install csharpier` with the .NET 10 SDK or later on a platform matching a listed runtime identifier
- **THEN** the SDK SHALL install the NativeAOT package for that runtime identifier and the `csharpier` command SHALL run the native executable

#### Scenario: Native executable runs without a .NET runtime lookup
- **WHEN** the installed native `csharpier` executable is started directly with `DOTNET_ROOT` unset
- **THEN** it SHALL run and print its version for `--version`

#### Scenario: Install with tool-path
- **WHEN** a user or IDE plugin runs `dotnet tool install csharpier --version <version> --tool-path <dir>` on a listed platform
- **THEN** on Linux and macOS `<dir>/csharpier` SHALL exist and SHALL start the native executable without a shell
- **AND** on Windows the native executable SHALL exist at `<dir>/.store/csharpier/<version>/csharpier.<rid>/<version>/tools/any/<rid>/CSharpier.exe`

#### Scenario: Windows before native packages are published
- **WHEN** a user or IDE plugin runs `dotnet tool install csharpier --version <version> --tool-path <dir>` on Windows for a version that does not list win-x64 or win-arm64
- **THEN** the `any` package SHALL be installed and `<dir>/csharpier.exe` SHALL exist

### Requirement: Plugins locate the executable on Windows

The VSCode, Rider, and Visual Studio plugins SHALL start `<tool-path>/csharpier.exe` when it exists and otherwise SHALL start the native executable under `<tool-path>/.store`. They SHALL NOT start `csharpier.cmd`.

#### Scenario: Native package installed on Windows
- **WHEN** a plugin has installed a CSharpier version with a Windows native package into its tool path
- **THEN** the plugin SHALL start the native `CSharpier.exe` directly and formatting SHALL work

#### Scenario: Fallback package installed on Windows
- **WHEN** a plugin has installed a CSharpier version that resolves to the `any` package on Windows
- **THEN** the plugin SHALL start `<tool-path>/csharpier.exe` as it does today

#### Scenario: Plugin stops the server
- **WHEN** a plugin stops a CSharpier `server` process it started from a native package on Windows
- **THEN** no CSharpier process SHALL be left running

### Requirement: Framework-dependent fallback package

The `CSharpier` tool SHALL publish a framework-dependent package for the `any` runtime identifier that contains the managed build for every target framework the CLI supports.

#### Scenario: Install on an unlisted platform
- **WHEN** a user installs the tool on a platform that matches none of the listed runtime identifiers
- **THEN** the SDK SHALL install the `any` package and the `csharpier` command SHALL run on the installed .NET runtime

#### Scenario: Fallback behaves like the current tool
- **WHEN** the `any` package is installed and run
- **THEN** it SHALL accept the same commands and options and produce the same output and exit codes as the managed tool did before this change

### Requirement: Native and managed builds behave the same

The NativeAOT build SHALL produce the same formatted output, console output, and exit codes as the managed build for the same inputs, configuration, and options. This covers the `format`, `check`, `pipe-files`, and `server` commands, `.csharpierrc` files in JSON and YAML, `.editorconfig`, `.csharpierignore`, the formatting cache, and the MSBuild version mismatch check.

#### Scenario: Formatting output matches
- **WHEN** the native and managed builds format the same set of C# and XML files with the same options
- **THEN** the resulting files SHALL be byte-for-byte identical

#### Scenario: YAML configuration is honored
- **WHEN** the native build formats a file in a directory containing a YAML `.csharpierrc` that sets `printWidth`, `useTabs`, `indentSize`, `endOfLine`, and `overrides`
- **THEN** the options SHALL be applied the same way the managed build applies them

#### Scenario: JSON configuration is honored
- **WHEN** the native build formats a file in a directory containing a JSON `.csharpierrc` with property names in any casing
- **THEN** the options SHALL be applied the same way the managed build applies them

#### Scenario: Server command round trip
- **WHEN** the native build is started with `server` and receives a POST to `/format` with a file name and file contents
- **THEN** it SHALL respond with JSON containing the formatted file and status in the same format the managed build returns

#### Scenario: Cache written by one build is safe for the other
- **WHEN** a cache file written by the managed build is read by the native build, or the reverse
- **THEN** the build SHALL either use the entries or ignore them, and SHALL NOT fail or produce different formatted output

#### Scenario: Invalid arguments
- **WHEN** the native build is run with an unknown option or a missing required argument
- **THEN** it SHALL print an error and return a non-zero exit code, as the managed build does

### Requirement: Build fails on trim and AOT warnings

The `CSharpier.Cli` project and the `net10.0` and `net11.0` builds of `CSharpier.Core` SHALL enable the trim and AOT analyzers, and the warnings-as-errors build used in CI SHALL fail when any trim or AOT warning is reported. Warnings SHALL NOT be suppressed without a comment at the suppression stating why the flagged code is safe.

#### Scenario: Reflection-based serialization is added
- **WHEN** a change adds a call that requires unreferenced or dynamic code, such as `JsonSerializer.Serialize<T>(T)` without a `JsonTypeInfo`
- **THEN** `dotnet build CSharpier.slnx -c release -p:TreatWarningsAsErrors=true` SHALL fail

#### Scenario: Native publish reports a warning from a dependency
- **WHEN** the NativeAOT publish in CI reports an IL trim or AOT warning from any assembly
- **THEN** the CI job SHALL fail

### Requirement: Pull requests test the native build

Pull request validation SHALL publish a NativeAOT build for at least one runtime identifier and run the CLI process tests against that executable.

#### Scenario: Native build breaks at run time
- **WHEN** a pull request introduces code that compiles but fails under NativeAOT at run time, such as reflection over a trimmed member
- **THEN** a pull request check that runs the native executable SHALL fail

### Requirement: Packages are published together

The release workflow SHALL build every runtime-specific package on a runner that can compile for it, and SHALL push the pointer package, every runtime-specific package, and the `any` package for a version only after all of them have been built successfully.

#### Scenario: One runtime identifier fails to build
- **WHEN** the NativeAOT build for any listed runtime identifier fails during a release
- **THEN** no `CSharpier` package for that version SHALL be pushed to nuget.org

#### Scenario: Version unchanged
- **WHEN** the workflow runs and the version in `Nuget/Build.props` is already published
- **THEN** no packages SHALL be pushed

### Requirement: MSBuild package keeps using the managed build

The `CSharpier.MsBuild` package SHALL continue to bundle the managed `CSharpier.dll` for each supported target framework and run it with `dotnet exec`.

#### Scenario: Build with CSharpier.MsBuild
- **WHEN** a project referencing `CSharpier.MsBuild` is built on any platform with a supported SDK
- **THEN** formatting SHALL run using the bundled managed assembly, and the scenarios in `Tests/MsBuild` SHALL pass unchanged
