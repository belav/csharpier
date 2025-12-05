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

        static string TargetLeftArguments(string temp, string target)
        {
            string tempTitle = Path.GetFileName(temp);
            string targetTitle = Path.GetFileName(target);
            return $"/u /wr /e \"{target}\" \"{temp}\" /dl \"{targetTitle}\" /dr \"{tempTitle}\"";
        }

        static string TargetRightArguments(string temp, string target)
        {
            string tempTitle = Path.GetFileName(temp);
            string targetTitle = Path.GetFileName(target);
            return $"/u /wr /e \"{temp}\" \"{target}\" /dl \"{tempTitle}\" /dr \"{targetTitle}\"";
        }

        DiffTools.AddToolBasedOn(
            DiffTool.WinMerge,
            name: "WritableWinMerge",
            launchArguments: new LaunchArguments(TargetLeftArguments, TargetRightArguments)
        );
    }
}
