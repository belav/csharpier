using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class ConversionOperatorDeclarationTests : BaseTest
{
  [Test]
  public void ConversionOperatorDeclarations()
  {
    this.RunTest(
      "ConversionOperatorDeclaration",
      "ConversionOperatorDeclarations"
    );
  }
}

}
