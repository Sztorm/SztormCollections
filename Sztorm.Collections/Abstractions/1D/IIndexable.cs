namespace Sztorm.Collections
{
    /// <summary>
    /// Exposes an indexer and a method checking it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIndexable<T>
    {
        T this[int index] { get; set; }

        bool IsValidIndex(int index);
    }
}
