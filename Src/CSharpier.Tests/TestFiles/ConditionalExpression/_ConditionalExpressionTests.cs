using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class ConditionalExpressionTests : BaseTest
{
  [Test]
  public void BasicConditionalExpression()
  {
    this.RunTest("ConditionalExpression", "ConditionalExpressions");
  }
}

}
