using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class AwaitExpressionTests : BaseTest
    {
        [Test]
        public void BasicAwaitExpression()
        {
            this.RunTest("AwaitExpression", "BasicAwaitExpression");
        }
    }
}
