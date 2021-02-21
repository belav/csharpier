using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ConditionalAccessExpressionTests : BaseTest
    {
        [Test]
        public void BasicConditionalAccessExpression()
        {
            this.RunTest("ConditionalAccessExpression", "BasicConditionalAccessExpression");
        }
    }
}
