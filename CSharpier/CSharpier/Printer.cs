using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        public static Doc BreakParent = new BreakParent();

        public static Doc HardLine = Concat(new LineDoc { Type = LineDoc.LineType.Hard }, BreakParent);
        public static Doc Line = new LineDoc { Type = LineDoc.LineType.Normal };
        public static Doc SoftLine = new LineDoc { Type = LineDoc.LineType.Soft };

        public static Doc Concat(Parts parts)
        {
            return new Concat
            {
                Parts = parts
            };
        }

        public static Doc Concat(params Doc[] parts)
        {
            return new Concat
            {
                Parts = new Parts(parts)
            };
        }

        public static Doc String(string value)
        {
            return new StringDoc(value);
        }

        public static Doc Join(Doc separator, IEnumerable<Doc> array)
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

        public static Doc Group(Doc contents)
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

        public static Doc Indent(Doc contents)
        {
            return new IndentDoc
            {
                Contents = contents
            };
        }

        private bool NotNull(SyntaxToken value)
        {
            return value.RawKind != 0;
        }

        // TODO kill after conversion from typescript
        private bool NotNull(SyntaxNode value)
        {
            return value != null;
        }

        private void PrintAttributeLists(SyntaxList<AttributeListSyntax> attributeLists, Parts parts)
        {
            parts.Push(String("TODO AttributeLists"));
        }

        private Doc PrintModifiers(SyntaxTokenList modifiers)
        {
            return String("TODO Modifiers");
        }

        private Doc PrintStatements(object statements, Doc separator, Doc endOfLineDoc = null)
        {
            return String("TODO Statements");
        }

        private Doc PrintCommaList(IEnumerable<Doc> docs)
        {
            return Join(Concat(String(","), Line), docs);
        }

        private void PrintConstraintClauses(object value, Parts parts)
        {
            parts.Push("TODO ConstraintClauses");
        }
    }
}