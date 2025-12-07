using System.IO.Abstractions;
using CSharpier.Cli;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests.Cli;

// TODO #1768 figure out what I did to break all of these, it was definitely something in this commit
[TestFixture]
public class IgnoreFileTests
{
    private GitRepository gitRepository = null!;
    private readonly FileSystem fileSystem = new();

    [SetUp]
    public void SetUp()
    {
        this.gitRepository = new(this.fileSystem);
    }

    [TearDown]
    public void TearDown()
    {
        this.gitRepository.DeleteRepoDirectory();
    }

    [Test]
    public void EmptyLines()
    {
        this.GitBasedTest(
            """
# empty lines, nothing is ignored


""",
            ["foo", "bar"]
        );
    }

    [Test]
    public void TrailingWhitespaces()
    {
        this.GitBasedTest(
            """
# trailing whitespaces handled
foo   
""",
            ["foo", "bar"]
        );
    }

    [Test]
    public void SimpleIgnore()
    {
        this.GitBasedTest(
            """
# exclude foo
foo
""",
            ["foo", "bar/foo", "foob/bar", "src/foo/bar", "src/foo/", "fooc"]
        );
    }

    // TODO #1768 definitely need to fix this one and probably the one below it
    [Test]
    public void SimpleIgnore_WithFileInSubdirectory()
    {
        this.GitBasedTest(
            """
bar.txt
""",
            ["src/foo/bar.txt", "foo/bar.txt", "bar.txt"]
        );
    }

    [Test]
    public void SimpleIgnore_WithSubdirs()
    {
        this.GitBasedTest(
            """
# exclude foo/bar
foo/bar
""",
            [
                "src/foo/bar",
                "foo/bar/",
                "foo/bar/char",
                "src/bar/char",
                "a/foo/bar/char",
                "src/testfoo/bar",
            ]
        );
    }

    [Test]
    public void SimpleIgnore_Dir()
    {
        this.GitBasedTest(
            """
foo/
""",
            ["foo/bar", "bar/foo", "foo/har", "tar/foo/bar", "tar/bar/foo"]
        );
    }

    [Test]
    public void SimpleIgnore_Prefix()
    {
        this.GitBasedTest(
            """
foo
""",
            ["src/testfoo", "testfoo", "testfoox", "bar/foo", "bar/tfoo"]
        );
    }

    [Test]
    public void SimpleIgnore_Dotfiles()
    {
        this.GitBasedTest(
            """
.foo/
""",
            [".foo/bar", ".bar/foo", ".foo/har", "tar/.foo/bar", "tar/bar/.foo"]
        );
    }

    [Test]
    public void SimpleIgnore_Dotfiles_WithStar()
    {
        this.GitBasedTest(
            """
.foo/*
""",
            [".foo/bar", ".foo/.foo/bar", ".foo/har"]
        );
    }

    [Test]
    public void SimpleIgnore_Dotfiles_WithStar2()
    {
        this.GitBasedTest(
            """
*.mm.*
""",
            ["file.mm", "commonFile.txt"]
        );
    }

    [Test]
    public void IgnoreDotFiles()
    {
        this.GitBasedTest(
            """
.*
""",
            [
                ".dotfile",
                "no.dotfile",
                "foo/.dotfile",
                "foo/no.dotfile",
                ".bar/nodot",
                ".bar/foo",
                "foo/.dot/adf",
                "bar/foo",
            ]
        );
    }

    [Test]
    public void StartsWithStar()
    {
        this.GitBasedTest(
            """
*.cs
""",
            ["foo.cs", "foo/bar/foo.cs", "foo/bar/bar.csproj"]
        );
    }

    [Test]
    public void StartsWithStar_Negated()
    {
        this.GitBasedTest(
            """
!*.cs
""",
            ["foo.cs", "foo/bar/foo.cs", "foo/bar/bar.csproj"]
        );
    }

    [Test]
    public void StartsWithStar_LeadingSlash()
    {
        this.GitBasedTest(
            """
/*.cs
""",
            ["foo.cs", "foo/bar/foo.cs", "foo/bar/bar.csproj"]
        );
    }

    [Test]
    public void SubdirStartsWithStar()
    {
        this.GitBasedTest(
            """
foo/*.cs
""",
            ["foo.cs", "foo/bar/foo.cs", "foo/foo.cs", "foo/bar/bar.csproj"]
        );
    }

    [Test]
    public void TrailingStar()
    {
        this.GitBasedTest(
            """
foo*
""",
            ["fooc", "foo/bar/foo", "foo/foob.cs", "foo/bar/bar.csproj", "bar/foo"]
        );
    }

    [Test]
    public void EscapedBang()
    {
        this.GitBasedTest(
            """
\!.foo/*
""",
            ["!.foo/bar", ".foo/.foo/bar", ".foo/har"]
        );
    }

    [Test]
    public void EscapedHash()
    {
        this.GitBasedTest(
            """
\#.foo/*
""",
            ["!#foo/bar", ".foo/.foo/bar", "#.foo/har"]
        );
    }

    [Test]
    public void SingleStar()
    {
        this.GitBasedTest(
            """
# * ignores everything
*
""",
            ["foo", "bar"]
        );
    }

    [Test]
    public void LiteralPlus()
    {
        this.GitBasedTest(
            """
*+
""",
            ["foo", "bar+", "foo+bar"]
        );
    }

    [Test]
    public void MiddleStar()
    {
        this.GitBasedTest(
            """
# intermediate *
fo*b
""",
            ["foobar", "bar", "foob"]
        );
    }

    [Test]
    public void LeadingSlash()
    {
        this.GitBasedTest(
            """
# leading slash
/fo*b
/bar
""",
            ["foobar", "bar", "foob"]
        );
    }

    [Test]
    public void EscapedSpaces()
    {
        this.GitBasedTest(
            """
# escaped spaces
/fo\ b
""",
            ["foobar", "bar", "fo b", "spacebar"]
        );
    }

    [Test]
    public void QuestionMark()
    {
        this.GitBasedTest(
            """
# ?
foo?
""",
            ["foob", "foo"]
        );
    }

    [Test]
    public void LeadingDoubleStar()
    {
        this.GitBasedTest(
            """
# leading **
**/foo
""",
            ["src/foo", "foo/bar", "src/bar/foo"]
        );
    }

    [Test]
    public void LeadingDoubleStar2()
    {
        this.GitBasedTest(
            """
# leading **
**foo.txt
""",
            ["src/foo.txt", "foo/bar/foo.txt", "foo.txt", "foo.bar"]
        );
    }

    [Test]
    public void LeadingDoubleStar3()
    {
        this.GitBasedTest(
            """
/**/foo.json
/**/*_generated.csproj
""",
            [
                "foo.json",
                "bar/foo.json",
                "tar/bar/foo.json",
                "_foo.json",
                "bar/car_foo.json",
                "x_generated.csproj",
                "foo/x_generated.csproj",
                "_generated.csproj",
            ]
        );
    }

    [Test]
    public void MiddleDoubleStar()
    {
        this.GitBasedTest(
            """
# middle **
foo/**/
""",
            ["src/foo/bar/char", "src/foo/tar", "src/foo/har/char/tar/har"]
        );
    }

    [Test]
    public void MiddleDoubleStar_2()
    {
        this.GitBasedTest(
            """
# middle **
foo/**/bar
""",
            [
                "foo/bar",
                "foo/tar/bar",
                "foo/har/tar/bar",
                "src/foo/tar/bar",
                "src/foo/har/char/tar/har/bar",
            ]
        );
    }

    [Test]
    public void MiddleDoubleStar_3()
    {
        this.GitBasedTest(
            """
# middle **
foo/**.ps
""",
            ["foo/bar/tar.ps", "foo/bar.ps", "foo/bar.js", "foo.ps"]
        );
    }

    [Test]
    public void MiddleDoubleStar_Complex()
    {
        this.GitBasedTest(
            """
# middle **
foo/**/**/bar
""",
            ["foo/bar", "src/foo/tar/bar", "foo/har/char/tar/bar", "foo/tar/bar", "foobar"]
        );
    }

    [Test]
    public void MiddleDoubleStar_Complex2()
    {
        this.GitBasedTest(
            """
# middle **
**/test/**/*.json
""",
            ["foo/test/unit/bar/car.json", "foo/test/tar.json", "src/foo/tar/car.json"]
        );
    }

    [Test]
    public void TrailingDoubleStar()
    {
        this.GitBasedTest(
            """
# trailing **
foo/**
""",
            ["foo/bar", "src/foo/tar/bar", "foo/har/char/tar/bar", "foo/tar/bar", "foobar"]
        );
    }

    [Test]
    public void TrailingDoubleStar_2()
    {
        this.GitBasedTest(
            """
# trailing **
src/foo/**
""",
            [
                "src/foo/bar",
                "src/foo/tar/bar",
                "foo/har/char/tar/bar",
                "foo/tar/bar",
                "srcfoo",
                "src/bar/foo",
            ]
        );
    }

    [Test]
    public void MiddleSlash()
    {
        this.GitBasedTest(
            """
# trailing **
src/foo
""",
            ["src/foo/bar", "foo/src/foo"]
        );
    }

    [Test]
    public void TrailingSlash()
    {
        this.GitBasedTest(
            """
# trailing **
src/foo/
""",
            ["src/foo"]
        );
    }

    [Test]
    public void TrailingSlash_2()
    {
        this.GitBasedTest(
            """
# trailing **
src/foo/
""",
            ["src/foo/", "foo/xy.txt", "src/foo/bar", "tar/src/foo/bar"]
        );
    }

    [Test]
    public void TrailingSlash_3()
    {
        this.GitBasedTest(
            """
# trailing **
src/
""",
            ["src/foo", "foo/src/bar", "mysrc/foo"]
        );
    }

    [Test]
    public void TrailingSlash_4()
    {
        this.GitBasedTest(
            """
[Bb]in/
[Oo]bj/
[Oo]ut/
[Ll]og/
[Ll]ogs/foo
""",
            ["foo/bar", "WpfObj/bar", "MyLog/foo", "Logs/foo", "src/Logs/foo/bar"]
        );
    }

    [Test]
    public void NoopNegate()
    {
        this.GitBasedTest(
            """
# negate
!foo
""",
            ["foo", "bar", "src/foo/tar", "har/foo", "src/bar/foo", "har/bar/foo/tar"]
        );
    }

    [Test]
    public void SimpleNegate()
    {
        this.GitBasedTest(
            """
# negate
foo
!foo
""",
            ["foo", "bar"]
        );
    }

    [Test]
    public void SimpleNegate_2()
    {
        this.GitBasedTest(
            """
# negate
foo
!foo
""",
            ["foo", "bar", "src/foo", "src/bar/foo"]
        );
    }

    [Test]
    public void ComplexNegate()
    {
        this.GitBasedTest(
            """
# negate
/*
!/foo
/foo/*
!/foo/bar
""",
            ["foo/bar", "bar", "src/foo", "src/bar/foo/bar"]
        );
    }

    [Test]
    public void Range()
    {
        this.GitBasedTest(
            """
# range regex
*.py[cod]
""",
            ["foo.py", "bar.p", "foo.pyc", "foo.pyco", "foo.pyd"]
        );
    }

    private void GitBasedTest(string gitignore, string[] files)
    {
        this.gitRepository.AddTrackedFileToRepo(".gitignore", gitignore);

        files
            .ToList()
            .ForEach(file =>
            {
                if (file.EndsWith('/'))
                {
                    this.gitRepository.AddUntrackedDirToRepo(file);
                }
                else
                {
                    this.gitRepository.AddUntrackedFileToRepo(file);
                }
            });

        var gitUntrackedFiles = this
            .gitRepository.GetUntrackedFiles()
            .Select(se => se.FilePath)
            .ToList();

        var ignoreFile = IgnoreFile
            .CreateAsync(this.gitRepository.RepoPath, this.fileSystem, null, CancellationToken.None)
            .GetAwaiter()
            .GetResult();

        var ignoreFileUnignoredFiles = files.Where(o => !ignoreFile!.IsIgnored(o, o.EndsWith('/')));

        ignoreFileUnignoredFiles.Should().BeEquivalentTo(gitUntrackedFiles);
    }
}
