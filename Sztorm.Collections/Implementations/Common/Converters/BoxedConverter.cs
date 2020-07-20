using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents a converter which uses another converter to convert an object from one type
    ///     to another type.<br/>
    /// </summary>
    /// <typeparam name="TInput">The type of object that is to be converted.</typeparam>
    /// <typeparam name="TOutput">The type the input object is to be converted to.</typeparam>
    internal readonly struct BoxedConverter<TInput, TOutput> : IConverter<TInput, TOutput>
    {
        private readonly Converter<TInput, TOutput> converter;

        /// <summary>
        ///     The predicate which defines specific convert process.
        /// </summary>
        public Converter<TInput, TOutput> InnerConverter
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => converter;
        }

        /// <summary>
        ///     Constructs a predicate that takes another predicate which determine specific
        ///     convert process.
        /// </summary>
        /// <param name="converter">The predicate which defines specific convert process.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoxedConverter(Converter<TInput, TOutput> converter) => this.converter = converter;

        /// <summary>
        ///     Returns an object converted from <typeparamref name="TInput"/> to
        ///     <typeparamref name="TOutput"/> in process defined by the
        ///     <see cref="InnerConverter"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TOutput Invoke(TInput input) => converter(input);
    }
}
