using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class CompilationUnitTests : BaseTest
{
  [Test]
  public void MultipleClasses()
  {
    this.RunTest("CompilationUnit", "MultipleClasses");
  }
}

}
