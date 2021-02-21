using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ConversionOperatorDeclarationTests : BaseTest
    {
        [Test]
        public void BasicConversionOperatorDeclaration()
        {
            this.RunTest("ConversionOperatorDeclaration", "BasicConversionOperatorDeclaration");
        }
    }
}
