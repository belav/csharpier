using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpier.DocPrinter
{
    public record IndentType(string Type, int Number);

    public record Indent(string Value, int Length, List<IndentType> Queue);

    public static class IndentBuilder
    {
        public static Indent MakeRoot()
        {
            return new Indent(string.Empty, 0, new List<IndentType>());
        }

        public static Indent Make(Indent indent, PrinterOptions printerOptions)
        {
            return GenerateIndent(indent, newPart: new IndentType("indent", 0), printerOptions);
        }

        private static Indent GenerateIndent(
            Indent indent,
            IndentType newPart,
            PrinterOptions printerOptions
        ) {
            var queue = new List<IndentType>(indent.Queue);
            if (newPart.Type == "dedent")
            {
                queue.RemoveAt(queue.Count - 1);
            }
            else
            {
                queue.Add(newPart);
            }

            var value = new StringBuilder();
            var length = 0;
            var lastTabs = 0;

            var lastSpaces = 0;
            foreach (var part in queue)
            {
                switch (part.Type)
                {
                    case "indent":
                        Flush();
                        if (printerOptions.UseTabs)
                        {
                            AddTabs(1);
                        }
                        else
                        {
                            AddSpaces(printerOptions.TabWidth);
                        }
                        break;
                    default:
                        throw new Exception(part.Type);
                }
            }

            FlushSpaces();

            void AddTabs(int count)
            {
                value.Append('\t', count);
                length += printerOptions.TabWidth * count;
            }

            void AddSpaces(int count)
            {
                value.Append(' ', count);
                length += count;
            }

            void Flush()
            {
                if (printerOptions.UseTabs)
                {
                    FlushTabs();
                }
                else
                {
                    FlushSpaces();
                }
            }

            void FlushTabs()
            {
                if (lastTabs > 0)
                {
                    AddTabs(lastTabs);
                }

                ResetLast();
            }

            void FlushSpaces()
            {
                if (lastSpaces > 0)
                {
                    AddSpaces(lastSpaces);
                }

                ResetLast();
            }

            void ResetLast()
            {
                lastTabs = 0;
                lastSpaces = 0;
            }

            return new Indent(value.ToString(), length, queue);
        }
    }
}
