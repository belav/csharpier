using AwesomeAssertions;
using CSharpier.Cli.Server;
using CSharpier.Core.PowerShell;
using Microsoft.Extensions.Logging.Abstractions;

namespace CSharpier.Tests.Cli;

internal sealed class CSharpierServiceImplementationTests
{
    private readonly string testFileDirectory = Directory
        .CreateTempSubdirectory("CsharpierTestFies")
        .FullName;

    [After(Test)]
    public void AfterEachTest()
    {
        void DeleteDirectory()
        {
            if (Directory.Exists(this.testFileDirectory))
            {
                Directory.Delete(this.testFileDirectory, true);
            }
        }

        try
        {
            DeleteDirectory();
        }
        catch (Exception)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            DeleteDirectory();
        }
    }

    [Test]
    public async Task Should_Report_Failure_When_Formatting_Produces_A_FailureMessage()
    {
        var service = new CSharpierServiceImplementation(NullLogger.Instance);

        var result = await service.FormatFile(
            new FormatFileParameter
            {
                fileName = Path.Combine(this.testFileDirectory, "DeepRecursion.cs"),
                fileContents = DeeplyConcatenatedString,
            },
            CancellationToken.None
        );

        result.status.Should().Be(Status.Failed);
        result.errorMessage.Should().Contain("deep of recursion");
    }

    [Test]
    public async Task Should_Not_Return_Empty_Content_When_Formatting_Fails()
    {
        var service = new CSharpierServiceImplementation(NullLogger.Instance);

        var result = await service.FormatFile(
            new FormatFileParameter
            {
                fileName = Path.Combine(this.testFileDirectory, "DeepRecursion.cs"),
                fileContents = DeeplyConcatenatedString,
            },
            CancellationToken.None
        );

        result.formattedFile.Should().BeNullOrEmpty();
        result.status.Should().NotBe(Status.Formatted);
    }

    [Test]
    public async Task Should_Resolve_Options_For_A_Relative_Request_Path()
    {
        await File.WriteAllTextAsync(
            Path.Combine(this.testFileDirectory, ".csharpierrc.json"),
            """
            {
                "overrides": [
                    {
                        "files": "*.cs",
                        "formatter": "csharp",
                        "indentSize": 1
                    }
                ]
            }
            """
        );

        var service = new CSharpierServiceImplementation(NullLogger.Instance);

        var result = await service.FormatFile(
            new FormatFileParameter
            {
                fileName = Path.GetRelativePath(
                    Directory.GetCurrentDirectory(),
                    Path.Combine(this.testFileDirectory, "Class.cs")
                ),
                fileContents =
                    "public class ClassName\n{\npublic string Property { get; set; }\n}\n",
            },
            CancellationToken.None
        );

        result.status.Should().Be(Status.Formatted);
        result.formattedFile.Should().Contain("\n public string Property");
    }

    [Test]
    public async Task Should_Pick_Up_A_Config_Edited_Between_Requests()
    {
        var configPath = Path.Combine(this.testFileDirectory, ".csharpierrc.json");

        async Task WriteConfigAsync(int indentSize)
        {
            await File.WriteAllTextAsync(
                configPath,
                $$"""
                {
                    "overrides": [
                        {
                            "files": "*.cs",
                            "formatter": "csharp",
                            "indentSize": {{indentSize}}
                        }
                    ]
                }
                """
            );
        }

        var service = new CSharpierServiceImplementation(NullLogger.Instance);

        Task<FormatFileResult> FormatAsync()
        {
            return service.FormatFile(
                new FormatFileParameter
                {
                    fileName = Path.Combine(this.testFileDirectory, "Class.cs"),
                    fileContents =
                        "public class ClassName\n{\npublic string Property { get; set; }\n}\n",
                },
                CancellationToken.None
            );
        }

        await WriteConfigAsync(1);
        var firstResult = await FormatAsync();

        await WriteConfigAsync(10);
        var secondResult = await FormatAsync();

        firstResult.formattedFile.Should().Contain("\n public string Property");
        secondResult.formattedFile.Should().Contain("\n          public string Property");
    }

    [Test]
    public async Task Should_Report_Warning_When_PowerShell_Is_Returned_Unchanged_With_A_Warning()
    {
        PowerShellFormatter.SimulateDscUnavailable.Value = true;
        var service = new CSharpierServiceImplementation(NullLogger.Instance);

        var result = await service.FormatFile(
            new FormatFileParameter
            {
                fileName = Path.Combine(this.testFileDirectory, "Configuration.ps1"),
                fileContents = "configuration Sample {\n}\n",
            },
            CancellationToken.None
        );

        result.status.Should().Be(Status.Failed);
        result.errorMessage.Should().Contain("DSC is not installed");
        result.formattedFile.Should().BeNull();
    }

    [Test]
    public async Task Should_Report_Failure_When_PowerShell_Is_Returned_Unchanged_With_A_FailureMessage()
    {
        var service = new CSharpierServiceImplementation(NullLogger.Instance);

        var result = await service.FormatFile(
            new FormatFileParameter
            {
                fileName = Path.Combine(this.testFileDirectory, "Deep.ps1"),
                fileContents = "$a = " + new string('(', 500) + "1" + new string(')', 500) + "\n",
            },
            CancellationToken.None
        );

        result.status.Should().Be(Status.Failed);
        result.errorMessage.Should().Contain("deep of recursion");
        result.formattedFile.Should().BeNull();
    }

    [Test]
    public async Task Should_Report_Failure_When_PowerShell_Fails_Validation()
    {
        PowerShellFormatter.SimulatedPrinterOutput.Value = "if ($a) {\n    Write-Output 1\n}\n";
        var service = new CSharpierServiceImplementation(NullLogger.Instance);

        var result = await service.FormatFile(
            new FormatFileParameter
            {
                fileName = Path.Combine(this.testFileDirectory, "Script.ps1"),
                fileContents = "if($a){Write-Host 1}",
            },
            CancellationToken.None
        );

        result.status.Should().Be(Status.Failed);
        result.errorMessage.Should().StartWith("Failed formatting validation.");
        result.errorMessage.Should().Contain("Write-Host");
        result.formattedFile.Should().BeNull();
    }

    [Test]
    public async Task Should_Format_PowerShell_That_Passes_Validation()
    {
        var service = new CSharpierServiceImplementation(NullLogger.Instance);

        var result = await service.FormatFile(
            new FormatFileParameter
            {
                fileName = Path.Combine(this.testFileDirectory, "Script.ps1"),
                fileContents = "if($a){Write-Host 1}",
            },
            CancellationToken.None
        );

        result.status.Should().Be(Status.Formatted);
        result.formattedFile.Should().Be("if ($a) {\n    Write-Host 1\n}\n");
    }

    private static readonly string DeeplyConcatenatedString =
        "public class ClassName\n{\n    private string field = "
        + string.Join(" + ", Enumerable.Repeat("\"1\"", 200))
        + ";\n}\n";
}
