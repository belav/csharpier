using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class SwitchExpressionTests : BaseTest
    {
        [Test]
        public void SwitchExpressions()
        {
            this.RunTest("SwitchExpression", "SwitchExpressions");
        }
    }
}
