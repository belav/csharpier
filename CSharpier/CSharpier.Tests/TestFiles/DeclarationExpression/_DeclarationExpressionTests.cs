using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class DeclarationExpressionTests : BaseTest
    {
        [Test]
        public void BasicDeclarationExpression()
        {
            this.RunTest("DeclarationExpression", "BasicDeclarationExpression");
        }
    }
}
