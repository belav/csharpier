using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.AddressOfExpression
{
    public class AddressOfExpressionTests : BaseTest
    {
        [Test]
        public void AddressOfExpressions()
        {
            this.RunTest("AddressOfExpression", "AddressOfExpressions");
        }
    }
}
