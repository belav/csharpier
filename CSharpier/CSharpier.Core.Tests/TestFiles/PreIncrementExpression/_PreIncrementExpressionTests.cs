using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class PreIncrementExpressionTests : BaseTest
    {
        [Test]
        public void BasicPreIncrementExpression()
        {
            this.RunTest("PreIncrementExpression", "BasicPreIncrementExpression");
        }
    }
}
