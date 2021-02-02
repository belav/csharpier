using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public class IndentBuilder : Builder
    {
        public Builder Contents { get; set; }
    }
    
    public class StringBuilder : Builder
    {
        public string Value { get; set; }
        
        public StringBuilder(string value)
        {
            this.Value = value;
        }
    }
    
    public class LineBuilder : Builder
    {
        public enum LineType
        {
            Normal,
            Hard,
            Soft
        }
        
        public LineType Type { get; set; }
        public bool IsLiteral { get; set; }
    }

    public class Group : Builder
    {
        public Builder Contents { get; set; }
    }
    
    public class BreakParent : Builder
    {
    }

    public class Concat : Builder
    {
        public Parts Parts { get; set; }
    }

    public abstract class Builder
    {
        public string Type { get; set; }
    }

    public static class Builders
    {
        public static Builder BreakParent = new BreakParent();

        public static Builder HardLine = Builders.Concat(new LineBuilder { Type = LineBuilder.LineType.Hard }, Builders.BreakParent);

        public static Builder Concat(Parts parts)
        {
            return new Concat
            {
                Parts = parts
            };
        }

        public static Builder Concat(params Builder[] parts)
        {
            return new Concat
            {
                Parts = new Parts(parts)
            };
        }

        public static Builder String(string value)
        {
            return new StringBuilder(value);
        }

        public static Builder Join(Builder separator, IEnumerable<Builder> array)
        {
            var parts = new Parts();

            var list = array.ToList();
            
            for (var x = 0; x < list.Count; x++) {
                if (x != 0) {
                    parts.Push(separator);
                }

                parts.Push(list[x]);
            }

            return Concat(parts);
        }

        public static Builder Group(Builder contents)
        {
            return new Group
            {
                Contents = contents,
                // TODO options if I use them?
                // id: opts.id,
                // break: !!opts.shouldBreak,
                // expandedStates: opts.expandedStates,
            };
        }

        public static Builder Indent(Builder contents)
        {
            return new IndentBuilder
            {
                Contents = contents
            };
        }
    }


    public partial class Printer
    {
        public Builder Print(SyntaxNode syntaxNode)
        {
            if (syntaxNode is CompilationUnitSyntax compilationUnit)
            {
                return this.PrintCompilationUnitSyntax(compilationUnit);
            }
            else if (syntaxNode is NamespaceDeclarationSyntax namespaceDeclaration)
            {
                return this.PrintNamespaceDeclarationSyntax(namespaceDeclaration);
            }
            else if (syntaxNode is IdentifierNameSyntax identifierName)
            {
                return this.PrintIdentifierNameSyntax(identifierName);
            }
            else if (syntaxNode is UsingDirectiveSyntax usingDirectiveSyntax)
            {
                return this.PrintUsingDirectiveSyntax(usingDirectiveSyntax);
            }
            else if (syntaxNode is QualifiedNameSyntax qualifiedNameSyntax)
            {
                return this.PrintQualifiedNameSyntax(qualifiedNameSyntax);
            }

            throw new Exception("Can't handle " + syntaxNode.GetType().Name);
        }

        private Builder PrintQualifiedNameSyntax(QualifiedNameSyntax node)
        {
            return Builders.Concat(this.Print(node.Left), Builders.String("."), this.Print(node.Right));
        }

        public Builder PrintUsingDirectiveSyntax(UsingDirectiveSyntax node)
        {
            var parts = new Parts();
            parts.Push("using ");
            if (node.StaticKeyword.RawKind != 0) {
                parts.Push("static ");
            }

            parts.Push(this.Print(node.Name), Builders.String(";"));

            return Builders.Concat(parts);
        }
        
        public Builder PrintIdentifierNameSyntax(IdentifierNameSyntax node)
        {
            return Builders.String(node.Identifier.Text);
        }

        public Builder PrintCompilationUnitSyntax(CompilationUnitSyntax node)
        {
            return this.Print(node.Members[0]);
        }

        public Builder PrintNamespaceDeclarationSyntax(NamespaceDeclarationSyntax node)
        {
            var parts = new Parts();
            foreach (var modifier in node.Modifiers)
            {
                parts.Push(modifier.Text);
                parts.Push(" ");
            }

            parts.Push("namespace");
            parts.Push(" ");
            parts.Push(this.Print(node.Name));
            if (node.Usings.Count > 0)
            {
                parts.Push(Builders.HardLine, Builders.String("{"));

                var innerParts = new Parts();
                innerParts.Push(Builders.HardLine);
                
                innerParts.Push(Builders.Join(Builders.HardLine, node.Usings.Select(this.PrintUsingDirectiveSyntax)));
                
                parts.Push(Builders.Indent(Builders.Concat(innerParts)));

                parts.Push(Builders.HardLine, Builders.String("}"));
            }
            else
            {
                parts.Push(" { }");   
            }

            return Builders.Concat(parts);
        }
    }
}