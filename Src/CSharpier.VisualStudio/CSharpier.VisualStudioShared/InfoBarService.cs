using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace CSharpier.VisualStudio
{
    public class InfoBarService : IVsInfoBarUIEvents
    {
        private readonly CSharpierPackage package;
        private uint cookie;

        private InfoBarService(CSharpierPackage package)
        {
            this.package = package;
        }

        public static InfoBarService Instance { get; private set; }

        public static Task InitializeAsync(CSharpierPackage package)
        {
            Instance = new InfoBarService(package);

            return Task.CompletedTask;
        }

        public void OnClosed(IVsInfoBarUIElement infoBarUiElement)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            infoBarUiElement.Unadvise(this.cookie);
        }

        public void OnActionItemClicked(
            IVsInfoBarUIElement infoBarUIElement,
            IVsInfoBarActionItem actionItem
        ) { }

        public void ShowInfoBar(string message)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var shell = this.package.GetServiceAsync(typeof(SVsShell)).Result as IVsShell;
            if (shell == null)
            {
                return;
            }

            shell.GetProperty((int)__VSSPROPID7.VSSPROPID_MainWindowInfoBarHost, out var property);
            if (!(property is IVsInfoBarHost infoBarHost))
            {
                return;
            }
            var text = new InfoBarTextSpan(message);

            var spans = new[] { text };
            var actions = new InfoBarActionItem[] { };
            var infoBarModel = new InfoBarModel(
                spans,
                actions,
                KnownMonikers.StatusInformation,
                isCloseButtonVisible: true
            );

            var factory =
                this.package.GetServiceAsync(typeof(SVsInfoBarUIFactory)).Result
                as IVsInfoBarUIFactory;
            var element = factory.CreateInfoBar(infoBarModel);
            element.Advise(this, out this.cookie);
            infoBarHost.AddInfoBar(element);
        }
    }
}
