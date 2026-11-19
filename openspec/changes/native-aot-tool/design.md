## Context

`CSharpier.Cli` is packed as a framework-dependent dotnet tool targeting `net10.0;net11.0` with `RollForward=Major`. `publish_nuget.yml` builds and pushes it from a single ubuntu job with `alirezanet/publish-nuget`.

.NET 10 added RID-specific tool packages: a pointer package lists the available runtime identifiers and the SDK downloads the matching `<PackageId>.<rid>` package at install time. Those packages can hold NativeAOT executables. Only the .NET 10 SDK and later can install them. That is already true of CSharpier: installing 1.4.0 with SDK 9.0.307 fails with `Settings file 'DotnetToolSettings.xml' was not found in the package`.

A trial `dotnet publish` with `PublishAot=true` (win-x64, net10.0) gave these results:

- The analyzer reported IL2026/IL3050 at seven call sites in the CLI project: `JsonSerializer` in `FormattingCache.cs` (3), `Options/CSharpierConfigParser.cs` (1), `Server/ServerFormatter.cs` (2), and YamlDotNet `DeserializerBuilder` in `Options/CSharpierConfigParser.cs` (1).
- The publish stopped with `Platform linker not found` because the development machine has no MSVC toolchain. The whole-program analysis (ILC) did not run, so warnings from `CSharpier.Core`, Roslyn, System.CommandLine, NReco.Logging.File, ini-parser-netstandard, and System.IO.Abstractions are not known yet.
- Setting `PublishAot` on the command line fails with NETSDK1207 because the property flows to the `netstandard2.0` projects. It has to be set in `CSharpier.Cli.csproj`.

### Full publish results (task 1.3)

The MSVC toolchain was already installed (VS 2026, MSVC 14.50). The first failure came from running the publish in Git Bash, which does not pass the `ProgramFiles(x86)` variable that `findvcvarsall.bat` needs. From PowerShell the link step also needs `C:\Program Files (x86)\Microsoft Visual Studio\Installer` on `PATH`; without it `vcvarsall.bat` prints `'vswhere.exe' is not recognized` and the ILCompiler targets capture that text as part of the linker path.

`dotnet publish Src/CSharpier.Cli -c Release -f net10.0 -r win-x64` then succeeds and produces a 25 MB `CSharpier.exe`. Warnings by assembly:

| Assembly | Warnings |
|---|---|
| CSharpier (CLI) | IL2026/IL3050 for `JsonSerializer` in `FormattingCache.cs` lines 70, 163, 191; `CSharpierConfigParser.cs` line 80; `ServerFormatter.cs` lines 79, 86. IL3050 for `DeserializerBuilder` in `CSharpierConfigParser.cs` line 86 |
| CSharpier.Core | IL2026/IL3050 for `JsonSerializer` in `CSharpFormatter.cs` lines 212, 213, 221; `DebugLogger.cs` line 16; `Xml/RawNodeSyntaxWriter.cs` line 9 |
| YamlDotNet | IL2104 and IL3053 (assembly produced trim and AOT warnings) |
| Microsoft.CodeAnalysis | IL3000 in `CommonCompiler.GetAssemblyLocation` (`Assembly.Location`); this is command-line compiler code the formatter does not call |
| System.CommandLine, NReco.Logging.File, ini-parser-netstandard, System.IO.Abstractions, StrongNamer | none |

`BooleanExpressionParser` produced no warning.

Behavior of the unmodified native build: `--version`, `--help`, `check`, and `format --no-cache` work. `format` with the cache enabled throws, as expected from the JSON warnings.

### Baseline timings (task 1.4)

Ryzen 9 7900X, Windows 11, 5 runs each, median. Managed is `dotnet CSharpier.dll` (Release, net10.0). `Src` is this repository's `Src` directory, 421 files after ignores.

| Run | Managed | Native |
|---|---|---|
| `--version` | 56 ms | 12 ms |
| `format one.cs --no-cache` (300-line file) | 289 ms | 33 ms |
| `check Src` | 394 ms | 166 ms |
| `format Src --skip-write --no-cache` | 394 ms | 156 ms |

The native numbers are from the build before any fixes and are repeated in task 7.1. A larger input is still needed to judge throughput on big repositories.

Other facts that constrain the design:

- `CSharpier.Core` multi-targets `netstandard2.0`. AOT analysis only applies to the `net10.0`/`net11.0` builds.
- `CSharpier.Core` calls reflection-based `JsonSerializer` in `CSharpFormatter.cs` (syntax tree JSON for the playground), `DebugLogger.cs`, and `Xml/RawNodeSyntaxWriter.cs`.
- `BooleanExpressionParser` builds `System.Linq.Expressions` trees to evaluate preprocessor conditions.
- System.CommandLine is pinned to `2.0.0-beta4.22272.1`. The stable 2.0 release is AOT-compatible and has a different API.
- `CSharpier.MsBuild` runs `dotnet build` in `Src/CSharpier.Cli`, copies `bin/<Configuration>/<TargetFramework>` into its package, and runs `dotnet exec CSharpier.dll`.
- VSCode, Rider, and Visual Studio plugins install with `dotnet tool install csharpier --tool-path` and start the `csharpier` shim in that directory.
- NativeAOT cannot compile across operating systems. Cross-architecture builds on the same OS work when the toolchain for the target architecture is installed.

## Goals / Non-Goals

**Goals:**

- `dotnet tool install csharpier` on a listed platform installs a native executable.
- Unlisted platforms keep working through the `any` framework-dependent package.
- The native and managed builds produce identical results.
- Trim and AOT warnings fail the build so compatibility does not regress.
- A measured startup and total-time comparison exists before the first release that includes native packages.

**Non-Goals:**

- Shipping a native binary inside `CSharpier.MsBuild`.
- Standalone downloads outside NuGet (GitHub release archives, Homebrew, winget).
- Changing how plugins talk to CSharpier. The only plugin change is how they locate the executable on Windows (decision 12).
- AOT compatibility for the `netstandard2.0` build of `CSharpier.Core`, the playground, or test projects.
- Changing CLI options, configuration formats, or formatting output.

## Decisions

### 1. Land the work in three stages, each releasable

1. **AOT-clean code** while still shipping the managed tool. Analyzers on, warnings fixed, dependencies swapped. This stage changes no packaging and can be released on its own.
2. **Native build in CI**. A pull request job publishes one RID and runs tests against the executable.
3. **Packaging and publish**. RID-specific packages and the new release workflow.

Alternative: one change with everything. Rejected because the System.CommandLine upgrade and the config parser change carry behavior risk unrelated to packaging and are easier to review and revert alone.

### 2. Hybrid packaging with `ToolPackageRuntimeIdentifiers`

Set in `CSharpier.Cli.csproj`:

```xml
<PublishAot>true</PublishAot>
<ToolPackageRuntimeIdentifiers>win-x64;win-arm64;linux-x64;linux-arm64;linux-musl-x64;osx-x64;osx-arm64;any</ToolPackageRuntimeIdentifiers>
```

`dotnet pack` produces the pointer package. `dotnet pack -r <rid>` produces each native package. The `any` package is packed with `PublishAot=false`. `ToolPackageRuntimeIdentifiers` is used instead of `RuntimeIdentifiers` so that plain `dotnet build` output paths stay as they are, which `CSharpier.MsBuild` and the test projects depend on.

Alternative: AOT only. Rejected by the maintainer; it drops unlisted platforms and removes the managed DLL that `CSharpier.MsBuild` bundles.

Alternative: self-contained or ReadyToRun packages. Rejected: larger packages and a smaller startup gain than NativeAOT.

### 3. JSON: source-generated contexts

- CLI: one `JsonSerializerContext` covering `ConcurrentDictionary<string, string>` (cache), `ConfigurationFileOptions` and its nested types with `PropertyNameCaseInsensitive = true`, `FormatFileParameter`, `FormatFileResult`, and `PrinterOptions` for the cache hash.
- The cache hash currently serializes `PrinterOptions` by reflection. Source generation must emit the same property set, or the cache key changes. A changed key only causes one cold run, because the key already includes the CSharpier version; this is acceptable but must be a deliberate outcome, checked by a test that pins the serialized form.
- `CaseInsensitiveEnumConverter` stays, registered on the context options.
- Core: `DebugLogger` and `RawNodeSyntaxWriter` get a context or hand-written `Utf8JsonWriter` output. `CSharpFormatter` re-indents JSON by round-tripping through `Deserialize<object>`; replace with `JsonNode.Parse(...).ToJsonString(indented)` or `JsonDocument` plus `Utf8JsonWriter`, and write the `{ exception }` object by hand.

Alternative: keep reflection and add `TrimmerRootDescriptor` entries. Rejected: it hides warnings and fails at run time when a type changes.

### 4. YAML: hand-mapped from YamlDotNet's representation model

Implemented with the fallback described below (`YamlStream` plus a hand-written mapping of the known keys) without trying the static generator first. It needs no new package, the config has eight keys, and the full publish reports no warnings from YamlDotNet once `DeserializerBuilder` is gone. Keys are matched case-sensitively in camelCase, as before. A YAML file that contains only comments now gives default options instead of a null reference error.

The original text:


Use `StaticDeserializerBuilder` with a `YamlStaticContext` generated by `Vecc.YamlDotNet.Analyzers.StaticGenerator` for `ConfigurationFileOptions` and its nested override type.

If the static generator cannot handle the types (it has limits around collections and custom converters) or YamlDotNet still produces ILC warnings, fall back to parsing with YamlDotNet's low-level `YamlStream` representation model and mapping the known keys by hand. The config has a small fixed set of keys (`printWidth`, `useTabs`, `indentSize`, `endOfLine`, `overrides[]` with `files`, `formatter`, and the same options).

Either way, unknown keys stay ignored and existing config tests must pass unchanged.

### 5. System.CommandLine: stay on beta4 for this change

Revised after task 1.3. The full publish reports no warnings from System.CommandLine `2.0.0-beta4`, and the native build parses arguments, prints help and version, and runs handlers. The commands use `SetHandler(context => ...)` with `ParseResult.GetValueForOption`, which does not bind by reflection. The upgrade to stable 2.0 is therefore not needed for NativeAOT and is left out of this change to avoid unrelated behavior risk. The native process tests cover help, version, unknown options, and missing arguments to confirm beta4 keeps working.

The original plan, kept for reference if beta4 fails a native test:

Rewrite `Program.cs`, `FormattingCommands.cs`, `PipeCommand.cs`, and `ServerCommand.cs` against the 2.0 API (`Options.Add`, `SetAction`, `ParseResult.GetValue`, `Parse(args).InvokeAsync()`). Before the rewrite, capture `--help` output for the root and each command, the output for an unknown option, and the output for `--version`, then compare after. Differences in help layout that come from the library are acceptable; option names, aliases, defaults, arity, and exit codes are not allowed to change.

Alternative: stay on beta4. To be confirmed by ILC, but beta4 binds handlers by reflection and is not annotated; it is also four years out of date.

Alternative: hand-written argument parsing. Rejected: more code to maintain for no gain once 2.0 works.

### 6. Analyzer setup

- `CSharpier.Cli.csproj`: `PublishAot=true` turns on the trim, AOT, and single-file analyzers at build time.
- `CSharpier.Core.csproj`: `IsAotCompatible=true` conditioned on the `net10.0`/`net11.0` target frameworks.
- The existing `-p:TreatWarningsAsErrors=true` CI build then fails on IL warnings with no extra flags.
- The native publish job also runs with warnings as errors so ILC-only warnings from dependencies fail it.

### 7. Remaining dependencies: decide from ILC output

Revised after task 1.3: NReco.Logging.File, ini-parser-netstandard, System.IO.Abstractions, StrongNamer, and `BooleanExpressionParser` produce no warnings and need no changes unless a native test fails. The one Roslyn warning (IL3000 in `CommonCompiler.GetAssemblyLocation`) is in command-line compiler code the formatter does not call; it is suppressed for that assembly only, with a comment.

The original expectations, kept for reference:

- **Roslyn** (`Microsoft.CodeAnalysis.CSharp`): parsing and syntax APIs are expected to work; if ILC warns inside Roslyn for code paths CSharpier does not reach, record the warning and cover the path with a native test before suppressing.
- **`BooleanExpressionParser`**: check whether it calls `Compile()`. Under AOT that runs through the expression interpreter, which works but is slow and may warn. If it warns or shows up in timing, replace the expression tree with a small evaluator over the existing token queue.
- **NReco.Logging.File**: used only by the `server` command. If it warns, replace with a small file `ILogger` in the CLI project.
- **ini-parser-netstandard**: used for `.editorconfig`. If it warns, replace with a hand-written section/key parser.
- **StrongNamer**: rewrites references at build time. If it interferes with ILC, strong-name handling for the affected dependency is revisited.
- **System.IO.Abstractions**: expected to be fine; confirm.
- **`AppDomain.CurrentDomain.BaseDirectory`** for the server log: maps to `AppContext.BaseDirectory` and is valid under AOT. No change.

### 8. Testing the native build

`CSharpierProcess.Tests` already drives the CLI as a process. Add a way to point it at an arbitrary executable (environment variable with the path), and have the native CI job publish for the runner's RID then run those tests against the output. Add process tests for any spec scenario not already covered: YAML and JSON config, `server` round trip, `pipe-files`, cache reuse, invalid arguments.

A parity check formats the repository's own test files with both builds and compares results.

### 9. Target framework of the native packages

Native packages are built from a single target framework, `net10.0`, so the .NET 10 SDK can install them. The `any` package keeps both `net10.0` and `net11.0` as today. How multi-targeting interacts with `ToolPackageRuntimeIdentifiers` is not verified; see Open Questions.

### 10. Release workflow

Replace the `alirezanet/publish-nuget` step for the CLI with:

1. A job that compares the version in `Nuget/Build.props` with nuget.org and outputs whether to publish.
2. A matrix job that packs native packages: `windows-latest` (win-x64, win-arm64), `ubuntu-latest` (linux-x64, linux-arm64, linux-musl-x64; the musl build runs in an Alpine container, arm64 uses the cross toolchain or `ubuntu-24.04-arm`), `macos-latest` (osx-arm64, osx-x64). Each uploads its `.nupkg` files as artifacts.
3. A job that packs the pointer package and the `any` package on ubuntu.
4. A final job that depends on all of the above, downloads every artifact, pushes them with `dotnet nuget push` using the existing trusted-publishing login, and creates the git tag.

`CSharpier.Core` and `CSharpier.MsBuild` keep their current publish steps. Runtime-specific packages are pushed before the pointer package so the pointer never refers to a package that is not yet available.

### 11. Results from implementation

**System.Text.Json source generation and init-only properties.** With `{ get; init; }` properties on `ConfigurationFileOptions`, the native build left `Overrides` null when a JSON `.csharpierrc` had no `overrides` key, and the CLI failed with a null reference. The managed build with the same generated context kept the `= []` initializer. The properties are now `{ get; set; }`. This was found by the process tests run against the native executable, not by any warning.

**Benchmark (task 7).** Ryzen 9 7900X, Windows 11, median of 5 runs (3 for the two repositories), after all fixes. The machine was busier than during the baseline run, so compare within this table only.

| Run | Managed | Native |
|---|---|---|
| `--version` | 76 ms | 19 ms |
| `format` one 300-line file, no cache | 449 ms | 46 ms |
| `check Src` (421 files) | 627 ms | 252 ms |
| `format --skip-write --no-cache` Newtonsoft.Json (931 files) | 4,887 ms | 3,627 ms |
| `format --skip-write --no-cache` aspnetcore (9,531 files) | 13,290 ms | 8,630 ms |

Native is faster on every input, so task 7.2 does not stop the work.

**Parity.** `Scripts/CompareManagedAndNative.ps1` formats 662 files (the formatting test inputs plus this repository's `Src`) with both builds; output and console text are identical. The full test suite passes against both the managed build and the native executable (829 passed, 5 skipped). Help, version, unknown option, and missing argument output is identical to the managed baseline.

**Pack commands (resolves open questions 1 to 3).** Verified with SDK 11.0.100:

- Native package: `dotnet pack Src/CSharpier.Cli -c Release -r <rid> -p:TargetFrameworks=net10.0`. Produces `CSharpier.<rid>.<version>.nupkg` with `tools/any/<rid>/CSharpier.exe`, 9.3 MB for win-x64.
- `any` package: `dotnet pack Src/CSharpier.Cli -c Release -r any -p:PublishAot=false`. Contains `tools/net10.0/any` and `tools/net11.0/any`, 13 MB.
- Pointer package: `dotnet pack Src/CSharpier.Cli -c Release -p:CreateRidSpecificToolPackages=false`. Contains only `tools/any/any/DotnetToolSettings.xml` listing all eight runtime identifiers, so it is not tied to a target framework.
- A plain `dotnet pack` does not work with SDK 11: besides the pointer package it starts inner packs for the runtime identifiers the host can build (win-x64 and win-arm64 on Windows) and they fail with NETSDK1047 because restore did not include those runtime identifiers. This differs from the .NET 10 documentation, which says a plain pack only produces the pointer package.
- Passing the runtime identifier list on the command line does not work: an escaped `;` produces a single runtime identifier named `linux-x64;any`.
- `CopyOutputSymbolsToPublishDirectory=false` for runtime-specific packs keeps the 73 MB native pdb out of the package.
- Packing reads everything in the publish directory, so a stale publish directory changes package contents. CI uses clean checkouts.
- `CSharpier.MsBuild` copied everything under `CSharpier.Cli/bin/<Configuration>`, which picked up the runtime-specific folders and a native executable after a native pack. It now excludes `any`, `win-*`, `linux-*`, and `osx-*` folders.

**Install results (Windows, local feed).** With SDK 10.0.204 and SDK 11.0.100: the win-x64 native package installs and runs as a local tool (`dotnet csharpier`) and with `--tool-path`; a pointer package that lists only `linux-x64;any` installs the `any` package and runs. The native executable runs with `DOTNET_ROOT` pointing at a missing directory.

**Windows `--tool-path` shim is a `.cmd` file (blocks Windows native packages for IDE plugins).** For a framework-dependent tool the SDK writes `tools/csharpier.exe`. For the native package it writes `tools/csharpier.cmd` containing a call to the executable in `.store`. Starting `<tool-path>\csharpier` directly, without a shell, fails with "The system cannot find the file specified". The VSCode (`spawn`), Rider (`ProcessBuilder`), and Visual Studio (`ProcessStartInfo` with `UseShellExecute = false`) plugins all start it that way, so every released plugin version would stop working on Windows after a user moves to a CSharpier version with Windows native packages. The spec scenario "Install with tool-path" expects `csharpier.exe` on Windows and cannot be met with the current SDK behavior. Not checked on Linux or macOS, where the shim is expected to be a file named `csharpier`.

Decided: see decision 12.

### 12. Windows native packages are released after a plugin update

Decided by the maintainer after the shim finding above. The work is released in three steps:

1. A CSharpier release with native packages for Linux and macOS plus `any`. `win-x64` and `win-arm64` are left out of `ToolPackageRuntimeIdentifiers`, so Windows installs the `any` package, gets `csharpier.exe`, and existing plugin versions keep working.
2. Plugin releases (VSCode, Rider, Visual Studio) that locate the executable on Windows: use `<tool-path>\csharpier.exe` when it exists, otherwise the native executable under `.store\csharpier\<version>\csharpier.<rid>\<version>\tools\any\<rid>\CSharpier.exe`.
3. A later CSharpier release that adds `win-x64` and `win-arm64`.

Details of the plugin change:

- The lookup checks which file exists and does not compare versions. One CSharpier version installs `csharpier.exe` on a Windows platform that uses the `any` package and `csharpier.cmd` on one with a native package.
- The plugins start the native executable, not `csharpier.cmd`. Running the `.cmd` puts `cmd.exe` between the plugin and the `server` or `pipe-files` process, so stopping the process can leave the CSharpier server running, and Node's `spawn` rejects `.cmd` files without `shell: true`. Neither effect was tested here.
- Plugin versions released before step 2 fail on Windows once a user installs a CSharpier version from step 3. Releasing step 2 at least one CSharpier release ahead reduces how many users hit that; the docs and changelog for step 3 state the minimum plugin versions.

Alternatives considered: shipping Windows native packages immediately with a plugin update (breaks installed plugins on Windows on day one), and reporting the shim to dotnet/sdk and waiting for a fix (no date, and already released SDKs would keep the `.cmd` shim). Reporting it is still worthwhile but nothing here depends on it.

## Risks / Trade-offs

- [Unknown ILC warnings in Roslyn or other dependencies] → Run the full publish first (task group 1) and revise this design if a dependency cannot be made clean.
- [Native build formats differently in a rare path, for example culture or encoding handling] → Parity check over the test files in CI; `System.Text.Encoding.CodePages` and invariant-globalization settings are reviewed explicitly.
- [System.CommandLine 2.0 changes parsing of an edge case] → Captured before/after output and existing CLI tests; any difference in options or exit codes blocks the upgrade.
- [Cache key changes with source-generated JSON] → Pinned by a test; worst case is one cold run after upgrade.
- [Release needs macOS and Windows runners; longer and more expensive releases] → Native matrix runs only when the version changed; pull requests build one RID on ubuntu.
- [A user on SDK 10 installs to a platform where the native binary fails, for example an old glibc] → Document how to force the managed fallback and state the minimum glibc of the build image in the docs.
- [Throughput: NativeAOT has no tiered JIT, so formatting a very large repository may be slower than the managed build] → Benchmark both single-file and whole-repository runs before release; results decide whether to proceed and are recorded in the change.
- [Nine packages per release on nuget.org; a partial push leaves a broken version] → All-or-nothing push job, runtime packages first, pointer last.

## Migration Plan

1. Release the AOT-clean managed tool (stage 1) as a normal version. Rollback is a normal revert.
2. Enable the native CI job (stage 2). No user-visible change.
3. Publish a prerelease version with native packages and verify `dotnet tool install` on Windows, Linux glibc, Alpine, and macOS, global, local, and `--tool-path`, plus each IDE plugin.
4. Publish the stable version.

Rollback after stage 3: remove `ToolPackageRuntimeIdentifiers` and `PublishAot` from the csproj and restore the old publish step; the next version is a plain managed tool again. Published versions can be unlisted on nuget.org.

## Open Questions

- Does `ToolPackageRuntimeIdentifiers` work with `TargetFrameworks=net10.0;net11.0`, and which target framework folder do the pointer and native packages use? If it does not, the CLI packs native packages from `net10.0` only through a pack-time property.
- Exact command to pack the `any` package when `PublishAot=true` is set in the project.
- Can the .NET 10 SDK install a pointer package produced by the .NET 11 SDK that `global.json` pins?
- Is there a supported way for a user to force the `any` package on a listed platform?
- Does `dotnet tool install` for a local tool manifest (`dotnet tool restore`) behave the same as global install for RID-specific packages on all three operating systems?
- Is linux-musl-arm64 wanted in the first release?
