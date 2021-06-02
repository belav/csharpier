using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class StructDeclarationTests : BaseTest
    {
        [Test]
        public void StructDeclarations()
        {
            this.RunTest("StructDeclaration", "StructDeclarations");
        }
    }
}
