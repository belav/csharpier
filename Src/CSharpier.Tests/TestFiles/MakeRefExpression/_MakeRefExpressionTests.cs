using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class MakeRefExpressionTests : BaseTest
    {
        [Test]
        public void MakeRefExpressions()
        {
            this.RunTest("MakeRefExpression", "MakeRefExpressions");
        }
    }
}
