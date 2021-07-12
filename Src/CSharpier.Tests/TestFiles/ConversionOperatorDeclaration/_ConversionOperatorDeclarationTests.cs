using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.ConversionOperatorDeclaration
{
    public class ConversionOperatorDeclarationTests : BaseTest
    {
        [Test]
        public void ConversionOperatorDeclarations()
        {
            this.RunTest("ConversionOperatorDeclaration", "ConversionOperatorDeclarations");
        }
    }
}
