using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class TypeParameterConstraintClauseTests : BaseTest
    {
        [Test]
        public void TypeParameterConstraintClauses()
        {
            this.RunTest("TypeParameterConstraintClause", "TypeParameterConstraintClauses");
        }
    }
}
