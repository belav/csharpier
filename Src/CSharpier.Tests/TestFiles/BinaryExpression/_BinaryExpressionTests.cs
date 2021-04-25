using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class BinaryExpressionTests : BaseTest
{
  [Test]
  public void StringConcatenation()
  {
    this.RunTest("BinaryExpression", "StringConcatenation");
  }
}

}
