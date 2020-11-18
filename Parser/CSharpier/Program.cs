using System;
using System.Diagnostics;
using System.IO;

namespace CSharpier
{
    class Program
    {
        static void Main(string[] args)
        {
            var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            var workingDirectory = Path.Combine(directoryInfo.Parent.Parent.Parent.Parent.Parent.FullName, "prettier-plugin-csharpier");
            Console.WriteLine(workingDirectory);
            Console.WriteLine(ExecuteApplication("node", workingDirectory, "./samples/_index.js Class"));
        }
        
        public static string ExecuteApplication(string pathToExe, string workingDirectory, string args)
        {

            var processStartInfo = new ProcessStartInfo(pathToExe, args)
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = workingDirectory,
                CreateNoWindow = true
            };

            var process = Process.Start(processStartInfo);
            var output = process.StandardError.ReadToEnd();
            process.WaitForExit();

            return output;
        }
    }
}