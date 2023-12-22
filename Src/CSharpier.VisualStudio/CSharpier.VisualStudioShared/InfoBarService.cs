using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly Dictionary<string, Action> buttonActions = new();

        private InfoBarService(CSharpierPackage package)
        {
            this.package = package;
            this.logger = Logger.Instance;
        }

        public static InfoBarService Instance { get; private set; } = default!;

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

        public void ShowInfoBar(string message, IEnumerable<InfoBarActionButton>? buttons = null)
        {
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
                .Select(
                    o =>
                        o.IsHyperLink
                            ? new InfoBarHyperlink(o.Text, o.Context) as InfoBarActionItem
                            : new InfoBarButton(o.Text, o.Context)
                )
                .ToArray();
            var infoBarModel = new InfoBarModel(
                spans,
                actions,
                KnownMonikers.StatusInformation,
                isCloseButtonVisible: true
            );

            this.ShowInfoBar(infoBarModel);
        }

        public void ShowInfoBar(InfoBarModel infoBarModel)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

#pragma warning disable VSSDK006, VSTHRD002
            if (this.package.GetServiceAsync(typeof(SVsShell)).Result is not IVsShell shell)
            {
                return;
            }
#pragma warning restore

            shell.GetProperty((int)__VSSPROPID7.VSSPROPID_MainWindowInfoBarHost, out var property);
            if (property is not IVsInfoBarHost infoBarHost)
            {
                return;
            }

#pragma warning disable VSTHRD002, VSSDK006
            var factory =
                this.package.GetServiceAsync(typeof(SVsInfoBarUIFactory)).Result
                as IVsInfoBarUIFactory;
#pragma warning restore
            var element = factory!.CreateInfoBar(infoBarModel);
            element.Advise(this, out this.cookie);
            infoBarHost.AddInfoBar(element);
        }
    }

    public class InfoBarActionButton
    {
        public bool IsHyperLink { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Context { get; set; } = string.Empty;
        public Action OnClicked { get; set; } = default!;
    }
}
