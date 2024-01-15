namespace CSharpier.Cli.Tests;

using System.Diagnostics;
using System.Text;
using CSharpier.Proto;
using FluentAssertions;
using Grpc.Core;
using NUnit.Framework;

[TestFixture]
public class GrpcTests
{
    // TODO proto add other tests
    // starting on port
    // ignore file
    // option file
    [Test]
    [Ignore("TODO proto this doesn't seem to work now, plus it needs some clean up")]
    public async Task Stuff()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "dotnet-csharpier.dll");

        var processStartInfo = new ProcessStartInfo("dotnet", $"{path} --grpc")
        {
            UseShellExecute = false,
            ErrorDialog = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            EnvironmentVariables = { ["DOTNET_NOLOGO"] = "1" },
        };

        var process = new Process { StartInfo = processStartInfo };
        process.Start();

        var portString = (await process.StandardOutput.ReadLineAsync() ?? string.Empty).Replace(
            "Started on ",
            string.Empty
        );
        var port = int.Parse(portString);

        // TODO why the shit won't this connect now?
        var channel = new Channel("localhost", port, ChannelCredentials.Insecure);
        var client = new CSharpierService.CSharpierServiceClient(channel);

        var data = new FormatFileDto
        {
            FileName = "test.cs",
            FileContents = "public class TestClass    { }"
        };
        var result = await client.FormatFileAsync(data);

        result.FormattedFile.TrimEnd().Should().Be("public class TestClass { }");
    }
}
