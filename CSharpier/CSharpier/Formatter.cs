using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public class Formatter
    {
        // TODO we should make this work in parallel to speed things up
        public string Format(string code, Options options)
        {
            var rootNode = CSharpSyntaxTree.ParseText(code).GetRoot();

            var document = new Printer().Print(rootNode);

            if (options.PrintDocTree)
            {
                return this.PrintDocTree(document, "");
            }
            
            return new DocPrinter().Print(document, new Options());
        }

        private string PrintDocTree(Doc document, string indent)
        {
            switch (document)
            {
                case StringDoc stringDoc:
                    return indent + "\"" + stringDoc.Value + "\"";
                case Concat concat:
                    var result = indent + "concat:" + Environment.NewLine;
                    foreach (var child in concat.Contents)
                    {
                        result += this.PrintDocTree(child, indent + "    ") + Environment.NewLine;
                    }
                    return result;
                case LineDoc lineDoc:
                    return indent + lineDoc.Type;
                case BreakParent breakParent:
                    return indent + "breakParent";
                case IndentDoc indentDoc:
                    return indent + "indent:" + Environment.NewLine + this.PrintDocTree(indentDoc.Contents, indent + "    ");
                case Group group:
                    return indent + "group:" + Environment.NewLine + this.PrintDocTree(group.Contents, indent + "    ");
                default:
                    throw new Exception("Can't handle " + document);
            }
        }
    }
}