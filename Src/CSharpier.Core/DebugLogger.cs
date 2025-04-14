using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

namespace CSharpier.Core;

internal class DebugLogger
{
    private static readonly object lockObject = new();

    [Conditional("DEBUG")]
    public static void Log(object message)
    {
        Log(JsonSerializer.Serialize(message));
    }

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
    public static void LogIf(bool condition, object message)
    {
        if (!condition)
        {
            return;
        }

        Log(message);
    }
}
