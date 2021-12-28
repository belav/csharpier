using EnvDTE;

namespace CSharpier.VisualStudio
{
    public class FormattingService
    {
        private readonly Logger logger;
        private readonly CSharpierService csharpierService;

        public FormattingService(Logger logger, CSharpierService csharpierService)
        {
            this.logger = logger;
            this.csharpierService = csharpierService;
        }

        public bool CanFormat => this.csharpierService.CanFormat;

        public void Format(Document document)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            if (document.Language != "CSharp")
            {
                return;
            }

            if (!(document.Object("TextDocument") is TextDocument textDocument))
            {
                this.logger.Log("There was no TextDocument for the current Document");
                return;
            }

            var startPoint = textDocument.StartPoint.CreateEditPoint();
            var endPoint = textDocument.EndPoint.CreateEditPoint();
            var text = startPoint.GetText(endPoint);

            var newText = this.csharpierService.Format(text, document.FullName);
            if (string.IsNullOrEmpty(newText))
            {
                return;
            }

            startPoint.ReplaceText(
                endPoint,
                newText,
                (int)vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers
            );
        }
    }
}
