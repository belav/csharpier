using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
