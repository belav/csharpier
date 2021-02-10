using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class TypeOfExpressionTests : BaseTest
    {
        [Test]
        public void BasicTypeOfExpression()
        {
            this.RunTest("TypeOfExpression", "BasicTypeOfExpression");
        }
    }
}
