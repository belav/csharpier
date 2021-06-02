using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class IsExpressionTests : BaseTest
    {
        [Test]
        public void IsExpressions()
        {
            this.RunTest("IsExpression", "IsExpressions");
        }
    }
}
