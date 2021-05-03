using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class InitializerExpressionTests : BaseTest
    {
        [Test]
        public void InitializerExpressions()
        {
            this.RunTest("InitializerExpression", "InitializerExpressions");
        }
    }
}
