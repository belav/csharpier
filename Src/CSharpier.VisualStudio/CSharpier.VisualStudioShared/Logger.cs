using System;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace CSharpier.VisualStudio
{
    public class Logger
    {
        private readonly IVsOutputWindowPane pane;
        private readonly CSharpierOptionsPage options;

        public static Logger Instance { get; private set; }

        public static async Task InitializeAsync(CSharpierPackage package)
        {
            var outputWindow = await package.GetServiceAsync<IVsOutputWindow>();
            var cSharpierOptions = package.GetDialogPage<CSharpierOptionsPage>();
            Instance = new Logger(outputWindow, cSharpierOptions);

            var solution = await package.GetServiceAsync<SVsSolution>() as IVsSolution;
            solution.GetSolutionInfo(out var dir, out var fileName, out var userOptsFile);
            Instance.Info(dir);
            Instance.Info(fileName);
            Instance.Info(userOptsFile);
        }

        private Logger(IVsOutputWindow outputWindow, CSharpierOptionsPage options)
        {
            this.options = options;

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
            if (this.options.LogDebugMessages)
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
