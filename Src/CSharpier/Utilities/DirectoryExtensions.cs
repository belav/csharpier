using System.IO.Abstractions;

namespace CSharpier.Utilities;

internal static class DirectoryExtensions
{
    public static void EnsureDirectoryExists(this IFileInfo fileInfo)
    {
        fileInfo.Directory?.EnsureExists();
    }

    public static void EnsureExists(this IDirectoryInfo directoryInfo)
    {
        directoryInfo.Parent?.EnsureExists();

        if (directoryInfo.Exists)
        {
            return;
        }

        try
        {
            directoryInfo.Create();
        }
        catch (Exception ex)
        {
            throw new Exception(
                "It was not possible to create the path " + directoryInfo.FullName,
                ex
            );
        }
    }
}
