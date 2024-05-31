namespace CSharpier.Cli.Server;

using System.Net;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Logging;

internal static class ServerFormatter
{
    public static async Task<int> StartServer(
        int? port,
        ConsoleLogger logger,
        string? actualConfigPath,
        CancellationToken cancellationToken
    )
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.ConfigureKestrel(
            (_, serverOptions) =>
            {
                serverOptions.Listen(IPAddress.Loopback, 0);
            }
        );

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
                logger.LogInformation("Started on " + uri.Port);
            }
        });
        var service = new CSharpierServiceImplementation(actualConfigPath, logger);
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
