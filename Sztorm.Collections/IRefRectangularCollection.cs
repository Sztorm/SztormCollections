using System.Collections.Generic;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Defines size, enumerators and methods which operate on rectangular collections with
    ///     ref-returning indexer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRefRectangularCollection<T> : IEnumerable<T>, IRefIndexable2D<T>
    {
        int Length1 { get; }
        int Length2 { get; }
    }
}
