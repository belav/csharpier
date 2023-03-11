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

        public static readonly Guid CommandSet = new("3675aa7e-5cd1-4ea2-a077-17e6eefdc3b1");

        private readonly DTE dte;
        private readonly FormattingService formattingService;

        public static ReformatWithCSharpier Instance { get; private set; } = default!;

        public static async Task InitializeAsync(CSharpierPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var commandService =
                await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

            var dte = await package.GetServiceAsync(typeof(DTE)) as DTE;

            Instance = new ReformatWithCSharpier(package, commandService!, dte!);
        }

        private ReformatWithCSharpier(
            CSharpierPackage package,
            IMenuCommandService commandService,
            DTE dte
        )
        {
            this.dte = dte;

            var menuCommandId = new CommandID(CommandSet, CommandId);
            var menuItem = new OleMenuCommand(this.Execute, menuCommandId);
            menuItem.BeforeQueryStatus += this.QueryStatus;
            commandService.AddCommand(menuItem);
            this.formattingService = FormattingService.GetInstance(package);
        }

        private void QueryStatus(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var button = (OleMenuCommand)sender;

            button.Visible = this.dte.ActiveDocument.Name.EndsWith(".cs");
            button.Enabled = this.formattingService.ProcessSupportsFormatting(
                this.dte.ActiveDocument
            );
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            this.formattingService.Format(this.dte.ActiveDocument);
        }
    }
}
