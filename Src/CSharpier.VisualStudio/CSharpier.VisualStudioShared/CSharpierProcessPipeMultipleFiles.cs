using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Thread = System.Threading.Thread;
using Process = System.Diagnostics.Process;

namespace CSharpier.VisualStudio
{
    public class CSharpierProcessPipeMultipleFiles : ICSharpierProcess
    {
        private readonly Logger logger;
        private readonly Process process;
        private bool done;

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
                UseShellExecute = false,
                CreateNoWindow = true
            };
            this.process = new Process { StartInfo = processStartInfo };
            process.Start();

            this.FormatFile("public class ClassName { }", "Test.cs");
        }

        public bool CanFormat => true;

        public string FormatFile(string content, string filePath)
        {
            this.logger.Log("Formatting " + filePath);
            var stopwatch = Stopwatch.StartNew();

            process.StandardInput.Write(filePath);
            process.StandardInput.Write('\u0003');
            process.StandardInput.Write(content);
            process.StandardInput.Write('\u0003');

            var output = new StringBuilder();
            var errorOutput = new StringBuilder();
            this.done = false;

            var outputReaderThread = CreateReadingThread(process.StandardOutput, output);
            var errorReaderThread = CreateReadingThread(process.StandardError, errorOutput);

            outputReaderThread.Start();
            errorReaderThread.Start();

            while (!this.done)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(1));
            }

            outputReaderThread.Interrupt();
            errorReaderThread.Interrupt();

            var errorResult = errorOutput.ToString();
            var result = output.ToString();
            if (string.IsNullOrEmpty(errorResult))
            {
                if (string.IsNullOrEmpty(result))
                {
                    this.logger.Log("File is ignored by .csharpierignore");
                    return null;
                }
                else
                {
                    this.logger.Log("Formatted in " + stopwatch.ElapsedMilliseconds + "ms");
                    return output.ToString();
                }
            }

            this.logger.Log("Got error output: " + errorResult);
            return null;
        }

        private Thread CreateReadingThread(StreamReader reader, StringBuilder stringBuilder)
        {
            return new Thread(
                () =>
                {
                    try
                    {
                        var nextCharacter = reader.Read();
                        while (nextCharacter != -1)
                        {
                            if (nextCharacter == '\u0003')
                            {
                                done = true;
                                return;
                            }
                            stringBuilder.Append((char)nextCharacter);
                            nextCharacter = reader.Read();
                        }
                    }
                    catch (Exception e)
                    {
                        logger.Log(e);
                        done = true;
                    }
                }
            );
        }
    }
}
