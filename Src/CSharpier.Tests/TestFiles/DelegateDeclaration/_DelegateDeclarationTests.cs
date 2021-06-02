using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class DelegateDeclarationTests : BaseTest
    {
        [Test]
        public void DelegateDeclarations()
        {
            this.RunTest("DelegateDeclaration", "DelegateDeclarations");
        }
    }
}
