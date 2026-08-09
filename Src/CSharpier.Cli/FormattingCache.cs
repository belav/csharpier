using System.Collections.Concurrent;
using System.IO.Abstractions;
using System.IO.Hashing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using CSharpier.Core;
using CSharpier.Core.Utilities;

namespace CSharpier.Cli;

internal interface IFormattingCache
{
    Task ResolveAsync(CancellationToken cancellationToken);
    bool CanSkipFormatting(FileToFormatInfo fileToFormatInfo, PrinterOptions printerOptions);
    void CacheResult(string code, FileToFormatInfo fileToFormatInfo, PrinterOptions printerOptions);
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
        IFileSystem fileSystem,
        CancellationToken cancellationToken
    )
    {
        if (commandLineOptions.NoCache)
        {
            return NullCache;
        }

        var cacheDictionary = new ConcurrentDictionary<string, string>();
        if (fileSystem.File.Exists(CacheFilePath))
        {
            // in my testing we don't normally have to wait more than a couple MS, but just in case
            const int attempts = 20;
            var content = string.Empty;
            for (var x = 0; x < attempts; x++)
            {
                try
                {
                    content = await fileSystem.File.ReadAllTextAsync(
                        CacheFilePath,
                        cancellationToken
                    );
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
                    fileSystem.File.Delete(CacheFilePath);
                }
                catch (Exception)
                {
                    // if it fails to delete it should still get overwritten at the end of formatting
                }
            }
        }

        return new FormattingCache(CacheFilePath, cacheDictionary, fileSystem);
    }

    private class FormattingCache(
        string cacheFile,
        ConcurrentDictionary<string, string> cacheDictionary,
        IFileSystem fileSystem
    ) : IFormattingCache
    {
        public bool CanSkipFormatting(
            FileToFormatInfo fileToFormatInfo,
            PrinterOptions printerOptions
        )
        {
            if (cacheDictionary.TryGetValue(fileToFormatInfo.Path, out var cachedHash))
            {
                if (HashMatches(cachedHash, fileToFormatInfo.FileContents, printerOptions))
                {
                    return true;
                }

                cacheDictionary.TryRemove(fileToFormatInfo.Path, out _);
            }

            return false;
        }

        private static bool HashMatches(
            string cachedHash,
            string fileContents,
            PrinterOptions printerOptions
        )
        {
            var contentHash = Hash(fileContents);

            var optionsHash = GetPrinterOptionsHash(printerOptions);

            return cachedHash.Length == contentHash.Length + optionsHash.Length
                && cachedHash.AsSpan(0, contentHash.Length).SequenceEqual(contentHash.AsSpan())
                && cachedHash.AsSpan(contentHash.Length).SequenceEqual(optionsHash.AsSpan());
        }

        public void CacheResult(
            string code,
            FileToFormatInfo fileToFormatInfo,
            PrinterOptions printerOptions
        )
        {
            cacheDictionary[fileToFormatInfo.Path] = GetCacheHash(code, printerOptions);
        }

        private static string GetCacheHash(string code, PrinterOptions printerOptions)
        {
            return Hash(code) + GetPrinterOptionsHash(printerOptions);
        }

        private static string GetPrinterOptionsHash(PrinterOptions printerOptions)
        {
            var csharpierVersion = typeof(FormattingCache).Assembly.GetName().Version;
            var hash = new XxHash32();
            hash.Append(Encoding.UTF8.GetBytes(csharpierVersion?.ToString() ?? string.Empty));
            hash.Append(JsonSerializer.SerializeToUtf8Bytes(printerOptions));
            return Convert.ToHexString(hash.GetCurrentHash());
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

        public bool CanSkipFormatting(
            FileToFormatInfo fileToFormatInfo,
            PrinterOptions printerOptions
        )
        {
            return false;
        }

        public void CacheResult(
            string code,
            FileToFormatInfo fileToFormatInfo,
            PrinterOptions printerOptions
        ) { }
    }
}
