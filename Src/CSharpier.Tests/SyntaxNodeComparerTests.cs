using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests;

using Microsoft.CodeAnalysis;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class SyntaxNodeComparerTests
{
    [Test]
    public void Class_Not_Equal_Namespace()
    {
        var left = "class ClassName { }";
        var right = "namespace Namespace { }";

        var result = CompareSource(left, right);

        ResultShouldBe(
            result,
            """
            ----------------------------- Original: Around Line 0 -----------------------------
            class ClassName { }
            ----------------------------- Formatted: Around Line 0 -----------------------------
            namespace Namespace { }

            """
        );
    }

    [Test]
    public void Class_Not_Equal_Class_Different_Whitespace()
    {
        var left = "class ClassName { }";
        var right =
            @"class ClassName {
}";

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void Constructor_Missing_Base()
    {
        var left =
            @"public class ConstructorWithBase
{
    public ConstructorWithBase(string value)
        : base(value)
    {
        return;
    }
}";
        var right =
            @"public class ConstructorWithBase
{
    public ConstructorWithBase(string value)
    {
        return;
    }
}
";

        var result = CompareSource(left, right);

        ResultShouldBe(
            result,
            @"----------------------------- Original: Around Line 2 -----------------------------
public class ConstructorWithBase
{
    public ConstructorWithBase(string value)
        : base(value)
    {
        return;
    }
}
----------------------------- Formatted: Around Line 2 -----------------------------
public class ConstructorWithBase
{
    public ConstructorWithBase(string value)
    {
        return;
    }
}
"
        );
    }

    [Test]
    public void MissingAttribute()
    {
        var left =
            @"class Resources
{
    [Obsolete]
    public Resources()
    {
    }
}
";
        var right =
            @"class Resources
{
    public Resources() { }
}
";

        var result = CompareSource(left, right);

        ResultShouldBe(
            result,
            @"----------------------------- Original: Around Line 2 -----------------------------
class Resources
{
    [Obsolete]
    public Resources()
    {
    }
}
----------------------------- Formatted: Around Line 2 -----------------------------
class Resources
{
    public Resources() { }
}
"
        );
    }

    [Test]
    public void SeperatedSyntaxLists()
    {
        var left =
            @"namespace Insite.Automated.Core
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

        var right =
            @"namespace Insite.Automated.Core
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

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void MissingSemiColon()
    {
        var left =
            @"public enum Enum
{
    Integer,
    String,
};";
        var right =
            @"public enum Enum
{
    Integer,
    String,
}";
        var result = CompareSource(left, right);

        ResultShouldBe(
            result,
            @"----------------------------- Original: Around Line 4 -----------------------------
    Integer,
    String,
};
----------------------------- Formatted: Around Line 0 -----------------------------
public enum Enum
{
    Integer,
    String,
}
"
        );
    }

    [Test]
    public void Extra_SyntaxTrivia_Should_Work()
    {
        var left = "  public class ClassName { }";
        var right = "public class ClassName { }";

        var result = CompareSource(left, right);
        result.Should().BeEmpty();
    }

    [Test]
    public void Mismatched_Syntax_Trivia_Should_Print_Error()
    {
        var left =
            @"// leading comment
public class ClassName { }";
        var right = "public class ClassName { }";

        var result = CompareSource(left, right);
        ResultShouldBe(
            result,
            @"----------------------------- Original: Around Line 0 -----------------------------
// leading comment
public class ClassName { }
----------------------------- Formatted: Around Line 0 -----------------------------
public class ClassName { }
"
        );
    }

    [Test]
    public void Long_Mismatched_Syntax_Trivia_Should_Print_Error()
    {
        var left =
            @"// 1
// 2
// 3
// 4
// 5
// 6
public class ClassName { }";
        var right =
            @"// 1
// 2
// 3
// 4
// 5
// 7
public class ClassName { }";

        var result = CompareSource(left, right);
        ResultShouldBe(
            result,
            @"----------------------------- Original: Around Line 5 -----------------------------
// 4
// 5
// 6
public class ClassName { }
----------------------------- Formatted: Around Line 5 -----------------------------
// 4
// 5
// 7
public class ClassName { }
"
        );
    }

    [TestCase("@")]
    [TestCase("@$")]
    [TestCase("$@")]
    public void Mismatched_Line_Endings_In_Verbatim_String_Should_Not_Print_Error(string start)
    {
        var left =
            "public class ClassName\n{\npublic string Value = "
            + start
            + "\"EndThisLineWith\r\nEndThisLineWith\n\";\n}";
        var right =
            "public class ClassName\n{\npublic string Value = "
            + start
            + "\"EndThisLineWith\nEndThisLineWith\n\";\n}";

        var result = CompareSource(left, right);
        result.Should().BeEmpty();
    }

    [Test]
    public void Mismatched_Disabled_Text_Should_Not_Print_Error()
    {
        var left =
            @"class ClassName
{
#if DEBUG
    public string    Tester;
#endif
}";
        var right =
            @"class ClassName
{
#if DEBUG
    public string Tester;
#endif
}
";
        var result = CompareSource(left, right);
        result.Should().BeEmpty();
    }

    [Test]
    public void Comments_Should_Ignore_Indent_Width()
    {
        var left =
            @"class ClassName
{
    /// multiline
    /// multiline
    private string field;

    /* multiline
     * multiline
     */
    private string field;
}";

        var right =
            @"class ClassName
{
  /// multiline
  /// multiline
  private string field;

  /* multiline
   * multiline
   */
  private string field;
}";

        var result = CompareSource(left, right);
        result.Should().BeEmpty();
    }

    [Test]
    public void Handles_Ternary_In_Disabled_Text()
    {
        var left =
            @"namespace Namespace;
#if DEBUG
class Class
{
    void MethodName()
    {
        var x = true?1:2;
    }
}
#endif
";

        var right =
            @"namespace Namespace;
#if DEBUG
class Class
{
    void MethodName()
    {
        var x = true ? 1 : 2;
    }
}
#endif
";

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void Unsorted_Modifiers_Pass_Validation()
    {
        var left = @"static public class { }";

        var right = @"public static class { }";

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void Sorted_Usings_Pass_Validation()
    {
        var left =
            @"using Monday;
using Zebra;
using Apple;
using Banana;
using Yellow;";

        var right =
            @"using Apple;
using Banana;
using Monday;
using Yellow;
using Zebra;
";

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void Extra_Usings_Fails_Validation()
    {
        var left =
            @"using Zebra;
using Apple;
";

        var right =
            @"using Apple;
using Monday;
using Zebra;
";

        var result = CompareSource(left, right);

        ResultShouldBe(
            result,
            @"----------------------------- Original: Around Line 0 -----------------------------
using Zebra;
using Apple;
----------------------------- Formatted: Around Line 0 -----------------------------
using Apple;
using Monday;
using Zebra;
"
        );
    }

    [Test]
    public void Sorted_Usings_With_Header_Pass_Validation()
    {
        var left =
            @"// some copyright 

using Zebra;
using Apple;
";

        var right =
            @"// some copyright1

using Apple;
using Zebra;
";

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [TestCase("namespace Namespace { }")]
    [TestCase("namespace Namespace;")]
    public void Usings_With_Directives_Pass_Validation(string content)
    {
        // The problem is that the #endif leading trivia to the ClassDeclaration
        // which then fails the compare
        // that class could be an interface, enum, top level statement, etc
        // so there doesn't seem to be any good way to handle this
        // it will only fail the compare the first time that it sorts, so doesn't seem worth fixing
        var left =
            @$"#if DEBUG
using System;
#endif
using System.IO;

{content}
";

        var right =
            @$"using System.IO;
#if DEBUG
using Microsoft;
#endif

{content}
";

        var result = CompareSource(left, right, reorderedUsingsWithDisabledText: true);

        result.Should().BeEmpty();
    }

    [Test]
    public void RawStringLiterals_Work_With_Moving_Indentation()
    {
        var left = """
public class ClassName
{
    public void MethodName()
    {
        CallMethod(
                \"\"\"
                SomeString
                \"\"\"
        );
    }
}
""";

        var right = """
public class ClassName
{
   public void MethodName()
   {
       CallMethod(
           \"\"\"
           SomeString
           \"\"\"
       );
   }
}
""";

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void RawStringLiterals_Work_With_Moving_Indentation_2()
    {
        var left = """"
            CallMethod(CallMethod(
                """
                SomeString
                """, someValue));
            """";
        var right = """"
            CallMethod(
                CallMethod(
                    """
                    SomeString
                    """,
                    someValue
                )
            );
            """";

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void RawStringLiterals_Work_With_Moving_Indentation_3()
    {
        var left = """"
            CallMethod(CallMethod(
                $$"""
                SomeString
                """, someValue));
            """";
        var right = """"
            CallMethod(
                CallMethod(
                    $$"""
                    SomeString
                    """,
                    someValue
                )
            );
            """";

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void RawStringLiterals_Work_With_Moving_Indentation_And_Tabs_To_Spaces()
    {
        var left = """"
	var someValue = $"""
		SomeRawStringWithTab
	""";

"""";
        var right = """"
    var someValue = $"""
        	SomeRawStringWithTab
        """;

"""";

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void RawStringLiterals_Errors_When_Tabs_Change_To_Spaces()
    {
        var left = """"
var someValue = $"""
    	SomeRawStringWithTab
    """;

"""";
        var right = """"
var someValue = $"""
        SomeRawStringWithTab
    """;

"""";

        var result = CompareSource(left, right);

        result.Should().NotBeEmpty();
    }

    [Test]
    public void RawStringLiterals_Error_With_Adding_Indentation_When_There_Was_None()
    {
        var left = """"
            var x = $$"""

            """;
            """";
        var right = """"
            var x = $$"""

                """;
            """";

        var result = CompareSource(left, right);

        result.Should().NotBeEmpty();
    }

    [Test]
    public void CollectionExpression_Works_With_Adding_Trailing_Comma_When_There_Was_None()
    {
        var left = """
            int[][] a =
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9]
            ];
            """;
        var right = """
            int[][] a =
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
            ];
            """;

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void CollectionExpression_Works_With_Removing_Trailing_Comma_When_There_Was_One()
    {
        var left = "int[] a = [1, 2, 3,];";
        var right = "int[] a = [1, 2, 3];";

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void AnonymousObjectCreationExpression_Works_With_Adding_Trailing_Comma_When_There_Was_None()
    {
        var left = """
            public dynamic a = new
            {
                One = "One",
                Two = "Two",
                ThreeThreeThree = "ThreeThreeThree"
            };
            """;
        var right = """
            public dynamic a = new
            {
                One = "One",
                Two = "Two",
                ThreeThreeThree = "ThreeThreeThree",
            };
            """;

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void AnonymousObjectCreationExpression_Works_With_Removing_Trailing_Comma_When_There_Was_One()
    {
        var left = "public dynamic a = new { Property = true, }";
        var right = "public dynamic a = new { Property = true }";

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void InitializerExpression_Works_With_Adding_Trailing_Comma_When_There_Was_None()
    {
        var left = """
            string[] a =
            {
                "someLongValue_____________________________________",
                "someLongValue_____________________________________"
            };
            """;
        var right = """
            string[] a =
            {
                "someLongValue_____________________________________",
                "someLongValue_____________________________________",
            };
            """;

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void InitializerExpression_Works_With_Removing_Trailing_Comma_When_There_Was_One()
    {
        var left = "int[] a = { 1, 2, };";
        var right = "int[] a = { 1, 2 };";

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void SwitchExpression_Works_With_Adding_Trailing_Comma_When_There_Was_None()
    {
        var left = """
            int switchExpressionNoTrailingComma()
            {
                return 1 switch { 1 => 100, _ => throw new global::System.Exception() };
            }
            """;
        var right = """
            int switchExpressionNoTrailingComma()
            {
                return 1 switch
                {
                    1 => 100,
                    _ => throw new global::System.Exception(),
                };
            }
            """;

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void ListPattern_Works_With_Adding_Trailing_Comma_When_There_Was_None()
    {
        var left = """
            object listPatternTrailingComma(object list)
            {
                return list switch
                {
                    [var elem] => elem * elem,
                    [] => 0,
                    [..] elems => elems.Sum(e => e + e)
                };
            }
            """;
        var right = """
            object listPatternTrailingComma(object list)
            {
                return list switch
                {
                    [var elem] => elem * elem,
                    [] => 0,
                    [..] elems => elems.Sum(e => e + e),
                };
            }
            """;

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void EnumDeclaration_Works_With_Adding_Trailing_Comma_When_There_Was_None()
    {
        var left = """
            public enum Enum
            {
                Foo = 1
            }
            """;
        var right = """
            public enum Enum
            {
                Foo = 1,
            }
            """;

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void EnumDeclaration_Works_Without_Removing_Trailing_Comma_When_There_Already_Was_One()
    {
        var left = """
            public enum Enum { Foo = 1, }
            """;
        var right = """
            public enum Enum
            {
                Foo = 1,
            }
            """;

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    [Test]
    public void EnumDeclaration_Works_With_Adding_Trailing_Comma_When_There_Was_None_Inside_If_Directive()
    {
        var left = """
            #if DEBUG
            public enum Enum
            {
                Foo = 1
            }
            #endif
            """;
        var right = """
            #if DEBUG
            public enum Enum
            {
                Foo = 1,
            }
            #endif
            """;

        var result = CompareSource(left, right);

        result.Should().BeEmpty();
    }

    private static void ResultShouldBe(string actual, string expected)
    {
        actual.ReplaceLineEndings().Should().Be(expected.ReplaceLineEndings());
    }

    private static string CompareSource(
        string left,
        string right,
        bool reorderedUsingsWithDisabledText = false
    )
    {
        var result = new SyntaxNodeComparer(
            left,
            right,
            false,
            reorderedUsingsWithDisabledText,
            SourceCodeKind.Regular,
            CancellationToken.None
        ).CompareSource();

        return result;
    }
}
