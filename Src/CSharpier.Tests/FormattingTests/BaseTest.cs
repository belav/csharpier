using System.IO.Abstractions;
using System.Text;
using AwesomeAssertions;
using CSharpier.Cli;
using CSharpier.Core;
using CSharpier.Core.CSharp;
using DiffEngine;
using Microsoft.CodeAnalysis;

namespace CSharpier.Tests.FormattingTests;

public class BaseTest
{
    private readonly DirectoryInfo rootDirectory = DirectoryFinder.FindParent("CSharpier.Tests");

    public void BuildTests<T>(DynamicTestBuilderContext context, string folder)
        where T : BaseTest
    {
        var testFilesDirectory = new DirectoryInfo(
            Path.Combine(this.rootDirectory.FullName, "FormattingTests/TestFiles/" + folder)
        );

        var files = testFilesDirectory
            .EnumerateFiles("*.test", SearchOption.AllDirectories)
            .Where(o =>
                !o.FullName.EndsWith(".actual.test", StringComparison.OrdinalIgnoreCase)
                && !o.FullName.EndsWith(".expected.test", StringComparison.OrdinalIgnoreCase)
            );

        foreach (var group in files.GroupBy(o => o.Directory!.Name))
        {
            foreach (var file in group)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);
                var useTabs = fileNameWithoutExtension.EndsWith("_Tabs", StringComparison.Ordinal);

                context.AddTest(
                    new DynamicTest<T>
                    {
                        TestMethod = @class => @class.RunTest(string.Empty, string.Empty, false),
                        TestMethodArguments =
                        [
                            fileNameWithoutExtension,
                            file.Directory!.Name,
                            useTabs,
                        ],
                        DisplayName = fileNameWithoutExtension,
                    }
                );
            }
        }
    }

    public async Task RunTest(string fileName, string fileExtensionWithoutDot, bool useTabs = false)
    {
        var filePath = Path.Combine(
            this.rootDirectory.FullName,
            "FormattingTests",
            "TestFiles",
            fileExtensionWithoutDot,
            fileName + ".test"
        );
        var fileReaderResult = await FileReader.ReadFileAsync(
            filePath,
            new FileSystem(),
            CancellationToken.None
        );

        var formatter = fileExtensionWithoutDot switch
        {
            "cs" => Formatter.CSharp,
            "csx" => Formatter.CSharpScript,
            "xml" => Formatter.XML,
            _ => Formatter.Unknown,
        };

        var result = await CodeFormatter.FormatAsync(
            fileReaderResult.FileContents,
            new PrinterOptions(formatter)
            {
                Width = PrinterOptions.WidthUsedByTests,
                UseTabs = useTabs,
                IndentSize = formatter == Formatter.XML ? 2 : 4,
            },
            CancellationToken.None
        );

        var actualFilePath = filePath.Replace(".test", ".actual.test");
        await File.WriteAllTextAsync(actualFilePath, result.Code, fileReaderResult.Encoding);

        var filePathToChange = filePath;
        var expectedFilePath = actualFilePath.Replace(".actual.", ".expected.");

        var expectedCode = fileReaderResult.FileContents;

        if (File.Exists(expectedFilePath))
        {
            expectedCode = await File.ReadAllTextAsync(expectedFilePath, Encoding.UTF8);
            filePathToChange = expectedFilePath;
        }

        var normalizedCode = result.Code;

        if (Environment.GetEnvironmentVariable("NormalizeLineEndings") != null)
        {
            expectedCode = expectedCode.Replace("\r\n", "\n");
            normalizedCode = normalizedCode.Replace("\r\n", "\n");
        }

        // TODO #1359 xml comparer here
        var comparer = new SyntaxNodeComparer(
            expectedCode,
            normalizedCode,
            false,
            false,
            false,
            SourceCodeKind.Regular,
            CancellationToken.None
        );

        result.CompilationErrors.Should().BeEmpty();
        result.FailureMessage.Should().BeEmpty();
        result.WarningMessage.Should().BeEmpty();

        if (normalizedCode != expectedCode && !BuildServerDetector.Detected)
        {
            await DiffRunner.LaunchAsync(filePathToChange, actualFilePath);
        }
        normalizedCode.Should().Be(expectedCode);

        var compareResult = comparer.CompareSource();
        compareResult.Should().BeNullOrEmpty();
    }
}
