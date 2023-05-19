using System.IO.Abstractions;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

public static class HasMismatchedCliAndMsBuildVersions
{
    // TODO really this check should be bypassed when msbuild is the one running csharpier
    // but these changes are still good to have
    public static bool Check(string directory, IFileSystem fileSystem, ILogger logger)
    {
        var csProjPaths = fileSystem.Directory
            .EnumerateFiles(directory, "*.csproj", SearchOption.AllDirectories)
            .ToArray();

        var versionOfDotnetTool = typeof(CommandLineFormatter).Assembly
            .GetName()
            .Version!.ToString(3);

        string? GetPackagesVersion(string pathToCsProj)
        {
            var directory = new DirectoryInfo(Path.GetDirectoryName(pathToCsProj));
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

                        continue;
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
            if (versionOfMsBuildPackage == null)
            {
                versionOfMsBuildPackage = GetPackagesVersion(pathToCsProj);

                // TODO an unknown version should also probably just be a pass, if we aren't sure, don't fail it
                if (versionOfMsBuildPackage == null)
                {
                    logger.LogError(
                        $"The csproj at {pathToCsProj} uses an unknown version of CSharpier.MsBuild"
                            + $" which is a mismatch with version {versionOfDotnetTool}"
                    );
                    return true;
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
