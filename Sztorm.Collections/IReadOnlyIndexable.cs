namespace Sztorm.Collections
{
    /// <summary>
    /// Exposes an read-only indexer and a method checking it..
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyIndexable<out T>
    {
        T this[int index] { get; }

        bool IsValidIndex(int index);
    }
}
