using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
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
