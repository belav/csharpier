using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ReturnStatementTests : BaseTest
    {
        [Test]
        public void EmptyReturn()
        {
            this.RunTest("ReturnStatement", "EmptyReturn");
        }
    }
}
