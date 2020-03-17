﻿/*
 * MIT License
 * 
 * Copyright (c) 2019 Sztorm
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
    ///     Represents boundaries of two-dimensional collection. Boundaries are guaranteed to be
    ///     greater or equal to zero.
    /// </summary>
    [Serializable]
    public struct Bounds2D : IEquatable<Bounds2D>
    {
        private int len1;
        private int len2;

        /// <summary>
        ///     Returns total amount of rows in this <see cref="Bounds2D"/> instance. This property
        ///     is equal to <see cref="Length1"/>.
        /// </summary>
        public int Rows
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => len1;
        }

        /// <summary>
        ///     Returns total amount of columns in this <see cref="Bounds2D"/> instance. This
        ///     property is equal to <see cref="Length2"/>.
        /// </summary>
        public int Columns
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => len2;
        }

        /// <summary>
        ///     Returns length of the first dimension in this <see cref="Bounds2D"/> instance. This
        ///     property is equal to <see cref="Rows"/>.
        /// </summary>
        public int Length1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => len1;
        }

        /// <summary>
        ///     Returns length of the second dimension in this <see cref="Bounds2D"/> instance.
        ///     This property is equal to <see cref="Columns"/>.
        /// </summary>
        public int Length2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => len2;
        }

        /// <summary>
        ///     Constructs boundaries of a two-dimensional collection with specified quantity of
        ///     rows and columns.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"></see>: All the arguments must be
        ///         greater or equal to zero.
        ///     </para>
        /// </summary>
        /// <param name="rows">
        ///     A number of rows in a boundary. Value must be greater or equal to zero.
        /// </param>
        /// <param name="columns">
        ///     A number of columns in a boundary Value must be greater or equal to zero.
        /// </param>
        public Bounds2D(int rows, int columns)
        {
            if (rows < 0 || columns < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "All the arguments must be greater or equal to zero.");
            }
            len1 = rows;
            len2 = columns;
        }

        /// <summary>
        ///     Returns true if specified index remains within boundaries. false otherwise.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidIndex(int row, int column)
            => (uint)row < len1 && (uint)column < len2;

        /// <summary>
        ///     Returns true if specified index remains within boundaries. false otherwise.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidIndex(Index2D index)
            => IsValidIndex(index.Dimension1Index, index.Dimension2Index);

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified
        ///     <see cref="Bounds2D"></see> value.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Bounds2D other)
        {
            return this == other;
        }

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified
        ///     <see cref="Bounds2D"></see> value.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
        {
            if (!(other is Bounds2D))
            {
                return false;
            }
            return this == (Bounds2D)other;    
        }

        /// <summary>
        ///     Returns the hashcode for this instance.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => len2.GetHashCode() ^ (len1.GetHashCode() << 2);

        /// <summary>
        ///     Converts the value of this instance to
        ///     <see cref="ValueTuple"/>&lt;<see cref="int"/>, <see cref="int"/>&gt;. 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (int Rows, int Columns) ToValueTuple() => new ValueTuple<int, int>(len1, len2);

        /// <summary>
        ///     Returns an <see cref="Bounds2D"></see> instance made from specified value.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"></see>: All the arguments must be
        ///         greater than zero.
        ///     </para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds2D FromValueTuple(ValueTuple<int, int> value)
        {
            try
            {
               return new Bounds2D(value.Item1, value.Item2);
            }
            catch(ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Constructs an <see cref="Bounds2D"/> instance without argument validation.
        ///     For internal purposes only.
        /// </summary>
        /// <param name="rows">Range: [0, <see cref="int.MaxValue"/>]</param>
        /// <param name="columns">Range: [0, <see cref="int.MaxValue"/>]</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Bounds2D NotCheckedConstructor(int rows, int columns)
        {
            Bounds2D bounds;
            bounds.len1 = rows;
            bounds.len2 = columns;

            return bounds;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Bounds2D left, Bounds2D right)
            => left.len1 == right.len1 && left.len2 == right.len2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Bounds2D left, Bounds2D right)
            => left.len1 != right.len1 || left.len2 != right.len2;
    }
}
