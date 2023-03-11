using System;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace CSharpier.VisualStudio
{
    public class Logger
    {
        private readonly IVsOutputWindowPane pane;

        public static Logger Instance { get; private set; } = default!;

        public static async Task InitializeAsync(CSharpierPackage package)
        {
            var outputWindow = await package.GetServiceAsync<IVsOutputWindow>();
            Instance = new Logger(outputWindow);
        }

        private Logger(IVsOutputWindow outputWindow)
        {
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
            if (CSharpierOptions.Instance.GlobalLogDebugMessages)
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
