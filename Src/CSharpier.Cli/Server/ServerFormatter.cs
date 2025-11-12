using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;

namespace CSharpier.Cli.Server;

internal static class ServerFormatter
{
    public static async Task<int> StartServer(int? port, ConsoleLogger logger)
    {
        // Editor plugins, like the Rider extension, run the server in the root directory
        // of a solution. Using the root directory as the content root for an ASP.NET application
        // is a bad idea, because the default host setup installs a recursive file system watch on the entire
        // directory. This file system watch does not honor any ignore files and will quickly exhaust OS resource
        // with large solutions/multiple server processes.
        // We thus configure the server to run against a temporary, empty directory instead. This also means that
        // the server won't pick up the default appsettings.json files in the working directory.
        // We probably don't want this anyway because these appsettings could be intended for the user's solution,
        // not for csharpier.
        var emptyContentRoot = Directory.CreateTempSubdirectory("csharpier-empty-content-root");
        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
            Directory.Delete(emptyContentRoot.FullName, true);
        var builder = WebApplication.CreateBuilder(
            new WebApplicationOptions { ContentRootPath = emptyContentRoot.FullName }
        );
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
                        string.Format(
                            CultureInfo.InvariantCulture,
                            name,
                            currentPort == 0 ? string.Empty : currentPort
                        );
                }
            );
        });

        var app = builder.Build();
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            foreach (
                var address in (app as IApplicationBuilder)
                    .ServerFeatures.Get<IServerAddressesFeature>()
                    ?.Addresses
                ?? []
            )
            {
                var uri = new Uri(address);
                currentPort = uri.Port;
                logger.LogInformation("Started on " + uri.Port);
            }
        });
        var service = new CSharpierServiceImplementation(
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
