namespace CSharpierProcess.Tests;

// Set CSHARPIER_TEST_EXECUTABLE to the path of a published csharpier executable (for example a NativeAOT build)
// to run these tests against it instead of the CSharpier.dll that is built with the tests.
internal static class CSharpierExecutable
{
    private static readonly string? executableOverride = Environment.GetEnvironmentVariable(
        "CSHARPIER_TEST_EXECUTABLE"
    );

    public static string FileName
    {
        get
        {
            if (executableOverride is null)
            {
                return "dotnet";
            }

            if (!File.Exists(executableOverride))
            {
                throw new Exception("No file found at " + executableOverride);
            }

            return executableOverride;
        }
    }

    public static string[] Arguments
    {
        get
        {
            if (executableOverride is not null)
            {
                return [];
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "CSharpier.dll");
            if (!File.Exists(path))
            {
                throw new Exception("No file found at " + path);
            }

            return [path];
        }
    }

    public static string ArgumentsPrefix => string.Join(" ", Arguments);
}
