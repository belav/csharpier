using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.DelegateDeclaration
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
