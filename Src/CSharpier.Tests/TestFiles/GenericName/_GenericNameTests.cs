using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class GenericNameTests : BaseTest
{
  [Test]
  public void BasicGenericName()
  {
    this.RunTest("GenericName", "BasicGenericName");
  }
}

}
