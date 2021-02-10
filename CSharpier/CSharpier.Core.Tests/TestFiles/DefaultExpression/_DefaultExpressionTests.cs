using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class DefaultExpressionTests : BaseTest
    {
        [Test]
        public void BasicDefaultExpression()
        {
            this.RunTest("DefaultExpression", "BasicDefaultExpression");
        }
    }
}
