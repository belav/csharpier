using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.Text.Editor.OptionsExtensionMethods;
using Process = System.Diagnostics.Process;

namespace CSharpier.VisualStudio
{
    public class CSharpierProcessPipeMultipleFiles : ICSharpierProcess
    {
        private readonly Logger logger;
        private readonly Process process;

        private readonly StreamWriter standardIn;

        public CSharpierProcessPipeMultipleFiles(string csharpierPath, Logger logger)
        {
            this.logger = logger;

            var processStartInfo = new ProcessStartInfo(csharpierPath, " --pipe-multiple-files")
            {
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            processStartInfo.EnvironmentVariables["DOTNET_NOLOGO"] = "1";
            this.process = new Process { StartInfo = processStartInfo };
            this.process.Start();
            this.standardIn = new StreamWriter(
                this.process.StandardInput.BaseStream,
                Encoding.UTF8
            );

            this.logger.Debug("Warm CSharpier with initial format");
            // warm by formatting a file twice, the 3rd time is when it gets really fast
            this.FormatFile("public class ClassName { }", "Test.cs");
            this.FormatFile("public class ClassName { }", "Test.cs");
        }

        public string FormatFile(string content, string filePath)
        {
            this.standardIn.Write(filePath);
            this.standardIn.Write('\u0003');
            this.standardIn.Write(content);
            this.standardIn.Write('\u0003');
            this.standardIn.Flush();

            var stringBuilder = new StringBuilder();

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
