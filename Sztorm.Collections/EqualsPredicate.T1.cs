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
    ///     Represents a predicate which determines whether any <typeparamref name="TEquatable"/>
    ///     object equals <see cref="InnerObject"/>
    /// </summary>
    /// <typeparam name="TEquatable">
    ///     <typeparamref name = "TEquatable"/> is <see cref="IEquatable{T}"/>
    /// </typeparam>
    public readonly struct EqualsPredicate<TEquatable> : IPredicate<TEquatable> 
        where TEquatable : IEquatable<TEquatable>
    {
        private readonly TEquatable innerObj;

        /// <summary>
        ///     The object which may be used in comparisons.
        /// </summary>
        public TEquatable InnerObject
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => innerObj;
        }

        /// <summary>
        ///     Constructs a predicate that takes an object which may be used to determine whether
        ///     any other <typeparamref name="TEquatable"/> object equals object passed in
        ///     constructor.
        /// </summary>
        /// <param name="obj">The object which may be used in comparisons.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EqualsPredicate(TEquatable obj) => innerObj = obj;

        /// <summary>
        ///     Returns a value indicating whether <paramref name="other"/> equals
        ///     <see cref="InnerObject"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(TEquatable other) => other.Equals(innerObj);
    }
}
