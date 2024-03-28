namespace CSharpier.Cli.Server;

using System.Net;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        var thePort = port ?? FindFreePort();
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.ConfigureKestrel(
            (_, serverOptions) =>
            {
                serverOptions.Listen(IPAddress.Loopback, thePort);
            }
        );

        var app = builder.Build();
        var service = new CSharpierServiceImplementation(actualConfigPath, logger);
        app.MapPost(
            "/format",
            (FormatFileParameter formatFileDto, CancellationToken cancellationToken) =>
                service.FormatFile(formatFileDto, cancellationToken)
        );
        logger.LogInformation("Started on " + thePort);

        await app.RunAsync();

        Console.ReadKey();

        return 0;
    }

    public static int FindFreePort()
    {
        const int startPort = 49152;
        const int endPort = 65535;
        var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        var tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
        var ipEndPoint = ipGlobalProperties.GetActiveTcpListeners();

        var usedPorts = ipEndPoint
            .Select(o => o.Port)
            .Concat(tcpConnInfoArray.Select(o => o.LocalEndPoint.Port))
            .ToList();

        for (var i = startPort; i < endPort; i++)
        {
            if (!usedPorts.Contains(i))
            {
                return i;
            }
        }

        throw new InvalidOperationException(
            $"Could not find any free TCP port between ports {startPort}-{endPort}"
        );
    }
}
