using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents index of a two-dimensional collection.
    /// </summary>
    [Serializable]
    public readonly struct Index2D : IEquatable<Index2D>
    {
        /// <summary>
        ///     Returns first index of an item. This field is equal to <see cref="Row"/>.
        /// </summary>
        public readonly int Dimension1Index;

        /// <summary>
        ///     Returns second index of an item. This field is equal to <see cref="Column"/>.
        /// </summary>
        public readonly int Dimension2Index;

        /// <summary>
        ///     Returns row position of an item. This property is equal to
        ///     <see cref="Dimension1Index"/>.
        /// </summary>
        public int Row
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Dimension1Index;
        }

        /// <summary>
        ///     Returns column position of an item. This property is equal to
        ///     <see cref="Dimension2Index"/>.
        /// </summary>
        public int Column
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Dimension2Index;
        }

        /// <summary>
        ///     Constructs a two-dimensional index that consist of a row and a column.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public Index2D(int row, int column)
        {
            Dimension1Index = row;
            Dimension2Index = column;
        }

        /// <summary>
        ///     Gets item stored at this instance index from an array. 
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="IndexOutOfRangeException"/>: Specified index does not exist in
        ///         an array.
        ///     </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public T GetItemFrom<T>(Array2D<T> array)
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
        ///     Tries to get value stored at this instance index from an array.
        ///     If operation fails, this returns null (<typeparamref name="T"/>?
        ///     with HasValue property set to false).
        /// </summary>
        /// <typeparam name="T"><typeparamref name="T"/> is struct.</typeparam>
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
        ///     Tries to get reference stored at this instance index from an array.
        ///     If operation fails, this function returns null.
        /// </summary>
        /// <typeparam name="T"><typeparamref name="T"/> is class.</typeparam>
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
        ///     Returns a value indicating whether this instance is equal to a specified
        ///     <see cref="Index2D"></see> value.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Index2D other)
        {
            return this == other;
        }

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified
        ///     <see cref="Index2D"/> value.
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
        ///     Returns the hashcode for this instance.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => Dimension1Index.GetHashCode() ^ (Dimension2Index.GetHashCode() << 2);

        /// <summary>
        ///     Returns a <see cref="string"/> representation of this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            const int MaxPossibleLength = 1 + 11 + 2 + 11 + 1;
            var sb = new StringBuilder(MaxPossibleLength);
            sb.Append('(');
            sb.Append(Dimension1Index);
            sb.Append(", ");
            sb.Append(Dimension2Index);
            sb.Append(')');
            return sb.ToString();
        }

        /// <summary>
        ///     Converts the value of this instance to 
        ///     <see cref="ValueTuple"/>&lt;<see cref="int"/>,<see cref="int"/>&gt;.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (int Row, int Column) ToValueTuple() 
            => new ValueTuple<int, int>(Dimension1Index, Dimension2Index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Index2D(ValueTuple<int, int> value)
            => new Index2D(value.Item1, value.Item2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator== (Index2D left, Index2D right)
            => left.Dimension1Index == right.Dimension1Index && left.Dimension2Index == right.Dimension2Index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator!= (Index2D left, Index2D right)
            => left.Dimension1Index != right.Dimension1Index || left.Dimension2Index != right.Dimension2Index;
    }
}
