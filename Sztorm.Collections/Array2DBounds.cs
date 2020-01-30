using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents boundaries of <see cref="Array2D{T}"/>
    /// </summary>
    public struct Array2DBounds : IEquatable<Array2DBounds>
    {
        readonly internal int len1;
        readonly internal int len2;

        /// <summary>
        ///     Returns total amount of rows in this array boundary. This property is equal to
        ///     <see cref="Length1"/>.
        /// </summary>
        public int Rows
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => len1;
        }

        /// <summary>
        ///     Returns total amount of columns in this array boundary. This property is equal to
        ///     <see cref="Length2"/>.
        /// </summary>
        public int Columns
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => len2;
        }

        /// <summary>
        ///     Returns length of the first dimension in this array boundary. This property is
        ///     equal to <see cref="Rows"/>.
        /// </summary>
        public int Length1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => len1;
        }

        /// <summary>
        ///     Returns length of the second dimension in this array boundary. This property is
        ///     equal to <see cref="Columns"/>.
        /// </summary>
        public int Length2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => len2;
        }

        /// <summary>
        ///     Constructs a two-dimensional array boundary with specified quantity of rows and
        ///     columns.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"></see>: All the arguments must be
        ///         greater than zero.
        ///     </para>
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        public Array2DBounds(int rows, int columns)
        {
            if (rows < 0 || columns < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "All the arguments must be greater than zero.");
            }
            len1 = rows;
            len2 = columns;
        }

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified
        ///     <see cref="Array2DBounds"></see> value.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Array2DBounds other)
        {
            return this == other;
        }

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified
        ///     <see cref="Array2DBounds"></see> value.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
        {
            if (!(other is Array2DBounds))
            {
                return false;
            }
            return this == (Array2DBounds)other;    
        }

        /// <summary>
        ///     Returns the hashcode for this instance.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => len2.GetHashCode() ^ (len1.GetHashCode() << 2);

        /// <summary>
        ///     Returns an <see cref="Array2DBounds"></see> instance made from specified value.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"></see>: All the arguments must be
        ///         greater than zero.
        ///     </para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Array2DBounds FromValueTuple(ValueTuple<int, int> value)
        {
            try
            {
               return new Array2DBounds(value.Item1, value.Item2);
            }
            catch(ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Converts the value of this instance to
        ///     <see cref="ValueTuple"/>&lt;<see cref="int"/>, <see cref="int"/>&gt;. 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (int Rows, int Columns) ToValueTuple() => new ValueTuple<int, int>(len1, len2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Array2DBounds left, Array2DBounds right)
            => left.len1 == right.len1 && left.len2 == right.len2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Array2DBounds left, Array2DBounds right)
            => left.len1 != right.len1 || left.len2 != right.len2;
    }
}
