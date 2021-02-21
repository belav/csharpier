using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class WithExpressionTests : BaseTest
    {
        [Test]
        public void BasicWithExpression()
        {
            this.RunTest("WithExpression", "BasicWithExpression");
        }
    }
}
