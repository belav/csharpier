using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
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
