using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
