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
    ///     Represents a predicate which uses another predicate to determine criteria.
    /// </summary>
    /// <typeparam name="T"><typeparamref name = "T"/> is <see cref="IEquatable{T}"/></typeparam>
    internal readonly struct BoxedPredicate<T> : IPredicate<T>
    {
        private readonly Predicate<T> predicate;

        public Predicate<T> InnerPredicate
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => predicate;
        }

        /// <summary>
        ///     Constructs a predicate that takes another predicate which may be used to determine
        ///     criteria.
        /// </summary>
        /// <param name="predicate">The predicate which may be used to determine criteria.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoxedPredicate(Predicate<T> predicate) => this.predicate = predicate;

        /// <summary>
        ///     Returns a value indicating whether specified object meets criteria defined by
        ///     <see cref="InnerPredicate"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(T obj) => predicate(obj);
    }
}
