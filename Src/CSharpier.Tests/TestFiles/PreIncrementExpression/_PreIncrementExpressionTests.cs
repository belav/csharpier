using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class PreIncrementExpressionTests : BaseTest
    {
        [Test]
        public void PreIncrementExpressions()
        {
            this.RunTest("PreIncrementExpression", "PreIncrementExpressions");
        }
    }
}
