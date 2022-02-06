using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Process = System.Diagnostics.Process;

namespace CSharpier.VisualStudio
{
    public class CSharpierProcessPipeMultipleFiles : ICSharpierProcess
    {
        private readonly Logger logger;
        private readonly Process process;
        private readonly AutoResetEvent autoEvent = new AutoResetEvent(false);
        private readonly StringBuilder output = new StringBuilder();
        private readonly StringBuilder errorOutput = new StringBuilder();
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
            this.output.Clear();
            this.errorOutput.Clear();

            this.standardIn.Write(filePath);
            this.standardIn.Write('\u0003');
            this.standardIn.Write(content);
            this.standardIn.Write('\u0003');
            this.standardIn.Flush();

            ThreadPool.QueueUserWorkItem(this.ReadOutput, this.autoEvent);
            ThreadPool.QueueUserWorkItem(this.ReadError, this.autoEvent);

            this.autoEvent.WaitOne();

            var errorResult = this.errorOutput.ToString();
            var result = this.output.ToString();
            if (string.IsNullOrEmpty(errorResult))
            {
                if (string.IsNullOrEmpty(result))
                {
                    this.logger.Info("File is ignored by .csharpierignore");
                    return string.Empty;
                }
                else
                {
                    return this.output.ToString();
                }
            }

            this.logger.Info("Got error output: " + errorResult);
            return string.Empty;
        }

        private void ReadOutput(object state)
        {
            this.ReadFromProcess(this.process.StandardOutput, this.output, (AutoResetEvent)state);
        }

        private void ReadError(object state)
        {
            this.ReadFromProcess(
                this.process.StandardError,
                this.errorOutput,
                (AutoResetEvent)state
            );
        }

        private void ReadFromProcess(
            StreamReader reader,
            StringBuilder stringBuilder,
            AutoResetEvent autoResetEvent
        )
        {
            try
            {
                var nextCharacter = reader.Read();
                while (nextCharacter != -1)
                {
                    if (nextCharacter == '\u0003')
                    {
                        return;
                    }

                    stringBuilder.Append((char)nextCharacter);
                    nextCharacter = reader.Read();
                }
            }
            catch (Exception e)
            {
                this.logger.Error(e);
            }
            finally
            {
                autoResetEvent.Set();
            }
        }

        public void Dispose()
        {
            this.process.Kill();
        }
    }
}
