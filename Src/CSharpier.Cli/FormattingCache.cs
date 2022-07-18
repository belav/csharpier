using System.Collections.Concurrent;
using System.IO.Abstractions;
using Newtonsoft.Json;

namespace CSharpier.Cli;

internal interface IFormattingCache
{
    Task ResolveAsync(CancellationToken cancellationToken);
    Task<bool> CanSkipFormattingAsync(string actualFilePath, CancellationToken cancellationToken);
}

internal static class FormattingCacheFactory
{
    public static async Task<IFormattingCache> InitializeAsync(
        string directoryOrFile,
        CommandLineOptions commandLineOptions,
        IFileSystem fileSystem,
        CancellationToken cancellationToken
    )
    {
        // https://github.com/prettier/prettier/pull/12800/files

        // TODO some option needs to be on to actually cache things
        // return new AlwaysFormatCache();

        // TODO what about msbuild?

        // TODO where do we really store it?
        var cacheFile = Path.Combine(directoryOrFile, ".cache");
        ConcurrentDictionary<string, string> cacheDictionary;
        if (!File.Exists(cacheFile))
        {
            cacheDictionary = new ConcurrentDictionary<string, string>();
        }
        else
        {
            cacheDictionary = JsonConvert.DeserializeObject<ConcurrentDictionary<string, string>>(
                await File.ReadAllTextAsync(cacheFile, cancellationToken)
            );
        }

        return new FormattingCache(cacheFile, cacheDictionary, fileSystem);
    }

    private class FormattingCache : IFormattingCache
    {
        private readonly string cacheFile;
        private readonly ConcurrentDictionary<string, string> cacheDictionary;
        private readonly IFileSystem fileSystem;

        public FormattingCache(
            string cacheFile,
            ConcurrentDictionary<string, string> cacheDictionary,
            IFileSystem fileSystem
        )
        {
            this.cacheFile = cacheFile;
            this.cacheDictionary = cacheDictionary;
            this.fileSystem = fileSystem;
        }

        public async Task ResolveAsync(CancellationToken cancellationToken)
        {
            await this.fileSystem.File.WriteAllTextAsync(
                cacheFile,
                JsonConvert.SerializeObject(cacheDictionary),
                cancellationToken
            );
        }

        public async Task<bool> CanSkipFormattingAsync(
            string actualFilePath,
            CancellationToken cancellationToken
        )
        {
            // TODO "hash" should include version of csharpier + csharpier options
            // TODO maybe the options should be outside of this dictionary somehow
            // TODO we also may want date or a hash of the file contents
            var hash = this.fileSystem.File.GetLastWriteTimeUtc(actualFilePath).ToString();
            if (cacheDictionary.TryGetValue(actualFilePath, out var cachedHash))
            {
                if (hash == cachedHash)
                {
                    return true;
                }
            }
            else
            {
                this.cacheDictionary[actualFilePath] = hash;
            }

            return false;
        }
    }

    private class AlwaysFormatCache : IFormattingCache
    {
        public Task ResolveAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<bool> CanSkipFormattingAsync(
            string actualFilePath,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(false);
        }
    }
}
