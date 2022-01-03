using System;
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

        public void Log(string message)
        {
            this.pane.OutputStringThreadSafe(message + Environment.NewLine);
        }

        public void Log(Exception ex)
        {
            this.Log("Exception: " + ex);
        }
    }
}
