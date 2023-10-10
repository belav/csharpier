using System.Collections.Concurrent;
using System.IO.Abstractions;
using System.IO.Hashing;
using System.Text;
using System.Text.Json;
using CSharpier.Utilities;

namespace CSharpier.Cli;

using CSharpier.Cli.Options;

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
        OptionsProvider optionsProvider,
        IFileSystem fileSystem,
        CancellationToken cancellationToken
    )
    {
        if (commandLineOptions.NoCache || commandLineOptions.Check)
        {
            return NullCache;
        }

        var cacheDictionary = new ConcurrentDictionary<string, string>();
        if (File.Exists(CacheFilePath))
        {
            // in my testing we don't normally have to wait more than a couple MS, but just in case
            const int attempts = 20;
            var content = string.Empty;
            for (var x = 0; x < attempts; x++)
            {
                try
                {
                    content = await File.ReadAllTextAsync(CacheFilePath, cancellationToken);
                    break;
                }
                catch (Exception)
                {
                    if (x + 1 == attempts)
                    {
                        // if we are still failing, fall back to this
                        return NullCache;
                    }
                    await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
                }
            }

            try
            {
                var newDictionary = JsonSerializer.Deserialize<
                    ConcurrentDictionary<string, string>
                >(content);
                if (newDictionary != null)
                {
                    cacheDictionary = newDictionary;
                }
            }
            catch (Exception)
            {
                // file must be bad json
                try
                {
                    File.Delete(CacheFilePath);
                }
                catch (Exception)
                {
                    // if it fails to delete it should still get overwritten at the end of formatting
                }
            }
        }

        return new FormattingCache(optionsProvider, CacheFilePath, cacheDictionary, fileSystem);
    }

    private class FormattingCache : IFormattingCache
    {
        private readonly string optionsHash;
        private readonly string cacheFile;
        private readonly ConcurrentDictionary<string, string> cacheDictionary;
        private readonly IFileSystem fileSystem;

        public FormattingCache(
            OptionsProvider optionsProvider,
            string cacheFile,
            ConcurrentDictionary<string, string> cacheDictionary,
            IFileSystem fileSystem
        )
        {
            this.optionsHash = GetOptionsHash(optionsProvider);
            this.cacheFile = cacheFile;
            this.cacheDictionary = cacheDictionary;
            this.fileSystem = fileSystem;
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

                this.cacheDictionary.TryRemove(fileToFormatInfo.Path, out _);
            }

            return false;
        }

        public void CacheResult(string code, FileToFormatInfo fileToFormatInfo)
        {
            this.cacheDictionary[fileToFormatInfo.Path] = Hash(code) + this.optionsHash;
        }

        private static string GetOptionsHash(OptionsProvider optionsProvider)
        {
            var csharpierVersion = typeof(FormattingCache).Assembly.GetName().Version;
            return Hash($"{csharpierVersion}_${optionsProvider.Serialize()}");
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
                await using var fileStream = this.fileSystem.File.Open(
                    this.cacheFile,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.None
                );
                await using var streamWriter = new StreamWriter(fileStream);
                await streamWriter.WriteAsync(
                    JsonSerializer.Serialize(
                        this.cacheDictionary
#if DEBUG
                        ,
                        new JsonSerializerOptions { WriteIndented = true }
#endif
                    )
                );

                await fileStream.FlushAsync(cancellationToken);
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
