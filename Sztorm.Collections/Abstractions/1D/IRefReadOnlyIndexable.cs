namespace Sztorm.Collections
{
    /// <summary>
    /// Exposes an <see langword="ref readonly"/> indexer and a method checking it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRefReadOnlyIndexable<T>
    {
        ref readonly T this[int index] { get; }

        bool IsValidIndex(int index);
    }
}
