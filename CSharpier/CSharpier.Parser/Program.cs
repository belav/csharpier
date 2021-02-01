using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Parser;

namespace CSharpier.Parser
{
    class Program
    {
        private const string TestClass = @"
public class ClassName
{
    public void MethodName()
    {
        return;
    }
}

";
        
        static void Main(string[] args)
        {
            var stringToParse = args.Length == 0 ? TestClass : args[0];

            var rootNode = CSharpSyntaxTree.ParseText(stringToParse).GetRoot() as CompilationUnitSyntax;
            
            
            Console.OutputEncoding = Encoding.UTF8;

            var result = WriteWithSyntaxNodeJsonWrite(rootNode);
            
            Console.WriteLine(result);
        }

        private static string WriteWithSyntaxNodeJsonWrite(CompilationUnitSyntax rootNode)
        {
            var stringBuilder = new StringBuilder();
            SyntaxNodeJsonWriter.WriteCompilationUnitSyntax(stringBuilder, rootNode);
            return stringBuilder.ToString();
        }
    }
}