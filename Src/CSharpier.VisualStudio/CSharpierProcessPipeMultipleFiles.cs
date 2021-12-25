using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Thread = System.Threading.Thread;
using Process = System.Diagnostics.Process;

namespace CSharpier.VisualStudio
{
    // TODO what about disposing this?
    public class CSharpierProcessPipeMultipleFiles : ICSharpierProcess
    {
        private readonly Logger logger;
        private readonly Process process;
        private bool done;

        public CSharpierProcessPipeMultipleFiles(string csharpierPath, Logger logger)
        {
            this.logger = logger;
            
            var processStartInfo = new ProcessStartInfo("dotnet", csharpierPath + " --pipe-multiple-files")
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

        public string FormatFile(string content, string fileName)
        {
            process.StandardInput.Write(fileName);
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
            if (string.IsNullOrEmpty(errorResult))
            {
                return output.ToString();
            }
            
            this.logger.Log("Got error output: " + errorResult);
            return "";

        }

        private Thread CreateReadingThread(StreamReader reader, StringBuilder stringBuilder)
        {
            return new Thread(() => {
                try {
                    var nextCharacter = reader.Read();
                    while (nextCharacter != -1) {
                        if (nextCharacter == '\u0003')
                        {
                            done = true;
                            return;
                        }
                        stringBuilder.Append((char) nextCharacter);
                        nextCharacter = reader.Read();
                    }
                } catch (Exception e)
                {
                    // TODO log
                    done = true;
                }
            });
        }
    }
}