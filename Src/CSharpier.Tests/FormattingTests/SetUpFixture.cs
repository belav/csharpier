using DiffEngine;
using NUnit.Framework;

namespace CSharpier.Tests.FormattingTests;

[SetUpFixture]
public class SetUpFixture
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // EmptyFiles.FileExtensions.AddTextExtension(".test");

        DiffTools.AddToolBasedOn(
            DiffTool.WinMerge,
            name: "WritableWinMerge",
            launchArguments: new(Left: TargetLeftArguments, Right: TargetRightArguments)
        );
    }

    static string TargetLeftArguments(string temp, string target)
    {
        var tempTitle = Path.GetFileName(temp);
        var targetTitle = Path.GetFileName(target);
        return $"/u /wr /e \"{target}\" \"{temp}\" /dl \"{targetTitle}\" /dr \"{tempTitle}\"";
    }

    static string TargetRightArguments(string temp, string target)
    {
        var tempTitle = Path.GetFileName(temp);
        var targetTitle = Path.GetFileName(target);
        return $"/u /wr /e \"{temp}\" \"{target}\" /dl \"{tempTitle}\" /dr \"{targetTitle}\"";
    }
}
