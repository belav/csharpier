using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class IsExpressionTests : BaseTest
    {
        [Test]
        public void BasicIsExpression()
        {
            this.RunTest("IsExpression", "BasicIsExpression");
        }
    }
}
