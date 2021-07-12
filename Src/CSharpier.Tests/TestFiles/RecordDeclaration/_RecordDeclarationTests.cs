using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.RecordDeclaration
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
