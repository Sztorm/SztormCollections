namespace Sztorm.Collections
{
    /// <summary>
    /// Exposes a read-only two-dimensional indexer and a method checking it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyIndexable2D<out T>
    {
        T this[Index2D index] { get; }

        bool IsValidIndex(Index2D index);
    }
}
