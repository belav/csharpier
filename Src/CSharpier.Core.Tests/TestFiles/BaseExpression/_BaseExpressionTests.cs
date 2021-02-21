using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class BaseExpressionTests : BaseTest
    {
        [Test]
        public void BasicBaseExpression()
        {
            this.RunTest("BaseExpression", "BasicBaseExpression");
        }
    }
}
