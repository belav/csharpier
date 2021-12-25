using System;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace CSharpier.VisualStudio
{
    // https://michaelscodingspot.com/visual-studio-2017-extension-development-tutorial-part-2-add-menu-item/

    // TODO doc now to add a keyboard shortcut
    // EditorContextMenus.CodeWindow.ReformatWithCSharpier

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
            menuItem.BeforeQueryStatus += QueryStatus;
            commandService.AddCommand(menuItem);
            this.formattingService = formattingService;
        }

        private void QueryStatus(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var button = (OleMenuCommand)sender;

            button.Visible = dte.ActiveDocument.Name.EndsWith(".cs");
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
            this.formattingService.Format(this.dte.ActiveDocument);
        }
    }
}
