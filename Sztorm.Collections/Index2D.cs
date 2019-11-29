using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Sztorm.Collections
{
    /// <summary>
    /// Represents index of two-dimensional collection.
    /// </summary>
    public readonly struct Index2D
    {
        public readonly int PositionIn1stDimension;
        public readonly int PositionIn2ndDimention;

        public int Row
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => PositionIn1stDimension;
        }

        public int Column
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => PositionIn2ndDimention;
        }

        /// <summary>
        /// Constructs a two-dimensional index that consist of a row and a column.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public Index2D(int row, int column)
        {
            PositionIn1stDimension = row;
            PositionIn2ndDimention = column;
        }

        /// <summary>
        /// Gets value from an array. 
        /// <para>
        /// Throws <see cref="IndexOutOfRangeException"></see> if specified index does not exist in an array.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public T GetValueFrom<T>(Array2D<T> array)
        {
            try
            {
                return array[this];
            }
            catch (IndexOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        /// Tries to get value from an array.
        /// If operation fails, this returns null (<see cref="T"></see>?
        /// with HasValue property set to false).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public T? TryGetValueFrom<T>(Array2D<T> array) where T : struct
        {
            if (!array.IsValidIndex(this))
            {
                return null;
            }
            return array[this];
        }

        /// <summary>
        /// Tries to get reference from an array.
        /// If operation fails, this function returns null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public T TryGetRefFrom<T>(Array2D<T> array) where T : class
        {
            if (!array.IsValidIndex(this))
            {
                return null;
            }
            return array[this];
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified
        /// <see cref="Index2D"></see> value.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Index2D other)
        {
            return this == other;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified
        /// <see cref="Index2D"/> value.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            if (!(other is Index2D))
            {
                return false;
            }
            return this == (Index2D)other;
        }

        /// <summary>
        /// Returns the hashcode for this instance.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return PositionIn1stDimension.GetHashCode() ^ (PositionIn2ndDimention.GetHashCode() << 2);
        }

        /// <summary>
        /// Returns <see cref="string"/> representation of this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            const int MaxPossibleLength = 1 + 11 + 2 + 11 + 1;
            var sb = new StringBuilder(MaxPossibleLength);
            sb.Append('(');
            sb.Append(PositionIn1stDimension);
            sb.Append(", ");
            sb.Append(PositionIn2ndDimention);
            sb.Append(')');
            return sb.ToString();
        }

        /// <summary>
        /// Converts the value of this instance to <see cref="ValueTuple{int}{int}"/>.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (int Row, int Column) ToValueTuple() => new ValueTuple<int, int>(PositionIn1stDimension, PositionIn2ndDimention);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator== (Index2D left, Index2D right)
            => left.PositionIn1stDimension == right.PositionIn1stDimension && left.PositionIn2ndDimention == right.PositionIn2ndDimention;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator!= (Index2D left, Index2D right)
            => left.PositionIn1stDimension != right.PositionIn1stDimension || left.PositionIn2ndDimention != right.PositionIn2ndDimention;
    }
}
