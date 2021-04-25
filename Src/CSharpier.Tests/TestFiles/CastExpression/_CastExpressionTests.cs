using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class CastExpressionTests : BaseTest
{
  [Test]
  public void BasicCastExpression()
  {
    this.RunTest("CastExpression", "BasicCastExpression");
  }
}

}
