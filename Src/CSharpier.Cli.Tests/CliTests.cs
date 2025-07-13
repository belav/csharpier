using System.Diagnostics;
using System.Text;
using CliWrap;
using CliWrap.Buffered;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Cli.Tests;

// these tests are kind of nice as c# because they run in the same place.
// except the one test that has issues with console input redirection
// they used to be powershell, but doing the multiple file thing didn't work
// that worked in by writing js, but that felt worse than powershell
// the CSharpierProcess abstraction is also a little fragile, but makes for clean tests when they
// are written properly
public class CliTests
{
    private static readonly string testFileDirectory = Directory
        .CreateTempSubdirectory("CsharpierTestFies")
        .FullName;

    [SetUp]
    public void BeforeEachTest()
    {
        if (File.Exists(FormattingCacheFactory.CacheFilePath))
        {
            File.Delete(FormattingCacheFactory.CacheFilePath);
        }

        Directory.CreateDirectory(testFileDirectory);
    }

    [TearDown]
    public void AfterEachTest()
    {
        static void DeleteDirectory()
        {
            if (Directory.Exists(testFileDirectory))
            {
                Directory.Delete(testFileDirectory, true);
            }
        }

        try
        {
            DeleteDirectory();
        }
        catch (Exception)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            DeleteDirectory();
        }
    }

    [TestCase("\n")]
    [TestCase("\r\n")]
    public async Task Format_Should_Format_Basic_File(string lineEnding)
    {
        var formattedContent = "public class ClassName { }" + lineEnding;
        var unformattedContent = $"public class ClassName {{{lineEnding}{lineEnding}}}";

        await WriteFileAsync("BasicFile.cs", unformattedContent);

        var result = await new CsharpierProcess()
            .WithArguments("format BasicFile.cs")
            .ExecuteAsync();

        result.ErrorOutput.Should().BeNullOrEmpty();
        result.Output.Should().StartWith("Formatted 1 files in ");
        result.ExitCode.Should().Be(0);
        (await ReadAllTextAsync("BasicFile.cs")).Should().Be(formattedContent);
    }

    [TestCase("Subdirectory")]
    [TestCase("./Subdirectory")]
    public async Task Format_Should_Format_Subdirectory(string subdirectory)
    {
        var formattedContent = "public class ClassName { }\n";
        var unformattedContent = "public class ClassName {\n\n}";

        await WriteFileAsync("Subdirectory/BasicFile.cs", unformattedContent);

        var result = await new CsharpierProcess()
            .WithArguments($"format {subdirectory}")
            .ExecuteAsync();

        result.Output.Should().StartWith("Formatted 1 files in ");
        result.ExitCode.Should().Be(0);
        (await ReadAllTextAsync("Subdirectory/BasicFile.cs")).Should().Be(formattedContent);
    }

    [Test]
    public async Task Format_Should_Respect_Ignore_File_With_Subdirectory_When_DirectorOrFile_Is_Dot()
    {
        var unformattedContent = "public class Unformatted {     }";
        var filePath = "Subdirectory/IgnoredFile.cs";
        await WriteFileAsync(filePath, unformattedContent);
        await WriteFileAsync(".csharpierignore", filePath);

        await new CsharpierProcess().WithArguments("format .").ExecuteAsync();
        var fileContents = await ReadAllTextAsync(filePath);

        fileContents
            .Should()
            .Be(unformattedContent, $"The file at {filePath} should have been ignored");
    }

    [Test]
    public async Task Format_Should_Respect_Ignore_Path()
    {
        var unformattedContent = "public class Unformatted {     }";
        var filePath = "IgnoredFile.cs";
        await WriteFileAsync(filePath, unformattedContent);
        await WriteFileAsync(".config/.csharpierignore", filePath);

        await new CsharpierProcess()
            .WithArguments("format . --ignore-path ./config/.csharpierignore")
            .ExecuteAsync();
        var fileContents = await ReadAllTextAsync(filePath);

        fileContents
            .Should()
            .Be(unformattedContent, $"The file at {filePath} should have been ignored");
    }

    [Test]
    public async Task Check_Should_Respect_Ignore_Path()
    {
        var unformattedContent = "public class Unformatted {     }";
        var filePath = "IgnoredFile.cs";
        await WriteFileAsync(filePath, unformattedContent);
        await WriteFileAsync(".config/.csharpierignore", filePath);

        var result = await new CsharpierProcess()
            .WithArguments("check . --ignore-path .config/.csharpierignore")
            .ExecuteAsync();
        result.ExitCode.Should().Be(0);
    }

    [TestCase(".git")]
    [TestCase("subdirectory/.git")]
    [TestCase("node_modules")]
    [TestCase("subdirectory/node_modules")]
    [TestCase("obj")]
    [TestCase("subdirectory/obj")]
    public async Task Should_Ignore_Special_Case_Files(string path)
    {
        var unformattedContent = "public class Unformatted {     }";
        var filePath = $"{path}/IgnoredFile.cs";
        await WriteFileAsync(filePath, unformattedContent);

        await new CsharpierProcess().WithArguments(".").ExecuteAsync();
        var result = await ReadAllTextAsync(filePath);

        result.Should().Be(unformattedContent, $"The file at {filePath} should have been ignored");
    }

    [Test]
    public async Task Format_Should_Support_Config_Path()
    {
        const string fileContent = "var myVariable = someLongValue;";
        var fileName = "TooWide.cs";
        await WriteFileAsync(fileName, fileContent);
        await WriteFileAsync("config/.csharpierrc", "printWidth: 10");

        await new CsharpierProcess()
            .WithArguments("format --config-path config/.csharpierrc . ")
            .ExecuteAsync();

        var result = await ReadAllTextAsync(fileName);

        result.Should().Be("var myVariable =\n    someLongValue;\n");
    }

    [Test]
    public async Task Should_Support_Config_Path_With_Editorconfig()
    {
        const string fileContent = "var myVariable = someLongValue;";
        var fileName = "TooWide.cs";
        await WriteFileAsync(fileName, fileContent);
        await WriteFileAsync(
            "config/.editorconfig",
            """
            [*]
            max_line_length = 10
            """
        );

        await new CsharpierProcess()
            .WithArguments("format --config-path config/.editorconfig . ")
            .ExecuteAsync();

        var result = await ReadAllTextAsync(fileName);

        result.Should().Be("var myVariable =\n    someLongValue;\n");
    }

    [Test]
    public async Task Check_Should_Support_Config_Path()
    {
        const string fileContent = "var myVariable = someLongValue;\n";
        var fileName = "TooWide.cs";
        await WriteFileAsync(fileName, fileContent);
        await WriteFileAsync("config/.csharpierrc", "printWidth: 10");

        var result = await new CsharpierProcess()
            .WithArguments("check --config-path config/.csharpierrc . ")
            .ExecuteAsync();

        result.ExitCode.Should().Be(1);
        result
            .ErrorOutput.Replace('\\', '/')
            .Should()
            .StartWith("Error ./TooWide.cs - Was not formatted.");
    }

    [Test]
    public async Task Format_Should_Return_Error_When_No_DirectoryOrFile_And_Not_Piping_StdIn()
    {
        if (Console.IsInputRedirected)
        {
            Assert.Ignore(
                "This test cannot run if Console.IsInputRedirected is true. Running it from the command line is required. See https://github.com/dotnet/runtime/issues/1147\""
            );
        }

        // Console.IsInputRedirected is always true when commands are
        // executed via CliWrap. This is because CliWrap initializes ProcessStartInfo with
        // the parameter `RedirectStandardInput = true`, which interferes
        // with this test.
        var startInfo = new ProcessStartInfo("dotnet")
        {
            ArgumentList =
            {
                Path.Combine(Directory.GetCurrentDirectory(), "dotnet-csharpier.dll"),
                "format",
            },
            RedirectStandardInput = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        using var process = Process.Start(startInfo) ?? throw new InvalidOperationException();
        await process.WaitForExitAsync();
        var errorOutput = await process.StandardError.ReadToEndAsync();

        process.ExitCode.Should().Be(1);
        errorOutput
            .Should()
            .Contain("directoryOrFile is required when not piping stdin to CSharpier");
    }

    [TestCase("\n")]
    [TestCase("\r\n")]
    public async Task Format_Should_Format_Piped_File(string lineEnding)
    {
        var formattedContent1 = "public class ClassName1 { }" + lineEnding;
        var unformattedContent1 = $"public class ClassName1 {{{lineEnding}{lineEnding}}}";

        var result = await new CsharpierProcess()
            .WithArguments("format")
            .WithPipedInput(unformattedContent1)
            .ExecuteAsync();

        result.Output.Should().Be(formattedContent1);
        result.ExitCode.Should().Be(0);
    }

    [Test]
    public async Task Format_Should_Format_Piped_File_With_Config()
    {
        await WriteFileAsync(".csharpierrc", "printWidth: 10");

        var formattedContent1 = "var x =\n    _________________longName;\n";
        var unformattedContent1 = "var x = _________________longName;\n";

        var result = await new CsharpierProcess()
            .WithArguments("format")
            .WithPipedInput(unformattedContent1)
            .ExecuteAsync();

        result.Output.Should().Be(formattedContent1);
        result.ExitCode.Should().Be(0);
    }

    [Test]
    public async Task Format_Should_Format_Piped_File_With_EditorConfig()
    {
        await WriteFileAsync(
            ".editorconfig",
            @"[*]
max_line_length = 10"
        );

        var formattedContent1 = "var x =\n    _________________longName;\n";
        var unformattedContent1 = "var x = _________________longName;\n";

        var result = await new CsharpierProcess()
            .WithArguments("format")
            .WithPipedInput(unformattedContent1)
            .ExecuteAsync();

        result.Output.Should().Be(formattedContent1);
        result.ExitCode.Should().Be(0);
    }

    [Test]
    public async Task Format_Should_Handle_Unicode()
    {
        // use the \u so that we don't accidentally reformat this to be '?'
        var unicodeContent = $"var test = '{'\u3002'}';\n";

        var result = await new CsharpierProcess()
            .WithArguments("format")
            .WithPipedInput(unicodeContent)
            .ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.Output.Should().Be(unicodeContent);
        result.ExitCode.Should().Be(0);
    }

    [TestCase("BasicFile.cs")]
    [TestCase("./BasicFile.cs")]
    [TestCase("/BasicFile.cs")]
    public async Task Format_Should_Print_NotFound(string path)
    {
        var result = await new CsharpierProcess().WithArguments($"format {path}").ExecuteAsync();

        result.Output.Should().BeEmpty();
        result.ErrorOutput.Should().StartWith("There was no file or directory found at " + path);
        result.ExitCode.Should().Be(1);
    }

    [TestCase("BasicFile.cs")]
    [TestCase("./BasicFile.cs")]
    [TestCase("/BasicFile.cs")]
    public async Task Check_Should_Print_NotFound(string path)
    {
        var result = await new CsharpierProcess().WithArguments($"check {path}").ExecuteAsync();

        result.Output.Should().BeEmpty();
        result.ErrorOutput.Should().StartWith("There was no file or directory found at " + path);
        result.ExitCode.Should().Be(1);
    }

    [Test]
    public async Task Format_Should_Write_To_StdError_For_Piped_Invalid_File()
    {
        const string invalidFile = "public class ClassName { ";

        var result = await new CsharpierProcess()
            .WithArguments("format")
            .WithPipedInput(invalidFile)
            .ExecuteAsync();

        result.Output.Should().BeEmpty();
        result.ExitCode.Should().Be(1);
        result.ErrorOutput.Should().Contain("Failed to compile so was not formatted");
    }

    [Test]
    public async Task Check_Should_Write_To_StdError_For_Piped_Invalid_File()
    {
        const string invalidFile = "public class ClassName { ";

        var result = await new CsharpierProcess()
            .WithArguments("check")
            .WithPipedInput(invalidFile)
            .ExecuteAsync();

        result.Output.Should().BeEmpty();
        result.ExitCode.Should().Be(1);
        result.ErrorOutput.Should().Contain("Failed to compile so was not formatted");
    }

    [Test]
    public async Task Check_Should_Write_Unformatted_File()
    {
        var unformattedContent = "public class ClassName1 {\n\n}";

        await WriteFileAsync("CheckUnformatted.cs", unformattedContent);

        var result = await new CsharpierProcess()
            .WithArguments("check CheckUnformatted.cs")
            .ExecuteAsync();

        result
            .ErrorOutput.Replace('\\', '/')
            .Should()
            .StartWith("Error ./CheckUnformatted.cs - Was not formatted.");
        result.ExitCode.Should().Be(1);
    }

    // TODO overrides tests for piping files
    [TestCase("\n")]
    [TestCase("\r\n")]
    public async Task PipeFiles_Should_Format_Multiple_Piped_Files(string lineEnding)
    {
        var formattedContent1 = "public class ClassName1 { }" + lineEnding;
        var formattedContent2 = "public class ClassName2 { }" + lineEnding;
        var unformattedContent1 = $"public class ClassName1 {{{lineEnding}{lineEnding}}}";
        var unformattedContent2 = $"public class ClassName2 {{{lineEnding}{lineEnding}}}";

        var input =
            $"Test1.cs{'\u0003'}{unformattedContent1}{'\u0003'}"
            + $"Test2.cs{'\u0003'}{unformattedContent2}{'\u0003'}";

        var result = await new CsharpierProcess()
            .WithArguments("pipe-files")
            .WithPipedInput(input)
            .ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.ExitCode.Should().Be(0);
        var results = result.Output.Split('\u0003');
        results[0].Should().Be(formattedContent1);
        results[1].Should().Be(formattedContent2);
    }

    [TestCase("InvalidFile.cs", "./InvalidFile.cs")]
    [TestCase("./InvalidFile.cs", "./InvalidFile.cs")]
    public async Task PipeFiles_Should_Write_Error_With_Multiple_Piped_Files(
        string input,
        string output
    )
    {
        const string invalidFile = "public class ClassName { ";

        var result = await new CsharpierProcess()
            .WithArguments("pipe-files")
            .WithPipedInput($"{input}{'\u0003'}{invalidFile}{'\u0003'}")
            .ExecuteAsync();

        result
            .ErrorOutput.Should()
            .StartWith(
                $"Error {output} - Failed to compile so was not formatted.{Environment.NewLine}  (1,26): error CS1513: }}"
            );
        result.ExitCode.Should().Be(1);
    }

    [Test]
    public async Task PipeFiles_Should_Ignore_Piped_File_With_Multiple_Piped_Files()
    {
        const string ignoredFile = "public class ClassName {     }";
        var fileName = Path.Combine(testFileDirectory, "Ignored.cs");
        await WriteFileAsync(".csharpierignore", "Ignored.cs");

        var result = await new CsharpierProcess()
            .WithArguments("pipe-files")
            .WithPipedInput($"{fileName}{'\u0003'}{ignoredFile}{'\u0003'}")
            .ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.Output.TrimEnd('\u0003').Should().BeEmpty();
    }

    [Test]
    public async Task PipeFiles_Should_Support_Config_With_Multiple_Piped_Files()
    {
        const string fileContent = "var myVariable = someLongValue;";
        var fileName = Path.Combine(testFileDirectory, "TooWide.cs");
        await WriteFileAsync(".csharpierrc", "printWidth: 10");

        var result = await new CsharpierProcess()
            .WithArguments("pipe-files")
            .WithPipedInput($"{fileName}{'\u0003'}{fileContent}{'\u0003'}")
            .ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.Output.TrimEnd('\u0003').Should().Be("var myVariable =\n    someLongValue;\n");
    }

    [Test]
    public async Task PipeFiles_Should_Support_Override_Config_With_Multiple_Piped_Files()
    {
        const string fileContent = "var myVariable = someLongValue;";
        var fileName = Path.Combine(testFileDirectory, "TooWide.cst");
        await WriteFileAsync(
            ".csharpierrc",
            """
            overrides:
              - files: "*.cst"
                formatter: "csharp"
                printWidth: 10
            """
        );

        var result = await new CsharpierProcess()
            .WithArguments("pipe-files")
            .WithPipedInput($"{fileName}{'\u0003'}{fileContent}{'\u0003'}")
            .ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.Output.TrimEnd('\u0003').Should().Be("var myVariable =\n    someLongValue;\n");
    }

    [Test]
    public async Task Format_Should_Not_Fail_On_Empty_File()
    {
        await WriteFileAsync("BasicFile.cs", "");

        var result = await new CsharpierProcess().WithArguments("format .").ExecuteAsync();

        result.Output.Should().StartWith("Formatted 0 files in ");
        result.ErrorOutput.Should().BeEmpty();
        result.ExitCode.Should().Be(0);
    }

    [Test]
    public async Task Format_Should_Not_Fail_On_Bad_Csproj()
    {
        await WriteFileAsync("Empty.csproj", "");

        var result = await new CsharpierProcess().WithArguments("format .").ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.ExitCode.Should().Be(0);
        result.Output.Should().StartWith("Warning The csproj at ");
    }

    [Test]
    public async Task Format_Should_Not_Fail_On_Mismatched_MSBuild_With_No_Check()
    {
        await WriteFileAsync(
            "Test.csproj",
            @"<Project Sdk=""Microsoft.NET.Sdk"">
    <ItemGroup>
        <PackageReference Include=""CSharpier.MsBuild"" Version=""99"" />
    </ItemGroup>
</Project>"
        );

        var result = await new CsharpierProcess()
            .WithArguments("format --no-msbuild-check .")
            .ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.ExitCode.Should().Be(0);
        result.Output.Should().StartWith("Formatted 1 files in ");
    }

    [Test]
    public async Task Check_Should_Not_Fail_On_Mismatched_MSBuild_With_No_Check()
    {
        await WriteFileAsync(
            "Test.csproj",
            """
            <Project Sdk="Microsoft.NET.Sdk">
              <ItemGroup>
                <PackageReference Include="CSharpier.MsBuild" Version="99" />
              </ItemGroup>
            </Project>

            """
        );

        var result = await new CsharpierProcess()
            .WithArguments("check --no-msbuild-check .")
            .ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.ExitCode.Should().Be(0);
        result.Output.Should().StartWith("Checked 1 files in ");
    }

    [Test]
    public async Task Format_Should_Fail_On_Mismatched_MSBuild()
    {
        await WriteFileAsync(
            "Test.csproj",
            @"<Project Sdk=""Microsoft.NET.Sdk"">
    <ItemGroup>
        <PackageReference Include=""CSharpier.MsBuild"" Version=""99"" />
    </ItemGroup>
</Project>"
        );

        var result = await new CsharpierProcess().WithArguments("format .").ExecuteAsync();

        result
            .ErrorOutput.Should()
            .Contain("uses version 99 of CSharpier.MsBuild which is a mismatch with version");
        result.ExitCode.Should().Be(1);
    }

    [Test]
    public async Task Check_Should_Fail_On_Mismatched_MSBuild()
    {
        await WriteFileAsync(
            "Test.csproj",
            @"<Project Sdk=""Microsoft.NET.Sdk"">
    <ItemGroup>
        <PackageReference Include=""CSharpier.MsBuild"" Version=""99"" />
    </ItemGroup>
</Project>"
        );

        var result = await new CsharpierProcess().WithArguments("check .").ExecuteAsync();

        result
            .ErrorOutput.Should()
            .Contain("uses version 99 of CSharpier.MsBuild which is a mismatch with version");
        result.ExitCode.Should().Be(1);
    }

    [Test]
    public async Task Format_Should_Cache_And_Validate_Too_Many_Things()
    {
        var unformattedContent = "public class ClassName {     }\n";
        var formattedContent = "public class ClassName { }\n";
        var filePath = "Unformatted.cs";
        await WriteFileAsync(filePath, unformattedContent);

        await new CsharpierProcess().WithArguments("format .").ExecuteAsync();
        var firstModifiedDate = GetLastWriteTime(filePath);
        await new CsharpierProcess().WithArguments("format .").ExecuteAsync();
        var secondModifiedDate = GetLastWriteTime(filePath);
        await WriteFileAsync(filePath, unformattedContent);
        await new CsharpierProcess().WithArguments("format .").ExecuteAsync();
        var thirdModifiedDate = GetLastWriteTime(filePath);

        // I don't know that this exactly validates caching, because I don't think we write out a file unless it changes.
        firstModifiedDate.Should().Be(secondModifiedDate);
        secondModifiedDate.Should().BeBefore(thirdModifiedDate);

        (await ReadAllTextAsync(filePath)).Should().Be(formattedContent);
    }

    [Test]
    public async Task Format_Should_Reformat_When_Options_Change_With_Cache()
    {
        var unformattedContent = "public class ClassName { \n// break\n }\n";

        await WriteFileAsync("Unformatted.cs", unformattedContent);
        await new CsharpierProcess().WithArguments("format .").ExecuteAsync();
        await WriteFileAsync(".csharpierrc", "useTabs: true");
        await new CsharpierProcess().WithArguments("format .").ExecuteAsync();

        var result = await ReadAllTextAsync("Unformatted.cs");
        result.Should().Contain("\n\t// break\n");
    }

    [Test]
    public void Format_Should_Handle_Concurrent_Processes()
    {
        var unformattedContent = "public class ClassName {     }\n";
        var totalFolders = 10;
        var filesPerFolder = 100;
        var folders = new List<string>();
        for (var x = 0; x < totalFolders; x++)
        {
            folders.Add("Folder" + x);
        }

        async Task WriteFiles(string folder)
        {
            for (var y = 0; y < filesPerFolder; y++)
            {
                await WriteFileAsync($"{folder}/File{y}.cs", unformattedContent);
            }
        }

        var tasks = folders.Select(WriteFiles).ToArray();
        Task.WaitAll(tasks);

        async Task FormatFolder(string folder)
        {
            var result = await new CsharpierProcess()
                .WithArguments($"format {folder}")
                .ExecuteAsync();
            result.ErrorOutput.Should().BeEmpty();
        }

        var formatTasks = folders.Select(FormatFolder).ToArray();
        Task.WaitAll(formatTasks);
    }

    [Test]
    [Ignore(
        "This is somewhat useful for testing locally, but doesn't reliably reproduce a problem and takes a while to run. Commenting out the delete cache file line helps to reproduce problems"
    )]
    public async Task Format_Should_Handle_Concurrent_Processes_2()
    {
        var unformattedContent = "public class ClassName {     }\n";
        var filesPerFolder = 1000;

        for (var x = 0; x < filesPerFolder; x++)
        {
            await WriteFileAsync($"{Guid.NewGuid()}.cs", unformattedContent);
        }

        var result = await new CsharpierProcess().WithArguments(".").ExecuteAsync();
        result.ErrorOutput.Should().BeEmpty();

        var newFiles = new List<string>();

        for (var x = 0; x < 100; x++)
        {
            var fileName = Guid.NewGuid() + ".cs";
            await WriteFileAsync(fileName, unformattedContent);
            newFiles.Add(fileName);
        }

        static async Task FormatFile(string file)
        {
            var result = await new CsharpierProcess().WithArguments(file).ExecuteAsync();
            result.ErrorOutput.Should().BeEmpty();
        }

        var formatTasks = newFiles.Select(FormatFile).ToArray();
        Task.WaitAll(formatTasks);
    }

    private static DateTime GetLastWriteTime(string path)
    {
        return File.GetLastWriteTime(Path.Combine(testFileDirectory, path));
    }

    private static async Task WriteFileAsync(string path, string content)
    {
        var fileInfo = new FileInfo(Path.Combine(testFileDirectory, path));
        EnsureExists(fileInfo.Directory!);

        await File.WriteAllTextAsync(fileInfo.FullName, content);
    }

    private static async Task<string> ReadAllTextAsync(string path)
    {
        return await File.ReadAllTextAsync(Path.Combine(testFileDirectory, path));
    }

    private static void EnsureExists(DirectoryInfo directoryInfo)
    {
        if (directoryInfo.Parent != null)
        {
            EnsureExists(directoryInfo.Parent);
        }

        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }
    }

    private sealed class CsharpierProcess
    {
        private readonly StringBuilder output = new();
        private readonly StringBuilder errorOutput = new();
        private Command command;

        private readonly Encoding encoding = Encoding.UTF8;

        public CsharpierProcess()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "CSharpier.dll");

            if (!File.Exists(path))
            {
                throw new Exception("No file found at " + path);
            }

            this.command = CliWrap
                .Cli.Wrap("dotnet")
                .WithArguments(path)
                .WithWorkingDirectory(testFileDirectory)
                .WithValidation(CommandResultValidation.None)
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(this.output, this.encoding))
                .WithStandardErrorPipe(PipeTarget.ToStringBuilder(this.errorOutput, this.encoding));
        }

        public CsharpierProcess WithArguments(string arguments)
        {
            this.command = this.command.WithArguments(this.command.Arguments + " " + arguments);
            return this;
        }

        public CsharpierProcess WithPipedInput(string input)
        {
            this.command = this.command.WithStandardInputPipe(
                PipeSource.FromString(input, this.encoding)
            );

            return this;
        }

        public async Task<ProcessResult> ExecuteAsync()
        {
            var result = await this.command.ExecuteBufferedAsync(this.encoding);
            return new ProcessResult(
                this.output.ToString(),
                this.errorOutput.ToString(),
                result.ExitCode
            );
        }

        public sealed record ProcessResult(string Output, string ErrorOutput, int ExitCode);
    }
}
