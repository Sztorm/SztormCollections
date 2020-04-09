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
    ///     Represents a predicate which determines whether inner object in is less than or equal
    ///     to any <typeparamref name="T"/> object it may be compared to.
    /// </summary>
    /// <typeparam name="T"><typeparamref name = "T"/> is <see cref="IComparable{T}"/></typeparam>
    public readonly struct LessThanOrEqualToPredicate<T> : IPredicate<T> where T : IComparable<T>
    {
        public readonly T InnerObject;

        /// <summary>
        ///     Constructs a predicate which determines whether object passed in is less than or
        ///     equal to any <typeparamref name="T"/> object it may be compared to.
        /// </summary>
        /// <param name="obj">The object that may be compared to</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LessThanOrEqualToPredicate(T obj) => InnerObject = obj;

        /// <summary>
        ///     Returns a value indicating whether <see cref="InnerObject"/> is less than or equal
        ///     to other.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(T other) => InnerObject.CompareTo(other) <= 0;
    }
}

