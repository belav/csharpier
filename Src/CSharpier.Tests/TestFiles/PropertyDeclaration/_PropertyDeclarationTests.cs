using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class PropertyDeclarationTests : BaseTest
    {
        [Test]
        public void PropertyDeclarations()
        {
            this.RunTest("PropertyDeclaration", "PropertyDeclarations");
        }
    }
}
