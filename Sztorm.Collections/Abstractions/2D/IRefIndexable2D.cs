namespace Sztorm.Collections
{
    /// <summary>
    /// Exposes a two-dimensional indexer with <see langword="ref"/> specifier and a method
    /// checking it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRefIndexable2D<T>
    {
        ref T this[Index2D index] { get; }

        bool IsValidIndex(Index2D index);
    }
}
