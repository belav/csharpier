using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.ClassDeclaration
{
    public class ClassDeclarationTests : BaseTest
    {
        [Test]
        public void ClassDeclarations()
        {
            this.RunTest("ClassDeclaration", "ClassDeclarations");
        }
    }
}
