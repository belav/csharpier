using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
