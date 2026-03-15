using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Json;
using AwesomeAssertions;
using CliWrap;
using CliWrap.EventStream;
using CSharpier.Cli.Server;

namespace CSharpierProcess.Tests;

public class ServerTests
{
    private static readonly HttpClient httpClient = new();

    // TODO server add other tests
    // ignore file
    // option file

    [Test]
    public async Task Server_Should_Use_Empty_Port()
    {
        var data = new FormatFileParameter
        {
            fileName = "/tmp/test.cs",
            fileContents = "public class TestClass    { }",
        };

        var result = await RunServer(data);

        result.errorMessage.Should().BeNullOrEmpty();
        result.status.Should().Be(Status.Formatted);
        result.formattedFile!.TrimEnd().Should().Be("public class TestClass { }");
    }

    [Test]
    public async Task Server_Should_Use_Defined_Port()
    {
        var data = new FormatFileParameter
        {
            fileName = "/tmp/test.cs",
            fileContents = "public class TestClass    { }",
        };

        var result = await RunServer(data, "--server-port 51123");

        result.errorMessage.Should().BeNullOrEmpty();
        result.status.Should().Be(Status.Formatted);
        result.formattedFile!.TrimEnd().Should().Be("public class TestClass { }");
    }

    private static async Task<FormatFileResult> RunServer(
        FormatFileParameter data,
        string? arguments = null
    )
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "CSharpier.dll");

        var processStartInfo = new ProcessStartInfo("dotnet", $"{path} server {arguments}")
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
            var port = int.Parse(portString, CultureInfo.InvariantCulture);

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

            return result!;
        }
        finally
        {
            process.Kill();
        }
    }

    [Test]
    [Skip("this doesn't kill the processes after it starts them")]
    public void RunTwo()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "CSharpier.dll");

        async Task NewFunction()
        {
            var command = Cli.Wrap("dotnet")
                .WithArguments(path + " server")
                .WithValidation(CommandResultValidation.None);

            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(15));

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

        var tasks = new[] { NewFunction(), NewFunction(), Task.Delay(TimeSpan.FromSeconds(10)) };

        var taskIndex = Task.WaitAny(tasks);

        if (taskIndex >= 0 && tasks[taskIndex].Exception is { } exception)
        {
            throw exception;
        }
    }
}
