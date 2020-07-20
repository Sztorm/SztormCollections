using System.Collections.Generic;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Defines size, enumerators and methods which operate on rectangular collections with
    ///     read-only ref-returning indexer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRefReadOnlyRectangularCollection<T> :
        IEnumerable<T>, IRefReadOnlyIndexable2D<T>, IHasRectangularBoundaries { }
}
