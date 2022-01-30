using System.Collections.Generic;
using System.Diagnostics;

namespace CSharpier.VisualStudio
{
    public static class ProcessHelper
    {
        public static string ExecuteCommand(
            string command,
            string arguments,
            Dictionary<string, string>? environmentVariables = null,
            string? workingDirectory = null
        )
        {
            var processStartInfo = new ProcessStartInfo(command, arguments)
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
            };

            if (environmentVariables != null)
            {
                foreach (var keyPair in environmentVariables)
                {
                    processStartInfo.EnvironmentVariables[keyPair.Key] = keyPair.Value;
                }
            }

            if (workingDirectory != null)
            {
                processStartInfo.WorkingDirectory = workingDirectory;
            }

            using var process = new Process { StartInfo = processStartInfo };
            process.Start();
            process.WaitForExit();

            return process.StandardOutput.ReadToEnd();
        }
    }
}
