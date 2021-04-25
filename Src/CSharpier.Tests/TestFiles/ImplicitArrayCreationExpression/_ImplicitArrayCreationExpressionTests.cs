using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class ImplicitArrayCreationExpressionTests : BaseTest
{
  [Test]
  public void BasicImplicitArrayCreationExpression()
  {
    this.RunTest(
      "ImplicitArrayCreationExpression",
      "BasicImplicitArrayCreationExpression"
    );
  }
  [Test]
  public void ImplicityArrayWithCommas()
  {
    this.RunTest("ImplicitArrayCreationExpression", "ImplicityArrayWithCommas");
  }
}

}
