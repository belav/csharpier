using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class CasePatternSwitchLabelTests : BaseTest
    {
        [Test]
        public void BasicCasePatternSwitchLabel()
        {
            this.RunTest(
                "CasePatternSwitchLabel",
                "BasicCasePatternSwitchLabel"
            );
        }
    }
}
