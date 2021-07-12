using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.StackAllocArrayCreationExpression
{
    public class StackAllocArrayCreationExpressionTests : BaseTest
    {
        [Test]
        public void StackAllocArrayCreationExpressions()
        {
            this.RunTest("StackAllocArrayCreationExpression", "StackAllocArrayCreationExpressions");
        }
    }
}
