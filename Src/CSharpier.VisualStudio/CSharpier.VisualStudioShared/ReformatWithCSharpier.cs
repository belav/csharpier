using System;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace CSharpier.VisualStudio
{
    internal sealed class ReformatWithCSharpier
    {
        public const int CommandId = 0x0100;

        public static readonly Guid CommandSet = new Guid("3675aa7e-5cd1-4ea2-a077-17e6eefdc3b1");

        private readonly CSharpierPackage package;
        private readonly DTE dte;
        private readonly FormattingService formattingService;

        private ReformatWithCSharpier(
            CSharpierPackage package,
            OleMenuCommandService commandService,
            DTE dte,
            FormattingService formattingService
        )
        {
            this.package = package;
            this.dte = dte;

            var menuCommandId = new CommandID(CommandSet, CommandId);
            var menuItem = new OleMenuCommand(this.Execute, menuCommandId);
            menuItem.BeforeQueryStatus += this.QueryStatus;
            commandService.AddCommand(menuItem);
            this.formattingService = formattingService;
        }

        private void QueryStatus(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var button = (OleMenuCommand)sender;

            button.Visible = this.dte.ActiveDocument.Name.EndsWith(".cs");
            button.Enabled = this.formattingService.CanFormat;
        }

        public static ReformatWithCSharpier Instance { get; private set; }

        public static async Task InitializeAsync(
            CSharpierPackage package,
            FormattingService formattingService
        )
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var commandService =
                await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

            var dte = await package.GetServiceAsync(typeof(DTE)) as DTE;

            Instance = new ReformatWithCSharpier(package, commandService, dte, formattingService);
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!this.formattingService.CanFormat)
            {
                return;
            }
            this.formattingService.Format(this.dte.ActiveDocument);
        }
    }
}
