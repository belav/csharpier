namespace CSharpier.Tests;

public static class DirectoryFinder
{
    public static DirectoryInfo FindParent(string name)
    {
        var rootDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (rootDirectory.Name != name)
        {
            if (rootDirectory.Parent == null)
            {
                throw new Exception($"There was no parent directory found with the name '{name}'");
            }

            rootDirectory = rootDirectory.Parent;
        }



        return rootDirectory;
    }
}
