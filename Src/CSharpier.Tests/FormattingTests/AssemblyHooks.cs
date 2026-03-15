using DiffEngine;

namespace CSharpier.Tests.FormattingTests;

public class AssemblyHooks
{
    [Before(Assembly)]
    public static void AssemblySetup()
    {
        EmptyFiles.Extensions.AddTextExtension(".test");

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
            targetLeftArguments: TargetLeftArguments,
            targetRightArguments: TargetRightArguments
        );
    }
}
