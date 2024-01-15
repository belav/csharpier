using System.Diagnostics;
using CSharpier.Proto;
using Grpc.Core;
using Process = System.Diagnostics.Process;

namespace CSharpier.VisualStudio
{
    public class CSharpierProcessGrpc : ICSharpierProcess
    {
        private readonly Logger logger;
        private Process? process;
        private readonly string csharpierPath;
        private CSharpierService.CSharpierServiceClient? client;

        public CSharpierProcessGrpc(string csharpierPath, Logger logger)
        {
            this.logger = logger;
            this.csharpierPath = csharpierPath;

            this.StartProcess();
        }

        private void StartProcess()
        {
            var processStartInfo = new ProcessStartInfo(this.csharpierPath, " --grpc")
            {
                UseShellExecute = false,
                ErrorDialog = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                EnvironmentVariables = { ["DOTNET_NOLOGO"] = "1" },
            };
            this.process = new Process { StartInfo = processStartInfo };
            this.process.Start();

            var output = this.process.StandardOutput.ReadLine() ?? string.Empty;
            var portString = output.Replace("Started on ", string.Empty);
            if (!int.TryParse(portString, out var port))
            {
                this.logger.Warn("Failed to parse port from csharpier process " + output);
                return;
            }

            this.logger.Debug("Connecting via port " + portString);

            var channel = new Channel("localhost", port, ChannelCredentials.Insecure);
            this.client = new CSharpierService.CSharpierServiceClient(channel);

            this.logger.Debug("Warm CSharpier with initial format");
            // warm by formatting a file twice, the 3rd time is when it gets really fast
            this.FormatFile("public class ClassName { }", "Test.cs");
            this.FormatFile("public class ClassName { }", "Test.cs");
        }

        public string FormatFile(string content, string filePath)
        {
            if (this.client == null)
            {
                return string.Empty;
            }

            var data = new FormatFileDto { FileName = filePath, FileContents = content };
            var result = this.client.FormatFile(data);
            return result.FormattedFile;
        }

        public void Dispose()
        {
            this.process?.Kill();
        }
    }
}
