using System.Net.NetworkInformation;
using CSharpier.Proto;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

using System.IO.Abstractions;
using CSharpier.Cli.Options;

public static class GrpcFormatter
{
    public static Task<int> StartServer(
        int? port,
        ConsoleLogger logger,
        string? actualConfigPath,
        CancellationToken cancellationToken
    )
    {
        var thePort = port ?? FindFreePort();
        var server = new Server
        {
            Services =
            {
                CSharpierService.BindService(
                    new CSharpierServiceImplementation(actualConfigPath, logger)
                )
            },
            Ports = { new ServerPort("localhost", thePort, ServerCredentials.Insecure) }
        };
        server.Start();

        logger.LogInformation("Started on " + thePort);
        Console.ReadKey();

        return Task.FromResult(0);
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

public class CSharpierServiceImplementation : CSharpierService.CSharpierServiceBase
{
    private readonly string? configPath;
    private readonly IFileSystem fileSystem;
    private readonly ConsoleLogger logger;

    public CSharpierServiceImplementation(string? configPath, ConsoleLogger logger)
    {
        this.configPath = configPath;
        this.logger = logger;
        this.fileSystem = new FileSystem();
    }

    public override async Task<FormatFileResult> FormatFile(
        FormatFileDto formatFileDto,
        ServerCallContext context
    )
    {
        try
        {
            var directoryName = this.fileSystem.Path.GetDirectoryName(formatFileDto.FileName);
            if (directoryName == null)
            {
                // TODO proto we can probably still make this work, and just use default options
                throw new Exception(
                    $"There was no directory found for file {formatFileDto.FileName}"
                );
            }

            var optionsProvider = await OptionsProvider.Create(
                directoryName,
                this.configPath,
                this.fileSystem,
                this.logger,
                context.CancellationToken
            );

            if (
                GeneratedCodeUtilities.IsGeneratedCodeFile(formatFileDto.FileName)
                || optionsProvider.IsIgnored(formatFileDto.FileName)
            )
            {
                // TODO proto should we send back that this is ignored?
                return new FormatFileResult();
            }

            var result = await CSharpFormatter.FormatAsync(
                formatFileDto.FileContents,
                optionsProvider.GetPrinterOptionsFor(formatFileDto.FileName),
                context.CancellationToken
            );

            // TODO proto what about checking if this actually formatted?
            // could send back any error messages now
            return new FormatFileResult { FormattedFile = result.Code };
        }
        catch (Exception ex)
        {
            // TODO proto should this return this as an error?
            DebugLogger.Log(ex.ToString());
            return new FormatFileResult();
        }
    }
}
