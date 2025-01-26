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
        private readonly CSharpierProcessProvider cSharpierProcessProvider;

        public static ReformatWithCSharpier Instance { get; private set; } = null!;

        public static async Task InitializeAsync(CSharpierPackage package)
        {
            var commandService =
                await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

            var dte = await package.GetServiceAsync(typeof(DTE)) as DTE;

            Instance = new ReformatWithCSharpier(commandService!, dte!);
        }

        private ReformatWithCSharpier(IMenuCommandService commandService, DTE dte)
        {
            this.dte = dte;

            var menuCommandId = new CommandID(CommandSet, CommandId);
            var menuItem = new OleMenuCommand(this.Execute, menuCommandId);
            menuItem.BeforeQueryStatus += this.QueryStatus;
            commandService.AddCommand(menuItem);
            this.formattingService = FormattingService.GetInstance();
            this.cSharpierProcessProvider = CSharpierProcessProvider.GetInstance();
        }

        private void QueryStatus(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var button = (OleMenuCommand)sender;

            var hasWarmedProcess = this.cSharpierProcessProvider.HasWarmedProcessFor(
                this.dte.ActiveDocument.FullName
            );
            button.Visible = FormattingService.IsSupportedLanguage(
                this.dte.ActiveDocument.Language
            );

            if (!hasWarmedProcess)
            {
                // default to assuming they can format, that way if they do format we can start everything up properly
                button.Enabled = true;
            }
            else
            {
                button.Enabled = this.formattingService.ProcessSupportsFormatting(
                    this.dte.ActiveDocument.FullName
                );
            }
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            this.formattingService.Format(this.dte.ActiveDocument);
        }
    }
}
