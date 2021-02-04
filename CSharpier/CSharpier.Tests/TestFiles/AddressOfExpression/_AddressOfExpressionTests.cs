using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class AddressOfExpressionTests : BaseTest
    {
        [Test]
        public void BasicAddressOfExpression()
        {
            this.RunTest("AddressOfExpression", "BasicAddressOfExpression");
        }
    }
}
