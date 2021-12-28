using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace CSharpier.VisualStudio
{
    public class CSharpierService
    {
        private readonly string csharpierPath;
        private readonly ICSharpierProcess csharpierProcess;
        private readonly Logger logger;

        public CSharpierService(Logger logger)
        {
            this.logger = logger;

            this.csharpierPath = this.GetCSharpierPath();

            logger.Log("Using command dotnet " + this.csharpierPath);

            this.csharpierProcess = this.SetupCSharpierProcess();
        }

        public string GetCSharpierPath()
        {
            // TODO make this some kind of build property so it only works when testing the plugin
            // or maybe make it a setting?
            //        try {
            //            String csharpierDebugPath = "C:\\projects\\csharpier\\Src\\CSharpier.Cli\\bin\\Debug\\net6.0\\dotnet-csharpier.dll";
            //            String csharpierReleasePath = csharpierDebugPath.replace("Debug", "Release");
            //
            //            if (new File(csharpierDebugPath).exists()) {
            //                return csharpierDebugPath;
            //            } else if (new File(csharpierReleasePath).exists()) {
            //                return csharpierReleasePath;
            //            }
            //        } catch (Exception ex) {
            //            Log.debug("Could not find local csharpier " + ex.getMessage());
            //        }

            return "csharpier";
        }

        public string ExecuteCommand(string cmd, string arguments)
        {
            var processStartInfo = new ProcessStartInfo(cmd, arguments)
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
#if DEBUG
                // TODO when testing, this runs from in the csharpier directory, which means it uses csharpier from there instead of globally
                WorkingDirectory = "C:/"
#endif
            };

            using var process = new Process { StartInfo = processStartInfo };
            process.Start();
            process.WaitForExit();

            return process.StandardOutput.ReadToEnd();
        }

        private ICSharpierProcess SetupCSharpierProcess()
        {
            try
            {
                var version = this.ExecuteCommand("dotnet", this.csharpierPath + " --version");
                this.logger.Log("CSharpier version: " + version);
                if (string.IsNullOrEmpty(version))
                {
                    this.DisplayInstallNeededMessage();
                }
                else
                {
                    var installedVersion = new Version(version);
                    var pipeFilesVersion = new Version("0.12.0");
                    if (installedVersion.CompareTo(pipeFilesVersion) < 0)
                    {
                        var content =
                            "Please upgrade to CSharpier >= 0.12.0 for bug fixes and improved formatting speed.";
                        InfoBarService.Instance.ShowInfoBar(content);

                        return new CSharpierProcessSingleFile(this.csharpierPath, this.logger);
                    }
                    return new CSharpierProcessPipeMultipleFiles(this.csharpierPath, this.logger);
                }
            }
            catch (Exception ex)
            {
                this.logger.Log(ex);
            }

            return new NullCSharpierProcess();
        }

        private void DisplayInstallNeededMessage()
        {
            InfoBarService.Instance.ShowInfoBar(
                "CSharpier must be installed globally to support formatting."
            );
        }

        public bool CanFormat => this.csharpierProcess.CanFormat;

        public string Format(string content, string filePath)
        {
            if (!this.csharpierProcess.CanFormat)
            {
                return null;
            }

            try
            {
                var result = this.csharpierProcess.FormatFile(content, filePath);
                return result;
            }
            catch (Exception ex)
            {
                this.logger.Log(ex);
            }

            return null;
        }
    }
}
