using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.InitializerExpression
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
