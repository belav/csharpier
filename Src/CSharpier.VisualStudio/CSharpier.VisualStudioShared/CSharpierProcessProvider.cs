using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;

namespace CSharpier.VisualStudio
{
    public class CSharpierProcessProvider : IProcessKiller
    {
        private readonly CustomPathInstaller customPathInstaller;
        private readonly Logger logger;

        private bool warnedForOldVersion;

        private readonly Dictionary<string, bool> warmingByDirectory =
            new Dictionary<string, bool>();
        private readonly Dictionary<string, string> csharpierVersionByDirectory =
            new Dictionary<string, string>();
        private readonly Dictionary<string, ICSharpierProcess> csharpierProcessesByVersion =
            new Dictionary<string, ICSharpierProcess>();

        private static CSharpierProcessProvider? instance;

        public static CSharpierProcessProvider GetInstance(CSharpierPackage package)
        {
            return instance ??= new CSharpierProcessProvider(package);
        }

        private CSharpierProcessProvider(CSharpierPackage package)
        {
            this.logger = Logger.Instance;
            this.customPathInstaller = CustomPathInstaller.GetInstance(package);
        }

        public void FindAndWarmProcess(string filePath)
        {
            var directory = new FileInfo(filePath).DirectoryName;
            if (directory == null)
            {
                this.logger.Warn("There was no directory for " + filePath);
                return;
            }
            if (this.warmingByDirectory.TryGetValue(directory, out var warming) && warming)
            {
                return;
            }
            this.logger.Debug("Ensure there is a csharpier process for " + directory);
            this.warmingByDirectory[directory] = true;
            if (!this.csharpierVersionByDirectory.TryGetValue(directory, out var version))
            {
                version = this.GetCSharpierVersion(directory);
                if (string.IsNullOrEmpty(version))
                {
                    InstallerService.Instance.DisplayInstallNeededMessage(directory, this);
                }
                this.csharpierVersionByDirectory[directory] = version;
            }

            if (!this.csharpierProcessesByVersion.ContainsKey(version))
            {
                this.csharpierProcessesByVersion[version] = this.SetupCSharpierProcess(
                    directory,
                    version
                );
            }

            this.warmingByDirectory.Remove(directory);
        }

        public ICSharpierProcess GetProcessFor(string filePath)
        {
            var directory = new FileInfo(filePath).DirectoryName;
            if (!this.csharpierVersionByDirectory.TryGetValue(directory, out var version))
            {
                this.FindAndWarmProcess(filePath);
                version = this.csharpierVersionByDirectory[directory];
            }

            if (
                version == null
                || !this.csharpierProcessesByVersion.TryGetValue(version, out var cSharpierProcess)
            )
            {
                // this shouldn't really happen, but just in case
                return new NullCSharpierProcess();
            }

            return cSharpierProcess;
        }

        private string GetCSharpierVersion(string directoryThatContainsFile)
        {
            var currentDirectory = new DirectoryInfo(directoryThatContainsFile);
            try
            {
                while (true)
                {
                    var csProjVersion = this.FindVersionInCsProj(currentDirectory);
                    if (csProjVersion != null)
                    {
                        return csProjVersion;
                    }

                    var dotnetToolsPath = Path.Combine(
                        currentDirectory.FullName,
                        ".config/dotnet-tools.json"
                    );
                    this.logger.Debug("Looking for " + dotnetToolsPath);
                    if (File.Exists(dotnetToolsPath))
                    {
                        var data = File.ReadAllText(dotnetToolsPath);
                        var toolsManifest = JsonConvert.DeserializeObject<ToolsManifest>(data);
                        var versionFromTools = toolsManifest?.Tools?.Csharpier?.Version;
                        if (versionFromTools != null)
                        {
                            this.logger.Debug(
                                "Found version " + versionFromTools + " in " + dotnetToolsPath
                            );
                            return versionFromTools;
                        }
                    }

                    if (currentDirectory.Parent == null)
                    {
                        break;
                    }
                    currentDirectory = currentDirectory.Parent;
                }
            }
            catch (Exception ex)
            {
                this.logger.Error(ex);
            }

            this.logger.Debug(
                "Unable to find dotnet-tools.json, falling back to running dotnet csharpier --version"
            );

            var env = new Dictionary<string, string> { { "DOTNET_NOLOGO", "1" } };

            var versionFromCommand = ProcessHelper.ExecuteCommand(
                "dotnet",
                "csharpier --version",
                env,
                directoryThatContainsFile
            );

            this.logger.Debug("dotnet csharpier --version output: " + versionFromCommand);

            if (versionFromCommand.Contains("+"))
            {
                versionFromCommand = versionFromCommand.Split('+')[0];
                this.logger.Debug($"Removing everything after + to use {versionFromCommand}");
            }

            return versionFromCommand;
        }

        private string? FindVersionInCsProj(DirectoryInfo currentDirectory)
        {
            foreach (var pathToCsProj in currentDirectory.GetFiles("*.csproj"))
            {
                var csProjXmlDocument = new XmlDocument();
                try
                {
                    csProjXmlDocument.Load(pathToCsProj.FullName);
                }
                catch (Exception ex)
                {
                    this.logger.Warn(
                        $"The csproj at {pathToCsProj} failed to load with the following exception {ex.Message}"
                    );

                    continue;
                }

                var csharpierMsBuildElement = csProjXmlDocument
                    .SelectNodes("//PackageReference[@Include='CSharpier.MsBuild']")
                    .Cast<XmlNode>()
                    .FirstOrDefault();

                var versionOfMsBuildPackage = csharpierMsBuildElement?.Attributes["Version"]?.Value;
                if (versionOfMsBuildPackage != null)
                {
                    this.logger.Debug(
                        "Found version " + versionOfMsBuildPackage + " in " + pathToCsProj.FullName
                    );
                    return versionOfMsBuildPackage;
                }
            }

            return null;
        }

        private ICSharpierProcess SetupCSharpierProcess(string directory, string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                return new NullCSharpierProcess();
            }

            this.customPathInstaller.EnsureVersionInstalled(version);
            var customPath = this.customPathInstaller.GetPathForVersion(version);
            try
            {
                this.logger.Debug("Adding new version " + version + " process for " + directory);

                var installedVersion = new Version(version);
                var pipeFilesVersion = new Version("0.12.0");
                if (installedVersion.CompareTo(pipeFilesVersion) < 0)
                {
                    if (!this.warnedForOldVersion)
                    {
                        var content =
                            "Please upgrade to CSharpier >= 0.12.0 for bug fixes and improved formatting speed.";
                        InfoBarService.Instance.ShowInfoBar(content);
                        this.warnedForOldVersion = true;
                    }

                    return new CSharpierProcessSingleFile(customPath, this.logger);
                }
                return new CSharpierProcessPipeMultipleFiles(customPath, this.logger);
            }
            catch (Exception ex)
            {
                this.logger.Error(ex);
            }

            return new NullCSharpierProcess();
        }

        public void KillRunningProcesses()
        {
            foreach (var version in this.csharpierProcessesByVersion.Keys)
            {
                this.logger.Debug(
                    "disposing of process for version " + (version == "" ? "null" : version)
                );
                this.csharpierProcessesByVersion[version].Dispose();
            }
            this.warmingByDirectory.Clear();
            this.csharpierVersionByDirectory.Clear();
            this.csharpierProcessesByVersion.Clear();
        }

        private class ToolsManifest
        {
            public Tools? Tools { get; set; }
        }

        private class Tools
        {
            public CSharpierTool? Csharpier { get; set; }
        }

        private class CSharpierTool
        {
            public string? Version { get; set; }
        }
    }
}
