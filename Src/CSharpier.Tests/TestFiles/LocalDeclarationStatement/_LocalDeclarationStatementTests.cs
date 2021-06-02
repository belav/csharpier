using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class LocalDeclarationStatementTests : BaseTest
    {
        [Test]
        public void LocalDeclarationStatements()
        {
            this.RunTest("LocalDeclarationStatement", "LocalDeclarationStatements");
        }
    }
}
