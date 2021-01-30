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

            // TODO 0 we need to report back errors and what not
            if (!System.IO.File.Exists(formattedFilePath))
            {
                System.IO.File.WriteAllText(filePath, content, Encoding.UTF8);

                // TODO  the node thing needs to build into here? It probably makes sense for this to make node build into here. Meaning the publish of this kicks off the build steps in the plugin that get files into the Prettier directory
                var workingDirectory = Path.Combine(this.webHostEnvironment.ContentRootPath, this.options.PrettierDirectory);
                Console.WriteLine(ExecuteApplication("node", workingDirectory, "./index.js " + filePath));   
            }

            // TODO we need to use node to format this
            // TODO I switch the parser project to .net50 to get it running on the web server, I should rename the folder that it lives in.
            // what if it fails to parse with roslyn?
            // we should also wait until the file is done changing on the client side
            // we should also auto deploy
            // dotnet publish -c release creates a publishable thing for this, but we also need to build the Parser/prettier plugin and deploy those along with it somehow.
            // maybe that is just another step, and we automate this whole thing with github actions, or azure devops.
            return System.IO.File.ReadAllText(filePath.Replace(".cs", ".Formatted.cs"));
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