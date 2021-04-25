using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class DestructorDeclarationTests : BaseTest
{
  [Test]
  public void DestructorDeclarations()
  {
    this.RunTest("DestructorDeclaration", "DestructorDeclarations");
  }
}

}
