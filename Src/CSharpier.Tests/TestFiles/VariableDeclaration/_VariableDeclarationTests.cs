using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
