using System.Collections.Generic;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Defines size, enumerators and methods which operate on rectangular collections.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRectangularCollection<T> : IEnumerable<T>, IIndexable2D<T>
    {
        int Length1 { get; }
        int Length2 { get; }
    }
}
