using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class ContinueStatementTests : BaseTest
{
  [Test]
  public void BasicContinueStatement()
  {
    this.RunTest("ContinueStatement", "BasicContinueStatement");
  }
}

}
