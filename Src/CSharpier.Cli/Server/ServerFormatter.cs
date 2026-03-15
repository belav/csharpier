using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;

namespace CSharpier.Cli.Server;

internal class ServerFormatter
{
    public static async Task<int> StartServer(
        int? port,
        ConsoleLogger logger,
        CancellationToken cancellationToken
    )
    {
        if (port is null or 0)
        {
            var tcp = new TcpListener(IPAddress.Loopback, 0);
            tcp.Start();
            port = ((IPEndPoint)tcp.LocalEndpoint).Port;
            tcp.Stop();
        }

        using var listener = new HttpListener();
        listener.Prefixes.Add($"http://{IPAddress.Loopback}:{port}/");

        listener.Start();

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .SetMinimumLevel(LogLevel.Information)
                .AddFile(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "server{0}.log"),
                    o =>
                    {
                        o.MaxRollingFiles = 1;
                        o.FileSizeLimitBytes = 10000;
                        o.HandleFileError = _ => { };
                        o.FormatLogFileName = name =>
                            string.Format(CultureInfo.InvariantCulture, name, port);
                    }
                );
        });

        logger.LogInformation("Started on " + port);

        var fileLogger = loggerFactory.CreateLogger<ServerFormatter>();
        var service = new CSharpierServiceImplementation(
            // we want any further logging to happen in the file log, not out to the console
            fileLogger
        );

        while (true)
        {
            var context = await listener.GetContextAsync();
            _ = Task.Run(() => ProcessRequestAsync(context, service), cancellationToken);
        }
    }

    private static async Task ProcessRequestAsync(
        HttpListenerContext context,
        CSharpierServiceImplementation service
    )
    {
        var request = context.Request;
        var response = context.Response;

        if (request.Url?.AbsolutePath == "/format" && request.HttpMethod == "POST")
        {
            using var reader = new StreamReader(
                context.Request.InputStream,
                context.Request.ContentEncoding
            );
            var body = await reader.ReadToEndAsync();

            var formatFileDto = JsonSerializer.Deserialize<FormatFileParameter>(body);
            if (formatFileDto is null)
            {
                throw new Exception("No body!");
            }

            var result = await service.FormatFile(formatFileDto, CancellationToken.None);

            response.ContentType = "application/json";

            var buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result));
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer);
        }
        else
        {
            response.StatusCode = 405;
        }

        response.OutputStream.Close();
    }
}
