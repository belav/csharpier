using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class RefExpressionTests : BaseTest
{
  [Test]
  public void BasicRefExpression()
  {
    this.RunTest("RefExpression", "BasicRefExpression");
  }
}

}
