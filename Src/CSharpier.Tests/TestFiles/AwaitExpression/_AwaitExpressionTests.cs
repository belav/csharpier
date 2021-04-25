using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class AwaitExpressionTests : BaseTest
{
  [Test]
  public void BasicAwaitExpression()
  {
    this.RunTest("AwaitExpression", "BasicAwaitExpression");
  }
}

}
