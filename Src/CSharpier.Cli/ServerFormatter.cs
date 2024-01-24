namespace CSharpier.Cli;

using System.IO.Abstractions;
using System.Net;
using System.Net.NetworkInformation;
using CSharpier.Cli.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

public static class ServerFormatter
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
                serverOptions.Listen(IPAddress.Any, thePort);
            }
        );
        var app = builder.Build();
        var service = new CSharpierServiceImplementation(actualConfigPath, logger);
        app.MapPost(
            "/format",
            async (FormatFileDto formatFileDto, CancellationToken cancellationToken) =>
                await service.FormatFile(formatFileDto, cancellationToken)
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

public class FormatFileDto
{
    public required string FileContents { get; set; }
    public required string FileName { get; set; }
}

public class FormatFileResult
{
    public string? FormattedFile { get; set; }
}

public class CSharpierServiceImplementation
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

    public async Task<FormatFileResult> FormatFile(
        FormatFileDto formatFileDto,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var directoryName = this.fileSystem.Path.GetDirectoryName(formatFileDto.FileName);
            DebugLogger.Log(directoryName ?? string.Empty);
            if (directoryName == null)
            {
                // TODO server we can probably still make this work, and just use default options
                throw new Exception(
                    $"There was no directory found for file {formatFileDto.FileName}"
                );
            }

            var optionsProvider = await OptionsProvider.Create(
                directoryName,
                this.configPath,
                this.fileSystem,
                this.logger,
                cancellationToken
            );

            if (
                GeneratedCodeUtilities.IsGeneratedCodeFile(formatFileDto.FileName)
                || optionsProvider.IsIgnored(formatFileDto.FileName)
            )
            {
                // TODO server should we send back that this is ignored?
                return new FormatFileResult();
            }

            var result = await CSharpFormatter.FormatAsync(
                formatFileDto.FileContents,
                optionsProvider.GetPrinterOptionsFor(formatFileDto.FileName),
                cancellationToken
            );

            // TODO server what about checking if this actually formatted?
            // could send back any error messages now
            return new FormatFileResult { FormattedFile = result.Code };
        }
        catch (Exception ex)
        {
            // TODO server should this return this as an error?
            DebugLogger.Log(ex.ToString());
            return new FormatFileResult();
        }
    }
}
