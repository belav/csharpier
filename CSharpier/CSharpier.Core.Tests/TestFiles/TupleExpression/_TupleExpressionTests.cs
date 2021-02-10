using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
