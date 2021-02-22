using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ConditionalExpressionTests : BaseTest
    {
        [Test]
        public void BasicConditionalExpression()
        {
            this.RunTest("ConditionalExpression", "BasicConditionalExpression");
        }
    }
}
