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
    public class FormatResult
    {
        public string Code { get; set; }
        public string Json { get; set; }
    }
    
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
        public FormatResult Post([FromBody] string content)
        {
            var filePath = Path.Combine(this.webHostEnvironment.ContentRootPath, "App_Data", "Uploads", content.CalculateHash() + ".cs");
            new FileInfo(filePath).EnsureDirectoryExists();

            var formattedFilePath = filePath.Replace(".cs", ".Formatted.cs");
            
            // TODO we need to report back errors and what not
            // failing to compile/parse with roslyn
            // what about when the prettier plugin fails because of missing node types or other errors?
            this.WriteAllText(filePath, content);
            var workingDirectory = Path.Combine(this.webHostEnvironment.ContentRootPath, this.options.PrettierDirectory);
            var output = ExecuteApplication("node", workingDirectory, "./index.js " + filePath);

            // TODO we also want to eventually expose options
            // TODO deploy stuff - this is done, but should be redone because .net!
            // right now the playground deploys, but it doesn't update anything in the Prettier folder that it uses
            // we should build CSharpier.Parser in release, and copy that in
            // we should build the prettier-plugin-csharpier and copy it in
            // we should copy in the package.json and index.js that we use in that folder, and run npm install

            var jsonFilePath = filePath.Replace(".cs", ".json");
            var json = this.Exists(jsonFilePath)
                ? this.ReadAllText(jsonFilePath)
                : "";
            
            if (!this.Exists(formattedFilePath))
            {
                return new FormatResult
                {
                    Code = output,
                    Json = json,
                };
            }

            return new FormatResult
            {
                Code = this.ReadAllText(formattedFilePath),
                Json = json,
            };
        }

        private string ReadAllText(string filePath)
        {
            return System.IO.File.ReadAllText(filePath);
        }

        private void WriteAllText(string filePath, string content)
        {
            System.IO.File.WriteAllText(filePath, content, Encoding.UTF8);
        }

        private bool Exists(string filePath)
        {
            return System.IO.File.Exists(filePath);
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