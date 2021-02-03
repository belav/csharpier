using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public class Formatter
    {
        // TODO we should make this work in parallel to speed things up
        public string Format(string code)
        {
            var rootNode = CSharpSyntaxTree.ParseText(code).GetRoot();

            var document = new Printer().Print(rootNode);

            return PrintDocument(document);
        }

        private Indent RootIndent()
        {
            return new Indent
            {
                Value = "",
                Length = 0,
                Queue = new List<IndentType>()
            };
        }

        private string PrintDocument(Doc document)
        {
            var width = 120; // TODO options
            var newLine = Environment.NewLine; // TODO options
            var position = 0;

            var currentStack = new Stack<PrintCommand>();
            currentStack.Push(new PrintCommand
            {
                Doc = document,
                Indent = this.RootIndent(),
                IsModeBreak = true,
            });

            var output = new System.Text.StringBuilder();
            var shouldRemeasure = false;

            var lineSuffix = new List<PrintCommand>();

            while (currentStack.TryPop(out var command))
            {
                switch (command.Doc)
                {
                    case StringDoc stringBuilder:
                        // TODO new line stuff
                        output.Append(stringBuilder.Value);
                        break;
                    case Concat concat:
                        for (var x = concat.Parts.TheParts.Count - 1; x >= 0; x--)
                        {
                            currentStack.Push(new PrintCommand
                            {
                                Doc = concat.Parts.TheParts[x],
                                IsModeBreak = command.IsModeBreak,
                                Indent = command.Indent
                            });
                        }

                        break;
                    case IndentDoc indentBuilder:
                        currentStack.Push(new PrintCommand
                        {
                            Indent = MakeIndent(command.Indent),
                            IsModeBreak = command.IsModeBreak,
                            Doc = indentBuilder.Contents
                        });

                        break;
                    case LineDoc line:
                        if (!command.IsModeBreak)
                        {
                            if (line.Type != LineDoc.LineType.Hard)
                            {
                                if (line.Type != LineDoc.LineType.Soft)
                                {
                                    output.Append(' ');

                                    position += 1;
                                }

                                break;
                            }

                            // This line was forced into the output even if we
                            // were in flattened mode, so we need to tell the next
                            // group that no matter what, it needs to remeasure
                            // because the previous measurement didn't accurately
                            // capture the entire expression (this is necessary
                            // for nested groups)
                            shouldRemeasure = true;
                        }
                        else
                        {
                            if (lineSuffix.Any())
                            {
                                currentStack.Push(command);
                                lineSuffix.Reverse();
                                foreach (var otherCommand in lineSuffix)
                                {
                                    currentStack.Push(otherCommand);
                                }

                                lineSuffix.Clear();
                                break;
                            }

                            if (line.IsLiteral)
                            {
                                // TODO what is this?
                                // if (ind.root) {
                                //     out.push(newLine, ind.root.value);
                                //     pos = ind.root.length;
                                // } else {
                                output.Append(newLine);
                                position = 0;
                            }
                            else
                            {
                                position -= Trim(output);
                                output.Append(newLine + command.Indent.Value);
                                position = command.Indent.Length;
                            }

                            break;
                        }

                        break;
                    case Group group:
                        switch (command.IsModeBreak)
                        {
                            case false:
                                if (!shouldRemeasure)
                                {
                                    currentStack.Push(new PrintCommand
                                    {
                                        Indent = command.Indent,
                                        IsModeBreak = group.Break,
                                        Doc = group.Contents,
                                    });
                                    break;
                                }

                                continue;
                            case true:
                                shouldRemeasure = false;

                                var next = new PrintCommand
                                {
                                    Indent = command.Indent,
                                    IsModeBreak = false,
                                    Doc = group.Contents
                                };

                                var rem = width - position;

                                if (!group.Break && Fits(next, currentStack, rem))
                                {
                                    currentStack.Push(next);
                                }
                                else
                                {
                                    // TODO expandedStates is a big complicated thing here, but I don't think I'll use it?
                                    // TODO can't this just push the same next as above but flipping the break mode?
                                    currentStack.Push(new PrintCommand
                                    {
                                        Indent = command.Indent,
                                        IsModeBreak = true,
                                        Doc = group.Contents
                                    });
                                }

                                break;
                        }

                        // TODO we may not use ids for groups
                        // if (doc.id)
                        // {
                        //     groupModeMap[doc.id] = cmds[cmds.length - 1][1];
                        // }

                        break;


                    case BreakParent breakParent: // this doesn't seem to be used in here
                        break;
                    default:
                        throw new Exception("didn't handle " + command.Doc);
                }
            }

            return
                output.ToString();
        }

        private bool Fits(PrintCommand next, Stack<PrintCommand> currentStack, int rem)
        {
            return true;
            // TODO this is huge!!
            throw new NotImplementedException();
        }


        // TODO there is more going on here with dedent and number/string align
        private Indent MakeIndent(Indent indent)
        {
            // TODO options;
            var useTabs = false;
            var tabWidth = 4;
            var queue = indent.Queue;
            queue.Add(new IndentType
            {
                Type = "Indent"
            });
            var value = "";
            var length = 0;
            var lastTabs = 0;

            var lastSpaces = 0;
            foreach (var part in queue)
            {
                switch (part.Type)
                {
                    case "Indent":
                        flush();
                        if (useTabs)
                        {
                            addTabs(1);
                        }
                        else
                        {
                            addSpaces(tabWidth);
                        }

                        break;
                    case "stringAlign":
                        flush();
                        value += part.Number;
                        // TODO huh? length += part.n.length;
                        break;
                    case "numberAlign":
                        lastTabs += 1;
                        // TODO huh? lastSpaces += part.n;
                        break;
                    default:
                        throw new Exception(part.Type);
                }
            }

            flushSpaces();

            void addTabs(int count)
            {
                value += new string('\t', count);
                length += tabWidth * count;
            }

            void addSpaces(int count)
            {
                value += new string(' ', count);
                length += count;
            }

            void flush()
            {
                if (useTabs)
                {
                    flushTabs();
                }
                else
                {
                    flushSpaces();
                }
            }

            void flushTabs()
            {
                if (lastTabs > 0)
                {
                    addTabs(lastTabs);
                }

                resetLast();
            }

            void flushSpaces()
            {
                if (lastSpaces > 0)
                {
                    addSpaces(lastSpaces);
                }

                resetLast();
            }

            ;

            void resetLast()
            {
                lastTabs = 0;
                lastSpaces = 0;
            }

            ;
            return new Indent
            {
                Value = value,
                Length = length,
                Queue = queue
            };
        }

        private int Trim(System.Text.StringBuilder stringBuilder)
        {
            if (stringBuilder.Length == 0)
            {
                return 0;
            }

            var trimCount = 0;
            var i = stringBuilder.Length - 1;
            for (; i >= 0; i--)
            {
                if (!char.IsWhiteSpace(stringBuilder[i]))
                {
                    break;
                }

                trimCount++;
            }

// TODO what does this do?
// if (out.length && typeof out[out.length - 1] === "string") {
//     const trimmed = out[out.length - 1].replace(/[\t ]*$/, "");
//     trimCount += out[out.length - 1].length - trimmed.length;
//         out[out.length - 1] = trimmed;
// }
            return trimCount;
        }
    }

    internal class IndentType
    {
        public string Type { get; set; }
        public int Number { get; set; }
    }

    internal class PrintCommand
    {
        public Indent Indent { get; set; }
        public bool IsModeBreak { get; set; }
        public Doc Doc { get; set; }
    }

    internal class Indent
    {
        public string Value { get; set; }
        public int Length { get; set; }
        public List<IndentType> Queue { get; set; }
    }
}