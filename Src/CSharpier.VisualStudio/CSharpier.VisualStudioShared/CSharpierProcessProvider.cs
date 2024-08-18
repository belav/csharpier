using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace CSharpier.VisualStudio
{
    public class CSharpierProcessProvider : IProcessKiller
    {
        private readonly CustomPathInstaller customPathInstaller;
        private readonly CSharpierPackage package;
        private readonly Logger logger;

        private bool warnedForOldVersion;

        private readonly Dictionary<string, bool> warmingByDirectory = new();
        private readonly Dictionary<string, string> csharpierVersionByDirectory = new();
        private readonly Dictionary<string, ICSharpierProcess> csharpierProcessesByVersion = new();

        private static CSharpierProcessProvider? instance;

        public static CSharpierProcessProvider GetInstance(CSharpierPackage package)
        {
            return instance ??= new CSharpierProcessProvider(package);
        }

        private CSharpierProcessProvider(CSharpierPackage package)
        {
            this.logger = Logger.Instance;
            this.customPathInstaller = CustomPathInstaller.GetInstance(package);
            this.package = package;
        }

        public void FindAndWarmProcess(string filePath)
        {
            var directory = new FileInfo(filePath).DirectoryName;
            if (directory == null)
            {
                this.logger.Warn($"There was no directory for {filePath}");
                return;
            }
            if (this.warmingByDirectory.TryGetValue(directory, out var warming) && warming)
            {
                return;
            }
            this.logger.Debug($"Ensure there is a csharpier process for {directory}");
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
                return NullCSharpierProcess.Instance;
            }

            return cSharpierProcess;
        }

        private string GetCSharpierVersion(string directoryThatContainsFile)
        {
            var csharpierVersion = FunctionRunner.RunUntilNonNull(
                () => this.FindVersionInCsProjOfParentsDirectories(directoryThatContainsFile),
                () => this.FindCSharpierVersionInToolOutput(directoryThatContainsFile, false),
                () => this.FindCSharpierVersionInToolOutput(directoryThatContainsFile, true)
            );

            if (csharpierVersion == null)
            {
                return "";
            }

            var versionWithoutHash = csharpierVersion.Split('+')[0];
            this.logger.Debug($"Using {versionWithoutHash} as the version number.");

            return versionWithoutHash;
        }

        private string? FindVersionInCsProjOfParentsDirectories(string directoryThatContainsFile)
        {
            this.logger.Debug(
                $"Looking for csproj in or above {directoryThatContainsFile} that references CSharpier.MsBuild"
            );
            var currentDirectory = new DirectoryInfo(directoryThatContainsFile);
            try
            {
                while (currentDirectory != null)
                {
                    var csProjVersion = this.FindVersionInCsProj(currentDirectory);
                    if (csProjVersion != null)
                    {
                        return csProjVersion;
                    }

                    currentDirectory = currentDirectory.Parent;
                }
            }
            catch (Exception ex)
            {
                this.logger.Error(ex);
            }

            return null;
        }

        private string? FindCSharpierVersionInToolOutput(
            string directoryThatContainsFile,
            bool isGlobal
        )
        {
            var env = new Dictionary<string, string> { { "DOTNET_NOLOGO", "1" } };
            var output = ProcessHelper.ExecuteCommand(
                "dotnet",
                $"tool list{(isGlobal ? " -g" : "")}",
                env,
                directoryThatContainsFile
            );

            this.logger.Debug(
                $"Running 'dotnet tool list{(isGlobal ? "-g" : "")}' to look for version"
            );
            this.logger.Debug($"Output was: \n {output}");

            var lines = output.Split('\n').Select(o => o.Trim()).ToList();

            // The first two lines are headers, so we start at index 2
            for (var i = 2; i < lines.Count; i++)
            {
                var columns = Regex.Split(lines[i], "\\s{2,}"); // Split by 2 or more spaces
                if (columns.Length >= 2)
                {
                    if (columns[0].ToLower() == "csharpier")
                    {
                        return columns[1];
                    }
                }
            }

            return null;
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
                        $"Found version {versionOfMsBuildPackage} in {pathToCsProj.FullName}"
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
                return NullCSharpierProcess.Instance;
            }

            try
            {
                if (!this.customPathInstaller.EnsureVersionInstalled(version))
                {
                    this.DisplayFailureMessage();
                    return NullCSharpierProcess.Instance;
                }
                var customPath = this.customPathInstaller.GetPathForVersion(version);

                this.logger.Debug($"Adding new version {version} process for {directory}");
                var installedVersion = this.GetInstalledVersion(version);
                var pipeFilesVersion = new Version("0.12.0");
                var serverVersion = new Version("0.29.0");
                ICSharpierProcess cSharpierProcess;

                if (
                    installedVersion.CompareTo(serverVersion) >= 0
                    && !CSharpierOptions.Instance.DisableCSharpierServer
                )
                {
                    cSharpierProcess = new CSharpierProcessServer(customPath, version, this.logger);
                }
                else if (installedVersion.CompareTo(pipeFilesVersion) >= 0)
                {
                    if (
                        installedVersion.CompareTo(serverVersion) >= 0
                        && CSharpierOptions.Instance.DisableCSharpierServer
                    )
                    {
                        this.logger.Debug(
                            "CSharpier server is disabled, falling back to piping via stdin"
                        );
                    }

                    cSharpierProcess = new CSharpierProcessPipeMultipleFiles(
                        customPath,
                        version,
                        this.logger
                    );
                }
                else
                {
                    if (!this.warnedForOldVersion)
                    {
                        var content =
                            "Please upgrade to CSharpier >= 0.12.0 for bug fixes and improved formatting speed.";
                        InfoBarService.Instance.ShowInfoBar(content);
                        this.warnedForOldVersion = true;
                    }

                    cSharpierProcess = new CSharpierProcessSingleFile(
                        customPath,
                        version,
                        this.logger
                    );
                }

                if (cSharpierProcess.ProcessFailedToStart)
                {
                    this.DisplayFailureMessage();
                }

                return cSharpierProcess;
            }
            catch (Exception ex)
            {
                this.logger.Error(ex);
                this.DisplayFailureMessage();
            }

            return NullCSharpierProcess.Instance;
        }

        private Version GetInstalledVersion(string version)
        {
            if (Version.TryParse(version, out var installedVersion) && installedVersion != null)
            {
                return installedVersion;
            }

            // handle semver versions
            // regex from https://semver.org/#is-there-a-suggested-regular-expression-regex-to-check-a-semver-string
            var regex = Regex.Match(
                version,
                @"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$"
            );
            if (regex.Success && regex.Groups.Count > 3)
            {
                return new Version($"{regex.Groups[1]}.{regex.Groups[2]}.{regex.Groups[3]}");
            }

            throw new ArgumentException($"Unable to parse version '{version}'", nameof(version));
        }

        private void DisplayFailureMessage()
        {
            var actionButton = new InfoBarActionButton
            {
                IsHyperLink = true,
                Text = "Read More",
                Context = "ReadMore",
                OnClicked = () =>
                {
                    Process.Start(
                        new ProcessStartInfo
                        {
                            FileName = "https://csharpier.com/docs/EditorsTroubleshooting",
                            UseShellExecute = true,
                        }
                    );
                },
            };

            InfoBarService.Instance.ShowInfoBar(
                "CSharpier could not be set up properly so formatting is not currently supported.",
                new[] { actionButton }
            );
        }

        public void KillRunningProcesses()
        {
            foreach (var version in this.csharpierProcessesByVersion.Keys)
            {
                this.logger.Debug(
                    $"disposing of process for version {(version == "" ? "null" : version)}"
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
