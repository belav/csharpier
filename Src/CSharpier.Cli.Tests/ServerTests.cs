using System.Diagnostics;
using System.Net.Http.Json;
using CliWrap;
using CliWrap.EventStream;
using CSharpier.Cli.Server;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Cli.Tests;

[TestFixture]
public class ServerTests
{
    private static readonly HttpClient httpClient = new HttpClient();

    // TODO server add other tests
    // starting on port
    // ignore file
    // option file
    [Test]
    [Ignore("Not working on GH, test locally on linux?")]
    public async Task Stuff()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "dotnet-csharpier.dll");

        var processStartInfo = new ProcessStartInfo("dotnet", $"{path} --server")
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
        try
        {
            process.Start();

            var portString = (await process.StandardOutput.ReadLineAsync() ?? string.Empty).Replace(
                "Started on ",
                string.Empty
            );
            var port = int.Parse(portString);
            var data = new FormatFileParameter
            {
                fileName = "/Temp/test.cs",
                fileContents = "public class TestClass    { }",
            };

            var response = await httpClient.PostAsJsonAsync(
                $"http://127.0.0.1:{port}/format",
                data
            );
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<FormatFileResult>();
            if (result == null)
            {
                Assert.Fail("Result is null");
            }

            result!.status.Should().Be(Status.Formatted);
            result!.formattedFile!.TrimEnd().Should().Be("public class TestClass { }");
        }
        finally
        {
            process.Kill();
        }
    }

    [Test]
    [Ignore("leaves things running when it fails and probably won't work on GH")]
    public void RunTwo()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "dotnet-csharpier.dll");

        async Task NewFunction()
        {
            var command = CliWrap
                .Cli.Wrap("dotnet")
                .WithArguments(path + " --server")
                .WithValidation(CommandResultValidation.None);

            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));

            await foreach (var cmdEvent in command.ListenAsync(cancellationToken: cts.Token))
            {
                switch (cmdEvent)
                {
                    case StartedCommandEvent started:
                        Console.WriteLine($"Process started; ID: {started.ProcessId}");
                        break;
                    case StandardOutputCommandEvent stdOut:
                        Console.WriteLine($"Out> {stdOut.Text}");
                        break;
                    case StandardErrorCommandEvent stdErr:
                        throw new Exception(stdErr.Text);
                    case ExitedCommandEvent exited:
                        Console.WriteLine($"Process exited; Code: {exited.ExitCode}");
                        break;
                }
            }
        }

        Task.WaitAll(NewFunction(), NewFunction());
    }
}
