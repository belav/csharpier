using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Process = System.Diagnostics.Process;

namespace CSharpier.VisualStudio
{
    public class CSharpierProcessPipeMultipleFiles : ICSharpierProcess
    {
        private readonly Logger logger;
        private Process process;
        private readonly string csharpierPath;
        private StreamWriter standardIn;

        public string Version { get; }
        public bool ProcessFailedToStart { get; private set; }

        public CSharpierProcessPipeMultipleFiles(
            string csharpierPath,
            string version,
            Logger logger
        )
        {
            this.logger = logger;
            this.csharpierPath = csharpierPath;
            this.Version = version;

            this.StartProcess();

            this.logger.Debug("Warm CSharpier with initial format");
            // warm by formatting a file twice, the 3rd time is when it gets really fast
            this.FormatFile("public class ClassName { }", "Test.cs");
            this.FormatFile("public class ClassName { }", "Test.cs");
        }

        private void StartProcess()
        {
            try
            {
                var processStartInfo = new ProcessStartInfo(
                    this.csharpierPath,
                    " --pipe-multiple-files"
                )
                {
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    EnvironmentVariables = { ["DOTNET_NOLOGO"] = "1" },
                };
                this.process = new Process { StartInfo = processStartInfo };
                this.process.Start();
                this.standardIn = new StreamWriter(
                    this.process.StandardInput.BaseStream,
                    Encoding.UTF8
                );
            }
            catch (Exception ex)
            {
                this.logger.Warn(
                    "Failed to spawn the needed csharpier process. Formatting cannot occur."
                );
                this.logger.Error(ex);
                this.ProcessFailedToStart = true;
            }
        }

        public string FormatFile(string content, string filePath)
        {
            if (this.ProcessFailedToStart)
            {
                this.logger.Warn("CSharpier process failed to start. Formatting cannot occur.");
                return "";
            }

            var stringBuilder = new StringBuilder();

            var task = Task.Run(() =>
            {
                this.standardIn.Write(filePath);
                this.standardIn.Write('\u0003');
                this.standardIn.Write(content);
                this.standardIn.Write('\u0003');
                this.standardIn.Flush();

                var nextCharacter = this.process.StandardOutput.Read();
                while (nextCharacter != -1)
                {
                    if (nextCharacter == '\u0003')
                    {
                        break;
                    }

                    stringBuilder.Append((char)nextCharacter);
                    nextCharacter = this.process.StandardOutput.Read();
                }
            });

            // csharpier will freeze in some instances when "Format on Save" is also installed and the file has compilation errors
            // this detects that and recovers from it
            if (!task.Wait(TimeSpan.FromSeconds(3)))
            {
                this.logger.Warn("CSharpier process appears to be hung, restarting it.");
                this.process.Kill();
                this.StartProcess();
                return string.Empty;
            }

            var result = stringBuilder.ToString();

            if (string.IsNullOrEmpty(result))
            {
                this.logger.Debug("File is ignored by .csharpierignore or there was an error");
            }

            return result;
        }

        public void Dispose()
        {
            this.process.Kill();
        }
    }
}
