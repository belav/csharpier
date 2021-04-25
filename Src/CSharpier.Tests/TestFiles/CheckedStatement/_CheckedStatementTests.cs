using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class CheckedStatementTests : BaseTest
{
  [Test]
  public void CheckedStatements()
  {
    this.RunTest("CheckedStatement", "CheckedStatements");
  }
}

}
