using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.AssignmentExpression
{
    public class AssignmentExpressionTests : BaseTest
    {
        [Test]
        public void AssignmentExpressions()
        {
            this.RunTest("AssignmentExpression", "AssignmentExpressions");
        }
    }
}
