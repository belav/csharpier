using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class QueryExpressionTests : BaseTest
    {
        [Test]
        public void BasicQueryExpression()
        {
            this.RunTest("QueryExpression", "BasicQueryExpression");
        }
        [Test]
        public void ComplexQuery()
        {
            this.RunTest("QueryExpression", "ComplexQuery");
        }
        [Test]
        public void QueryWithGroup()
        {
            this.RunTest("QueryExpression", "QueryWithGroup");
        }
        [Test]
        public void QueryWithSelectInto()
        {
            this.RunTest("QueryExpression", "QueryWithSelectInto");
        }
        [Test]
        public void QueryWithWhere()
        {
            this.RunTest("QueryExpression", "QueryWithWhere");
        }
    }
}
