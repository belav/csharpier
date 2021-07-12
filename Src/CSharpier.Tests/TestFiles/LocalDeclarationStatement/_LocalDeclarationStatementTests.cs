using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.LocalDeclarationStatement
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
