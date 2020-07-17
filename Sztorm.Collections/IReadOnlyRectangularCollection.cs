using System.Collections.Generic;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Defines size, enumerators and methods which operate on read-only rectangular
    ///     collections.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyRectangularCollection<T> :
        IEnumerable<T>, IReadOnlyIndexable2D<T>, IHasRectangularBoundaries
    { }
}
