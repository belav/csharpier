using System.Collections.Concurrent;

namespace CSharpier.Core;

internal sealed class SharedFunc<T> : SemaphoreSlim
{
    private OperationResult result;
    private static readonly ConcurrentDictionary<string, SharedFunc<T>> RunningFuncs = new();

    private SharedFunc()
        : base(1, int.MaxValue) { }

    public static async Task<T> GetOrAddAsync(
        string key,
        Func<Task<T>> factory,
        CancellationToken cancellationToken
    )
    {
        var workspace = RunningFuncs.GetOrAdd(key, _ => new());
        await workspace.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (!workspace.result.HasResult)
            {
                workspace.result = new(await factory());
            }
        }
        finally
        {
            workspace.Release();
            RunningFuncs.TryRemove(key, out _);
        }

        return workspace.result.Result;
    }

    private readonly struct OperationResult(T result)
    {
        public readonly bool HasResult = true;
        public readonly T Result = result;
    }
}
