using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class SwitchStatementTests : BaseTest
{
  [Test]
  public void SwitchStatements()
  {
    this.RunTest("SwitchStatement", "SwitchStatements");
  }
}

}
