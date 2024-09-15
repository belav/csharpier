using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class DisabledTextComparerTests
{
    [Test]
    public void IsCodeBasicallyEqual_Should_Return_True_For_Basic_Case()
    {
        var before = "public string    Tester;";

        var after = """
            public
            string Tester;

            """;

        DisabledTextComparer.IsCodeBasicallyEqual(before, after).Should().BeTrue();
    }

    [Test]
    public void Squash_Should_Work_For_Trailing_Space_And_New_Line()
    {
        var before = "    public \n";

        var after = "    public\n";

        Squash(before).Should().Be(Squash(after));
    }

    [Test]
    public void Squash_Should_Work_For_Trailing_Comma()
    {
        var before = """
            public enum Enum
            {
                Foo = 1,
            }
            """;

        var after = "public enum Enum {Foo=1}\n";

        Squash(before).Should().Be(Squash(after));
    }

    [Test]
    public void Squash_Should_Work_For_Trailing_Comma_With_Attribute()
    {
        var before = """
            [
                SomeAttribute,
            ]
            public class ClassName { }
            """;

        var after = """
            [SomeAttribute]
            public class ClassName { }

            """;

        Squash(before).Should().Be(Squash(after));
    }

    [Test]
    public void Squash_Should_Work_With_Pointer_Stuff()
    {
        var before = """
                    [MethodImpl (MethodImplOptions.InternalCall)]
                    private static unsafe extern void ApplyUpdate_internal (IntPtr base_assm, byte* dmeta_bytes, int dmeta_length, byte *dil_bytes, int dil_length, byte *dpdb_bytes, int dpdb_length);
            """;

        var after = """
            [MethodImpl(MethodImplOptions.InternalCall)]
                    private static unsafe extern void ApplyUpdate_internal(
                        IntPtr base_assm,
                        byte* dmeta_bytes,
                        int dmeta_length,
                        byte* dil_bytes,
                        int dil_length,
                        byte* dpdb_bytes,
                        int dpdb_length
                    );

            """;
        Squash(before).Should().Be(Squash(after));
    }

    [Test]
    public void Squash_Should_Work_With_Commas()
    {
        var before = """

                        TypeBuilder typeBuilder = moduleBuilder.DefineType(assemblyName.FullName
                            , TypeAttributes.Public |
                              TypeAttributes.Class |
                              TypeAttributes.AutoClass |
                              TypeAttributes.AnsiClass |
                              TypeAttributes.BeforeFieldInit |
                              TypeAttributes.AutoLayout
                            , null);

            """;

        var after = """

            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                            assemblyName.FullName,
                            TypeAttributes.Public
                                | TypeAttributes.Class
                                | TypeAttributes.AutoClass
                                | TypeAttributes.AnsiClass
                                | TypeAttributes.BeforeFieldInit
                                | TypeAttributes.AutoLayout,
                            null
                        );

            """;
        Squash(before).Should().Be(Squash(after));
    }

    [Test]
    public void Squash_Should_Work_With_Period()
    {
        var before = """

                        var options2 = (ProxyGenerationOptions)proxy.GetType().
                            GetField("proxyGenerationOptions", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

            """;

        var after = """

                        var options2 = (ProxyGenerationOptions)proxy.GetType()
                            .GetField("proxyGenerationOptions", BindingFlags.Static | BindingFlags.NonPublic)
                            .GetValue(null);

            """;
        Squash(before).Should().Be(Squash(after));
    }

    [Test]
    public void Squash_Should_Work_With_Starting_Indent()
    {
        var before = @"array = new ulong[] { (ulong)dy.Property_ulong };";

        var after = @"            array = new ulong[] { (ulong)dy.Property_ulong };";
        Squash(before).Should().Be(Squash(after));
    }

    private static string Squash(string value)
    {
        return TestableDisabledTextComparer.TestSquash(value);
    }

    private class TestableDisabledTextComparer : DisabledTextComparer
    {
        public static string TestSquash(string value)
        {
            return Squash(value);
        }
    }
}
