using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class FunctionPointerType : BaseTest
{
  [Test]
  public void FunctionPointerTypes()
  {
    this.RunTest("FunctionPointerType", "FunctionPointerTypes");
  }
}

}
