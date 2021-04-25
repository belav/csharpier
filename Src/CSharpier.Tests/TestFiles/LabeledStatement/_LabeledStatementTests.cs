using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles {

public class LabeledStatementTests : BaseTest
{
  [Test]
  public void BasicLabeledStatement()
  {
    this.RunTest("LabeledStatement", "BasicLabeledStatement");
  }
}

}
