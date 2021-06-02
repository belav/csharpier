using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ArrayCreationExpressionTests : BaseTest
    {
        [Test]
        public void ArrayCreationExpressions()
        {
            this.RunTest("ArrayCreationExpression", "ArrayCreationExpressions");
        }
    }
}
