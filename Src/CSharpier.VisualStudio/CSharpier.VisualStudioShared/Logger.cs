using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CSharpier.VisualStudio
{
    public class Logger
    {
        private readonly IVsOutputWindow outputWindow;
        private IVsOutputWindowPane pane;

        public Logger(IVsOutputWindow outputWindow)
        {
            this.outputWindow = outputWindow;
        }

        public void Log(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            ThreadHelper.JoinableTaskFactory.Run(
                async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    try
                    {
                        if (pane == null)
                        {
                            var guid = Guid.NewGuid();

                            outputWindow.CreatePane(ref guid, "CSharpier", 1, 1);
                            outputWindow.GetPane(ref guid, out pane);
                        }

                        pane.OutputStringThreadSafe(message + Environment.NewLine);
                    }
                    catch { }
                }
            );
        }

        public void Log(Exception ex)
        {
            this.Log("Exception: " + ex);
        }
    }
}
