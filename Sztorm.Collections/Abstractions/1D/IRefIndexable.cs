namespace Sztorm.Collections
{
    /// <summary>
    /// Exposes an indexer with <see langword="ref"/> specifier and a method checking it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRefIndexable<T>
    {
        ref T this[int index] { get; }

        bool IsValidIndex(int index);
    }
}
