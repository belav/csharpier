using System.Diagnostics;

namespace CSharpier.Core.Utilities;

// From https://github.com/dotnet/roslyn/blob/38f239fb81b72bfd313cd18aeff0b0ed40f34c5c/src/Dependencies/PooledObjects/PooledHashSet.cs#L12
internal sealed class PooledHashSet<T> : HashSet<T>
{
    private readonly ObjectPool<PooledHashSet<T>> _pool;

    private PooledHashSet(ObjectPool<PooledHashSet<T>> pool, IEqualityComparer<T> equalityComparer)
        : base(equalityComparer)
    {
        _pool = pool;
    }

    public void Free()
    {
        if (this.Count <= 100_000)
        {
            this.Clear();
            _pool?.Free(this);
        }
    }

    // global pool
    private static readonly ObjectPool<PooledHashSet<T>> s_poolInstance = CreatePool(
        EqualityComparer<T>.Default
    );

    // if someone needs to create a pool;
    public static ObjectPool<PooledHashSet<T>> CreatePool(IEqualityComparer<T> equalityComparer)
    {
        ObjectPool<PooledHashSet<T>>? pool = null;
        pool = new ObjectPool<PooledHashSet<T>>(
            () => new PooledHashSet<T>(pool!, equalityComparer),
            16
        );
        return pool;
    }

    public static PooledHashSet<T> GetInstance()
    {
        var instance = s_poolInstance.Allocate();
        Debug.Assert(instance.Count == 0);
        return instance;
    }
}
