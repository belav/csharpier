using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Settings;

namespace CSharpier.VisualStudio
{
    public class InstallerService
    {
        private readonly Logger logger;
        private readonly DTE dte;

        private bool warnedAlready;
        private const string InstallerWarningShownKey = "CSharpier.InstallerWarningShown";
        private const string CollectionName = "CSharpier";
        private readonly WritableSettingsStore settingsStore;

        public static InstallerService Instance { get; private set; } = null!;

        public static async Task InitializeAsync(CSharpierPackage package)
        {
            var dte = await package.GetServiceAsync(typeof(DTE)) as DTE;
            var settingsManager =
                await package.GetServiceAsync(typeof(SVsSettingsManager)) as IVsSettingsManager;

            var writableSettingsStore = new ShellSettingsManager(
                settingsManager
            ).GetWritableSettingsStore(SettingsScope.UserSettings);
            Instance = new InstallerService(dte!, writableSettingsStore);
        }

        private InstallerService(DTE dte, WritableSettingsStore settingsStore)
        {
            this.logger = Logger.Instance;
            this.dte = dte;

            this.settingsStore = settingsStore;

            if (!this.settingsStore.CollectionExists(CollectionName))
            {
                this.settingsStore.CreateCollection(CollectionName);
            }

            this.warnedAlready = this.settingsStore.GetBoolean(
                CollectionName,
                InstallerWarningShownKey,
                false
            );
            this.logger.Debug(InstallerWarningShownKey + " was " + this.warnedAlready);
        }

        public void DisplayInstallNeededMessage(
            string directoryThatContainsFile,
            IProcessKiller processKiller
        )
        {
            if (this.warnedAlready || this.IgnoreDirectory(directoryThatContainsFile))
            {
                return;
            }

            this.warnedAlready = true;
            this.settingsStore.SetBoolean(CollectionName, InstallerWarningShownKey, true);
            this.logger.Warn("CSharpier was not found so files may not be formatted.");

            var solutionFullName = this.dte.Solution?.FullName;
            var solutionBasePath =
                solutionFullName == null
                    ? "SomeNotRealPath"
                    : new FileInfo(solutionFullName).DirectoryName!.Replace('\\', '/');

            var isOnlyGlobal = !directoryThatContainsFile
                .Replace('\\', '/')
                .StartsWith(solutionBasePath);

            var message = isOnlyGlobal
                ? (
                    "CSharpier needs to be installed globally to format files in "
                    + directoryThatContainsFile
                )
                : "CSharpier needs to be installed to support formatting files";

            var actions = new List<InfoBarActionButton>
            {
                new()
                {
                    Text = "Install CSharpier Globally",
                    Context = "InstallGlobally",
                    OnClicked = () =>
                    {
                        this.logger.Debug("Installing CSharpier globally");
                        ProcessHelper.ExecuteCommand("dotnet", "tool install -g csharpier");
                        processKiller.KillRunningProcesses();
                    },
                },
            };

            if (!isOnlyGlobal)
            {
                actions.Add(
                    new InfoBarActionButton
                    {
                        Text = "Install CSharpier Locally",
                        Context = "InstallLocally",
                        OnClicked = () =>
                        {
                            this.logger.Debug("Installing CSharpier locally");
                            if (
                                !File.Exists(
                                    Path.Combine(solutionBasePath, ".config", "dotnet-tools.json")
                                )
                            )
                            {
                                ProcessHelper.ExecuteCommand(
                                    "dotnet",
                                    "new tool-manifest",
                                    null,
                                    solutionBasePath
                                );
                            }
                            ProcessHelper.ExecuteCommand(
                                "dotnet",
                                "tool install csharpier",
                                null,
                                solutionBasePath
                            );
                            processKiller.KillRunningProcesses();
                        },
                    }
                );
            }

            InfoBarService.Instance.ShowInfoBar(message, actions);
        }

        private bool IgnoreDirectory(string directoryThatContainsFile)
        {
            var normalizedPath = directoryThatContainsFile.Replace("\\", "/");
            return normalizedPath.ContainsIgnoreCase("Temp/TFSTemp")
                || normalizedPath.ContainsIgnoreCase("Temp/MetadataAsSource")
                || normalizedPath.ContainsIgnoreCase("Temp/.vsdbgsrc")
                || normalizedPath.ContainsIgnoreCase("MSBuild/Current/Bin");
        }
    }

    public interface IProcessKiller
    {
        void KillRunningProcesses();
    }
}
