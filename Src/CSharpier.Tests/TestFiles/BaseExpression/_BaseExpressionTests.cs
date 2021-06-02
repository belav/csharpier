using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class BaseExpressionTests : BaseTest
    {
        [Test]
        public void BaseExpressions()
        {
            this.RunTest("BaseExpression", "BaseExpressions");
        }
    }
}
