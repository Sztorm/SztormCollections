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
    ///     Represents a predicate which determines whether <see cref="InnerObject"/> equals any
    ///     other <typeparamref name="T"/> object.
    /// </summary>
    /// <typeparam name="TEquatable">
    ///     <typeparamref name = "TEquatable"/> is <see cref="IEquatable{T}"/>
    /// </typeparam>
    /// <typeparam name="TOther">
    ///     <typeparamref name = "TOther"/> is type of any other object.
    /// </typeparam>
    public readonly struct EqualsPredicate<TEquatable, TOther> : IPredicate<TOther>
        where TEquatable : IEquatable<TOther>
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
        ///     Does a method call to trigger <see cref="NullReferenceException"/> if object is
        ///     null, then throws <see cref="ArgumentNullException"/>. This avoids unnecessary
        ///     boxing when argument is a value type.
        /// </summary>
        /// <param name="obj"></param>
        private static void ThrowExceptionIfArgumentIsNull(TEquatable obj)
        {
            try
            {
                obj.GetHashCode();
            }
            catch (NullReferenceException)
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        ///     Constructs a predicate that takes an object which may be used to determine whether
        ///     object passed in constructor equals any other <typeparamref name="T"/> object.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="obj"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <param name="obj">The object which may be used in comparisons.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EqualsPredicate(TEquatable obj)
        {
            try
            {
                ThrowExceptionIfArgumentIsNull(obj);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(obj), "obj cannot be null.");
            }
            this.innerObj = obj;
        }

        /// <summary>
        ///     Returns a value indicating whether <see cref="InnerObject"/> equals 
        ///     <paramref name="other"/>
        /// </summary>
        /// <param name="other">
        ///     The other object that does not need to implement <see cref="IEquatable{T}"/>
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(TOther other) => innerObj.Equals(other);
    }
}
