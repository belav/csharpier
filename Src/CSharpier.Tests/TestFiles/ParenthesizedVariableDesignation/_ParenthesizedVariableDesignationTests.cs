using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ParenthesizedVariableDesignationTests : BaseTest
    {
        [Test]
        public void ParenthesizedVariableDesignations()
        {
            this.RunTest("ParenthesizedVariableDesignation", "ParenthesizedVariableDesignations");
        }
    }
}
