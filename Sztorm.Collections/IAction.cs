namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents interface which defines a method that has a single parameter and does not
    ///     return a value.<br/>
    ///     This interface mimics <see cref="System.Action{T}"/> behavior. If the interface is
    ///     implemented by a <see langword="struct"/> and is used as constraint in generic method,
    ///     the implemented method call is direct and the garbage collector's work is avoided.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAction<in T>
    {
        /// <summary>
        ///     Represents a method that has a single parameter and does not return a value.
        /// </summary>
        /// <param name="obj"></param>
        void Invoke(T obj);
    }
}
