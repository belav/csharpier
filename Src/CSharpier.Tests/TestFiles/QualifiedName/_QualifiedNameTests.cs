using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class QualifiedNameTests : BaseTest
{
  [Test]
  public void UsingWithDots()
  {
    this.RunTest("QualifiedName", "UsingWithDots");
  }
}

}
