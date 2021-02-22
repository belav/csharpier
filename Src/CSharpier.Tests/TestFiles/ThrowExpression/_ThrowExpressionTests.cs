using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ThrowExpressionTests : BaseTest
    {
        [Test]
        public void BasicThrowExpression()
        {
            this.RunTest("ThrowExpression", "BasicThrowExpression");
        }
    }
}
