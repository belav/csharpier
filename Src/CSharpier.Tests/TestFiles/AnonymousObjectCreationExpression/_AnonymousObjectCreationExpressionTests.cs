using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.AnonymousObjectCreationExpression
{
    public class AnonymousObjectCreationExpressionTests : BaseTest
    {
        [Test]
        public void AnonymousObjectCreationExpressions()
        {
            this.RunTest("AnonymousObjectCreationExpression", "AnonymousObjectCreationExpressions");
        }
    }
}
