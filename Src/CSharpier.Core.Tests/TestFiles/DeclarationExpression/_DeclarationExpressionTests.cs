using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
