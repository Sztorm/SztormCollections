namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents interface which defines a method that converts an object from one type to
    ///     another type.<br/>
    ///     This interface mimics <see cref="System.Converter{TInput, TOutput}"/> behavior. If the
    ///     interface is implemented by a <see langword="struct"/> and is used as constraint in
    ///     generic method, the implemented method call is direct and the garbage collector's work
    ///     is avoided.
    /// </summary>
    /// <typeparam name="TInput">The type of object that is to be converted.</typeparam>
    /// <typeparam name="TOutput">The type the input object is to be converted to.</typeparam>
    public interface IConverter<in TInput, out TOutput>
    {
        /// <summary>
        ///     Represents a method that converts an object from one type to another type.
        /// </summary>
        /// <param name="input">The object to convert.</param>
        /// <returns></returns>
        TOutput Invoke(TInput input);
    }
}
