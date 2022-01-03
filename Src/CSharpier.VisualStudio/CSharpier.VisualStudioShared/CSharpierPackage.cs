using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace CSharpier.VisualStudio
{
    [Guid(PackageGuidString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(CSharpierOptionsPage), "CSharpier", "General", 0, 0, true)]
    public sealed class CSharpierPackage : AsyncPackage
    {
        public const string PackageGuidString = "d348ba73-11dc-46be-8660-6d9819fc2c52";

        protected override async Task InitializeAsync(
            CancellationToken cancellationToken,
            IProgress<ServiceProgressData> progress
        )
        {
            var outputPane = await this.GetServiceAsync<IVsOutputWindow>();
            var logger = new Logger(outputPane);
            logger.Log("Starting");

            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            await InfoBarService.InitializeAsync(this);

            var csharpierService = new CSharpierService(logger);
            var formattingService = new FormattingService(logger, csharpierService);

            var csharpierOptionsPage = this.GetDialogPage<CSharpierOptionsPage>();
            await ReformatWithCSharpierOnSave.InitializeAsync(
                this,
                formattingService,
                csharpierOptionsPage
            );
            await ReformatWithCSharpier.InitializeAsync(this, formattingService);
        }
    }
}
