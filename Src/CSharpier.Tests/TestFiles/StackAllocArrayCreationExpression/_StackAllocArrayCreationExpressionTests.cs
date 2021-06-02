using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
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
