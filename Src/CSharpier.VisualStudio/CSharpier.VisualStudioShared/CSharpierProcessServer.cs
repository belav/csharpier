using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CSharpier.VisualStudio;
using Newtonsoft.Json;

public class CSharpierProcessServer : ICSharpierProcess2, IDisposable
{
    private readonly string csharpierPath;
    private readonly Logger logger;
    private int port;
    private Process? process;
    public bool ProcessFailedToStart { get; private set; }

    public string Version { get; }

    public CSharpierProcessServer(string csharpierPath, string version, Logger logger)
    {
        this.logger = logger;
        this.csharpierPath = csharpierPath;
        this.Version = version;

        this.StartProcess();

        this.logger.Debug("Warm CSharpier with initial format");
        // warm by formatting a file twice, the 3rd time is when it gets really fast
        this.FormatFile("public class ClassName { }", "/Temp/Test.cs");
        this.FormatFile("public class ClassName { }", "/Temp/Test.cs");
    }

    private void StartProcess()
    {
        try
        {
            var processStartInfo = new ProcessStartInfo(this.csharpierPath, "--server")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Environment = { ["DOTNET_NOLOGO"] = "1" }
            };
            this.process = Process.Start(processStartInfo);

            var output = string.Empty;

            var task = Task.Run(() =>
            {
                output = this.process!.StandardOutput.ReadLine();
            });

            if (!task.Wait(TimeSpan.FromSeconds(2)))
            {
                this.logger.Warn(
                    "Spawning the csharpier server timed out. Formatting cannot occur."
                );
                this.process!.Kill();
                return;
            }

            if (this.process!.HasExited)
            {
                this.logger.Warn(
                    "Spawning the csharpier server failed because it exited. "
                        + this.process!.StandardError.ReadToEnd()
                );
                this.ProcessFailedToStart = true;
                return;
            }

            var portString = output.Replace("Started on ", "");
            this.port = int.Parse(portString);

            this.logger.Debug("Connecting via port " + portString);
        }
        catch (Exception e)
        {
            this.logger.Warn("Failed to spawn the needed csharpier server." + e);
            this.ProcessFailedToStart = true;
        }
    }

    public string FormatFile(string content, string filePath)
    {
        var parameter = new FormatFileParameter { fileName = filePath, fileContents = content };

        var result = this.formatFile(parameter);
        return result?.formattedFile ?? string.Empty;
    }

    public FormatFileResult? formatFile(FormatFileParameter parameter)
    {
        if (this.ProcessFailedToStart)
        {
            this.logger.Warn("CSharpier process failed to start. Formatting cannot occur.");
            return null;
        }

        var url = "http://localhost:" + this.port + "/format";

        try
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(JsonConvert.SerializeObject(parameter));
            }

            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                this.logger.Warn(
                    "Csharpier server returned non-200 status code of " + response.StatusCode
                );
                response.Close();
                return null;
            }

            using (var streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                var result = JsonConvert.DeserializeObject<FormatFileResult>(
                    streamReader.ReadToEnd()
                );
                return result;
            }
        }
        catch (Exception e)
        {
            this.logger.Warn("Failed posting to the csharpier server. " + e);
        }

        return null;
    }

    public void Dispose()
    {
        this.process?.Kill();
    }
}
