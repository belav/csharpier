using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
