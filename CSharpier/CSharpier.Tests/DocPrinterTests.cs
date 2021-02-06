using FluentAssertions;
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
            var doc = Group(Indent(Concat(SoftLine, "1", Line, "2", Line, "3", BreakParent)));

            var result = this.Print(doc);

            result.Should().Be("\r\n    1\r\n    2\r\n    3");
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
            var doc = Indent(Concat(HardLine, "1", HardLine, "2"));

            var result = this.Print(doc);

            result.Should().Be($"\r\n    1\r\n    2");
        }

        [Test]
        public void Two_Indents_With_Hardline()
        {
            var doc = Concat(Indent(Concat(HardLine, "1")),
                HardLine,
                Indent(Concat(HardLine, "2")));

            var result = this.Print(doc);

            result.Should().Be($"\r\n    1\r\n\r\n    2");
        }

        [Test]
        public void Scratch()
        {
            var doc = " ";
            var result = this.Print(doc);
            result.Should().Be(" ");
        }

        private string Print(Doc doc)
        {
            return new DocPrinter().Print(doc, new Options());
        }

        public static Doc HardLine = Printer.HardLine;
        public static Doc Line = Printer.Line;
        public static Doc SoftLine = Printer.SoftLine;
        public static Doc BreakParent = Printer.BreakParent;

        public static Doc Group(Doc contents)
        {
            return Printer.Group(contents);
        }

        public static Doc Concat(Parts parts)
        {
            return Printer.Concat(parts);
        }

        public static Doc Concat(params Doc[] parts)
        {
            return Printer.Concat(parts);
        }

        public static Doc Indent(Doc contents)
        {
            return Printer.Indent(contents);
        }
    }
}