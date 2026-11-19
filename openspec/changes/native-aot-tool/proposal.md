## Why

The `csharpier` dotnet tool starts on the .NET runtime and pays JIT cost on every invocation, which is most of the time spent on short runs such as formatting one file or a pre-commit check. .NET 10 added RID-specific tool packages that can carry NativeAOT binaries (GitHub issue 474), and the CLI already targets only `net10.0;net11.0`, so the SDK requirement that blocked this is already in place: SDK 9 cannot install CSharpier 1.4.0 today.

## What Changes

- Publish the `CSharpier` tool as a pointer package plus one NativeAOT package per supported runtime identifier: win-x64, win-arm64, linux-x64, linux-arm64, linux-musl-x64, osx-x64, osx-arm64.
- Keep an `any` framework-dependent package as the fallback for every other platform. It is the same managed build that ships today.
- Make `CSharpier.Cli` and the `net10.0`/`net11.0` builds of `CSharpier.Core` free of trim and AOT warnings:
  - Replace reflection-based `System.Text.Json` calls with source-generated contexts.
  - Replace the reflection-based YamlDotNet deserializer used for `.csharpierrc` files.
  - Upgrade System.CommandLine from `2.0.0-beta4` to the stable, AOT-compatible 2.0 release.
  - Fix or replace any dependency the full AOT analysis reports.
- Add CI that builds a NativeAOT binary and runs CLI tests against it on pull requests.
- Replace the single-package publish step with a per-OS build matrix that packs every RID and pushes all packages together.
- No change to command-line options, configuration file formats, output, or exit codes. `CSharpier.MsBuild` and `CSharpier.Core` packages keep shipping managed assemblies.

## Capabilities

### New Capabilities
- `native-aot-distribution`: how the `CSharpier` tool is packaged and published (native packages per runtime identifier, managed fallback), the requirement that native and managed builds behave the same, and the build checks that keep the code AOT-compatible.

### Modified Capabilities

None. The requirements in `cli-formatting-pipeline` and `formatter-performance-invariants` are unchanged; the native build must satisfy them as written.

## Impact

- **Code**: `Src/CSharpier.Cli` (`Program.cs`, `FormattingCommands.cs`, `PipeCommand.cs`, `ServerCommand.cs`, `FormattingCache.cs`, `Options/CSharpierConfigParser.cs`, `Options/ConfigurationFileOptions.cs`, `Server/ServerFormatter.cs`), `Src/CSharpier.Core` (`CSharp/CSharpFormatter.cs`, `DebugLogger.cs`, `Xml/RawNodeSyntaxWriter.cs`, `CSharp/BooleanExpressionParser.cs`), both `.csproj` files.
- **Dependencies**: System.CommandLine (major upgrade), YamlDotNet (static deserializer or removal), possibly NReco.Logging.File, ini-parser-netstandard and StrongNamer depending on analysis results.
- **CI**: `.github/workflows/ValidatePullRequest.yml`, `.github/workflows/publish_nuget.yml`. Adds Windows and macOS runners to the release build.
- **Packages on nuget.org**: one `CSharpier` package becomes nine (`CSharpier`, seven `CSharpier.<rid>`, `CSharpier.any`).
- **Users**: installing with the .NET 10 SDK or later on a listed platform gets the native binary with no command changes. On Windows the SDK creates a `csharpier.cmd` shim for native packages, which the IDE plugins cannot start, so Windows native packages are released after a plugin update that locates the native executable; until then Windows uses the `any` package.
- **Plugins**: `CustomPathInstaller` in `Src/CSharpier.VSCode`, `Src/CSharpier.Rider`, and `Src/CSharpier.VisualStudio`.
- **Docs**: installation page and changelog.
