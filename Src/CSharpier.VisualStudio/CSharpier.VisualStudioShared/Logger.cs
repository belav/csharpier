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
            this.Log(message, "Warn");
        }

        public void Debug(string message)
        {
            if (CSharpierOptions.Instance.GlobalLogDebugMessages)
            {
                this.Log(message, "Debug");
            }
        }

        public void Info(string message)
        {
            this.Log(message, "Info");
        }

        public void Error(Exception ex)
        {
            this.Log(ex.ToString(), "Error");
        }

        private void Log(string message, string logLevel)
        {
            this.pane.OutputStringThreadSafe($"[{logLevel} - {DateTime.Now}] {message}\n");
        }
    }
}
