using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CSharpier.Core.Utilities;

// From https://github.com/dotnet/runtime/blob/f5c73447ca9dcb3407d0143829bbf708c04170c1/src/libraries/System.Private.CoreLib/src/System/Collections/Generic/ValueListBuilder.cs#L10
internal ref partial struct ValueListBuilder<T>
{
    private Span<T> span;
    private T[]? arrayFromPool;
    private int pos;

    public ValueListBuilder(Span<T?> scratchBuffer)
    {
        this.span = scratchBuffer!;
    }

    public ValueListBuilder(int capacity)
    {
        this.Grow(capacity);
    }

    public int Length
    {
        readonly get => this.pos;
        set
        {
            Debug.Assert(value >= 0);
            Debug.Assert(value <= this.span.Length);
            this.pos = value;
        }
    }

    public ref T this[int index]
    {
        get
        {
            Debug.Assert(index < this.pos);
            return ref this.span[index];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear() => Length = 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(T item)
    {
        var pos = this.pos;

        // Workaround for https://github.com/dotnet/runtime/issues/72004
        var span = this.span;
        if ((uint)pos < (uint)span.Length)
        {
            span[pos] = item;
            this.pos = pos + 1;
        }
        else
        {
            this.AddWithResize(item);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(params ReadOnlySpan<T> source)
    {
        var pos = this.pos;
        var span = this.span;
        if (source.Length == 1 && (uint)pos < (uint)span.Length)
        {
            span[pos] = source[0];
            this.pos = pos + 1;
        }
        else
        {
            this.AppendMultiChar(source);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void AppendMultiChar(scoped ReadOnlySpan<T> source)
    {
        if ((uint)(this.pos + source.Length) > (uint)this.span.Length)
        {
            this.Grow(this.span.Length - this.pos + source.Length);
        }

        source.CopyTo(this.span[this.pos..]);
        this.pos += source.Length;
    }

    public void Insert(int index, scoped ReadOnlySpan<T> source)
    {
        Debug.Assert(index == 0, "Implementation currently only supports index == 0");

        if ((uint)(this.pos + source.Length) > (uint)this.span.Length)
        {
            this.Grow(source.Length);
        }

        this.span[..this.pos].CopyTo(this.span[source.Length..]);
        source.CopyTo(this.span);
        this.pos += source.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AppendSpan(int length)
    {
        Debug.Assert(length >= 0);

        var pos = this.pos;
        var span = this.span;
        if ((ulong)(uint)pos + (ulong)(uint)length <= (ulong)(uint)span.Length) // same guard condition as in Span<T>.Slice on 64-bit
        {
            this.pos = pos + length;
            return span.Slice(pos, length);
        }
        else
        {
            return this.AppendSpanWithGrow(length);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private Span<T> AppendSpanWithGrow(int length)
    {
        var pos = this.pos;
        this.Grow(this.span.Length - pos + length);
        this.pos += length;
        return this.span.Slice(pos, length);
    }

    // Hide uncommon path
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void AddWithResize(T item)
    {
        Debug.Assert(this.pos == this.span.Length);
        var pos = this.pos;
        this.Grow(1);
        this.span[pos] = item;
        this.pos = pos + 1;
    }

    public readonly ReadOnlySpan<T> AsSpan()
    {
        return this.span[..this.pos];
    }

    public readonly bool TryCopyTo(Span<T> destination, out int itemsWritten)
    {
        if (this.span[..this.pos].TryCopyTo(destination))
        {
            itemsWritten = this.pos;
            return true;
        }

        itemsWritten = 0;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        var toReturn = this.arrayFromPool;
        if (toReturn != null)
        {
            this.arrayFromPool = null;
            ArrayPool<T>.Shared.Return(toReturn);
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

        var array = ArrayPool<T>.Shared.Rent(nextCapacity);
        this.span.CopyTo(array);

        var toReturn = this.arrayFromPool;
        this.span = this.arrayFromPool = array;
        if (toReturn != null)
        {
            ArrayPool<T>.Shared.Return(toReturn);
        }
    }
}
