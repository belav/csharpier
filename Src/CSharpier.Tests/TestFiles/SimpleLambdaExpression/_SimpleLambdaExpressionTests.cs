using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class SimpleLambdaExpressionTests : BaseTest
{
  [Test]
  public void SimpleLambdaExpressions()
  {
    this.RunTest("SimpleLambdaExpression", "SimpleLambdaExpressions");
  }
}

}
