using System;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CSharpier.Tests
{
    public class DocPrinterTests
    {
        [Test]
        public void Basic_Concat()
        {
            var doc = Concat("1", "2", "3");

            var result = this.Print(doc);

            result.Should().Be("123");
        }

        [Test]
        public void Concat_With_Hardline()
        {
            var doc = Concat("1", HardLine, "3");

            var result = this.Print(doc);

            result.Should().Be("1\r\n3");
        }

        [Test]
        public void Concat_With_Line()
        {
            var doc = Concat("1", Line, "3");

            var result = this.Print(doc);

            result.Should().Be("1\r\n3");
        }

        [Test]
        public void Group_With_Line()
        {
            var doc = Group(Concat("1", Line, "3"));

            var result = this.Print(doc);

            result.Should().Be("1 3");
        }

        [Test]
        public void Group_With_HardLine()
        {
            var doc = Group(Concat("1", HardLine, "3"));

            var result = this.Print(doc);

            result.Should().Be("1\r\n3");
        }

        [Test]
        public void Group_With_Line_And_Hardline()
        {
            var doc = Group(Concat("1", Line, "2", HardLine, "3"));

            var result = this.Print(doc);

            result.Should().Be("1\r\n2\r\n3");
        }

        [Test]
        public void Group_With_Line_And_BreakParent()
        {
            var doc = Group(Concat("1", Line, "2", Line, "3", BreakParent));

            var result = this.Print(doc);

            result.Should().Be("1\r\n2\r\n3");
        }

        [Test]
        public void Indent_With_BreakParent()
        {
            var doc = Concat(
                "0",
                Group(
                    Indent(
                        Concat(
                            SoftLine,
                            "1",
                            Line,
                            "2",
                            Line,
                            "3",
                            BreakParent))));

            var result = this.Print(doc);

            result.Should().Be("0\r\n    1\r\n    2\r\n    3");
        }

        [Test]
        public void Large_Group_Concat_With_Line()
        {
            const string longText = "LongTextLongTextLongTextLongText";
            var doc = Group(Concat(longText, Line, longText, Line, longText));

            var result = this.Print(doc);

            result.Should().Be($"{longText}\r\n{longText}\r\n{longText}");
        }

        [Test]
        public void Indent_With_Hardline()
        {
            var doc = Concat("0", Indent(Concat(HardLine, "1", HardLine, "2")));

            var result = this.Print(doc);

            result.Should().Be($"0\r\n    1\r\n    2");
        }

        [Test]
        public void Two_Indents_With_Hardline()
        {
            var doc = Concat(
                "0",
                Concat(
                    Indent(Concat(HardLine, "1")),
                    HardLine,
                    Indent(Concat(HardLine, "2"))));

            var result = this.Print(doc);

            result.Should().Be($"0\r\n    1\r\n\r\n    2");
        }

        [Test]
        public void Lines_Removed_From_Beginning()
        {
            var doc = Concat(HardLine, "1");

            var result = this.Print(doc);

            result.Should().Be($"1");
        }

        [Test]
        public void Literal_Lines_Removed_From_Beginning()
        {
            var doc = Concat(LiteralLine, "1");

            var result = this.Print(doc);

            result.Should().Be($"1");
        }

        [Test]
        public void ForceFlat_Prevents_Breaking()
        {
            var doc = ForceFlat("1", HardLine, "2");

            var result = this.Print(doc);

            result.Should().Be("1 2");
        }

        [Test]
        public void LiteralLine_Trims_Space()
        {
            var doc = Concat("{", Indent(HardLine, "indent", LiteralLine), "}");

            var result = this.Print(doc);

            result.Should().Be("{\r\n    indent\r\n}");
        }

        [Test]
        public void HardLine_LiteralLine_Skips_HardLine_And_Trims()
        {
            var doc = Concat(
                "{",
                Indent(HardLine, LiteralLine, "noindent"),
                HardLine,
                "}");

            var result = this.Print(doc);

            result.Should().Be("{\r\nnoindent\r\n}");
        }

        [Test]
        public void HardLine_LiteralLine_Skips_HardLine()
        {
            var doc = Concat("1", HardLine, LiteralLine, "2");

            var result = this.Print(doc);

            result.Should().Be("1\r\n2");
        }

        [Test]
        public void ForceFlat_Prevents_Breaking_With_Long_Content()
        {
            var doc = ForceFlat(
                "lkjasdkfljalsjkdfkjlasdfjklakljsdfjkasdfkljsdafjk",
                Line,
                "jaksdlflkasdlfjkajklsdfkljasfjklaslfkjasdfkj");

            var result = this.Print(doc);

            result.Should().Be($"lkjasdkfljalsjkdfkjlasdfjklakljsdfjkasdfkljsdafjk jaksdlflkasdlfjkajklsdfkljasfjklaslfkjasdfkj");
        }

        [Test]
        public void Long_Statement_With_Line_Should_Not_Break_Unrelated_Group()
        {
            var doc = Concat(
                "1",
                Group(Line, Concat("2")),
                HardLine,
                Concat(
                    "1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111",
                    Line,
                    "2"));
            var result = this.Print(doc);
            result.Should()
                .Be(
                    @"1 2
1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111
2");
        }

        [Test]
        public void Scratch()
        {
            var doc = "";
            var result = this.Print(doc);
            result.Should().Be("");
        }

        private string Print(Doc doc)
        {
            return new DocPrinter().Print(doc, new Options())
                .TrimEnd('\r', '\n');
        }

        public static Doc HardLine => Printer.HardLine;
        public static Doc LiteralLine => Printer.LiteralLine;
        public static Doc Line => Printer.Line;
        public static Doc SoftLine => Printer.SoftLine;
        public static Doc BreakParent => new BreakParent();

        public static Doc SpaceIfNoPreviousComment
            => Printer.SpaceIfNoPreviousComment;

        public static Doc Group(Doc contents)
        {
            return Printer.Group(contents);
        }

        public static Doc Group(params Doc[] parts)
        {
            return Printer.Group(parts);
        }

        public static Doc Concat(Parts parts)
        {
            return Printer.Concat(parts);
        }

        public static Doc Concat(params Doc[] parts)
        {
            return Printer.Concat(parts);
        }

        public static Doc Indent(params Doc[] contents)
        {
            return Printer.Indent(contents);
        }

        public static Doc ForceFlat(params Doc[] parts)
        {
            return Printer.ForceFlat(parts);
        }
    }
}
