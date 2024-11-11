using System.IO.Abstractions;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal static class HasMismatchedCliAndMsBuildVersions
{
    public static bool Check(string directoryName, IFileSystem fileSystem, ILogger logger)
    {
        var csProjPaths = fileSystem
            .Directory.EnumerateFiles(directoryName, "*.csproj", SearchOption.AllDirectories)
            .ToArray();

        var versionOfDotnetTool = typeof(CommandLineFormatter)
            .Assembly.GetName()
            .Version!.ToString(3);

        string? GetPackagesVersion(string pathToCsProj)
        {
            var directory = new DirectoryInfo(Path.GetDirectoryName(pathToCsProj)!);
            while (directory != null)
            {
                var filePath = Path.Combine(directory.FullName, "Directory.Packages.props");
                if (fileSystem.File.Exists(filePath))
                {
                    XElement packagesXElement;
                    try
                    {
                        packagesXElement = XElement.Load(fileSystem.File.OpenRead(filePath));
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(
                            $"The file at {filePath} failed to load with the following exception {ex.Message}"
                        );

                        return null;
                    }

                    var csharpierMsBuildElement = packagesXElement
                        .XPathSelectElements("//PackageVersion[@Include='CSharpier.MsBuild']")
                        .FirstOrDefault();
                    return csharpierMsBuildElement?.Attribute("Version")?.Value;
                }

                directory = directory.Parent;
            }

            return null;
        }

        foreach (var pathToCsProj in csProjPaths)
        {
            // this could potentially use the Microsoft.Build.Evaluation class, but that was
            // throwing exceptions trying to load project files with "The SDK 'Microsoft.NET.Sdk' specified could not be found."
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
            if (versionOfMsBuildPackage == "0.0.1")
            {
                // csharpier uses 0.0.1 as a placeholder in some tests, just ignore it
                continue;
            }

            if (versionOfMsBuildPackage == null)
            {
                versionOfMsBuildPackage = GetPackagesVersion(pathToCsProj);

                if (versionOfMsBuildPackage == null)
                {
                    logger.LogWarning(
                        $"The csproj at {pathToCsProj} uses an unknown version of CSharpier.MsBuild"
                    );
                    continue;
                }
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
