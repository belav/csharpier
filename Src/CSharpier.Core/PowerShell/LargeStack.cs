namespace CSharpier.Core.PowerShell;

// The parser and the walks over its tree recurse once per level of nesting, and a long chain of
// operators nests once per operator. The parser reports an error when its own recursion runs low
// on stack, but the checks it then runs over the tree do not, so a deep tree can overflow the
// stack inside the parser. All code is handled on a thread with a stack sized for its length, so
// the result does not depend on the stack of the calling thread, and the depth checks can reject
// the code instead of the process dying from a stack overflow.
internal static class LargeStack
{
    internal static Task<T> Run<T>(
        int codeLength,
        Func<T> function,
        CancellationToken cancellationToken
    )
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<T>(cancellationToken);
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
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    completion.SetCanceled(cancellationToken);
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
