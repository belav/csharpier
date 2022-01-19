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

        // TODO make a proper setting for this so real users can see debug messages
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
            this.pane.OutputStringThreadSafe(message + Environment.NewLine);
        }

        public void Log(Exception ex)
        {
            this.Log("Exception: " + ex);
        }
    }
}
