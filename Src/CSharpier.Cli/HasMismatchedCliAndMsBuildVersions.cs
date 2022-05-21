using System.IO.Abstractions;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

public static class HasMismatchedCliAndMsBuildVersions
{
    public static bool Check(string directory, IFileSystem fileSystem, ILogger logger)
    {
        var csProjPaths = fileSystem.Directory
            .EnumerateFiles(directory, "*.csproj", SearchOption.AllDirectories)
            .ToArray();

        var versionOfDotnetTool = typeof(CommandLineFormatter).Assembly
            .GetName()
            .Version!.ToString(3);

        foreach (var pathToCsProj in csProjPaths)
        {
            // this could potentially use the Microsoft.CodeAnalysis.Project class, but that was
            // proving difficult to use
            XElement csProjXElement;
            try
            {
                csProjXElement = XElement.Load(fileSystem.File.OpenRead(pathToCsProj));
            }
            catch (Exception ex)
            {
                logger.LogWarning(
                    $"The csproj at {pathToCsProj} failed to load with the following exception {ex.Message}"
                );

                continue;
            }

            var csharpierMsBuildElement = csProjXElement
                .XPathSelectElements("//PackageReference[@Include='CSharpier.MsBuild']")
                .FirstOrDefault();
            if (csharpierMsBuildElement == null)
            {
                continue;
            }

            var versionOfMsBuildPackage = csharpierMsBuildElement.Attribute("Version")?.Value;
            if (versionOfMsBuildPackage == null)
            {
                logger.LogError(
                    $"The csproj at {pathToCsProj} uses an unknown version of CSharpier.MsBuild"
                        + $" which is a mismatch with version {versionOfDotnetTool}"
                );
                return true;
            }

            if (versionOfDotnetTool != versionOfMsBuildPackage)
            {
                logger.LogError(
                    $"The csproj at {pathToCsProj} uses version {versionOfMsBuildPackage} of CSharpier.MsBuild"
                        + $" which is a mismatch with version {versionOfDotnetTool}"
                );
                return true;
            }
        }

        return false;
    }
}
