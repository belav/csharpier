using System.Globalization;
using System.Net;
using System.Net.Sockets;
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
            // ProcessRequestAsync yields at its first await, so requests still overlap without the
            // extra thread pool hop Task.Run added
            _ = ProcessRequestAsync(context, service, fileLogger, cancellationToken);
        }
    }

    private static async Task ProcessRequestAsync(
        HttpListenerContext context,
        CSharpierServiceImplementation service,
        ILogger logger,
        CancellationToken cancellationToken
    )
    {
        var response = context.Response;

        try
        {
            var request = context.Request;

            if (request.Url?.AbsolutePath == "/format" && request.HttpMethod == "POST")
            {
                var formatFileDto =
                    await JsonSerializer.DeserializeAsync<FormatFileParameter>(
                        request.InputStream,
                        cancellationToken: cancellationToken
                    ) ?? throw new Exception("No body!");

                var result = await service.FormatFile(formatFileDto, cancellationToken);

                // the length is still sent so that clients keep getting a non chunked response
                var buffer = JsonSerializer.SerializeToUtf8Bytes(result);
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, cancellationToken);
            }
            else
            {
                response.StatusCode = 405;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to handle a request");
        }
        finally
        {
            response.OutputStream.Close();
        }
    }
}
