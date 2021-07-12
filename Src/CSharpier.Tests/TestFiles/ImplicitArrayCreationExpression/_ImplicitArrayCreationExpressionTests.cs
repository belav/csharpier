using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.ImplicitArrayCreationExpression
{
    public class ImplicitArrayCreationExpressionTests : BaseTest
    {
        [Test]
        public void ImplicitArrayCreationExpressions()
        {
            this.RunTest("ImplicitArrayCreationExpression", "ImplicitArrayCreationExpressions");
        }
    }
}
