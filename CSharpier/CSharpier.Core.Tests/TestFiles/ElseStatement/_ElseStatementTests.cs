using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ElseStatementTests : BaseTest
    {
        [Test]
        public void BasicElse()
        {
            this.RunTest("ElseStatement", "BasicElse");
        }
        [Test]
        public void ElseIf()
        {
            this.RunTest("ElseStatement", "ElseIf");
        }
    }
}
