using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.MakeRefExpression
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
