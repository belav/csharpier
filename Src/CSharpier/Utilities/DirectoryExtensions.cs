namespace CSharpier.Utilities;

internal static class DirectoryExtensions
{
    public static void EnsureDirectoryExists(this FileInfo fileInfo)
    {
        fileInfo.Directory?.EnsureExists();
    }

    public static void EnsureExists(this DirectoryInfo directoryInfo)
    {
        if (directoryInfo.Name.EndsWith("$"))
        {
            return;
        }

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
