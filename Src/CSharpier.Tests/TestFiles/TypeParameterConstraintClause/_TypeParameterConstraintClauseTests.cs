using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class TypeParameterConstraintClauseTests : BaseTest
    {
        [Test]
        public void BasicTypeParameterConstraintClause()
        {
            this.RunTest(
                "TypeParameterConstraintClause",
                "BasicTypeParameterConstraintClause");
        }
        [Test]
        public void ConstraintClauses()
        {
            this.RunTest("TypeParameterConstraintClause", "ConstraintClauses");
        }
    }
}
