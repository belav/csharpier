using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class EnumDeclarationTests : BaseTest
    {
        [Test]
        public void BasicEnumDeclaration()
        {
            this.RunTest("EnumDeclaration", "BasicEnumDeclaration");
        }
        [Test]
        public void EnumWithValues()
        {
            this.RunTest("EnumDeclaration", "EnumWithValues");
        }
    }
}
