using System.Diagnostics;
using System.Text;

namespace CSharpier.Utilities;

//  From https://github.com/dotnet/roslyn/blob/38f239fb81b72bfd313cd18aeff0b0ed40f34c5c/src/Dependencies/PooledObjects/PooledStringBuilder.cs#L18

/// <summary>
/// The usage is:
///        var inst = PooledStringBuilder.GetInstance();
///        var sb = inst.builder;
///        ... Do Stuff...
///        ... sb.ToString() ...
///        inst.Free();
/// </summary>
internal sealed class PooledStringBuilder
{
    public readonly StringBuilder Builder = new();
    private readonly ObjectPool<PooledStringBuilder> _pool;

    private PooledStringBuilder(ObjectPool<PooledStringBuilder> pool)
    {
        Debug.Assert(pool != null);
        _pool = pool!;
    }

    public int Length
    {
        get { return this.Builder.Length; }
    }

    public void Free()
    {
        var builder = this.Builder;

        // do not store builders that are too large.
        if (builder.Capacity <= 2_000_000)
        {
            builder.Clear();
            _pool.Free(this);
        }
    }

    public string ToStringAndFree()
    {
        var result = this.Builder.ToString();
        this.Free();

        return result;
    }

    public string ToStringAndFree(int startIndex, int length)
    {
        var result = this.Builder.ToString(startIndex, length);
        this.Free();

        return result;
    }

    // global pool
    private static readonly ObjectPool<PooledStringBuilder> s_poolInstance = CreatePool();

    // if someone needs to create a private pool;
    /// <summary>
    /// If someone need to create a private pool
    /// </summary>
    /// <param name="size">The size of the pool.</param>
    public static ObjectPool<PooledStringBuilder> CreatePool(int size = 16)
    {
        ObjectPool<PooledStringBuilder>? pool = null;
        pool = new ObjectPool<PooledStringBuilder>(() => new PooledStringBuilder(pool!), size);
        return pool;
    }

    public static PooledStringBuilder GetInstance()
    {
        var builder = s_poolInstance.Allocate();
        Debug.Assert(builder.Builder.Length == 0);
        return builder;
    }

    public static implicit operator StringBuilder(PooledStringBuilder obj)
    {
        return obj.Builder;
    }
}
