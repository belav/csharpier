using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
