using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class TupleTypeTests : BaseTest
{
  [Test]
  public void BasicTupleType()
  {
    this.RunTest("TupleType", "BasicTupleType");
  }
}

}
