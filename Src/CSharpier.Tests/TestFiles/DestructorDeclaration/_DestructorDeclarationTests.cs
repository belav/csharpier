using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.DestructorDeclaration
{
    public class DestructorDeclarationTests : BaseTest
    {
        [Test]
        public void DestructorDeclarations()
        {
            this.RunTest("DestructorDeclaration", "DestructorDeclarations");
        }
    }
}
