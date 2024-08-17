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

            var csharpierProcess = this.cSharpierProcessProvider.GetProcessFor(document.FullName);

            this.logger.Info(
                "Formatting started for "
                    + document.FullName
                    + " using CSharpier "
                    + csharpierProcess.Version
            );
            var stopwatch = Stopwatch.StartNew();

            void UpdateText(string formattedText)
            {
                startPoint.ReplaceText(
                    endPoint,
                    formattedText,
                    (int)vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers
                );
            }

            if (csharpierProcess is ICSharpierProcess2 csharpierProcess2)
            {
                var parameter = new FormatFileParameter
                {
                    fileContents = text,
                    fileName = document.FullName,
                };
                var result = csharpierProcess2.formatFile(parameter);

                this.logger.Info("Formatted in " + stopwatch.ElapsedMilliseconds + "ms");

                if (result == null)
                {
                    return;
                }

                switch (result.status)
                {
                    case Status.Formatted:
                        UpdateText(result.formattedFile);
                        break;
                    case Status.Ignored:
                        this.logger.Info("File is ignored by csharpier cli.");
                        break;
                    case Status.Failed:
                        this.logger.Warn(
                            "CSharpier cli failed to format the file and returned the following error: "
                                + result.errorMessage
                        );
                        break;
                }
            }
            else
            {
                var result = csharpierProcess.FormatFile(text, document.FullName);

                this.logger.Info("Formatted in " + stopwatch.ElapsedMilliseconds + "ms");

                if (string.IsNullOrEmpty(result) || result.Equals(text))
                {
                    this.logger.Debug(
                        "Skipping write because "
                            + (
                                string.IsNullOrEmpty(result)
                                    ? "result is empty"
                                    : "current document equals result"
                            )
                    );
                }
                else
                {
                    UpdateText(result);
                }
            }
        }

        public bool ProcessSupportsFormatting(Document document) =>
            this.cSharpierProcessProvider.GetProcessFor(document.FullName)
                is not NullCSharpierProcess;
    }
}
