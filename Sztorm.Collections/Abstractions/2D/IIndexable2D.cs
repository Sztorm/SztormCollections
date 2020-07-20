namespace Sztorm.Collections
{
    /// <summary>
    /// Exposes a two-dimensional indexer and a method checking it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIndexable2D<T>
    {
        T this[Index2D index] { get; set; }

        bool IsValidIndex(Index2D index);
    }
}
