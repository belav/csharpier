using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace CSharpier.VisualStudio
{
    public class InfoBarService : IVsInfoBarUIEvents
    {
        private readonly Logger logger;
        private readonly CSharpierPackage package;
        private uint cookie;
        private Dictionary<string, Action> buttonActions = new Dictionary<string, Action>();

        private InfoBarService(CSharpierPackage package)
        {
            this.package = package;
            this.logger = Logger.Instance;
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
        )
        {
            if (
                this.buttonActions.TryGetValue(
                    actionItem.ActionContext.ToString(),
                    out Action onClick
                )
            )
            {
                onClick();
            }
            else
            {
                this.logger.Debug("No action found for the context " + actionItem.ActionContext);
            }

            infoBarUIElement.Close();
        }

        public void ShowInfoBar(string message, IEnumerable<InfoBarActionButton> buttons = null)
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

            if (buttons != null)
            {
                foreach (var infoBarActionButton in buttons)
                {
                    this.buttonActions[infoBarActionButton.Context] = infoBarActionButton.OnClicked;
                }
            }

            var actions = (buttons ?? Enumerable.Empty<InfoBarActionButton>())
                .Select(o => new InfoBarButton(o.Text, o.Context))
                .ToArray();
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

    public class InfoBarActionButton
    {
        public string Text { get; set; }
        public string Context { get; set; }
        public Action OnClicked { get; set; }
    }
}
