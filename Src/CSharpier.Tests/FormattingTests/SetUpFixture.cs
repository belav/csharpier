using System.IO;
using DiffEngine;
using NUnit.Framework;

namespace CSharpier.Tests.FormattingTests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            EmptyFiles.Extensions.AddTextExtension(".cst");
            DiffTools.AddToolBasedOn(
                DiffTool.WinMerge,
                name: "WritableWinMerge",
                arguments: (temp, target) =>
                {
                    var leftTitle = Path.GetFileName(temp);
                    var rightTitle = Path.GetFileName(target);
                    return $"/u /wr /e \"{temp}\" \"{target}\" /dl \"{leftTitle}\" /dr \"{rightTitle}\"";
                }
            );
        }
    }
}
