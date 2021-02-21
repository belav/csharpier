using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class MakeRefExpressionTests : BaseTest
    {
        [Test]
        public void BasicMakeRefExpression()
        {
            this.RunTest("MakeRefExpression", "BasicMakeRefExpression");
        }
    }
}
