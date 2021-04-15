using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests
{
    public class DocUtilitiesTests
    {
        [Test]
        public void RemoveInitialDoubleHardLine_Should_Handle_Empty_List()
        {
            var doc = new List<Doc>();

            DocUtilities.RemoveInitialDoubleHardLine(doc);

            doc.Should().BeEmpty();
        }

        [Test]
        public void RemoveInitialDoubleHardLine_Should_Remove_Null()
        {
            var doc = new List<Doc> { Docs.Null };

            DocUtilities.RemoveInitialDoubleHardLine(doc);

            doc.Should().BeEmpty();
        }

        [Test]
        public void RemoveInitialDoubleHardLine_Should_Not_Remove_Simple_HardLine() {
            var doc = new List<Doc> { Docs.HardLine };

            DocUtilities.RemoveInitialDoubleHardLine(doc);

            doc.Should().HaveCount(1);
        }

        [Test]
        public void RemoveInitialDoubleHardLine_Should_Remove_Simple_Double_HardLine() {
            var doc = new List<Doc> { Docs.HardLine, Docs.HardLine };

            DocUtilities.RemoveInitialDoubleHardLine(doc);

            doc.Should().HaveCount(1);
        }

        [Test]
        public void RemoveInitialDoubleHardLine_Should_Not_Remove_Concated_HardLine() {
            var concat = Docs.Concat(Docs.HardLine);
            var doc = new List<Doc> { concat };

            DocUtilities.RemoveInitialDoubleHardLine(doc);

            concat.Contents.Should().BeEquivalentTo(Docs.HardLine);
        }

        [Test]
        public void RemoveInitialDoubleHardLine_Should_Remove_Concated_HardLine() {
            var concat = Docs.Concat(Docs.HardLine, Docs.HardLine);
            var doc = new List<Doc> { concat };

            DocUtilities.RemoveInitialDoubleHardLine(doc);

            concat.Contents.Should().ContainSingle();
        }

        [Test]
        public void RemoveInitialDoubleHardLine_Should_Not_Remove_Deep_Concated_HardLine() {
            var concat = Docs.Concat(Docs.HardLine);
            var doc = new List<Doc> { Docs.Concat(concat) };

            DocUtilities.RemoveInitialDoubleHardLine(doc);

            concat.Contents.Should().BeEquivalentTo(Docs.HardLine);
        }

        [Test]
        public void RemoveInitialDoubleHardLine_Should_Remove_Deep_Concated_HardLine() {
            var concat = Docs.Concat(Docs.HardLine, Docs.HardLine);
            var doc = new List<Doc> { Docs.Concat(concat) };

            DocUtilities.RemoveInitialDoubleHardLine(doc);

            concat.Contents.Should().ContainSingle();
        }

        [Test]
        public void RemoveInitialDoubleHardLine_Should_Remove_Single_HardLine() {
            var concat = Docs.Concat(
                Docs.HardLine,
                Docs.HardLine,
                Docs.HardLine
            );
            var doc = new List<Doc> { Docs.Concat(concat) };

            DocUtilities.RemoveInitialDoubleHardLine(doc);

            concat.Contents.Should().HaveCount(2);
        }

        [Test]
        public void RemoveInitialDoubleHardLine_Should_Not_Remove_Indented_HardLine() {
            var indent = Docs.Indent(Docs.HardLine);
            var doc = new List<Doc> { indent };

            DocUtilities.RemoveInitialDoubleHardLine(doc);

            indent.Contents.Should().BeEquivalentTo(Docs.HardLine);
        }

        [Test]
        public void RemoveInitialDoubleHardLine_Should_Remove_Indented_HardLine() {
            var indent = Docs.Indent(Docs.HardLine);
            var doc = new List<Doc> { Docs.HardLine, indent };

            DocUtilities.RemoveInitialDoubleHardLine(doc);

            indent.Contents.Should().Be(Docs.Null);
        }

        [Test]
        public void RemoveInitialDoubleHardLine_Should_Not_Remove_Deep_Indented_HardLine() {
            var indent = Docs.Indent(Docs.HardLine);
            var doc = new List<Doc> { Docs.Indent(indent) };

            DocUtilities.RemoveInitialDoubleHardLine(doc);

            indent.Contents.Should().BeEquivalentTo(Docs.HardLine);
        }

        [Test]
        public void RemoveInitialDoubleHardLine_Should_Remove_Grouped_Double_HardLine() {
            var contents = new List<Doc> { Docs.HardLine, Docs.HardLine };
            var doc = Docs.Group(contents);

            DocUtilities.RemoveInitialDoubleHardLine(doc);

            contents.Should().ContainSingle();
        }

        [Test]
        public void RemoveInitialDoubleHardLine_Should_Only_Remove_Initial_HardLines() {
            var doc = Docs.Concat("1", Docs.HardLine, Docs.HardLine);

            DocUtilities.RemoveInitialDoubleHardLine(doc);

            doc.Should()
                .BeEquivalentTo(Docs.Concat("1", Docs.HardLine, Docs.HardLine));
        }
    }
}
