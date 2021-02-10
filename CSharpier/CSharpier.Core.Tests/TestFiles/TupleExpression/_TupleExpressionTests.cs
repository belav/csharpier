using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class TupleExpressionTests : BaseTest
    {
        [Test]
        public void BasicTupleExpression()
        {
            this.RunTest("TupleExpression", "BasicTupleExpression");
        }
    }
}
