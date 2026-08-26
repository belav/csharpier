using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis;

namespace CSharpier.Core.DocTypes;

internal abstract class Doc
{
    public override string ToString()
    {
        return DocSerializer.Serialize(this);
    }

    public static implicit operator Doc(string value)
    {
        return StringDoc.Create(value);
    }

    public static NullDoc Null => NullDoc.Instance;

    public static readonly Doc BreakParent = new BreakParent();

    public static readonly HardLine HardLine = new();

    public static readonly HardLineNoTrim HardLineNoTrim = new();

    public static readonly HardLine HardLineSkipBreakIfFirstInGroup = new(false, true);

    public static readonly HardLine HardLineIfNoPreviousLine = new(true);

    public static readonly HardLine HardLineIfNoPreviousLineSkipBreakIfFirstInGroup = new(
        true,
        true
    );

    public static readonly LiteralLine LiteralLine = new();

    public static readonly LineDoc Line = new() { Type = LineDoc.LineType.Normal };

    public static readonly LineDoc SoftLine = new() { Type = LineDoc.LineType.Soft };

    public static readonly Trim Trim = new();

    public static LeadingComment LeadingComment(string comment, CommentType commentType)
    {
        return new LeadingComment { Type = commentType, Comment = comment };
    }

    public static TrailingComment TrailingComment(string comment, CommentType commentType)
    {
        return new TrailingComment { Type = commentType, Comment = comment };
    }

    public static Doc Concat(List<Doc> contents)
    {
        return contents.Count == 0 ? Doc.Null
            : contents.Count == 1 ? contents[0]
            : new Concat(contents);
    }

    // prevents allocating an array if there is only a single parameter
    public static Doc Concat(Doc contents)
    {
        return contents;
    }

    public static Doc Concat(params Doc[] contents)
    {
        return contents.Length switch
        {
            0 => Null,
            1 => contents[0],
            _ => new Concat(contents),
        };
    }

    public static Doc Concat(ref DocListBuilder contents)
    {
        return contents.Length switch
        {
            0 => Null,
            1 => contents[0],
            _ => new Concat(contents.ToArray()),
        };
    }

    public static Doc Join(Doc separator, IEnumerable<Doc> enumerable)
    {
        var docs = new List<Doc>();

        var x = 0;
        foreach (var doc in enumerable)
        {
            if (x != 0)
            {
                docs.Add(separator);
            }

            docs.Add(doc);
            x++;
        }

        return docs.Count == 1 ? docs[0] : Concat(docs);
    }

    public static Doc Join(Doc separator, ReadOnlySpan<Doc> docs)
    {
        if (docs.Length <= 1)
        {
            return docs.Length == 0 ? Null : docs[0];
        }

        var contents = new Doc[docs.Length * 2 - 1];
        for (var index = 0; index < docs.Length; index++)
        {
            if (index != 0)
            {
                contents[(index * 2) - 1] = separator;
            }

            contents[index * 2] = docs[index];
        }

        return Concat(contents);
    }

    // the overloads below take the roslyn list plus a print method instead of an
    // IEnumerable<Doc>, so that the call site avoids the Select(..) iterator. print is expected
    // to be a static method group, which the compiler caches into a static field
    public static Doc Join<TNode, TContext>(
        Doc separator,
        in SyntaxList<TNode> list,
        Func<TNode, TContext, Doc> print,
        TContext context
    )
        where TNode : SyntaxNode
    {
        if (list.Count <= 1)
        {
            return list.Count == 0 ? Null : print(list[0], context);
        }

        var contents = new Doc[(list.Count * 2) - 1];
        for (var index = 0; index < list.Count; index++)
        {
            if (index != 0)
            {
                contents[(index * 2) - 1] = separator;
            }

            contents[index * 2] = print(list[index], context);
        }

        return Concat(contents);
    }

    public static Doc Join<TContext>(
        Doc separator,
        in SyntaxTokenList tokens,
        Func<SyntaxToken, TContext, Doc> print,
        TContext context
    )
    {
        if (tokens.Count <= 1)
        {
            return tokens.Count == 0 ? Null : print(tokens[0], context);
        }

        var contents = new Doc[(tokens.Count * 2) - 1];
        for (var index = 0; index < tokens.Count; index++)
        {
            if (index != 0)
            {
                contents[(index * 2) - 1] = separator;
            }

            contents[index * 2] = print(tokens[index], context);
        }

        return Concat(contents);
    }

    public static Doc Join<TContext>(
        Doc separator,
        ReadOnlySpan<SyntaxToken> tokens,
        Func<SyntaxToken, TContext, Doc> print,
        TContext context
    )
    {
        if (tokens.Length <= 1)
        {
            return tokens.Length == 0 ? Null : print(tokens[0], context);
        }

        var contents = new Doc[(tokens.Length * 2) - 1];
        for (var index = 0; index < tokens.Length; index++)
        {
            if (index != 0)
            {
                contents[(index * 2) - 1] = separator;
            }

            contents[index * 2] = print(tokens[index], context);
        }

        return Concat(contents);
    }

    public static ForceFlat ForceFlat(List<Doc> contents)
    {
        return new ForceFlat { Contents = Concat(contents) };
    }

    public static ForceFlat ForceFlat(params Doc[] contents)
    {
        return new ForceFlat { Contents = Concat(contents) };
    }

    public static Group Group(List<Doc> contents)
    {
        return new Group { Contents = contents.Count == 1 ? contents[0] : Concat(contents) };
    }

    public static Group GroupWithId(string groupId, List<Doc> contents)
    {
        var group = Group(contents);
        group.GroupId = groupId;
        return group;
    }

    // prevents allocating an array if there is only a single parameter
    public static Group GroupWithId(string groupId, Doc contents)
    {
        var group = Group(contents);
        group.GroupId = groupId;
        return group;
    }

    private static int groupNumber;

    // ids only have to be unique within a single print, so an incrementing counter is enough
    // and is far cheaper than Guid.NewGuid()
    public static string NextGroupId()
    {
        return "Group_" + Interlocked.Increment(ref groupNumber);
    }

    public static Group GroupWithId(string groupId, params Doc[] contents)
    {
        var group = Group(contents);
        group.GroupId = groupId;
        return group;
    }

    // prevents allocating an array if there is only a single parameter
    public static Group Group(Doc contents)
    {
        return new Group { Contents = contents };
    }

    public static Group Group(params Doc[] contents)
    {
        return new Group { Contents = Concat(contents) };
    }

    // prevents allocating an array if there is only a single parameter
    public static IndentDoc Indent(Doc contents)
    {
        return new IndentDoc { Contents = contents };
    }

    public static IndentDoc Indent(params Doc[] contents)
    {
        return new IndentDoc { Contents = Concat(contents) };
    }

    public static IndentDoc Indent(List<Doc> contents)
    {
        return new IndentDoc { Contents = Concat(contents) };
    }

    public static Doc IndentIf(bool condition, Doc contents)
    {
        return condition ? Indent(contents) : contents;
    }

    public static IfBreak IfBreak(Doc breakContents, Doc flatContents, string? groupId = null)
    {
        return new IfBreak
        {
            FlatContents = flatContents,
            BreakContents = breakContents,
            GroupId = groupId,
        };
    }

    public static IndentIfBreak IndentIfBreak(Doc contents, string groupId)
    {
        return new IndentIfBreak(contents, groupId);
    }

    public static Doc Directive(string value)
    {
        return new StringDoc(value, true);
    }

    public static ConditionalGroup ConditionalGroup(params Doc[] options)
    {
        return new ConditionalGroup(options);
    }

    public static AlwaysFits AlwaysFits(Doc printedTrivia)
    {
        return new AlwaysFits(printedTrivia);
    }

    public static Region BeginRegion(string text)
    {
        return new Region(text);
    }

    public static Region EndRegion(string text)
    {
        return new Region(text) { IsEnd = true };
    }
}

internal enum CommentType
{
    SingleLine,
    MultiLine,
}

internal interface IHasContents
{
    Doc Contents { get; }
}
