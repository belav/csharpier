using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class VariableDeclarationTests : BaseTest
{
  [Test]
  public void VariableDeclarations()
  {
    this.RunTest("VariableDeclaration", "VariableDeclarations");
  }
}

}
