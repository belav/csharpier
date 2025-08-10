using System.Diagnostics;
using EnvDTE;

namespace CSharpier.VisualStudio
{
    public class FormattingService
    {
        private readonly Logger logger;
        private readonly CSharpierProcessProvider cSharpierProcessProvider;

        private static FormattingService? instance;

        public static FormattingService GetInstance()
        {
            return instance ??= new FormattingService();
        }

        private FormattingService()
        {
            this.logger = Logger.Instance;
            this.cSharpierProcessProvider = CSharpierProcessProvider.GetInstance();
        }

        public static bool IsSupportedLanguage(string language)
        {
            // if new languages are added, follow this guide to update VSCT
            // https://learn.microsoft.com/en-us/visualstudio/extensibility/walkthrough-creating-a-view-adornment-commands-and-settings-column-guides?view=vs-2022
            // at this point it will probably just be adding new command placements + guidSymbols like the xaml one
            return language is "CSharp" or "XML" or "XAML";
        }

        public void Format(Document document)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            if (!this.ProcessSupportsFormatting(document.FullName))
            {
                this.logger.Debug(
                    "Skipping formatting because process does not support formatting."
                );
                return;
            }

            if (!IsSupportedLanguage(document.Language))
            {
                this.logger.Debug("Skipping formatting because language was " + document.Language);
                return;
            }

            if (document.Object("TextDocument") is not TextDocument textDocument)
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

                if (result == null)
                {
                    return;
                }

                switch (result.status)
                {
                    case Status.Formatted:
                        this.logger.Info("Formatted in " + stopwatch.ElapsedMilliseconds + "ms");
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
                    case Status.UnsupportedFile:
                        this.logger.Warn(
                            "CSharpier does not support formatting the file " + document.FullName
                        );
                        break;
                    default:
                        this.logger.Warn("Unable to handle the status of " + result.status);
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

        public bool ProcessSupportsFormatting(string filePath) =>
            this.cSharpierProcessProvider.GetProcessFor(filePath) is not NullCSharpierProcess;
    }
}
