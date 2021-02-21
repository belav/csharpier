using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class CasePatternSwitchLabelTests : BaseTest
    {
        [Test]
        public void BasicCasePatternSwitchLabel()
        {
            this.RunTest("CasePatternSwitchLabel", "BasicCasePatternSwitchLabel");
        }
    }
}
