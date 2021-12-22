using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading;
using CSharpier.Cli;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class CommandLineFormatterTests
{
    private const string UnformattedClassContent = "public class ClassName { public int Field; }";
    private const string FormattedClassContent =
        "public class ClassName\n{\n    public int Field;\n}\n";

    [Test]
    public void Format_Writes_Failed_To_Compile()
    {
        var context = new TestContext();
        context.WhenAFileExists("Invalid.cs", "asdfasfasdf");

        var result = context.Format();

        result.ErrorLines
            .First()
            .Should()
            .Be("Error /Invalid.cs - Failed to compile so was not formatted.");
    }

    [Test]
    public void Format_Writes_Failed_To_Compile_With_Directory()
    {
        var context = new TestContext();
        context.WhenAFileExists("Directory/Invalid.cs", "asdfasfasdf");

        var result = context.Format();

        result.ErrorLines
            .First()
            .Should()
            .Be("Error /Directory/Invalid.cs - Failed to compile so was not formatted.");
    }

    [Test]
    public void Format_Writes_Unsupported()
    {
        var context = new TestContext();
        context.WhenAFileExists("Unsupported.js", "asdfasfasdf");

        var result = context.Format(directoryOrFilePaths: "Unsupported.js");

        result.ErrorLines
            .First()
            .Should()
            .Be(@"Error /Unsupported.js - Is an unsupported file type.");
    }

    [Test]
    public void Format_Writes_File_With_Directory_Path()
    {
        var context = new TestContext();
        const string unformattedFilePath = "Unformatted.cs";
        context.WhenAFileExists(unformattedFilePath, UnformattedClassContent);

        context.Format();

        context.GetFileContent(unformattedFilePath).Should().Be(FormattedClassContent);
    }

    [TestCase("0.9.0", false)]
    [TestCase("9999.0.0", false)]
    [TestCase("current", true)]
    public void Works_With_MSBuild_Version_Checking(string version, bool shouldPass)
    {
        var context = new TestContext();
        var currentVersion = typeof(CommandLineFormatter).Assembly.GetName().Version!.ToString(3);

        var versionToTest = version == "current" ? currentVersion : version;

        context.WhenAFileExists(
            "Test.csproj",
            $@"<Project Sdk=""Microsoft.NET.Sdk"">
    <ItemGroup>
        <PackageReference Include=""CSharpier.MsBuild"" Version=""{versionToTest}"" />
    </ItemGroup>
</Project>
"
        );

        var result = context.Format();

        if (shouldPass)
        {
            result.ExitCode.Should().Be(0);
            result.ErrorLines.Should().BeEmpty();
        }
        else
        {
            result.ExitCode.Should().Be(1);
            result.ErrorLines
                .First()
                .Should()
                .EndWith(
                    $@"Test.csproj uses version {version} of CSharpier.MsBuild which is a mismatch with version {currentVersion}"
                );
        }
    }

    [Test]
    public void Works_With_MSBuild_Version_Checking_When_No_Version_Specified()
    {
        var context = new TestContext();
        var currentVersion = typeof(CommandLineFormatter).Assembly.GetName().Version!.ToString(3);

        context.WhenAFileExists(
            "Test.csproj",
            $@"<Project Sdk=""Microsoft.NET.Sdk"">
    <ItemGroup>
        <PackageReference Include=""CSharpier.MsBuild"" />
    </ItemGroup>
</Project>
"
        );

        var result = context.Format();

        result.ExitCode.Should().Be(1);
        result.ErrorLines
            .First()
            .Should()
            .EndWith(
                $"Test.csproj uses an unknown version of CSharpier.MsBuild which is a mismatch with version {currentVersion}"
            );
    }

    [Test]
    public void Works_With_MSBuild_Version_Checking_When_No_Version_Included()
    {
        var context = new TestContext();

        context.WhenAFileExists(
            "Test.csproj",
            $@"<Project Sdk=""Microsoft.NET.Sdk"">
    <ItemGroup>
        <PackageReference Include=""SomeOtherLibrary"" />
    </ItemGroup>
</Project>
"
        );

        var result = context.Format();

        result.ExitCode.Should().Be(0);
        result.ErrorLines.Should().BeEmpty();
    }

    [Test]
    public void Format_Writes_File_With_File_Path()
    {
        var context = new TestContext();
        const string unformattedFilePath = "Unformatted.cs";
        context.WhenAFileExists(unformattedFilePath, UnformattedClassContent);

        context.Format(directoryOrFilePaths: "Unformatted.cs");

        context.GetFileContent(unformattedFilePath).Should().Be(FormattedClassContent);
    }

    [Test]
    public void Format_Supports_Skip_Write()
    {
        var context = new TestContext();
        const string unformattedFilePath = "Unformatted.cs";
        context.WhenAFileExists(unformattedFilePath, UnformattedClassContent);

        context.Format(skipWrite: true);

        context.GetFileContent(unformattedFilePath).Should().Be(UnformattedClassContent);
    }

    [Test]
    public void Format_Checks_Unformatted_File()
    {
        var context = new TestContext();
        const string unformattedFilePath = "Unformatted.cs";
        context.WhenAFileExists(unformattedFilePath, UnformattedClassContent);

        var result = context.Format(check: true);

        result.ExitCode.Should().Be(1);
        context.GetFileContent(unformattedFilePath).Should().Be(UnformattedClassContent);
        result.Lines.First().Should().StartWith("Warning /Unformatted.cs - Was not formatted.");
    }

    [Test]
    public void Format_Checks_Formatted_File()
    {
        var context = new TestContext();
        const string formattedFilePath = "Formatted.cs";
        context.WhenAFileExists(formattedFilePath, FormattedClassContent);
        var result = context.Format(check: true);

        result.ExitCode.Should().Be(0);
    }

    [TestCase("TemporaryGeneratedFile_Tester.cs")]
    [TestCase("TestFile.designer.cs")]
    [TestCase("TestFile.generated.cs")]
    [TestCase("TestFile.g.cs")]
    [TestCase("TestFile.g.i.cs")]
    public void Format_Skips_Generated_Files(string fileName)
    {
        var context = new TestContext();
        var unformattedFilePath = fileName;
        context.WhenAFileExists(unformattedFilePath, UnformattedClassContent);

        var result = context.Format();

        result.Lines.Should().Contain("Total files: 0 ");
    }

    [TestCase("File.cs", "File.cs")]
    [TestCase("File.cs", "*.cs")]
    [TestCase("SubFolder/File.cs", "*.cs")]
    [TestCase("Debug/Logs/File.cs", "**/Logs")]
    [TestCase("Debug/Logs/File.cs", "Logs/")]
    [TestCase("Debug/Logs/File.cs", "Debug/Logs/File.cs")]
    [TestCase(
        @"\Src\CSharpier.Playground\App_Data\Uploads\f45e11a81b926de2af29459af6974bb8.cs",
        "Uploads/"
    )]
    public void File_In_Ignore_Skips_Formatting(string fileName, string ignoreContents)
    {
        var context = new TestContext();
        var unformattedFilePath = fileName;
        context.WhenAFileExists(unformattedFilePath, UnformattedClassContent);
        context.WhenAFileExists(".csharpierignore", ignoreContents);

        var result = context.Format();

        result.Lines
            .FirstOrDefault(o => o.StartsWith("Total files"))
            .Should()
            .Be("Total files: 0 ");
    }

    [TestCase("SubFolder/File.cs", "*.cs", "SubFolder")]
    [TestCase("SubFolder/File.cs", "SubFolder/File.cs", "SubFolder")]
    public void File_In_Ignore_Skips_Formatting_With_BaseDirectory(
        string fileName,
        string ignoreContents,
        string baseDirectory
    )
    {
        var context = new TestContext();
        var unformattedFilePath = fileName;
        context.WhenAFileExists(unformattedFilePath, UnformattedClassContent);
        context.WhenAFileExists(".csharpierignore", ignoreContents);

        var result = context.Format(
            directoryOrFilePaths: Path.Combine(context.GetRootPath(), baseDirectory)
        );

        result.Lines
            .FirstOrDefault(o => o.StartsWith("Total files"))
            .Should()
            .Be("Total files: 0 ");
    }

    [Test]
    public void Multiple_Files_Should_Use_Root_Ignore()
    {
        var context = new TestContext();
        var unformattedFilePath1 = "SubFolder/1/File1.cs";
        var unformattedFilePath2 = "SubFolder/2/File2.cs";
        context.WhenAFileExists(unformattedFilePath1, UnformattedClassContent);
        context.WhenAFileExists(unformattedFilePath2, UnformattedClassContent);
        context.WhenAFileExists(".csharpierignore", "Subfolder/**/*.cs");

        var result = context.Format(
            directoryOrFilePaths: new[] { unformattedFilePath1, unformattedFilePath2 }
        );

        result.Lines
            .FirstOrDefault(o => o.StartsWith("Total files"))
            .Should()
            .Be("Total files: 0 ");
    }

    [Test]
    public void Multiple_Files_Should_Use_Multiple_Ignores()
    {
        var context = new TestContext();
        var unformattedFilePath1 = "SubFolder/1/File1.cs";
        var unformattedFilePath2 = "SubFolder/2/File2.cs";
        context.WhenAFileExists(unformattedFilePath1, UnformattedClassContent);
        context.WhenAFileExists(unformattedFilePath2, UnformattedClassContent);
        context.WhenAFileExists("SubFolder/1/.csharpierignore", "File1.cs");
        context.WhenAFileExists("SubFolder/2/.csharpierignore", "File2.cs");

        var result = context.Format(
            directoryOrFilePaths: new[] { unformattedFilePath1, unformattedFilePath2 }
        );

        result.Lines
            .FirstOrDefault(o => o.StartsWith("Total files"))
            .Should()
            .Be("Total files: 0 ");
    }

    [Test]
    public void Ignore_Should_Deal_With_Inconsistent_Slashes()
    {
        var context = new TestContext();
        var unformattedFilePath1 = @"SubFolder\1\File1.cs";
        context.WhenAFileExists(unformattedFilePath1, UnformattedClassContent);
        context.WhenAFileExists("SubFolder/1/.csharpierignore", "File1.cs");

        var result = context.Format(directoryOrFilePaths: unformattedFilePath1);

        result.Lines
            .FirstOrDefault(o => o.StartsWith("Total files"))
            .Should()
            .Be("Total files: 0 ");
    }

    [Test]
    public void Ignore_Reports_Errors()
    {
        var context = new TestContext();
        context.WhenAFileExists("Test.cs", UnformattedClassContent);
        var path = context.WhenAFileExists(".csharpierignore", @"\Src\Uploads\*.cs");

        var result = context.Format();

        result.ExitCode.Should().Be(1);
        result.ErrorLines
            .First()
            .Replace("\\", "/")
            .Should()
            .Contain(
                $"Error The .csharpierignore file at {path} could not be parsed due to the following line:"
            );
        result.ErrorLines.Skip(1).First().Should().Contain(@"\Src\Uploads\*.cs");
    }

    [Test]
    public void Write_Stdout_Should_Only_Write_File()
    {
        var context = new TestContext();
        context.WhenAFileExists("file1.cs", UnformattedClassContent);

        var result = context.Format(writeStdout: true);

        result.Lines.Should().ContainSingle();
        result.Lines.First().Should().Be(FormattedClassContent);
    }

    [Test]
    public void Should_Format_StandardInput_When_Provided()
    {
        var context = new TestContext();
        var result = context.Format(standardInFileContents: UnformattedClassContent);

        result.Lines.Should().ContainSingle();
        result.Lines.First().Should().Be(FormattedClassContent);
    }

    [Test]
    public void File_With_Mismatched_Line_Endings_In_Verbatim_String_Should_Pass_Validation()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "file1.cs",
            "public class ClassName\n{\npublic string Value = @\"EndThisLineWith\r\nEndThisLineWith\n\";\n}"
        );

        var result = context.Format();

        result.ExitCode.Should().Be(0);
    }

    [Test]
    public void File_With_Compilation_Error_Should_Not_Lose_Code()
    {
        var context = new TestContext();
        var contents =
            @"#if DEBUG
?using System;
#endif
";
        context.WhenAFileExists("Invalid.cs", contents);

        var result = context.Format();

        context.GetFileContent("Invalid.cs").Should().Be(contents);
        result.ErrorLines
            .First()
            .Should()
            .Be("Error /Invalid.cs - Failed to compile so was not formatted.");
    }

    [Test]
    public void File_Should_Format_With_Supplied_Symbols()
    {
        var context = new TestContext();
        context.WhenAFileExists(".csharpierrc", @"{ ""preprocessorSymbolSets"": [""FORMAT""] }");
        context.WhenAFileExists(
            "file1.cs",
            @"public class ClassName
{
#if FORMAT
    public     string ShortPropertyName;
#elif NO_FORMAT
    public     string ShortPropertyName;
#else
    public     string ShortPropertyName;
#endif
}
"
        );

        context.Format();

        var result = context.GetFileContent("file1.cs");

        result
            .Should()
            .Be(
                @"public class ClassName
{
#if FORMAT
    public string ShortPropertyName;
#elif NO_FORMAT
    public     string ShortPropertyName;
#else
    public     string ShortPropertyName;
#endif
}
"
            );
    }

    [TestCase(".csharpierrc")]
    [TestCase(".csharpierrc.json")]
    [TestCase(".csharpierrc.yaml")]
    public void Empty_Config_Files_Should_Log_Warning(string configFileName)
    {
        var context = new TestContext();
        var configPath = context.WhenAFileExists(".csharpierrc", "");
        context.WhenAFileExists("file1.cs", "public class ClassName { }");

        var result = context.Format();

        result.Lines
            .First()
            .Replace("\\", "/")
            .Should()
            .Be($"Warning The configuration file at {configPath} was empty.");
    }

    private class TestContext
    {
        private MockFileSystem fileSystem = new();

        public string WhenAFileExists(string path, string contents)
        {
            path = this.fileSystem.Path.Combine(GetRootPath(), path).Replace('\\', '/');
            this.fileSystem.AddFile(path, new MockFileData(contents));
            return path;
        }

        public string GetRootPath()
        {
            var result = OperatingSystem.IsWindows() ? @"c:\test" : "/Test";
            this.fileSystem.AddDirectory(result);
            return result;
        }

        public string GetFileContent(string path)
        {
            path = this.fileSystem.Path.Combine(GetRootPath(), path);
            return this.fileSystem.File.ReadAllText(path);
        }

        public FormatResult Format(
            bool skipWrite = false,
            bool check = false,
            bool writeStdout = false,
            string standardInFileContents = null,
            params string[] directoryOrFilePaths
        )
        {
            if (directoryOrFilePaths.Length == 0)
            {
                directoryOrFilePaths = new[] { GetRootPath() };
            }
            else
            {
                directoryOrFilePaths = directoryOrFilePaths
                    .Select(o => this.fileSystem.Path.Combine(GetRootPath(), o))
                    .ToArray();
            }

            var fakeConsole = new TestConsole();
            var testLogger = new ConsoleLogger(fakeConsole);
            var exitCode =
                CommandLineFormatter.Format(
                    new CommandLineOptions
                    {
                        DirectoryOrFilePaths = directoryOrFilePaths,
                        SkipWrite = skipWrite,
                        Check = check,
                        WriteStdout = writeStdout || standardInFileContents != null,
                        StandardInFileContents = standardInFileContents,
                    },
                    this.fileSystem,
                    fakeConsole,
                    testLogger,
                    CancellationToken.None
                ).Result;

            fakeConsole.Close();

            return new FormatResult(exitCode, fakeConsole.Lines, fakeConsole.ErrorLines);
        }
    }

    private record FormatResult(int ExitCode, IList<string> Lines, IList<string> ErrorLines);

    private class TestConsole : IConsole
    {
        public readonly List<string> Lines = new();
        public readonly List<string> ErrorLines = new();

        private string nextLine = "";
        private string nextErrorLine = "";

        public void WriteLine(string line = null)
        {
            while (line != null && line.Contains("  "))
            {
                line = line.Replace("  ", " ");
            }

            if (line != null)
            {
                nextLine += line;
                this.Lines.Add(nextLine);
                nextLine = "";
            }
        }

        public void WriteErrorLine(string line = null)
        {
            while (line != null && line.Contains("  "))
            {
                line = line.Replace("  ", " ");
            }

            if (line != null)
            {
                nextErrorLine += line;
                this.ErrorLines.Add(nextErrorLine);
                nextErrorLine = "";
            }
        }

        public void Write(string value)
        {
            nextLine += value;
        }

        public void WriteError(string value)
        {
            nextErrorLine += value;
        }

        public Encoding InputEncoding => Encoding.UTF8;
        public ConsoleColor ForegroundColor { get; set; }
        public void ResetColor() { }

        public void Close()
        {
            if (nextLine != "")
            {
                this.Lines.Add(nextLine);
            }
            if (nextErrorLine != "")
            {
                this.ErrorLines.Add(nextErrorLine);
            }
        }
    }
}
