using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class IsExpressionTests : BaseTest
{
  [Test]
  public void BasicIsExpression()
  {
    this.RunTest("IsExpression", "BasicIsExpression");
  }
}

}
