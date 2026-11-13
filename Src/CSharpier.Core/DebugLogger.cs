using System.Diagnostics;
using System.Globalization;

namespace CSharpier.Core;

internal class DebugLogger
{
#pragma warning disable IDE0330
    private static readonly object lockObject = new();
#pragma warning restore IDE0330

    [Conditional("DEBUG")]
    public static void Log(string message)
    {
        lock (lockObject)
        {
            try
            {
                File.AppendAllText(
                    @"C:\projects\csharpier\debug.txt",
                    $"{DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture)} {message}\n"
                );
            }
            catch (Exception)
            {
                // we don't care if this fails
            }
        }
    }

    [Conditional("DEBUG")]
    public static void LogIf(bool condition, string message)
    {
        if (!condition)
        {
            return;
        }

        Log(message);
    }
}
