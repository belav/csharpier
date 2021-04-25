using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class AliasQualifiedNameTests : BaseTest
{
  [Test]
  public void BasicAliasQualifiedName()
  {
    this.RunTest("AliasQualifiedName", "BasicAliasQualifiedName");
  }
}

}
