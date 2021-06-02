using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class DefaultExpressionTests : BaseTest
    {
        [Test]
        public void DefaultExpressions()
        {
            this.RunTest("DefaultExpression", "DefaultExpressions");
        }
    }
}
