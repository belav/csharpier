using EnvDTE;

namespace CSharpier.VisualStudio
{
    public class FormattingService
    {
        private readonly Logger logger;
        private readonly CSharpierService cSharpierService;

        public FormattingService(Logger logger, CSharpierService cSharpierService)
        {
            this.logger = logger;
            this.cSharpierService = cSharpierService;
        }

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

            var editPoint = textDocument.StartPoint.CreateEditPoint();
            var endPoint = textDocument.EndPoint.CreateEditPoint();
            var text = editPoint.GetText(endPoint);

            var newText = this.cSharpierService.Format(text, document.FullName);
            if (string.IsNullOrEmpty(newText))
            {
                return;
            }

            editPoint.ReplaceText(
                endPoint,
                newText,
                (int)vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers
            );
        }
    }
}
