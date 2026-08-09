using System.Collections.Concurrent;
using System.IO.Abstractions;
using System.IO.Hashing;
using System.Runtime.InteropServices;
using System.Text.Json;
using CSharpier.Cli.Options;
using CSharpier.Core.Utilities;

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
        OptionsProvider optionsProvider,
        IFileSystem fileSystem,
        CancellationToken cancellationToken
    )
    {
        if (commandLineOptions.NoCache)
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

    private class FormattingCache(
        OptionsProvider optionsProvider,
        string cacheFile,
        ConcurrentDictionary<string, string> cacheDictionary,
        IFileSystem fileSystem
    ) : IFormattingCache
    {
        private readonly string optionsHash = GetOptionsHash(optionsProvider);

        public bool CanSkipFormatting(FileToFormatInfo fileToFormatInfo)
        {
            if (cacheDictionary.TryGetValue(fileToFormatInfo.Path, out var cachedHash))
            {
                if (this.HashMatches(cachedHash, fileToFormatInfo.FileContents))
                {
                    return true;
                }

                cacheDictionary.TryRemove(fileToFormatInfo.Path, out _);
            }

            return false;
        }

        private bool HashMatches(string cachedHash, string fileContents)
        {
            var contentHash = Hash(fileContents);

            return cachedHash.Length == contentHash.Length + this.optionsHash.Length
                && cachedHash.AsSpan(0, contentHash.Length).SequenceEqual(contentHash.AsSpan())
                && cachedHash.AsSpan(contentHash.Length).SequenceEqual(this.optionsHash.AsSpan());
        }

        public void CacheResult(string code, FileToFormatInfo fileToFormatInfo)
        {
            cacheDictionary[fileToFormatInfo.Path] = Hash(code) + this.optionsHash;
        }

        private static string GetOptionsHash(OptionsProvider optionsProvider)
        {
            var csharpierVersion = typeof(FormattingCache).Assembly.GetName().Version;
            return Hash($"{csharpierVersion}_${optionsProvider.Serialize()}");
        }

        // hashes the utf-16 payload in place - transcoding to ascii first would both copy the whole
        // file and collapse every non-ascii character to '?', letting two different files collide
        private static string Hash(string input)
        {
            Span<byte> destination = stackalloc byte[sizeof(uint)];
            XxHash32.Hash(MemoryMarshal.AsBytes(input.AsSpan()), destination);
            return Convert.ToHexString(destination);
        }

        public async Task ResolveAsync(CancellationToken cancellationToken)
        {
            fileSystem.FileInfo.New(cacheFile).EnsureDirectoryExists();

            async Task WriteFile()
            {
                await using var fileStream = fileSystem.File.Open(
                    cacheFile,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.None
                );
                await using var streamWriter = new StreamWriter(fileStream);
                await streamWriter.WriteAsync(JsonSerializer.Serialize(cacheDictionary));

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
