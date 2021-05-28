using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class FieldDeclarationTests : BaseTest
    {
        [Test]
        public void FieldDeclarations()
        {
            this.RunTest("FieldDeclaration", "FieldDeclarations");
        }
    }
}
