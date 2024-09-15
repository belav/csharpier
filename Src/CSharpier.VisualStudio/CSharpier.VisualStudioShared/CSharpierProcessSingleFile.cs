using System.Diagnostics;
using System.Text;

namespace CSharpier.VisualStudio
{
    public class CSharpierProcessSingleFile : ICSharpierProcess
    {
        private readonly string csharpierPath;
        private readonly Logger logger;

        public string Version { get; }
        public bool ProcessFailedToStart => false;

        public CSharpierProcessSingleFile(string csharpierPath, string version, Logger logger)
        {
            this.csharpierPath = csharpierPath;
            this.logger = logger;
            this.Version = version;
        }

        public string FormatFile(string content, string fileName)
        {
            var output = new StringBuilder();
            var errorOutput = new StringBuilder();

            var processStartInfo = new ProcessStartInfo(this.csharpierPath)
            {
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            processStartInfo.EnvironmentVariables["DOTNET_NOLOGO"] = "1";

            using var process = new Process { StartInfo = processStartInfo };
            process.Start();

            process.StandardInput.Write(content);
            process.StandardInput.Close();

            output.Append(process.StandardOutput.ReadToEnd());
            errorOutput.Append(process.StandardError.ReadToEnd());

            process.WaitForExit();

            var result = output.ToString();
            if (
                process.ExitCode == 0
                && !result.Contains("Failed to compile so was not formatted.")
            )
            {
                return result;
            }

            return string.Empty;
        }

        public void Dispose() { }
    }
}
