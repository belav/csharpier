using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class AsExpressionTests : BaseTest
    {
        [Test]
        public void BasicAsExpression()
        {
            this.RunTest("AsExpression", "BasicAsExpression");
        }
    }
}
