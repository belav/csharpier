using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpier.DocPrinter
{
    public record Indent(string Value, int Length);

    public static class IndentBuilder
    {
        public static Indent MakeRoot()
        {
            return new Indent(string.Empty, 0);
        }

        public static Indent Make(Indent indent, PrinterOptions printerOptions)
        {
            return GenerateIndent(indent, printerOptions);
        }

        private static Indent GenerateIndent(Indent indent, PrinterOptions printerOptions)
        {
            if (printerOptions.UseTabs)
            {
                return new Indent(indent.Value + "\t", indent.Length + printerOptions.TabWidth);
            }
            else
            {
                return new Indent(
                    indent.Value + new string(' ', printerOptions.TabWidth),
                    indent.Length + printerOptions.TabWidth
                );
            }
        }
    }
}
