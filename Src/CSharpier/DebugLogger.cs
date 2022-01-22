using System.Diagnostics;

namespace CSharpier;

public class DebugLogger
{
    [Conditional("DEBUG")]
    public static void Log(string message)
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
