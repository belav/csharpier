using AwesomeAssertions;
using CSharpier.Cli.Server;
using Microsoft.Extensions.Logging.Abstractions;

namespace CSharpier.Tests.Cli;

internal sealed class CSharpierServiceImplementationTests
{
    [Test]
    public async Task Should_Report_Failure_When_Formatting_Produces_A_FailureMessage()
    {
        var service = new CSharpierServiceImplementation(NullLogger.Instance);

        var result = await service.FormatFile(
            new FormatFileParameter
            {
                fileName = Path.Combine(Path.GetTempPath(), "DeepRecursion.cs"),
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
                fileName = Path.Combine(Path.GetTempPath(), "DeepRecursion.cs"),
                fileContents = DeeplyConcatenatedString,
            },
            CancellationToken.None
        );

        result.formattedFile.Should().BeNullOrEmpty();
        result.status.Should().NotBe(Status.Formatted);
    }

    private static readonly string DeeplyConcatenatedString =
        "public class ClassName\n{\n    private string field = "
        + string.Join(" + ", Enumerable.Repeat("\"1\"", 200))
        + ";\n}\n";
}
