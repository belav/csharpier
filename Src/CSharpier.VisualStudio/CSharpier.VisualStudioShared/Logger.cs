using System;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace CSharpier.VisualStudio
{
    public class Logger
    {
        private readonly IVsOutputWindowPane pane;
        private readonly CSharpierOptionsPage cSharpierOptionsPage;

        public static Logger Instance { get; private set; }

        public static async Task InitializeAsync(CSharpierPackage package)
        {
            var outputWindow = await package.GetServiceAsync<IVsOutputWindow>();
            var cSharpierOptionsPage = package.GetDialogPage<CSharpierOptionsPage>();
            Instance = new Logger(outputWindow, cSharpierOptionsPage);
        }

        private Logger(IVsOutputWindow outputWindow, CSharpierOptionsPage cSharpierOptionsPage)
        {
            this.cSharpierOptionsPage = cSharpierOptionsPage;

            var guid = Guid.NewGuid();

            // if we do what this warning says, then things don't work
#pragma warning disable VSTHRD010
            outputWindow.CreatePane(ref guid, "CSharpier", 1, 1);
            outputWindow.GetPane(ref guid, out this.pane);
#pragma warning restore VSTHRD010
        }

        public void Warn(string message)
        {
            this.Log(message);
        }

        public void Debug(string message)
        {
            if (this.cSharpierOptionsPage.LogDebugMessages)
            {
                this.Log(message);
            }
        }

        public void Info(string message)
        {
            this.Log(message);
        }

        public void Error(Exception ex)
        {
            this.Log("Exception: " + ex);
        }

        private void Log(string message)
        {
            this.pane.OutputStringThreadSafe(message + Environment.NewLine);
        }
    }
}
