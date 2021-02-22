using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class DestructorDeclarationTests : BaseTest
    {
        [Test]
        public void BasicDestructorDeclaration()
        {
            this.RunTest("DestructorDeclaration", "BasicDestructorDeclaration");
        }
        [Test]
        public void DestructorDeclarationWithExpressionBody()
        {
            this.RunTest(
                "DestructorDeclaration",
                "DestructorDeclarationWithExpressionBody");
        }
    }
}
