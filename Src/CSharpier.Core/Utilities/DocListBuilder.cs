using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.Utilities;

// From https://github.com/dotnet/runtime/blob/f5c73447ca9dcb3407d0143829bbf708c04170c1/src/libraries/System.Private.CoreLib/src/System/Collections/Generic/ValueListBuilder.cs#L10
internal ref struct DocListBuilder
{
    private Span<Doc> span;
    private Doc[]? arrayFromPool;
    private int position;

    public DocListBuilder(int capacity)
    {
        this.Grow(capacity);
    }

    public int Length
    {
        readonly get => this.position;
        set
        {
            Debug.Assert(value >= 0);
            Debug.Assert(value <= this.span.Length);
            this.position = value;
        }
    }

    public ref Doc this[int index]
    {
        get
        {
            Debug.Assert(index < this.position);
            return ref this.span[index];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear() => this.Length = 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(Doc item)
    {
        var pos = this.position;

        // Workaround for https://github.com/dotnet/runtime/issues/72004
        var span = this.span;
        if ((uint)pos < (uint)span.Length)
        {
            span[pos] = item;
            this.position = pos + 1;
        }
        else
        {
            this.AddWithResize(item);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(params ReadOnlySpan<Doc> source)
    {
        var pos = this.position;
        var span = this.span;
        if (source.Length == 1 && (uint)pos < (uint)span.Length)
        {
            span[pos] = source[0];
            this.position = pos + 1;
        }
        else
        {
            this.AppendMultiChar(source);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void AppendMultiChar(scoped ReadOnlySpan<Doc> source)
    {
        if ((uint)(this.position + source.Length) > (uint)this.span.Length)
        {
            this.Grow(this.span.Length - this.position + source.Length);
        }

        source.CopyTo(this.span[this.position..]);
        this.position += source.Length;
    }

    public void Insert(int index, scoped ReadOnlySpan<Doc> source)
    {
        Debug.Assert(index == 0, "Implementation currently only supports index == 0");

        if ((uint)(this.position + source.Length) > (uint)this.span.Length)
        {
            this.Grow(source.Length);
        }

        this.span[..this.position].CopyTo(this.span[source.Length..]);
        source.CopyTo(this.span);
        this.position += source.Length;
    }

    // Hide uncommon path
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void AddWithResize(Doc item)
    {
        Debug.Assert(this.position == this.span.Length);
        var pos = this.position;
        this.Grow(1);
        this.span[pos] = item;
        this.position = pos + 1;
    }

    public readonly ReadOnlySpan<Doc> AsSpan()
    {
        return this.span[..this.position];
    }

    public readonly Doc[] ToArray()
    {
        return this.AsSpan().ToArray();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        var toReturn = this.arrayFromPool;
        if (toReturn != null)
        {
            this.arrayFromPool = null;
            ArrayPool<Doc>.Shared.Return(toReturn);
        }
    }

    // Note that consuming implementations depend on the list only growing if it's absolutely
    // required.  If the list is already large enough to hold the additional items be added,
    // it must not grow. The list is used in a number of places where the reference is checked
    // and it's expected to match the initial reference provided to the constructor if that
    // span was sufficiently large.
    private void Grow(int additionalCapacityRequired = 1)
    {
        const int ArrayMaxLength = 0x7FFFFFC7; // same as Array.MaxLength

        // Double the size of the span.  If it's currently empty, default to size 4,
        // although it'll be increased in Rent to the pool's minimum bucket size.
        var nextCapacity = Math.Max(
            this.span.Length != 0 ? this.span.Length * 2 : 4,
            this.span.Length + additionalCapacityRequired
        );

        // If the computed doubled capacity exceeds the possible length of an array, then we
        // want to downgrade to either the maximum array length if that's large enough to hold
        // an additional item, or the current length + 1 if it's larger than the max length, in
        // which case it'll result in an OOM when calling Rent below.  In the exceedingly rare
        // case where _span.Length is already int.MaxValue (in which case it couldn't be a managed
        // array), just use that same value again and let it OOM in Rent as well.
        if ((uint)nextCapacity > ArrayMaxLength)
        {
            nextCapacity = Math.Max(
                Math.Max(this.span.Length + 1, ArrayMaxLength),
                this.span.Length
            );
        }

        var array = ArrayPool<Doc>.Shared.Rent(nextCapacity);
        this.span.CopyTo(array);

        var toReturn = this.arrayFromPool;
        this.span = this.arrayFromPool = array;
        if (toReturn != null)
        {
            ArrayPool<Doc>.Shared.Return(toReturn);
        }
    }

    public override readonly string ToString()
    {
        return DocSerializer.Serialize(Doc.Concat(this.AsSpan().ToArray()));
    }
}
