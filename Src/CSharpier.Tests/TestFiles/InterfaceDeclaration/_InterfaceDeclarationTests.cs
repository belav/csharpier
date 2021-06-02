using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class InterfaceDeclarationTests : BaseTest
    {
        [Test]
        public void InterfaceDeclarations()
        {
            this.RunTest("InterfaceDeclaration", "InterfaceDeclarations");
        }
    }
}
