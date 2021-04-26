using System;
using CSharpier.DocTypes;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests
{
    public class DocPrinterTests
    {
        private static string NewLine = System.Environment.NewLine;

        [Test]
        public void Basic_Concat()
        {
            var doc = Docs.Concat("1", "2", "3");

            PrintedDocShouldBe(doc, "123");
        }

        [Test]
        public void Concat_With_Hardline()
        {
            var doc = Docs.Concat("1", Docs.HardLine, "3");

            PrintedDocShouldBe(doc, $"1{NewLine}3");
        }

        [Test]
        public void Concat_With_Line()
        {
            var doc = Docs.Concat("1", Docs.Line, "3");

            PrintedDocShouldBe(doc, $"1{NewLine}3");
        }

        [Test]
        public void Group_With_Line()
        {
            var doc = Docs.Group(Docs.Concat("1", Docs.Line, "3"));

            PrintedDocShouldBe(doc, "1 3");
        }

        [Test]
        public void Group_With_HardLine()
        {
            var doc = Docs.Group(Docs.Concat("1", Docs.HardLine, "3"));

            PrintedDocShouldBe(doc, $"1{NewLine}3");
        }

        [Test]
        public void Group_With_Line_And_Hardline()
        {
            var doc = Docs.Group(
                Docs.Concat("1", Docs.Line, "2", Docs.HardLine, "3")
            );

            PrintedDocShouldBe(doc, $"1{NewLine}2{NewLine}3");
        }

        [Test]
        public void Group_With_Line_And_BreakParent()
        {
            var doc = Docs.Group(
                Docs.Concat(
                    "1",
                    Docs.Line,
                    "2",
                    Docs.Line,
                    "3",
                    Docs.BreakParent
                )
            );

            PrintedDocShouldBe(doc, $"1{NewLine}2{NewLine}3");
        }

        [Test]
        public void Indent_With_BreakParent()
        {
            var doc = Docs.Concat(
                "0",
                Docs.Group(
                    Docs.Indent(
                        Docs.Concat(
                            Docs.SoftLine,
                            "1",
                            Docs.Line,
                            "2",
                            Docs.Line,
                            "3",
                            Docs.BreakParent
                        )
                    )
                )
            );

            PrintedDocShouldBe(
                doc,
                $"0{NewLine}    1{NewLine}    2{NewLine}    3"
            );
        }

        [Test]
        public void Large_Group_Concat_With_Line()
        {
            const string longText = "LongTextLongTextLongTextLongText";
            var doc = Docs.Group(
                Docs.Concat(longText, Docs.Line, longText, Docs.Line, longText)
            );

            PrintedDocShouldBe(
                doc,
                $"{longText}{NewLine}{longText}{NewLine}{longText}"
            );
        }

        [Test]
        public void Indent_With_Hardline()
        {
            var doc = Docs.Concat(
                "0",
                Docs.Indent(Docs.Concat(Docs.HardLine, "1", Docs.HardLine, "2"))
            );

            PrintedDocShouldBe(doc, $"0{NewLine}    1{NewLine}    2");
        }

        [Test]
        public void Two_Indents_With_Hardline()
        {
            var doc = Docs.Concat(
                "0",
                Docs.Concat(
                    Docs.Indent(Docs.Concat(Docs.HardLine, "1")),
                    Docs.HardLine,
                    Docs.Indent(Docs.Concat(Docs.HardLine, "2"))
                )
            );

            PrintedDocShouldBe(doc, $"0{NewLine}    1{NewLine}{NewLine}    2");
        }

        [Test]
        public void Lines_Removed_From_Beginning()
        {
            var doc = Docs.Concat(Docs.HardLine, "1");

            PrintedDocShouldBe(doc, "1");
        }

        [Test]
        public void Literal_Lines_Removed_From_Beginning()
        {
            var doc = Docs.Concat(Docs.LiteralLine, "1");

            PrintedDocShouldBe(doc, $"1");
        }

        [Test]
        public void ForceFlat_Prevents_Breaking()
        {
            var doc = Docs.ForceFlat("1", Docs.HardLine, "2");

            PrintedDocShouldBe(doc, "1 2");
        }

        [Test]
        public void LiteralLine_Trims_Space()
        {
            var doc = Docs.Concat(
                "{",
                Docs.Indent(Docs.HardLine, "indent", Docs.LiteralLine),
                "}"
            );

            PrintedDocShouldBe(doc, $"{{{NewLine}    indent{NewLine}}}");
        }

        [Test]
        public void LiteralLine_Does_Not_Trim_Output_Or_Indent_Next_Line()
        {
            var doc = Docs.Concat(
                "(",
                Docs.Indent(
                    Docs.HardLine,
                    "1",
                    " ",
                    Docs.LiteralLine,
                    "2",
                    Docs.HardLine,
                    "3"
                ),
                ")"
            );

            PrintedDocShouldBe(
                doc,
                $"({NewLine}    1 {NewLine}2{NewLine}    3)"
            );
        }

        [Test]
        public void ForceFlat_Prevents_Breaking_With_Long_Content()
        {
            var doc = Docs.ForceFlat(
                "lkjasdkfljalsjkdfkjlasdfjklakljsdfjkasdfkljsdafjk",
                Docs.Line,
                "jaksdlflkasdlfjkajklsdfkljasfjklaslfkjasdfkj"
            );

            PrintedDocShouldBe(
                doc,
                $"lkjasdkfljalsjkdfkjlasdfjklakljsdfjkasdfkljsdafjk jaksdlflkasdlfjkajklsdfkljasfjklaslfkjasdfkj"
            );
        }

        [Test]
        public void Long_Statement_With_Line_Should_Not_Break_Unrelated_Group()
        {
            var doc = Docs.Concat(
                "1",
                Docs.Group(Docs.Line, Docs.Concat("2")),
                Docs.HardLine,
                Docs.Concat(
                    "1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111",
                    Docs.Line,
                    "2"
                )
            );
            PrintedDocShouldBe(
                doc,
                $"1 2{NewLine}1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111{NewLine}2"
            );
        }

        [Test]
        public void ForceFlat_Should_Be_Included_In_Fits_Logic_Of_Printer()
        {
            var doc = Docs.Group(
                Docs.ForceFlat(
                    Docs.Concat(
                        "1111111111",
                        " ",
                        "1111111111",
                        " ",
                        "1111111111",
                        " ",
                        "1111111111",
                        " ",
                        "1111111111"
                    )
                ),
                Docs.Line,
                Docs.ForceFlat(
                    Docs.Concat(
                        "1111111111",
                        " ",
                        "1111111111",
                        " ",
                        "1111111111",
                        " ",
                        "1111111111",
                        " ",
                        "1111111111"
                    )
                )
            );

            PrintedDocShouldBe(
                doc,
                $"1111111111 1111111111 1111111111 1111111111 1111111111{NewLine}1111111111 1111111111 1111111111 1111111111 1111111111"
            );
        }

        [Test]
        public void IfBreak_Should_Print_Flat_Contents()
        {
            var doc = Docs.Group(Docs.IfBreak("break", "flat"));

            PrintedDocShouldBe(doc, "flat");
        }

        [Test]
        public void IfBreak_Should_Print_Break_Contents()
        {
            var doc = Docs.Group(Docs.HardLine, Docs.IfBreak("break", "flat"));

            PrintedDocShouldBe(doc, "break");
        }

        [Test]
        public void IfBreak_Should_Print_Break_Contents_When_Group_Does_Not_Fit()
        {
            var doc = Docs.Group(
                "another",
                Docs.Line,
                Docs.IfBreak("break", "flat")
            );

            PrintedDocShouldBe(doc, $"another{NewLine}break", 10);
        }

        [Test]
        public void IfBreak_Should_Print_Flat_Contents_When_GroupId_Does_Not_Break()
        {
            var doc = Docs.Concat(
                Docs.GroupWithId("1", "1"),
                Docs.IfBreak("break", "flat", "1")
            );

            PrintedDocShouldBe(doc, "1flat");
        }

        [Test]
        public void IfBreak_Should_Print_Break_Contents_When_GroupId_Breaks()
        {
            var doc = Docs.Concat(
                "1",
                Docs.GroupWithId("hl", Docs.HardLine),
                Docs.IfBreak("break", "flat", "hl")
            );

            PrintedDocShouldBe(doc, $"1{NewLine}break");
        }

        [TestCase(" ")]
        [TestCase("   ")]
        public void Trim_Should_Trim_Current_Line(string indent)
        {
            var doc = Docs.Concat($"1{indent}", Docs.Trim);

            PrintedDocShouldBe(doc, "1");
        }

        [TestCase("1")]
        [TestCase("")]
        public void Trim_Should_Not_Trim_NonWhitespace(string value)
        {
            var doc = Docs.Concat(value, Docs.Trim);

            PrintedDocShouldBe(doc, value);
        }

        [Test]
        public void Trim_Should_Trim_Indentation()
        {
            var doc = Docs.Concat(
                "{",
                Docs.Indent(
                    Docs.HardLine,
                    "1",
                    Docs.HardLine,
                    Docs.Trim,
                    "#if"
                ),
                Docs.HardLine,
                "}"
            );

            PrintedDocShouldBe(doc, $"{{{NewLine}    1{NewLine}#if{NewLine}}}");
        }

        [Test]
        public void Trim_Should_Not_Affect_Fits()
        {
            var doc = Docs.Group("test    ", Docs.Trim, Docs.Line, "test");

            PrintedDocShouldBe(doc, "test test", 10);
        }

        [Test]
        public void TrailingComment_Does_Not_Get_Extra_Space()
        {
            var doc = Docs.Concat(
                "x",
                Docs.TrailingComment("// comment", CommentType.SingleLine),
                " ",
                "y"
            );

            PrintedDocShouldBe(doc, $"x // comment{NewLine}y");
        }

        [Test]
        public void TrailingComment_Does_Not_Get_Extra_Space_From_Line()
        {
            var doc = Docs.Group(
                "x",
                Docs.TrailingComment("// comment", CommentType.SingleLine),
                Docs.Line,
                "y"
            );

            PrintedDocShouldBe(doc, $"x // comment{NewLine}y");
        }

        [Test]
        public void HardLineIfNoPreviousLine_Should_Insert_Line_If_There_Is_Not_One()
        {
            var doc = Docs.Concat("1", Docs.HardLineIfNoPreviousLine, "2");

            PrintedDocShouldBe(doc, $"1{NewLine}2");
        }

        [Test]
        public void HardLineIfNoPreviousLine_Should_Not_Insert_Line_If_There_Is_One()
        {
            var doc = Docs.Concat(
                "1",
                Docs.HardLine,
                Docs.HardLineIfNoPreviousLine,
                "2"
            );

            PrintedDocShouldBe(doc, $"1{NewLine}2");
        }

        [Test]
        public void HardLineIfNoPreviousLine_Does_Not_Blow_Up()
        {
            var doc = Docs.Concat(Docs.HardLineIfNoPreviousLine, "1");

            PrintedDocShouldBe(doc, "1");
        }

        [Test]
        public void HardLineIfNoPreviousLine_Should_Not_Insert_After_Indented_HardLine()
        {
            var doc = Docs.Concat(
                Docs.Indent(
                    "1",
                    Docs.HardLine,
                    Docs.HardLineIfNoPreviousLine,
                    "2"
                )
            );

            PrintedDocShouldBe(doc, $"1{NewLine}    2");
        }

        [Test]
        public void Scratch()
        {
            var doc = "";
            var result = Print(doc);
            result.Should().Be("");
        }

        private static void PrintedDocShouldBe(
            Doc doc,
            string expected,
            int width = 80
        ) {
            var result = Print(doc, width);

            result.Should().Be(expected);
        }

        private static string Print(Doc doc, int width = 80)
        {
            return DocPrinter.Print(
                    doc,
                    // TODO if we could use auto now, this wouldn't be needed
                    // or maybe all the tests get converted to lf
                    new Options
                    {
                        Width = width,
                        EndOfLine = "\r\n"
                    }
                )
                .TrimEnd('\r', '\n');
        }
    }
}
