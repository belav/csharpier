using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace CSharpier.VisualStudio
{
    [Guid(PackageGuidString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(CSharpierOptionsPage), "CSharpier", "General", 0, 0, true)]
    [ProvideProfile(typeof(CSharpierOptionsPage), "CSharpier", "General", 0, 0, true)]
    public sealed class CSharpierPackage : AsyncPackage, IVsPersistSolutionOpts
    {
        public const string PackageGuidString = "d348ba73-11dc-46be-8660-6d9819fc2c52";
        private const string OptionsStreamKey = "csharpier_options";

        private CSharpierOptions options;

        protected override async Task InitializeAsync(
            CancellationToken cancellationToken,
            IProgress<ServiceProgressData> progress
        )
        {
            await Logger.InitializeAsync(this);
            Logger.Instance.Info("Starting");

            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            LoadSettings();

            await InfoBarService.InitializeAsync(this);
            await ReformatWithCSharpierOnSave.InitializeAsync(this);
            await ReformatWithCSharpier.InitializeAsync(this);
            await InstallerService.InitializeAsync(this);

            var dte = await this.GetServiceAsync(typeof(DTE)) as DTE;
            if (dte.ActiveDocument != null)
            {
                CSharpierProcessProvider
                    .GetInstance(this)
                    .FindAndWarmProcess(dte.ActiveDocument.FullName);
            }
        }

        public void LoadSettings()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var solutionPersistence =
                GetGlobalService(typeof(SVsSolutionPersistence)) as IVsSolutionPersistence;
            solutionPersistence.LoadPackageUserOpts(this, OptionsStreamKey);
        }

        public bool PersistSettings()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var solutionPersistence =
                GetGlobalService(typeof(SVsSolutionPersistence)) as IVsSolutionPersistence;
            return solutionPersistence.SavePackageUserOpts(this, OptionsStreamKey)
                == VSConstants.S_OK;
        }

        // Called by the shell when a solution is opened and the SUO file is read.
        int IVsPersistSolutionOpts.LoadUserOptions(
            IVsSolutionPersistence pPersistence,
            uint grfLoadOpts
        )
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return pPersistence.LoadPackageUserOpts(this, OptionsStreamKey);
        }

        // Called by the shell if the `key` section declared in LoadUserOptions() as
        // being written by this package has been found in the suo file
        int IVsPersistSolutionOpts.ReadUserOptions(IStream pOptionsStream, string pszKey)
        {
            this.options = null;

            try
            {
                using var stream = new VSStreamWrapper(pOptionsStream);
                using var sr = new StreamReader(stream);
                var json = sr.ReadToEnd();
                Logger.Instance.Info("Read-" + json);
                this.options = JsonConvert.DeserializeObject<CSharpierOptions>(json);
            }
            catch (Exception e)
            {
                Logger.Instance.Error(e);
            }

            var optionsPage = (CSharpierOptionsPage)GetDialogPage(typeof(CSharpierOptionsPage));
            optionsPage.SetOnApply(this.PersistSettings);

            this.options ??= optionsPage.LoadDefaultOptions();

            optionsPage.OnOptionsLoaded(this.options);

            return this.options == null ? VSConstants.E_FAIL : VSConstants.S_OK;
        }

        // Called by the shell when the SUO file is saved when a solution is closed.
        // The provider calls the shell back to let it know which options keys it will use in the suo file.
        int IVsPersistSolutionOpts.SaveUserOptions(IVsSolutionPersistence pPersistence)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return pPersistence.SavePackageUserOpts(this, OptionsStreamKey);
        }

        // Called by the shell to let the package write user options under the specified key.
        int IVsPersistSolutionOpts.WriteUserOptions(IStream pOptionsStream, string pszKey)
        {
            var optionsPage = (CSharpierOptionsPage)GetDialogPage(typeof(CSharpierOptionsPage));
            if (
                this.options == null
                || optionsPage.OptionStorageType != CSharpierOptionsPage.StorageType.Local
            )
            {
                return VSConstants.S_OK;
            }

            try
            {
                using var stream = new VSStreamWrapper(pOptionsStream);
                using var sw = new StreamWriter(stream);
                var json = JsonConvert.SerializeObject(this.options);
                Logger.Instance.Info("Save-" + json);
                sw.Write(json);
            }
            catch (Exception e)
            {
                Logger.Instance.Error(e);
            }

            return VSConstants.S_OK;
        }
    }
}
