using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class IfStatementTests : BaseTest
{
  [Test]
  public void IfStatements()
  {
    this.RunTest("IfStatement", "IfStatements");
  }
}

}
