using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
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
