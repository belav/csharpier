using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class ArrayTypeTests : BaseTest
{
  [Test]
  public void BasicArrayType()
  {
    this.RunTest("ArrayType", "BasicArrayType");
  }
}

}
