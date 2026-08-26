#pragma warning disable CA1822

using System.IO.Abstractions;
using System.Text;
using BenchmarkDotNet.Attributes;
using CSharpier.Cli;
using CSharpier.Cli.EditorConfig;
using CSharpier.Cli.Options;
using Microsoft.Extensions.Logging.Abstractions;

namespace CSharpier.Benchmarks;

[MemoryDiagnoser]
public class OptionsBenchmarks
{
    private readonly FileSystem fileSystem = new();

    // Small is roughly the csharpier repo, Typical is roughly a solution using the standard
    // VisualStudio.gitignore plus a dotnet style .editorconfig, Large is a monorepo with
    // several nested ignore/editorconfig files
    [Params("Small", "Typical", "Large")]
    public string Profile { get; set; } = "Small";

    private const int FilesToCheck = 1000;

    private string rootDirectory = null!;
    private string targetDirectory = null!;
    private string targetFile = null!;
    private string[] filePathsToCheck = null!;
    private IgnoreFile warmIgnoreFile = null!;

    [GlobalSetup]
    public async Task Setup()
    {
        this.rootDirectory = Path.Combine(
            Path.GetTempPath(),
            "csharpier-options-bench",
            this.Profile
        );

        if (Directory.Exists(this.rootDirectory))
        {
            Directory.Delete(this.rootDirectory, true);
        }

        Directory.CreateDirectory(Path.Combine(this.rootDirectory, ".git"));

        var (ignoreRules, editorConfigSections, editorConfigProperties, depth, nested) =
            this.Profile switch
            {
                "Small" => (58, 3, 40, 2, false),
                "Typical" => (290, 8, 250, 4, false),
                "Large" => (1000, 20, 800, 8, true),
                _ => throw new Exception("unknown profile"),
            };

        var directory = this.rootDirectory;
        for (var i = 0; i < depth; i++)
        {
            directory = Path.Combine(directory, "Level" + i);
        }

        Directory.CreateDirectory(directory);
        this.targetDirectory = directory;
        this.targetFile = Path.Combine(directory, "SomeFile.cs");
        await File.WriteAllTextAsync(this.targetFile, "public class SomeFile { }");

        if (nested)
        {
            await File.WriteAllTextAsync(
                Path.Combine(this.rootDirectory, ".gitignore"),
                BuildIgnoreFile(ignoreRules * 6 / 10)
            );
            await File.WriteAllTextAsync(
                Path.Combine(this.rootDirectory, "Level0", ".gitignore"),
                BuildIgnoreFile(ignoreRules * 4 / 10)
            );
            await File.WriteAllTextAsync(
                Path.Combine(this.rootDirectory, ".csharpierignore"),
                BuildIgnoreFile(50)
            );
            await File.WriteAllTextAsync(
                Path.Combine(this.rootDirectory, ".editorconfig"),
                BuildEditorConfig(editorConfigSections / 2, editorConfigProperties / 2, true)
            );
            await File.WriteAllTextAsync(
                Path.Combine(this.rootDirectory, "Level0", ".editorconfig"),
                BuildEditorConfig(editorConfigSections / 4, editorConfigProperties / 4, false)
            );
            await File.WriteAllTextAsync(
                Path.Combine(this.rootDirectory, "Level0", "Level1", ".editorconfig"),
                BuildEditorConfig(editorConfigSections / 4, editorConfigProperties / 4, false)
            );
        }
        else
        {
            await File.WriteAllTextAsync(
                Path.Combine(this.rootDirectory, ".gitignore"),
                BuildIgnoreFile(ignoreRules)
            );
            await File.WriteAllTextAsync(
                Path.Combine(this.rootDirectory, ".editorconfig"),
                BuildEditorConfig(editorConfigSections, editorConfigProperties, true)
            );
        }

        this.filePathsToCheck = BuildFilePathsToCheck(this.targetDirectory);
        this.warmIgnoreFile =
            await IgnoreFile.CreateAsync(
                this.targetDirectory,
                this.fileSystem,
                null,
                null,
                CancellationToken.None
            ) ?? throw new Exception("no ignore file");

        this.CheckFilesIgnored();
    }

    private static string[] BuildFilePathsToCheck(string directory)
    {
        var paths = new string[FilesToCheck];
        for (var i = 0; i < paths.Length; i++)
        {
            var projectDirectory = Path.Combine(directory, "Project" + (i / 20));
            paths[i] = (i % 10) switch
            {
                0 => Path.Combine(projectDirectory, "obj", "Debug", "File" + i + ".cs"),
                1 => Path.Combine(projectDirectory, "Generated", "File" + i + ".g.cs"),
                _ => Path.Combine(projectDirectory, "Source", "File" + i + ".cs"),
            };
        }

        return paths;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(this.rootDirectory))
        {
            Directory.Delete(this.rootDirectory, true);
        }
    }

    [Benchmark(OperationsPerInvoke = FilesToCheck)]
    public int CheckFilesIgnored()
    {
        var ignored = 0;
        foreach (var path in this.filePathsToCheck)
        {
            if (this.warmIgnoreFile.IsIgnored(path, false))
            {
                ignored++;
            }
        }

        return ignored;
    }

    [Benchmark]
    public async Task CreateProviderAndResolveOptions()
    {
        var provider = await this.CreateProvider();
        _ = await provider.IsFileIgnoredAsync(this.targetFile, CancellationToken.None);
        _ = await provider.GetPrinterOptionsForAsync(this.targetFile, CancellationToken.None);
    }

    [Benchmark]
    public async Task CreateProviderOnly()
    {
        _ = await this.CreateProvider();
    }

    [Benchmark]
    public async Task IgnoreFileOnly()
    {
        _ = await IgnoreFile.CreateAsync(
            this.targetDirectory,
            this.fileSystem,
            null,
            null,
            CancellationToken.None
        );
    }

    [Benchmark]
    public async Task EditorConfigOnly()
    {
        _ = await EditorConfigLocator.FindForDirectoryNameAsync(
            this.targetDirectory,
            this.fileSystem,
            CancellationToken.None
        );
    }

    private Task<OptionsProvider> CreateProvider()
    {
        return OptionsProvider.Create(
            this.targetDirectory,
            null,
            null,
            this.fileSystem,
            NullLogger.Instance,
            CancellationToken.None
        );
    }

    // taken from the standard VisualStudio.gitignore, repeated with a distinct suffix
    // to reach the requested rule count
    private static readonly string[] IgnorePatterns =
    [
        "[Bb]in{0}/",
        "[Oo]bj{0}/",
        "*.suo{0}",
        "*.user{0}",
        "**/Properties{0}/launchSettings.json",
        "artifacts{0}/",
        "*_i{0}.c",
        "_ReSharper{0}*/",
        "!.axoCover{0}/axoCover.json",
        "coverage{0}*.json",
        "**/[Pp]ackages{0}/*",
        "!**/[Pp]ackages{0}/build/",
        "*.[Rr]e[Ss]harper{0}",
        "$tf{0}/",
        "~$*{0}",
        "node_modules{0}/",
        "*.nupkg{0}",
        "[Dd]ebug{0}/",
        "*.log{0}",
        "src/**/generated{0}/**/*.cs",
    ];

    private static string BuildIgnoreFile(int ruleCount)
    {
        var builder = new StringBuilder();
        builder.AppendLine("# generated for benchmarking");
        for (var i = 0; i < ruleCount; i++)
        {
            var pattern = IgnorePatterns[i % IgnorePatterns.Length];
            var suffix = i < IgnorePatterns.Length ? string.Empty : i.ToString();
            builder.AppendLine(string.Format(pattern, suffix));
            if (i % 10 == 0)
            {
                builder.AppendLine();
                builder.AppendLine("# a comment");
            }
        }

        return builder.ToString();
    }

    private static readonly string[] SectionHeaders =
    [
        "*",
        "*.cs",
        "*.{cs,vb}",
        "*.csproj",
        "*.{json,json5}",
        "*.{xml,config,props,targets}",
        "src/**/*.cs",
        "test/**/*.cs",
        "*.{sh,ps1}",
        "*.md",
    ];

    private static string BuildEditorConfig(int sectionCount, int propertyCount, bool isRoot)
    {
        var builder = new StringBuilder();
        if (isRoot)
        {
            builder.AppendLine("root = true");
            builder.AppendLine();
        }

        var propertiesPerSection = Math.Max(1, propertyCount / Math.Max(1, sectionCount));

        for (var i = 0; i < sectionCount; i++)
        {
            var header = SectionHeaders[i % SectionHeaders.Length];
            if (i >= SectionHeaders.Length)
            {
                header = "sub" + i + "/**/" + header;
            }

            builder.AppendLine("[" + header + "]");
            builder.AppendLine("indent_style = space");
            builder.AppendLine("indent_size = 4");
            builder.AppendLine("max_line_length = 100");
            builder.AppendLine("end_of_line = crlf");
            builder.AppendLine("insert_final_newline = true");
            for (var j = 0; j < propertiesPerSection; j++)
            {
                builder.AppendLine(
                    "dotnet_diagnostic.CA" + (1000 + (i * 100) + j) + ".severity = warning"
                );
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }
}
