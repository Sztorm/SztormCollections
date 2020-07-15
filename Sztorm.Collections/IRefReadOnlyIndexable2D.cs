namespace Sztorm.Collections
{
    /// <summary>
    /// Exposes a <see langword="ref readonly"/> two-dimensional indexer and a method checking it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRefReadOnlyIndexable2D<T>
    {
        ref readonly T this[Index2D index] { get; }

        bool IsValidIndex(Index2D index);
    }
}
