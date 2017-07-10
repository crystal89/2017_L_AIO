using System.Collections.Generic;

/// <summary>
/// 参考Unity - UISystem
/// </summary>
/// <typeparam name="T"></typeparam>
internal static class AIOListPool<T>
{
    // Object pool to avoid allocations.
    private static readonly AIOObjectPool<List<T>> s_ListPool = new AIOObjectPool<List<T>>(null, l => l.Clear());

    public static List<T> Get()
    {
        return s_ListPool.Get();
    }

    public static void Release(List<T> toRelease)
    {
        s_ListPool.Release(toRelease);
    }
}
