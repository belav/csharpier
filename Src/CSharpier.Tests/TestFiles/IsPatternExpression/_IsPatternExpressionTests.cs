using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class IsPatternExpressionTests : BaseTest
{
  [Test]
  public void IsPatternExpressions()
  {
    this.RunTest("IsPatternExpression", "IsPatternExpressions");
  }
}

}
