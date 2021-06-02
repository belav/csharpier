using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class RecordDeclarationTests : BaseTest
    {
        [Test]
        public void RecordDeclarations()
        {
            this.RunTest("RecordDeclaration", "RecordDeclarations");
        }
    }
}
