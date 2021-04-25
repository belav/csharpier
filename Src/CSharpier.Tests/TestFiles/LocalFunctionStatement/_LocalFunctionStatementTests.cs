using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class LocalFunctionStatementTests : BaseTest
{
  [Test]
  public void LocalFunctionStatements()
  {
    this.RunTest("LocalFunctionStatement", "LocalFunctionStatements");
  }
}

}
