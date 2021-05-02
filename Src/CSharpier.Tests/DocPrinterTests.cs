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
        public void Lines_Allowed()
        {
            var doc = Doc.Concat(Doc.HardLine, "1");

            PrintedDocShouldBe(doc, $"{NewLine}1");
        }

        [Test]
        public void Basic_Concat()
        {
            var doc = Doc.Concat("1", "2", "3");

            PrintedDocShouldBe(doc, "123");
        }

        [Test]
        public void Concat_With_Hardline()
        {
            var doc = Doc.Concat("1", Doc.HardLine, "3");

            PrintedDocShouldBe(doc, $"1{NewLine}3");
        }

        [Test]
        public void Concat_With_Line()
        {
            var doc = Doc.Concat("1", Doc.Line, "3");

            PrintedDocShouldBe(doc, $"1{NewLine}3");
        }

        [Test]
        public void Group_With_Line()
        {
            var doc = Doc.Group(Doc.Concat("1", Doc.Line, "3"));

            PrintedDocShouldBe(doc, "1 3");
        }

        [Test]
        public void Group_With_HardLine()
        {
            var doc = Doc.Group(Doc.Concat("1", Doc.HardLine, "3"));

            PrintedDocShouldBe(doc, $"1{NewLine}3");
        }

        [Test]
        public void Group_With_Line_And_Hardline()
        {
            var doc = Doc.Group(
                Doc.Concat("1", Doc.Line, "2", Doc.HardLine, "3")
            );

            PrintedDocShouldBe(doc, $"1{NewLine}2{NewLine}3");
        }

        [Test]
        public void Group_With_Line_And_BreakParent()
        {
            var doc = Doc.Group(
                Doc.Concat("1", Doc.Line, "2", Doc.Line, "3", Doc.BreakParent)
            );

            PrintedDocShouldBe(doc, $"1{NewLine}2{NewLine}3");
        }

        [Test]
        public void Indent_With_BreakParent()
        {
            var doc = Doc.Concat(
                "0",
                Doc.Group(
                    Doc.Indent(
                        Doc.Concat(
                            Doc.SoftLine,
                            "1",
                            Doc.Line,
                            "2",
                            Doc.Line,
                            "3",
                            Doc.BreakParent
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
            var doc = Doc.Group(
                Doc.Concat(longText, Doc.Line, longText, Doc.Line, longText)
            );

            PrintedDocShouldBe(
                doc,
                $"{longText}{NewLine}{longText}{NewLine}{longText}"
            );
        }

        [Test]
        public void Indent_With_Hardline()
        {
            var doc = Doc.Concat(
                "0",
                Doc.Indent(Doc.Concat(Doc.HardLine, "1", Doc.HardLine, "2"))
            );

            PrintedDocShouldBe(doc, $"0{NewLine}    1{NewLine}    2");
        }

        [Test]
        public void Two_Indents_With_Hardline()
        {
            var doc = Doc.Concat(
                "0",
                Doc.Concat(
                    Doc.Indent(Doc.Concat(Doc.HardLine, "1")),
                    Doc.HardLine,
                    Doc.Indent(Doc.Concat(Doc.HardLine, "2"))
                )
            );

            PrintedDocShouldBe(doc, $"0{NewLine}    1{NewLine}{NewLine}    2");
        }

        [Test]
        public void Lines_Removed_From_Beginning()
        {
            var doc = Doc.Concat(Doc.HardLine, "1");

            PrintedDocShouldBe(doc, "1", trimInitialLines: true);
        }

        [Test]
        public void Literal_Lines_Removed_From_Beginning()
        {
            var doc = Doc.Concat(Doc.LiteralLine, "1");

            PrintedDocShouldBe(doc, $"1");
        }

        [Test]
        public void ForceFlat_Prevents_Breaking()
        {
            var doc = Doc.ForceFlat("1", Doc.HardLine, "2");

            PrintedDocShouldBe(doc, "1 2");
        }

        [Test]
        public void LiteralLine_Trims_Space()
        {
            var doc = Doc.Concat(
                "{",
                Doc.Indent(Doc.HardLine, "indent", Doc.LiteralLine),
                "}"
            );

            PrintedDocShouldBe(doc, $"{{{NewLine}    indent{NewLine}}}");
        }

        [Test]
        public void LiteralLine_Does_Not_Trim_Output_Or_Indent_Next_Line()
        {
            var doc = Doc.Concat(
                "(",
                Doc.Indent(
                    Doc.HardLine,
                    "1",
                    " ",
                    Doc.LiteralLine,
                    "2",
                    Doc.HardLine,
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
            var doc = Doc.ForceFlat(
                "lkjasdkfljalsjkdfkjlasdfjklakljsdfjkasdfkljsdafjk",
                Doc.Line,
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
            var doc = Doc.Concat(
                "1",
                Doc.Group(Doc.Line, Doc.Concat("2")),
                Doc.HardLine,
                Doc.Concat(
                    "1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111",
                    Doc.Line,
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
            var doc = Doc.Group(
                Doc.ForceFlat(
                    Doc.Concat(
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
                Doc.Line,
                Doc.ForceFlat(
                    Doc.Concat(
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
            var doc = Doc.Group(Doc.IfBreak("break", "flat"));

            PrintedDocShouldBe(doc, "flat");
        }

        [Test]
        public void IfBreak_Should_Print_Break_Contents()
        {
            var doc = Doc.Group(Doc.HardLine, Doc.IfBreak("break", "flat"));

            PrintedDocShouldBe(doc, $"{NewLine}break");
        }

        [Test]
        public void IfBreak_Should_Print_Break_Contents_When_Group_Does_Not_Fit()
        {
            var doc = Doc.Group(
                "another",
                Doc.Line,
                Doc.IfBreak("break", "flat")
            );

            PrintedDocShouldBe(doc, $"another{NewLine}break", 10);
        }

        [Test]
        public void IfBreak_Should_Print_Flat_Contents_When_GroupId_Does_Not_Break()
        {
            var doc = Doc.Concat(
                Doc.GroupWithId("1", "1"),
                Doc.IfBreak("break", "flat", "1")
            );

            PrintedDocShouldBe(doc, "1flat");
        }

        [Test]
        public void IfBreak_Should_Print_Break_Contents_When_GroupId_Breaks()
        {
            var doc = Doc.Concat(
                "1",
                Doc.GroupWithId("hl", Doc.HardLine),
                Doc.IfBreak("break", "flat", "hl")
            );

            PrintedDocShouldBe(doc, $"1{NewLine}break");
        }

        [TestCase(" ")]
        [TestCase("   ")]
        public void Trim_Should_Trim_Current_Line(string indent)
        {
            var doc = Doc.Concat($"1{indent}", Doc.Trim);

            PrintedDocShouldBe(doc, "1");
        }

        [TestCase("1")]
        [TestCase("")]
        public void Trim_Should_Not_Trim_NonWhitespace(string value)
        {
            var doc = Doc.Concat(value, Doc.Trim);

            PrintedDocShouldBe(doc, value);
        }

        [Test]
        public void Trim_Should_Trim_Indentation()
        {
            var doc = Doc.Concat(
                "{",
                Doc.Indent(Doc.HardLine, "1", Doc.HardLine, Doc.Trim, "#if"),
                Doc.HardLine,
                "}"
            );

            PrintedDocShouldBe(doc, $"{{{NewLine}    1{NewLine}#if{NewLine}}}");
        }

        [Test]
        public void Trim_Should_Not_Affect_Fits()
        {
            var doc = Doc.Group("test    ", Doc.Trim, Doc.Line, "test");

            PrintedDocShouldBe(doc, "test test", 10);
        }

        [Test]
        public void TrailingComment_Does_Not_Get_Extra_Space()
        {
            var doc = Doc.Concat(
                "x",
                Doc.TrailingComment("// comment", CommentType.SingleLine),
                " ",
                "y"
            );

            PrintedDocShouldBe(doc, $"x // comment{NewLine}y");
        }

        [Test]
        public void TrailingComment_Does_Not_Get_Extra_Space_From_Line()
        {
            var doc = Doc.Group(
                "x",
                Doc.TrailingComment("// comment", CommentType.SingleLine),
                Doc.Line,
                "y"
            );

            PrintedDocShouldBe(doc, $"x // comment{NewLine}y");
        }

        [Test]
        public void HardLineIfNoPreviousLine_Should_Insert_Line_If_There_Is_Not_One()
        {
            var doc = Doc.Concat("1", Doc.HardLineIfNoPreviousLine, "2");

            PrintedDocShouldBe(doc, $"1{NewLine}2");
        }

        [Test]
        public void HardLineIfNoPreviousLine_Should_Not_Insert_Line_If_There_Is_One()
        {
            var doc = Doc.Concat(
                "1",
                Doc.HardLine,
                Doc.HardLineIfNoPreviousLine,
                "2"
            );

            PrintedDocShouldBe(doc, $"1{NewLine}2");
        }

        [Test]
        public void HardLineIfNoPreviousLine_Does_Not_Blow_Up()
        {
            var doc = Doc.Concat(Doc.HardLineIfNoPreviousLine, "1");

            PrintedDocShouldBe(doc, $"{NewLine}1");
        }

        [Test]
        public void HardLineIfNoPreviousLine_Should_Not_Insert_After_Indented_HardLine()
        {
            var doc = Doc.Concat(
                Doc.Indent("1", Doc.HardLine, Doc.HardLineIfNoPreviousLine, "2")
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
            int width = Options.TestingWidth,
            bool trimInitialLines = false
        ) {
            var result = Print(doc, width, trimInitialLines);

            result.Should().Be(expected);
        }

        private static string Print(
            Doc doc,
            int width = Options.TestingWidth,
            bool trimInitialLines = false
        ) {
            return DocPrinter.Print(
                    doc,
                    new Options
                    {
                        Width = width,
                        EndOfLine = Environment.NewLine,
                        TrimInitialLines = trimInitialLines,
                    }
                )
                .TrimEnd('\r', '\n');
        }
    }
}
