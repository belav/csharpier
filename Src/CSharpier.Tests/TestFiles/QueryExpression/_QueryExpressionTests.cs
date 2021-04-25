using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class QueryExpressionTests : BaseTest
{
  [Test]
  public void QueryExpressions()
  {
    this.RunTest("QueryExpression", "QueryExpressions");
  }
}

}
