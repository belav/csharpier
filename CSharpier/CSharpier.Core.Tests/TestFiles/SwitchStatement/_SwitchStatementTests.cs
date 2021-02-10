using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class SwitchStatementTests : BaseTest
    {
        [Test]
        public void BasicSwitchStatement()
        {
            this.RunTest("SwitchStatement", "BasicSwitchStatement");
        }
        [Test]
        public void DefaultWithNoBraces()
        {
            this.RunTest("SwitchStatement", "DefaultWithNoBraces");
        }
        [Test]
        public void EmptySwitchStatement()
        {
            this.RunTest("SwitchStatement", "EmptySwitchStatement");
        }
        [Test]
        public void GotoStatements()
        {
            this.RunTest("SwitchStatement", "GotoStatements");
        }
    }
}
