using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ParenthesizedVariableDesignationTests : BaseTest
    {
        [Test]
        public void BasicParenthesizedVariableDesignation()
        {
            this.RunTest(
                "ParenthesizedVariableDesignation",
                "BasicParenthesizedVariableDesignation");
        }
    }
}
