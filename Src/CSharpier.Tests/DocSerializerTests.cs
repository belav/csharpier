using CSharpier.DocTypes;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests
{
    [TestFixture]
    public class DocSerializerTests
    {
        [Test]
        public void Should_Format_Directive()
        {
            var doc = Doc.Directive("1");

            var actual = DocSerializer.Serialize(doc);

            actual.Should().Be("Doc.Directive(\"1\")");
        }

        [Test]
        public void Should_Format_Basic_Types()
        {
            var doc = Doc.Concat(
                Doc.Line,
                Doc.LiteralLine,
                Doc.HardLine,
                Doc.HardLineIfNoPreviousLine,
                Doc.HardLineSkipBreakIfFirstInGroup,
                Doc.HardLineIfNoPreviousLineSkipBreakIfFirstInGroup,
                Doc.SoftLine,
                Doc.Null,
                Doc.Trim,
                "1"
            );

            var actual = DocSerializer.Serialize(doc);

            actual.Should()
                .Be(
                    @"Doc.Concat(
    Doc.Line,
    Doc.LiteralLine,
    Doc.HardLine,
    Doc.HardLineIfNoPreviousLine,
    Doc.HardLineSkipBreakIfFirstInGroup,
    Doc.HardLineIfNoPreviousLineSkipBreakIfFirstInGroup,
    Doc.SoftLine,
    Doc.Null,
    Doc.Trim,
    ""1""
)"
                );
        }

        [Test]
        public void Should_Print_Basic_Group()
        {
            var doc = Doc.Group(Doc.Null, Doc.Null);

            var actual = DocSerializer.Serialize(doc);

            actual.Should()
                .Be(
                    @"Doc.Group(
    Doc.Null,
    Doc.Null
)"
                );
        }

        [Test]
        public void Should_Print_Group_With_Id()
        {
            var doc = Doc.GroupWithId("1", Doc.Null, Doc.Null);

            var actual = DocSerializer.Serialize(doc);

            actual.Should()
                .Be(
                    @"Doc.GroupWithId(
    ""1"",
    Doc.Null,
    Doc.Null
)"
                );
        }

        [Test]
        public void Should_Print_ConditionalGroup()
        {
            var doc = Doc.ConditionalGroup(
                Doc.Concat(Doc.Line, Doc.Line),
                Doc.Concat(Doc.LiteralLine, Doc.LiteralLine)
            );

            var actual = DocSerializer.Serialize(doc);

            actual.Should()
                .Be(
                    @"Doc.ConditionalGroup(
    Doc.Concat(
        Doc.Line,
        Doc.Line
    ),
    Doc.Concat(
        Doc.LiteralLine,
        Doc.LiteralLine
    )
)"
                );
        }

        [Test]
        public void Should_Print_Align()
        {
            var doc = Doc.Align(2, Doc.Null, Doc.Null);

            var actual = DocSerializer.Serialize(doc);

            actual.Should()
                .Be(
                    @"Doc.Align(
    2,
    Doc.Null,
    Doc.Null
)"
                );
        }

        [Test]
        public void Should_Print_ForceFlat()
        {
            var doc = Doc.ForceFlat(Doc.Null, Doc.Null);

            var actual = DocSerializer.Serialize(doc);

            actual.Should()
                .Be(
                    @"Doc.ForceFlat(
    Doc.Null,
    Doc.Null
)"
                );
        }

        [Test]
        public void Should_Print_Indent()
        {
            var doc = Doc.Indent(Doc.Null, Doc.Null);

            var actual = DocSerializer.Serialize(doc);

            actual.Should()
                .Be(
                    @"Doc.Indent(
    Doc.Null,
    Doc.Null
)"
                );
        }

        [Test]
        public void Should_Print_IndentIfBreak()
        {
            var doc = Doc.IndentIfBreak(Doc.Null, "1");

            var actual = DocSerializer.Serialize(doc);

            actual.Should()
                .Be(
                    @"Doc.IndentIfBreak(
    Doc.Null,
    ""1""
)"
                );
        }

        [Test]
        public void Should_Print_IfBreak_With_Id()
        {
            var doc = Doc.IfBreak(Doc.Null, Doc.Line, "1");

            var actual = DocSerializer.Serialize(doc);

            actual.Should()
                .Be(
                    @"Doc.IfBreak(
    Doc.Null,
    Doc.Line,
    ""1""
)"
                );
        }

        [Test]
        public void Should_Print_IfBreak_Without_Id()
        {
            var doc = Doc.IfBreak(Doc.Null, Doc.Line);

            var actual = DocSerializer.Serialize(doc);

            actual.Should()
                .Be(
                    @"Doc.IfBreak(
    Doc.Null,
    Doc.Line
)"
                );
        }

        [TestCase(CommentType.SingleLine)]
        [TestCase(CommentType.MultiLine)]
        public void Should_Print_LeadingComment(CommentType commentType)
        {
            var doc = Doc.LeadingComment("1", commentType);

            var actual = DocSerializer.Serialize(doc);

            actual.Should().Be(@$"Doc.LeadingComment(""1"", CommentType.{commentType})");
        }

        [TestCase(CommentType.SingleLine)]
        [TestCase(CommentType.MultiLine)]
        public void Should_Print_TrailingComment(CommentType commentType)
        {
            var doc = Doc.TrailingComment("1", commentType);

            var actual = DocSerializer.Serialize(doc);

            actual.Should().Be(@$"Doc.TrailingComment(""1"", CommentType.{commentType})");
        }
    }
}
