using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace CSharpier.Core.Tests
{
    public class SyntaxNodeComparerTests
    {
        [Test]
        public void Class_Not_Equal_Namespace()
        {
            var left = "class ClassName { }";
            var right = @"namespace Namespace { }";

            var result = this.AreEqual(left, right);

            result.MismatchedPath.Should().Be("Root-Members[0]");
        }
        
        [Test]
        public void Class_Not_Equal_Class_Different_Whitespace()
        {
            var left = "class ClassName { }";
            var right = @"class ClassName {
}";

            var result = this.AreEqual(left, right);
            
            result.MismatchedPath.Should().Be(null);
        }

        [Test]
        public void Missing_Attribute_Should_Mean_False()
        {
            var left = @"class Resources
{
    [Obsolete]
    public Resources()
    {
    }
}
";
            var right = @"class Resources
{
    public Resources() { }
}
";

            var result = this.AreEqual(left, right);

            result.MismatchedPath.Should().Be("Root-Members[0]-Members[0]-AttributeLists-Count(1 != 0)");
        }

        [Test]
        public void SeperatedSyntaxLists()
        {
            var left = @"namespace Insite.Automated.Core
{
    using System;

    public class DropdownAttribute : Attribute
    {
        public bool IgnoreIfNotPresent { get; set; }

        public DropdownAttribute()
        {
        }

        public DropdownAttribute(bool ignoreIfNotPresent)
        {
            this.IgnoreIfNotPresent = ignoreIfNotPresent;
        }
    }
}";

            var right = @"namespace Insite.Automated.Core
{
    using System;

    public class DropdownAttribute : Attribute
    {
        public bool IgnoreIfNotPresent { get; set; }

        public DropdownAttribute() { }

        public DropdownAttribute(bool ignoreIfNotPresent)
        {
            this.IgnoreIfNotPresent = ignoreIfNotPresent;
        }
    }
}
";

            var result = this.AreEqual(left, right);

            result.MismatchedPath.Should().BeNull();
        }

        
        [Test]
        // TODO we should write out the old/new version somewhere, then I can write a units that looks at them for me.
        [Ignore("ljkasdf")]
        public void Blah()
        {
            var left = "";
            var right = "";
            var result = this.AreEqual(left, right);

            result.MismatchedPath.Should().BeNull();
        }

        private AreEqualResult AreEqual(string left, string right)
        {
            var cSharpParseOptions = new CSharpParseOptions(LanguageVersion.CSharp9);
            var leftNode = CSharpSyntaxTree.ParseText(left, cSharpParseOptions);
            var rightNode = CSharpSyntaxTree.ParseText(right, cSharpParseOptions);


            return new SyntaxNodeComparer().AreEqualIgnoringWhitespace(leftNode.GetRoot(), rightNode.GetRoot(), "Root");
        }
    }
}