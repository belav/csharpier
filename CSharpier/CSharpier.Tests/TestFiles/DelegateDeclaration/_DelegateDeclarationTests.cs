using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class DelegateDeclarationTests : BaseTest
    {
        [Test]
        public void BasicDelegateDeclaration()
        {
            this.RunTest("DelegateDeclaration", "BasicDelegateDeclaration");
        }
        [Test]
        public void GenericDelegate()
        {
            this.RunTest("DelegateDeclaration", "GenericDelegate");
        }
    }
}
