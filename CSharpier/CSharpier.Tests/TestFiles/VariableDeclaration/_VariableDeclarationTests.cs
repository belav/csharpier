using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class VariableDeclarationTests : BaseTest
    {
        [Test]
        public void BasicVariableDeclaration()
        {
            this.RunTest("VariableDeclaration", "BasicVariableDeclaration");
        }
        [Test]
        public void VariableDeclarationComments()
        {
            this.RunTest("VariableDeclaration", "VariableDeclarationComments");
        }
    }
}
