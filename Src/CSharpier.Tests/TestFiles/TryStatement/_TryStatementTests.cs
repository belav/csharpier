using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class TryStatementTests : BaseTest
{
  [Test]
  public void BasicTryStatement()
  {
    this.RunTest("TryStatement", "BasicTryStatement");
  }
  [Test]
  public void TryStatementWithNoCatchDeclaration()
  {
    this.RunTest("TryStatement", "TryStatementWithNoCatchDeclaration");
  }
  [Test]
  public void TryStatementWithWhen()
  {
    this.RunTest("TryStatement", "TryStatementWithWhen");
  }
}

}
