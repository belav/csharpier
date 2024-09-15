namespace CSharpier.Cli.Server;

using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;

internal static class ServerFormatter
{
    public static async Task<int> StartServer(
        int? port,
        ConsoleLogger logger,
        string? actualConfigPath
    )
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.ConfigureKestrel(
            (_, serverOptions) =>
            {
                serverOptions.Listen(IPAddress.Loopback, port ?? 0);
            }
        );
        builder.Logging.ClearProviders();
        var values = new Dictionary<string, string?>
        {
            ["Logging:File:MaxRollingFiles"] = "1",
            ["Logging:File:FileSizeLimitBytes"] = "10000",
        };
        builder.Configuration.AddInMemoryCollection(values);
        var currentPort = port ?? 0;
        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddFile(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "server{0}.log"),
                o =>
                {
                    // name files based on the port so that multiple processes can log without fighting over a file
                    // however before the server is started fully we won't have a port
                    // this empty error handler will make sure if two processes both try to use that initial file
                    // at the same time they won't crash
                    o.HandleFileError = _ => { };
                    o.FormatLogFileName = name =>
                        string.Format(name, currentPort == 0 ? string.Empty : currentPort);
                }
            );
        });

        var app = builder.Build();
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            foreach (
                var address in (app as IApplicationBuilder)
                    .ServerFeatures.Get<IServerAddressesFeature>()
                    ?.Addresses ?? []
            )
            {
                var uri = new Uri(address);
                currentPort = uri.Port;
                logger.LogInformation("Started on " + uri.Port);
            }
        });
        var service = new CSharpierServiceImplementation(
            actualConfigPath,
            // we want any further logging to happen in the file log, not out to the console
            app.Services.GetRequiredService<ILogger<CSharpierServiceImplementation>>()
        );
        app.MapPost(
            "/format",
            (FormatFileParameter formatFileDto, CancellationToken cancellationToken) =>
                service.FormatFile(formatFileDto, cancellationToken)
        );

        await app.RunAsync();

        Console.ReadKey();

        return 0;
    }
}
