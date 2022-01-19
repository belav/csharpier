using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Thread = System.Threading.Thread;
using Process = System.Diagnostics.Process;

namespace CSharpier.VisualStudio
{
    // TODO clean this up, and then test the rider one
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

            var processStartInfo = new ProcessStartInfo(
                "dotnet",
                csharpierPath + " --pipe-multiple-files"
            )
            {
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            this.process = new Process { StartInfo = processStartInfo };
            this.process.Start();
            this.standardIn = new StreamWriter(
                this.process.StandardInput.BaseStream,
                Encoding.UTF8
            );

            try
            {
                this.FormatFile("public class ClassName { }", "Test.cs");
            }
            catch (Exception ex)
            {
                this.logger.Log(ex);
            }
        }

        public bool CanFormat => true;

        public string FormatFile(string content, string filePath)
        {
            this.output.Clear();
            this.errorOutput.Clear();

            this.logger.Info("Formatting " + filePath);
            var stopwatch = Stopwatch.StartNew();

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
                    return null;
                }
                else
                {
                    this.logger.Info("Formatted in " + stopwatch.ElapsedMilliseconds + "ms");
                    return this.output.ToString();
                }
            }

            this.logger.Info("Got error output: " + errorResult);
            return null;
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
                this.logger.Log(e);
            }
            finally
            {
                autoResetEvent.Set();
            }
        }
    }
}
