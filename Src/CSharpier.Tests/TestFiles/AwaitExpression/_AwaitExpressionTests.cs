using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class AwaitExpressionTests : BaseTest
    {
        [Test]
        public void AwaitExpressions()
        {
            this.RunTest("AwaitExpression", "AwaitExpressions");
        }
    }
}
