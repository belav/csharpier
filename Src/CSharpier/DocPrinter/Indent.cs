using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpier.DocPrinter
{
    public class Indent
    {
        public string Value = string.Empty;
        public int Length;
        public IList<IndentType>? TypesForTabs;
    }

    public class IndentType
    {
        public bool IsAlign { get; set; }
        public int AlignWidth { get; set; }
    }

    public static class IndentBuilder
    {
        public static Indent MakeRoot()
        {
            return new();
        }

        public static Indent MakeIndent(Indent indent, PrinterOptions printerOptions)
        {
            return GenerateIndent(indent, printerOptions);
        }

        private static Indent GenerateIndent(Indent indent, PrinterOptions printerOptions)
        {
            if (!printerOptions.UseTabs)
            {
                return new Indent
                {
                    Value = indent.Value + new string(' ', printerOptions.TabWidth),
                    Length = indent.Length + printerOptions.TabWidth
                };
            }

            if (indent.TypesForTabs != null)
            {
                return MakeIndentWithTypesForTabs(indent, new IndentType(), printerOptions);
            }

            return new Indent
            {
                Value = indent.Value + "\t",
                Length = indent.Length + printerOptions.TabWidth
            };
        }

        public static Indent MakeAlign(Indent indent, int alignment, PrinterOptions printerOptions)
        {
            if (printerOptions.UseTabs)
            {
                return MakeIndentWithTypesForTabs(
                    indent,
                    new IndentType { IsAlign = true, AlignWidth = alignment },
                    printerOptions
                );
            }
            else
            {
                return new Indent
                {
                    Value = indent.Value + new string(' ', alignment),
                    Length = indent.Length + alignment
                };
            }
        }

        // when using tabs we need to sometimes replace the spaces from align with tabs
        // trailing aligns stay as spaces, but any aligns before a tab get converted to a single tab
        // see https://github.com/prettier/prettier/blob/main/commands.md#align
        private static Indent MakeIndentWithTypesForTabs(
            Indent indent,
            IndentType nextIndentType,
            PrinterOptions printerOptions
        ) {
            List<IndentType> types;

            // if it doesn't exist yet, then all values on it are regular indents, not aligns
            if (indent.TypesForTabs == null)
            {
                types = new List<IndentType>();
                for (var x = 0; x < indent.Value.Length; x++)
                {
                    types.Add(new IndentType());
                }
            }
            else
            {
                var placeTab = false;
                types = new List<IndentType>(indent.TypesForTabs);
                for (var x = types.Count - 1; x >= 0; x--)
                {
                    if (!types[x].IsAlign)
                    {
                        placeTab = true;
                    }

                    if (placeTab)
                    {
                        types[x] = new IndentType();
                    }
                }
            }

            types.Add(nextIndentType);

            var length = 0;
            var value = new StringBuilder();
            foreach (var indentType in types)
            {
                if (indentType.IsAlign)
                {
                    value.Append(' ', indentType.AlignWidth);
                    length += indentType.AlignWidth;
                }
                else
                {
                    value.Append('\t');
                    length += printerOptions.TabWidth;
                }
            }

            return new Indent { Length = length, Value = value.ToString(), TypesForTabs = types };
        }
    }
}
