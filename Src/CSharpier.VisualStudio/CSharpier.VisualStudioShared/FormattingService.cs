using System.Diagnostics;
using EnvDTE;

namespace CSharpier.VisualStudio
{
    public class FormattingService
    {
        private readonly Logger logger;
        private readonly CSharpierProcessProvider cSharpierProcessProvider;

        private static FormattingService? instance;

        public static FormattingService GetInstance(CSharpierPackage package)
        {
            return instance ??= new FormattingService(package);
        }

        private FormattingService(CSharpierPackage package)
        {
            this.logger = Logger.Instance;
            this.cSharpierProcessProvider = CSharpierProcessProvider.GetInstance(package);
        }

        public void Format(Document document)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            if (!this.ProcessSupportsFormatting(document))
            {
                this.logger.Debug(
                    "Skipping formatting because process does not support formatting."
                );
                return;
            }

            if (document.Language != "CSharp")
            {
                this.logger.Debug("Skipping formatting because language was " + document.Language);
                return;
            }

            if (!(document.Object("TextDocument") is TextDocument textDocument))
            {
                this.logger.Debug("There was no TextDocument for the current Document");
                return;
            }

            var startPoint = textDocument.StartPoint.CreateEditPoint();
            var endPoint = textDocument.EndPoint.CreateEditPoint();
            var text = startPoint.GetText(endPoint);

            this.logger.Info("Formatting started for " + document.FullName + ".");
            var stopwatch = Stopwatch.StartNew();

            var newText = this.cSharpierProcessProvider
                .GetProcessFor(document.FullName)
                .FormatFile(text, document.FullName);

            this.logger.Info("Formatted in " + stopwatch.ElapsedMilliseconds + "ms");
            if (string.IsNullOrEmpty(newText) || newText.Equals(text))
            {
                this.logger.Debug(
                    "Skipping write because "
                        + (
                            string.IsNullOrEmpty(newText)
                              ? "result is empty"
                              : "current document equals result"
                        )
                );
                return;
            }

            startPoint.ReplaceText(
                endPoint,
                newText,
                (int)vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers
            );
        }

        public bool ProcessSupportsFormatting(Document document) =>
            !(
                this.cSharpierProcessProvider.GetProcessFor(document.FullName)
                is NullCSharpierProcess
            );
    }
}
