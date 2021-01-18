using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using NUnit.Framework;
using Parser;

namespace ParserTests
{
    [TestFixture]
    public class JsonWriterTests
    {
        [Test]
        public void Blah()
        {
            var code = File.ReadAllText(@"C:\Projects\csharpier\prettier-plugin-csharpier\Samples\AllInOne.cs");

            var rootNode = CSharpSyntaxTree.ParseText(code).GetRoot() as CompilationUnitSyntax;

            var stringBuilder = new StringBuilder();
            SyntaxNodeJsonWriter.WriteCompilationUnitSyntax(stringBuilder, rootNode);

            var result = stringBuilder.ToString();

            File.WriteAllText(@"c:\temp\custom.unindented.json", result);
            
            var theJson = JsonConvert.DeserializeObject(result);

            File.WriteAllText(@"c:\temp\custom.json", JsonConvert.SerializeObject(theJson, Formatting.Indented));
        }
    }
}