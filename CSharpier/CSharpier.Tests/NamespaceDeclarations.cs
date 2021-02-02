using System;
using System.Diagnostics;
using NUnit.Framework;

namespace CSharpier.Tests
{
    public class NamespaceDeclarations
    {
        [Test]
        public void BasicNamespace()
        {
            var code = "namespace Namespace { }";

            var formatter = new Formatter();

            var result = formatter.Format(code);
            
            Assert.AreEqual(code, result);
        }
        
        [Test]
        public void NamespaceWithModifier()
        {
            var code = "public namespace Namespace { }";

            var formatter = new Formatter();

            var result = formatter.Format(code);
            
            Assert.AreEqual(code, result);
        }

        [Test]
        public void NamespaceWithUsings()
        {
            var code = @"public namespace Namespace
{
    using System;
    using System.Text;
}";

            var formatter = new Formatter();

            var result = formatter.Format(code);

            Assert.AreEqual(code, result);
        }
    }
}