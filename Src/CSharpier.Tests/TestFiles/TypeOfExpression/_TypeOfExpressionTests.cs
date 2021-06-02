using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class TypeOfExpressionTests : BaseTest
    {
        [Test]
        public void TypeOfExpressions()
        {
            this.RunTest("TypeOfExpression", "TypeOfExpressions");
        }
    }
}
