using System.Collections.Concurrent;
using System.IO.Abstractions;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CSharpier.Cli;

internal interface IFormattingCache
{
    Task ResolveAsync(CancellationToken cancellationToken);
    bool CanSkipFormatting(FileToFormatInfo fileToFormatInfo);
}

internal static class FormattingCacheFactory
{
    public static async Task<IFormattingCache> InitializeAsync(
        string directoryOrFile,
        CommandLineOptions commandLineOptions,
        PrinterOptions printerOptions,
        IFileSystem fileSystem,
        CancellationToken cancellationToken
    )
    {
        if (!commandLineOptions.Cache)
        {
            return new AlwaysFormatCache();
        }

        // TODO what about msbuild?
        // TODO where do we really store it?
        // TODO how do we clear it out? if they run without --cache do we clear it?
        // TODO total files count doesn't include cached files
        // test after optimized, and in release build
        // initial format with caching is probably a bit slower
        var cacheFile = Path.Combine(directoryOrFile, ".cache");
        ConcurrentDictionary<string, string> cacheDictionary;
        if (!File.Exists(cacheFile))
        {
            cacheDictionary = new ConcurrentDictionary<string, string>();
        }
        else
        {
            cacheDictionary =
                JsonSerializer.Deserialize<ConcurrentDictionary<string, string>>(
                    await File.ReadAllTextAsync(cacheFile, cancellationToken)
                ) ?? new();
        }

        return new FormattingCache(printerOptions, cacheFile, cacheDictionary, fileSystem);
    }

    private class FormattingCache : IFormattingCache
    {
        private readonly string optionsHash;
        private readonly string cacheFile;
        private readonly ConcurrentDictionary<string, string> cacheDictionary;
        private readonly IFileSystem fileSystem;

        public FormattingCache(
            PrinterOptions printerOptions,
            string cacheFile,
            ConcurrentDictionary<string, string> cacheDictionary,
            IFileSystem fileSystem
        )
        {
            this.optionsHash = GetOptionsHash(printerOptions);
            this.cacheFile = cacheFile;
            this.cacheDictionary = cacheDictionary;
            this.fileSystem = fileSystem;
        }

        public async Task ResolveAsync(CancellationToken cancellationToken)
        {
            await this.fileSystem.File.WriteAllTextAsync(
                this.cacheFile,
                JsonSerializer.Serialize(this.cacheDictionary),
                cancellationToken
            );
        }

        public bool CanSkipFormatting(FileToFormatInfo fileToFormatInfo)
        {
            var currentHash = Hash(fileToFormatInfo.FileContents) + this.optionsHash;
            if (this.cacheDictionary.TryGetValue(fileToFormatInfo.Path, out var cachedHash))
            {
                if (currentHash == cachedHash)
                {
                    return true;
                }
            }
            else
            {
                this.cacheDictionary[fileToFormatInfo.Path] = currentHash;
            }

            return false;
        }

        private static string GetOptionsHash(PrinterOptions printerOptions)
        {
            var csharpierVersion = typeof(FormattingCache).Assembly.GetName().Version;
            return Hash($"{csharpierVersion}_${JsonSerializer.Serialize(printerOptions)}");
        }

        private static string Hash(string input)
        {
            using var md5 = MD5.Create();
            var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
            return Convert.ToHexString(result);
        }
    }

    private class AlwaysFormatCache : IFormattingCache
    {
        public Task ResolveAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public bool CanSkipFormatting(FileToFormatInfo fileToFormatInfo)
        {
            return false;
        }
    }
}
