namespace CSharpier.Utilities;

internal static class Symbol
{
    private static int Count;

    // TODO make this increment per value
    public static string For(string value)
    {
        return value + " #" + Interlocked.Increment(ref Count);
    }
}
