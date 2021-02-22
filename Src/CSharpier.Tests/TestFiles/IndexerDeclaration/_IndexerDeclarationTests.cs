using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class IndexerDeclarationTests : BaseTest
    {
        [Test]
        public void BasicIndexerDeclaration()
        {
            this.RunTest("IndexerDeclaration", "BasicIndexerDeclaration");
        }
    }
}
