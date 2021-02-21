using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class SwitchExpressionTests : BaseTest
    {
        [Test]
        public void BasicSwitchExpression()
        {
            this.RunTest("SwitchExpression", "BasicSwitchExpression");
        }
        [Test]
        public void SwitchWithRecursivePattern()
        {
            this.RunTest("SwitchExpression", "SwitchWithRecursivePattern");
        }
    }
}
