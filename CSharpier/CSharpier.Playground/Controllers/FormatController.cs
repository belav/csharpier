using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CSharpier.Playground.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FormatController : ControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger logger;
        private readonly PlaygroundOptions options;

        // ReSharper disable once SuggestBaseTypeForParameter
        public FormatController(IWebHostEnvironment webHostEnvironment, ILogger<FormatController> logger, IOptions<PlaygroundOptions> options)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            this.options = options.Value;
        }

        [HttpPost]
        public string Post([FromBody] string content)
        {
            var filePath = Path.Combine(this.webHostEnvironment.ContentRootPath, "App_Data", "Uploads", content.CalculateHash() + ".cs");
            new FileInfo(filePath).EnsureDirectoryExists();

            var formattedFilePath = filePath.Replace(".cs", ".Formatted.cs");

            var output = "";
            
            // TODO 0 we need to report back errors and what not
            // failing to compile/parse with roslyn
            // what about when the prettier plugin fails because of missing node types or other errors?
            System.IO.File.WriteAllText(filePath, content, Encoding.UTF8);
            var workingDirectory = Path.Combine(this.webHostEnvironment.ContentRootPath, this.options.PrettierDirectory);
            output = ExecuteApplication("node", workingDirectory, "./index.js " + filePath);

                // TODO 0 
            // we also want to eventually expose options
            // TODO 0 deploy stuff
            // right now the playground deploys, but it doesn't update anything in the Prettier folder that it uses
            // we should build CSharpier.Parser in release, and copy that in
            // we should build the prettier-plugin-csharpier and copy it in
            // we should copy in the package.json and index.js that we use in that folder, and run npm install

            if (!System.IO.File.Exists(formattedFilePath))
            {
                return output;
            }
            
            return System.IO.File.ReadAllText(formattedFilePath);
        }

        public string ExecuteApplication(string pathToExe, string workingDirectory, string args)
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
            
            this.logger.LogInformation("Output from '" + pathToExe + " " + args + "' was: " + Environment.NewLine + output);

            return output;
        }

    }
}