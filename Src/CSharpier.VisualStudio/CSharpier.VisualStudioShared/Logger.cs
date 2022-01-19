using System;
using System.IO;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CSharpier.VisualStudio
{
    public class Logger
    {
        private readonly IVsOutputWindowPane pane;

        public Logger(IVsOutputWindow outputWindow)
        {
            var guid = Guid.NewGuid();

            outputWindow.CreatePane(ref guid, "CSharpier", 1, 1);
            outputWindow.GetPane(ref guid, out this.pane);
        }

        public void Debug(string message)
        {
#if DEBUG
            this.Log(message);
#endif
        }

        public void Info(string message)
        {
            this.Log(message);
        }

        private void Log(string message)
        {
#if DEBUG
            File.AppendAllText("c:/temp/vs.txt", message);
            File.AppendAllText("c:/temp/vs.txt", "\n");
#endif

            this.pane.OutputStringThreadSafe(message + Environment.NewLine);
        }

        public void Log(Exception ex)
        {
            this.Log("Exception: " + ex);
        }
    }
}
