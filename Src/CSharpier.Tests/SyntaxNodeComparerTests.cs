using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace CSharpier.Tests
{
    public class SyntaxNodeComparerTests
    {
        [Test]
        public void Class_Not_Equal_Namespace()
        {
            var left = "class ClassName { }";
            var right = @"namespace Namespace { }";

            var result = this.AreEqual(left, right);

            result.Should().Be(
                @"    Original: Around Line 0
class ClassName { }
    Formatted: Around Line 0
namespace Namespace { }
");
        }

        [Test]
        public void Class_Not_Equal_Class_Different_Whitespace()
        {
            var left = "class ClassName { }";
            var right = @"class ClassName {
}";

            var result = this.AreEqual(left, right);

            result.Should().BeNull();
        }

        [Test]
        public void MissingAttribute()
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

            result.Should().Be(
                @"    Original: Around Line 2
class Resources
{
    [Obsolete]
    public Resources()
    {
    }
}
    Formatted: Around Line 2
class Resources
{
    public Resources() { }
}
");
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

            result.Should().BeNull();
        }


        [Test]
        public void MissingSemiColon()
        {
            var left = @"public enum Enum
{
    Integer,
    String,
};";
            var right = @"public enum Enum
{
    Integer,
    String,
}";
            var result = this.AreEqual(left, right);

            result.Should().Be(
                @"    Original: Around Line 4
    Integer,
    String,
};
    Formatted: Around Line 0
public enum Enum
{
    Integer,
    String,
}
");
        }

        private string AreEqual(string left, string right)
        {
            return new SyntaxNodeComparer(left, right).CompareSource();
        }
    }
}
