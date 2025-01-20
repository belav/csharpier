using System.Diagnostics;

namespace CSharpier;

internal class DebugLogger
{
    private static readonly object lockObject = new();

    [Conditional("DEBUG")]
    public static void Log(object message)
    {
        lock (lockObject)
        {
            try
            {
                File.AppendAllText(@"C:\projects\csharpier\debug.txt", message + "\n");
            }
            catch (Exception)
            {
                // we don't care if this fails
            }
        }
    }

    [Conditional("DEBUG")]
    public static void LogIf(bool condition, object message)
    {
        if (!condition)
        {
            return;
        }

        Log(message);
    }
}
