namespace CSharpier.Core.PowerShell;

// The parser and the walks over its tree recurse once per level of nesting, and a long chain of
// operators nests once per operator. Code large enough to hold such a chain is handled on a thread
// with a stack sized for it, so the depth checks can reject it instead of the process dying from a
// stack overflow.
internal static class LargeStack
{
    private const int Threshold = 20_000;

    internal static Task<T> Run<T>(int codeLength, Func<T> function)
    {
        if (codeLength < Threshold)
        {
            try
            {
                return Task.FromResult(function());
            }
            catch (Exception exception)
            {
                return Task.FromException<T>(exception);
            }
        }

        var completion = new TaskCompletionSource<T>(
            TaskCreationOptions.RunContinuationsAsynchronously
        );
        var stackSize = (int)Math.Min(1L << 30, Math.Max(16L << 20, codeLength * 2048L));
        var thread = new Thread(
            () =>
            {
                try
                {
                    completion.SetResult(function());
                }
                catch (Exception exception)
                {
                    completion.SetException(exception);
                }
            },
            stackSize
        )
        {
            IsBackground = true,
        };
        thread.Start();
        return completion.Task;
    }
}
