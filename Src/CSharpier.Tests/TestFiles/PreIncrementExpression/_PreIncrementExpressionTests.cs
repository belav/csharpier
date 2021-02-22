using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class PreIncrementExpressionTests : BaseTest
    {
        [Test]
        public void BasicPreIncrementExpression()
        {
            this.RunTest(
                "PreIncrementExpression",
                "BasicPreIncrementExpression");
        }
    }
}
