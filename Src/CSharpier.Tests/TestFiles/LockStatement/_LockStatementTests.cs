using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class LockStatementTests : BaseTest
{
  [Test]
  public void BasicLockStatement()
  {
    this.RunTest("LockStatement", "BasicLockStatement");
  }
}

}
