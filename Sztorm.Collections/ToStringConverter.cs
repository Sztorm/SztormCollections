namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents a converter which converts any input type to <see cref="string"/>.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    public readonly struct ToStringConverter<TInput> : IConverter<TInput, string>
    {
        /// <summary>
        ///     Takes an <typeparamref name="TInput"/> object and returns its conversion to
        ///     <see cref="string"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Invoke(TInput input) => input.ToString();
    }
}
