/*
 * MIT License
 * 
 * Copyright (c) 2020 Sztorm
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

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
