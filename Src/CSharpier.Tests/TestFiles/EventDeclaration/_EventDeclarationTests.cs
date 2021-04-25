using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class EventDeclarationTests : BaseTest
{
  [Test]
  public void BasicEventDeclaration()
  {
    this.RunTest("EventDeclaration", "BasicEventDeclaration");
  }
}

}
