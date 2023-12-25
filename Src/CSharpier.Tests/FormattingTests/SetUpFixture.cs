using System.IO;
using DiffEngine;
using NUnit.Framework;

namespace CSharpier.Tests.FormattingTests;

[SetUpFixture]
public class SetUpFixture
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        EmptyFiles.FileExtensions.AddTextExtension(".test");
        DiffTools.AddToolBasedOn(DiffTool.WinMerge, name: "WritableWinMerge");
    }
}
