using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class DeclarationExpressionTests : BaseTest
    {
        [Test]
        public void DeclarationExpressions()
        {
            this.RunTest("DeclarationExpression", "DeclarationExpressions");
        }
    }
}
