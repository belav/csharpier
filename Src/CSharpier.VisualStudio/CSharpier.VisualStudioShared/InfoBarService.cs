using System.Windows.Forms;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace CSharpier.VisualStudio
{
    public class InfoBarService : IVsInfoBarUIEvents
    {
        private readonly CSharpierPackage csharpierPackage;
        private uint cookie;

        private InfoBarService(CSharpierPackage csharpierPackage)
        {
            this.csharpierPackage = csharpierPackage;
        }

        public static InfoBarService Instance { get; private set; }

        public static Task InitializeAsync(CSharpierPackage serviceProvider)
        {
            Instance = new InfoBarService(serviceProvider);

            return Task.CompletedTask;
        }

        public void OnClosed(IVsInfoBarUIElement infoBarUiElement)
        {
            infoBarUiElement.Unadvise(this.cookie);
        }

        public void OnActionItemClicked(
            IVsInfoBarUIElement infoBarUIElement,
            IVsInfoBarActionItem actionItem
        ) { }

        public void ShowInfoBar(string message)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var shell = this.csharpierPackage.GetServiceAsync(typeof(SVsShell)).Result as IVsShell;
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
                this.csharpierPackage.GetServiceAsync(typeof(SVsInfoBarUIFactory)).Result
                as IVsInfoBarUIFactory;
            var element = factory.CreateInfoBar(infoBarModel);
            element.Advise(this, out this.cookie);
            infoBarHost.AddInfoBar(element);
        }
    }
}
