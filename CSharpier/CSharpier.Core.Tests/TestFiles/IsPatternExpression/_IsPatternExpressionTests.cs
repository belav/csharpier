using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class IsPatternExpressionTests : BaseTest
    {
        [Test]
        public void BasicIsPatternExpression()
        {
            this.RunTest("IsPatternExpression", "BasicIsPatternExpression");
        }
        [Test]
        public void RecursivePattern()
        {
            this.RunTest("IsPatternExpression", "RecursivePattern");
        }
    }
}
