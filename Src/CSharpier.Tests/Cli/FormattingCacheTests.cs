using System.IO.Abstractions.TestingHelpers;
using System.IO.Hashing;
using System.Text;
using System.Text.Json;
using AwesomeAssertions;
using CSharpier.Cli;
using CSharpier.Cli.Options;
using Microsoft.Extensions.Logging.Abstractions;

namespace CSharpier.Tests.Cli;

internal sealed class FormattingCacheTests
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
    public async Task Should_Not_Skip_File_That_Differs_Only_By_Non_Ascii_Character()
    {
        var cache = await this.CreateCacheAsync();
        var path = Path.Combine(this.testFileDirectory, "Class.cs");
        var file = FileAt("public class Café { }\n", path);

        cache.CacheResult(file.FileContents, file);

        var otherFile = FileAt("public class Cafè { }\n", path);
        cache.CanSkipFormatting(otherFile).Should().BeFalse();
    }

    [Test]
    public async Task Should_Skip_File_With_Unchanged_Non_Ascii_Content()
    {
        var cache = await this.CreateCacheAsync();
        var path = Path.Combine(this.testFileDirectory, "Class.cs");
        var file = FileAt("public class Café { }\n", path);

        cache.CacheResult(file.FileContents, file);

        cache.CanSkipFormatting(file).Should().BeTrue();
    }

    [Test]
    public async Task Should_Replace_Cache_Entry_Written_Under_The_Previous_Hash_Scheme()
    {
        var fileSystem = this.CreateFileSystem();
        var path = Path.Combine(this.testFileDirectory, "Class.cs");
        var file = FileAt("public class Class { }\n", path);

        // 1.3.0 and earlier hashed Encoding.ASCII.GetBytes(contents) rather than the utf-16
        // this sets up a cache file that would exist from an earlier version
        async Task SetupOldCacheFileAsync()
        {
            await CacheFileAsync(await this.CreateCacheAsync(fileSystem), file);

            var cacheEntry = ReadCacheEntry(fileSystem, file.Path);
            var optionsHash = cacheEntry[8..];

            var previousContentHash = Convert.ToHexString(
                XxHash32.Hash(Encoding.ASCII.GetBytes(file.FileContents))
            );
            WriteCacheEntry(fileSystem, file.Path, previousContentHash + optionsHash);
        }

        await SetupOldCacheFileAsync();

        var cacheWith130Hash = await this.CreateCacheAsync(fileSystem);
        cacheWith130Hash.CanSkipFormatting(file).Should().BeFalse();
        await CacheFileAsync(cacheWith130Hash, file);
        cacheWith130Hash.CanSkipFormatting(file).Should().BeTrue();

        var refreshedCache = await this.CreateCacheAsync(fileSystem);
        refreshedCache.CanSkipFormatting(file).Should().BeTrue();
    }

    private static FileToFormatInfo FileAt(string contents, string path)
    {
        return FileToFormatInfo.Create(path, contents, Encoding.UTF8);
    }

    private static async Task CacheFileAsync(IFormattingCache cache, FileToFormatInfo file)
    {
        cache.CacheResult(file.FileContents, file);
        await cache.ResolveAsync(CancellationToken.None);
    }

    private static string ReadCacheEntry(MockFileSystem fileSystem, string path)
    {
        var cache = JsonSerializer.Deserialize<Dictionary<string, string>>(
            fileSystem.File.ReadAllText(FormattingCacheFactory.CacheFilePath)
        );

        return cache![path];
    }

    private static void WriteCacheEntry(MockFileSystem fileSystem, string path, string entry)
    {
        fileSystem.File.WriteAllText(
            FormattingCacheFactory.CacheFilePath,
            JsonSerializer.Serialize(new Dictionary<string, string> { [path] = entry })
        );
    }

    private MockFileSystem CreateFileSystem()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(this.testFileDirectory);
        return fileSystem;
    }

    private async Task<IFormattingCache> CreateCacheAsync(MockFileSystem? fileSystem = null)
    {
        fileSystem ??= this.CreateFileSystem();

        var optionsProvider = await OptionsProvider.Create(
            this.testFileDirectory,
            null,
            null,
            fileSystem,
            NullLogger.Instance,
            CancellationToken.None
        );

        return await FormattingCacheFactory.InitializeAsync(
            new CommandLineOptions(),
            optionsProvider,
            fileSystem,
            CancellationToken.None
        );
    }
}
