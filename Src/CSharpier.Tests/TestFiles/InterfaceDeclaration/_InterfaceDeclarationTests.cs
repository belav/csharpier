using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class InterfaceDeclarationTests : BaseTest
    {
        [Test]
        public void BasicInterfaceDeclaration()
        {
            this.RunTest("InterfaceDeclaration", "BasicInterfaceDeclaration");
        }
        [Test]
        public void InterfaceWithBaseList()
        {
            this.RunTest("InterfaceDeclaration", "InterfaceWithBaseList");
        }
        [Test]
        public void InterfaceWithMethod()
        {
            this.RunTest("InterfaceDeclaration", "InterfaceWithMethod");
        }
        [Test]
        public void InterfaceWithTypeParameters()
        {
            this.RunTest("InterfaceDeclaration", "InterfaceWithTypeParameters");
        }
    }
}
