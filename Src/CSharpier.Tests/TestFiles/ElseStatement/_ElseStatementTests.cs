using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class ElseStatementTests : BaseTest
{
  [Test]
  public void ElseStatements()
  {
    this.RunTest("ElseStatement", "ElseStatements");
  }
}

}
