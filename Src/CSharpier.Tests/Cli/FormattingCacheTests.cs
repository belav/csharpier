using System.IO.Abstractions.TestingHelpers;
using System.Text;
using AwesomeAssertions;
using CSharpier.Cli;
using CSharpier.Cli.Options;
using CSharpier.Core;
using Microsoft.Extensions.Logging.Abstractions;

namespace CSharpier.Tests.Cli;

internal sealed class FormattingCacheTests
{
    [Test]
    public async Task Should_Not_Skip_File_That_Differs_Only_By_Non_Ascii_Character()
    {
        var cache = await CreateCacheAsync();
        var path = OperatingSystem.IsWindows() ? "c:/test/Class.cs" : "/test/Class.cs";

        var printerOptions = new PrinterOptions(Formatter.CSharp, XmlWhitespaceSensitivity.Ignore);
        cache.CacheResult(
            "public class Café { }\n",
            FileAt(path, "public class Café { }\n"),
            printerOptions
        );

        var otherFile = FileAt(path, "public class Cafè { }\n");

        cache.CanSkipFormatting(otherFile, printerOptions).Should().BeFalse();
    }

    [Test]
    public async Task Should_Skip_File_With_Unchanged_Non_Ascii_Content()
    {
        var cache = await CreateCacheAsync();
        var path = OperatingSystem.IsWindows() ? "c:/test/Class.cs" : "/test/Class.cs";
        var contents = "public class Café { }\n";

        var printerOptions = new PrinterOptions(Formatter.CSharp, XmlWhitespaceSensitivity.Ignore);
        cache.CacheResult(contents, FileAt(path, contents), printerOptions);

        cache.CanSkipFormatting(FileAt(path, contents), printerOptions).Should().BeTrue();
    }

    private static FileToFormatInfo FileAt(string path, string contents)
    {
        return FileToFormatInfo.Create(path, contents, Encoding.UTF8);
    }

    private static async Task<IFormattingCache> CreateCacheAsync()
    {
        var directory = OperatingSystem.IsWindows() ? "c:/test" : "/test";
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(directory);

        return await FormattingCacheFactory.InitializeAsync(
            new CommandLineOptions(),
            fileSystem,
            CancellationToken.None
        );
    }
}
