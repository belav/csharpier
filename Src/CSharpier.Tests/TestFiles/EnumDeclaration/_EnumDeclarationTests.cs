using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class EnumDeclarationTests : BaseTest
    {
        [Test]
        public void EnumDeclarations()
        {
            this.RunTest("EnumDeclaration", "EnumDeclarations");
        }
    }
}
