using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class RecordDeclarationTests : BaseTest
    {
        [Test]
        public void BasicRecordDeclaration()
        {
            this.RunTest("RecordDeclaration", "BasicRecordDeclaration");
        }
    }
}
