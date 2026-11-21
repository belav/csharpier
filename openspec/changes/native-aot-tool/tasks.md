## 1. Baseline and full warning list

- [x] 1.1 Install the NativeAOT prerequisites on the development machine (Visual Studio "Desktop development with C++" workload) and confirm a trivial `PublishAot` console app links
- [x] 1.2 Add `<PublishAot>true</PublishAot>` to `Src/CSharpier.Cli/CSharpier.Cli.csproj` and `<IsAotCompatible>true</IsAotCompatible>` to `Src/CSharpier.Core/CSharpier.Core.csproj` conditioned on the `net10.0`/`net11.0` target frameworks
- [x] 1.3 Run `dotnet publish Src/CSharpier.Cli -c Release -f net10.0 -r win-x64` and record every IL warning grouped by assembly in `design.md` under Context
- [x] 1.4 Record baseline timings for the managed tool: `--version`, formatting one file, `check` on this repository with and without cache
- [x] 1.5 Capture current CLI output to files for later comparison: `--help` for the root and each command, `--version`, an unknown option, a missing argument
- [x] 1.6 Update `design.md` decisions 4, 5, and 7 if the warning list contradicts them

## 2. JSON source generation

- [x] 2.1 Add a test that pins the JSON produced for `PrinterOptions` in the cache hash so a change to the cache key is visible
- [x] 2.2 Add a `JsonSerializerContext` in `CSharpier.Cli` covering the cache dictionary, `ConfigurationFileOptions` and nested types, `FormatFileParameter`, `FormatFileResult`, and `PrinterOptions`
- [x] 2.3 Switch `FormattingCache.cs` to the context for read, write, and hash
- [x] 2.4 Switch `CSharpierConfigParser.ReadJson` to the context, keeping case-insensitive property names and `CaseInsensitiveEnumConverter`
- [x] 2.5 Switch `ServerFormatter.cs` request and response serialization to the context
- [x] 2.6 Replace reflection-based JSON in `CSharpier.Core` (`CSharpFormatter.cs`, `DebugLogger.cs`, `Xml/RawNodeSyntaxWriter.cs`) with `JsonNode`/`Utf8JsonWriter` or a context, keeping the `netstandard2.0` build compiling
- [x] 2.7 Run the full test suite and confirm no IL2026/IL3050 warnings remain for `System.Text.Json`

## 3. YAML configuration

- [x] 3.1 Add tests for YAML `.csharpierrc` covering every supported key, `overrides`, unknown keys, and an empty file, if not already covered
- [x] 3.2 Skipped: went straight to 3.3 to avoid adding the YamlDotNet static generator package (see design decision 4)
- [x] 3.3 Map the keys by hand from YamlDotNet's `YamlStream` model in `CSharpierConfigParser.ReadYaml`
- [x] 3.4 Run the full test suite and confirm no YamlDotNet warnings remain

## 4. System.CommandLine under NativeAOT

- [x] 4.1 Keep System.CommandLine on `2.0.0-beta4` (design decision 5, revised) and compare native output with the files captured in 1.5 for help, version, unknown option, and missing argument
- [x] 4.2 If any output or exit code differs, stop and revisit the upgrade to stable 2.0

## 5. Remaining warnings

- [x] 5.1 Confirm files with `#if` directives format the same in the native build (`BooleanExpressionParser` produced no warning)
- [x] 5.2 Confirm the `server` command writes its log file in the native build (NReco.Logging.File produced no warning)
- [x] 5.3 Confirm `.editorconfig` settings are applied in the native build (ini-parser-netstandard produced no warning)
- [x] 5.4 Suppress the Roslyn IL3000 warning for `CommonCompiler.GetAssemblyLocation` with a comment stating why it is safe
- [x] 5.5 Confirm `dotnet publish` for win-x64 completes with zero IL warnings
- [x] 5.6 Confirm `dotnet build CSharpier.slnx -c release -p:TreatWarningsAsErrors=true` passes and fails when a reflection-based `JsonSerializer` call is added temporarily

## 6. Native build tests

- [x] 6.1 Let `CSharpierProcess.Tests` run against an executable path supplied by an environment variable, defaulting to the current behavior
- [x] 6.2 Add process tests for spec scenarios not yet covered: YAML and JSON config, `server` round trip, `pipe-files`, cache written by the other build, invalid arguments, `--version` with `DOTNET_ROOT` unset
- [x] 6.3 Add a parity script that formats the repository's test files with the managed and native builds into separate directories and fails on any difference
- [x] 6.4 Run 6.1 to 6.3 locally against the win-x64 native build and fix failures
- [ ] 6.5 (job added, not yet run on GitHub) Add a `test_native` job to `.github/workflows/ValidatePullRequest.yml` that publishes linux-x64 with warnings as errors and runs the process tests and parity script against it

## 7. Benchmark

- [x] 7.1 Repeat the timings from 1.4 with the native build and record both sets in `design.md`
- [x] 7.2 If whole-repository formatting is slower than the managed build, record the numbers and stop for a maintainer decision before continuing to packaging

## 8. Packaging

- [x] 8.1 Add `ToolPackageRuntimeIdentifiers` with the seven RIDs plus `any` to `CSharpier.Cli.csproj` and resolve the multi-targeting open question from `design.md`
- [x] 8.2 Work out and document the exact pack commands for the pointer package, a native package, and the `any` package; record the answers to the open questions in `design.md`
- [x] 8.3 Pack the pointer, win-x64, and `any` packages into a local feed and inspect their contents
- [x] 8.4 Install from the local feed with the .NET 10 SDK and the .NET 11 SDK as a local tool and with `--tool-path`; confirm the native executable runs (on Windows `--tool-path` creates `csharpier.cmd`, not `csharpier.exe`, see design decision 12)
- [x] 8.5 Install from a local feed that lacks the win-x64 package to confirm the `any` fallback is selected and runs
- [ ] 8.6 (package contents confirmed after adding excludes for runtime specific folders; `Tests/MsBuild/Run.ps1` not run, it needs docker) Confirm `dotnet pack Src/CSharpier.MsBuild` still bundles the managed `CSharpier.dll` for both target frameworks and `Tests/MsBuild/Run.ps1` passes
- [x] 8.7 Confirm `dotnet build` and `dotnet test` output paths are unchanged for the solution
- [x] 8.8 Build the linux-x64 native package in a Linux container, install it with `--tool-path`, and confirm `<tool-path>/csharpier` is a file that starts without a shell (it is a relative symlink to the native executable in `.store`; see design decision 11)
- [x] 8.9 Install with `dotnet tool install -g` on Windows and Linux and record what shim each creates (Windows: `csharpier.cmd`, Linux: symlink; see design decision 11)

## 9. Release workflow

- [ ] 9.1 (written in `publish_nuget.yml`, not yet run on GitHub) Add a job to `publish_nuget.yml` that outputs whether the version in `Nuget/Build.props` is already on nuget.org
- [ ] 9.2 (written in `publish_nuget.yml`, not yet run on GitHub) Add a matrix job that packs native packages on windows (win-x64, win-arm64), ubuntu (linux-x64, linux-arm64, linux-musl-x64 and linux-musl-arm64 with `docker run` of the Alpine AOT image), and macOS (osx-arm64, osx-x64) and uploads them as artifacts
- [ ] 9.3 (written in `publish_nuget.yml`, not yet run on GitHub) Add a job that packs the pointer and `any` packages
- [ ] 9.4 (written in `publish_nuget.yml`, not yet run on GitHub) Add a final job that requires all pack jobs, pushes runtime packages then the pointer package with the trusted-publishing login
- [ ] 9.5 (written in `publish_nuget.yml`, not yet run on GitHub) Remove the `alirezanet/publish-nuget` step for the CLI; keep the steps for `CSharpier.Core` and `CSharpier.MsBuild`
- [x] 9.6 Confirm the Alpine AOT SDK image tag used for linux-musl-x64 and linux-musl-arm64 exists for both architectures and has an SDK that satisfies `global.json`
- [ ] 9.7 Dry-run the workflow on a branch with the push step disabled and inspect all ten packages

## 10. Plugins: find the native executable on Windows

These are done in this change and released before the CSharpier version with native packages (design decision 12).

- [ ] 10.1 VSCode: in `CustomPathInstaller.getPathForVersion`, return `csharpier.exe` when it exists; otherwise return the native executable under `.store/csharpier/<version>/csharpier.<rid>/<version>/tools/any/<rid>/CSharpier.exe`; keep the current path on Linux and macOS
- [ ] 10.2 Rider: the same lookup in `CustomPathInstaller.java`
- [ ] 10.3 Visual Studio: the same lookup in `CustomPathInstaller.cs`
- [ ] 10.4 Base the lookup on which file exists, not on the CSharpier version, because the same version installs `csharpier.exe` on platforms that use the `any` package
- [ ] 10.5 Start the resolved executable directly, not `csharpier.cmd`, so stopping the process stops the CSharpier server and no shell is needed
- [ ] 10.6 Check the paths the plugins use for an existing global or local tool install and apply the same lookup where they start an executable directly
- [ ] 10.7 Test each plugin on Windows against a `--tool-path` install of a locally packed win-x64 native package and against an `any` install: format a file, restart the server, close the IDE and confirm no CSharpier process is left running
- [ ] 10.8 Test the VSCode and Rider plugins on Linux or macOS against a locally packed native package
- [ ] 10.9 Release the three plugins and record the first plugin versions that support native packages on Windows

## 11. Release

- [ ] 11.1 Publish a prerelease version and install it on Windows x64, Linux glibc, Alpine x64, Alpine arm64 if a machine or emulation is available, and macOS arm64
- [ ] 11.2 Verify the plugin versions from 10.9 install and run the prerelease on each platform, including the `server` command
- [ ] 11.3 Install the prerelease with a plugin version older than 10.9 on Windows and record the error the user sees
- [ ] 11.4 Update the installation docs: native packages, supported RIDs, the managed fallback, minimum glibc, the .NET 10 SDK requirement, and the minimum plugin versions on Windows with the error from 11.3 and how to fix it
- [ ] 11.5 Add a changelog entry that calls out the minimum plugin versions on Windows
- [ ] 11.6 Run `dotnet csharpier format .` and the full test suite
- [ ] 11.7 Publish the stable version
