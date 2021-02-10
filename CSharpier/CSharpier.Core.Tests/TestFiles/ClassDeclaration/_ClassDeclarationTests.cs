using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ClassDeclarationTests : BaseTest
    {
        [Test]
        public void ClassImplementsInterface()
        {
            this.RunTest("ClassDeclaration", "ClassImplementsInterface");
        }
        [Test]
        public void EmptyClass()
        {
            this.RunTest("ClassDeclaration", "EmptyClass");
        }
        [Test]
        public void LongBaseListFormatsOnLines()
        {
            this.RunTest("ClassDeclaration", "LongBaseListFormatsOnLines");
        }
        [Test]
        public void MultipleClassesWithModifiers()
        {
            this.RunTest("ClassDeclaration", "MultipleClassesWithModifiers");
        }
        [Test]
        public void MultipleClassesWithoutModifiers()
        {
            this.RunTest("ClassDeclaration", "MultipleClassesWithoutModifiers");
        }
        [Test]
        public void StaticAbstractClass()
        {
            this.RunTest("ClassDeclaration", "StaticAbstractClass");
        }
    }
}
