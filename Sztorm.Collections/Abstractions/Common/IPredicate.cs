namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents interface with method that defines a set of criteria and determines whether
    ///     the specified object meets those criteria.<br/>
    ///     This interface mimics <see cref="System.Predicate{T}"/> behavior. If the interface is
    ///     implemented by a <see langword="struct"/> and is used as constraint in generic method,
    ///     the implemented method call is direct and the garbage collector's work is avoided.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPredicate<in T>
    {
        /// <summary>
        ///     Represents a method that defines a set of criteria and determines whether the
        ///     specified object meets those criteria.
        /// </summary>
        /// <param name="obj">
        ///     The object to compare against the criteria defined within the implemented method.
        /// </param>
        /// <returns></returns>
        bool Invoke(T obj);
    }
}