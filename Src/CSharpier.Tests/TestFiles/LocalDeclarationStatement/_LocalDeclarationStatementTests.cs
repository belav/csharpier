using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class LocalDeclarationStatementTests : BaseTest
    {
        [Test]
        public void VariableWithInitializer()
        {
            this.RunTest(
                "LocalDeclarationStatement",
                "VariableWithInitializer"
            );
        }
        [Test]
        public void VariableWithoutInitializer()
        {
            this.RunTest(
                "LocalDeclarationStatement",
                "VariableWithoutInitializer"
            );
        }
    }
}
