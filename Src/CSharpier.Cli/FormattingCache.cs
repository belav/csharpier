using System.Collections.Concurrent;
using System.IO.Abstractions;
using System.IO.Hashing;
using System.Text;
using System.Text.Json;
using CSharpier.Utilities;

namespace CSharpier.Cli;

internal interface IFormattingCache
{
    Task ResolveAsync(CancellationToken cancellationToken);
    bool CanSkipFormatting(FileToFormatInfo fileToFormatInfo);
    void CacheResult(string code, FileToFormatInfo fileToFormatInfo);
}

internal static class FormattingCacheFactory
{
    public static readonly IFormattingCache NullCache = new AlwaysFormatCache();

    public static readonly string CacheFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "CSharpier",
        ".formattingCache"
    );

    public static async Task<IFormattingCache> InitializeAsync(
        CommandLineOptions commandLineOptions,
        PrinterOptions printerOptions,
        IFileSystem fileSystem,
        CancellationToken cancellationToken
    )
    {
        if (commandLineOptions.NoCache || commandLineOptions.Check)
        {
            return NullCache;
        }

        ConcurrentDictionary<string, string> cacheDictionary;
        if (!File.Exists(CacheFilePath))
        {
            cacheDictionary = new ConcurrentDictionary<string, string>();
        }
        else
        {
            cacheDictionary =
                JsonSerializer.Deserialize<ConcurrentDictionary<string, string>>(
                    await File.ReadAllTextAsync(CacheFilePath, cancellationToken)
                ) ?? new();
        }

        return new FormattingCache(printerOptions, CacheFilePath, cacheDictionary, fileSystem);
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

        public bool CanSkipFormatting(FileToFormatInfo fileToFormatInfo)
        {
            var currentHash = Hash(fileToFormatInfo.FileContents) + this.optionsHash;
            if (this.cacheDictionary.TryGetValue(fileToFormatInfo.Path, out var cachedHash))
            {
                DebugLogger.Log(fileToFormatInfo.Path + " " + currentHash + " " + cachedHash);
                if (currentHash == cachedHash)
                {
                    return true;
                }

                this.cacheDictionary.TryRemove(fileToFormatInfo.Path, out _);
            }

            return false;
        }

        public void CacheResult(string code, FileToFormatInfo fileToFormatInfo)
        {
            this.cacheDictionary[fileToFormatInfo.Path] = Hash(code) + this.optionsHash;
        }

        private static string GetOptionsHash(PrinterOptions printerOptions)
        {
            var csharpierVersion = typeof(FormattingCache).Assembly.GetName().Version;
            return Hash($"{csharpierVersion}_${JsonSerializer.Serialize(printerOptions)}");
        }

        private static string Hash(string input)
        {
            var result = XxHash32.Hash(Encoding.ASCII.GetBytes(input));
            return Convert.ToHexString(result);
        }

        public async Task ResolveAsync(CancellationToken cancellationToken)
        {
            this.fileSystem.FileInfo.FromFileName(this.cacheFile).EnsureDirectoryExists();

            async Task WriteFile()
            {
                await this.fileSystem.File.WriteAllTextAsync(
                    this.cacheFile,
                    JsonSerializer.Serialize(
                        this.cacheDictionary
#if DEBUG
                        ,
                        new JsonSerializerOptions { WriteIndented = true }
#endif
                    ),
                    cancellationToken
                );
            }

            // in my testing we don't normally have to wait more than a couple MS, but just in case
            for (var x = 0; x < 20; x++)
            {
                try
                {
                    await WriteFile();
                    return;
                }
                catch (Exception)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
                }
            }
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

        public void CacheResult(string code, FileToFormatInfo fileToFormatInfo) { }
    }
}
