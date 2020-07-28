using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    using static RectangularCollectionUtils;

    /// <summary>
    ///     Represents two-dimensional row-major ordered rectangular array of single type allocated
    ///     within single contiguous block of memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public sealed partial class Array2D<T> : IRefRectangularCollection<T>, ICollection
    {
        /// <summary>
        ///     Internal buffer for items. Exposed for optimization purposes.
        /// </summary>
        internal readonly T[] items;
        private readonly Bounds2D bounds;

        /// <summary>
        ///     Returns total number of rows in this two-dimensional array instance.
        /// </summary>
        public int Rows
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.Rows;
        }

        /// <summary>
        ///     Returns total number of columns in this two-dimensional array instance.
        /// </summary>
        public int Columns
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.Columns;
        }

        /// <summary>
        ///     Returns boundaries of current <see cref="Array2D{T}"/> instance.
        /// </summary>
        public Bounds2D Boundaries
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds;
        }

        /// <summary>
        ///     Returns total count of elements in all the dimensions.
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => items.Length;
        }

        /// <summary>
        ///     This collection is not synchronized internally. To synchronize access use lock
        ///     statement with <see cref="SyncRoot"/> property.
        /// </summary>
        public bool IsSynchronized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => false;
        }

        /// <summary>
        ///     Gets an object that can be used to synchronize access to the
        ///     <see cref="Array2D{T}"/>.
        /// </summary>
        public object SyncRoot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => items.SyncRoot;
        }

        /// <summary>
        ///     Returns a row at specified index. Indexing start at zero.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Index is out of boundaries of the row
        ///         count.
        ///     </para>
        /// </summary>
        /// <param name="index">A zero-based index that determines which row is to take.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RefRow<T, Array2D<T>> GetRow(int index)
        {
            try
            {
                return new RefRow<T, Array2D<T>>(this, index);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns a column at specified index. Indexing start at zero.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Index is out of boundaries of the
        ///         column count.
        ///     </para>
        /// </summary>
        /// <param name="index">A zero-based index that determines which column is to take.</param> 
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RefColumn<T, Array2D<T>> GetColumn(int index)
        {
            try
            {
                return new RefColumn<T, Array2D<T>>(this, index);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns an enumerator for all elements of two-dimensional array.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<T> GetEnumerator() => new Enumerator(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///     Returns an element stored at specified row and column.<br/>
        ///     Arguments are not checked. For internal purposes only.
        /// </summary>
        /// <param name="row">
        ///     Range: ([0, <see cref="Rows"/>).
        /// </param>
        /// <param name="column">
        ///     Range: ([0, <see cref="Columns"/>).
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T GetItemInternal(int row, int column)
            => ref items[row * Columns + column];

        /// <summary>
        ///     Returns an element stored at specified index.<br/>
        ///     Argument is not checked. For internal purposes only.
        /// </summary>
        /// <param name="index">
        ///     Range: ([0, <see cref="Rows"/>), [0, <see cref="Columns"/>)).
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T GetItemInternal(Index2D index)
            => ref items[index.Dimension1Index * Columns + index.Dimension2Index];

        /// <summary>
        ///     Returns a shallow copy of a sector of this <see cref="Array2D{T}"/> instance.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         must be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds along with
        ///         <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <param name="startIndex">The zero-based index at which the sector starts.</param>
        /// <param name="sectorSize">The size of the sector to be copied.</param>
        /// <returns></returns>
        public Array2D<T> GetSector(Index2D startIndex, Bounds2D sectorSize)
        {
            if (!IsValidIndex(startIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startIndex), "startIndex must be within array bounds.");
            }
            if (startIndex.Row + sectorSize.Rows > this.Rows ||
                startIndex.Column + sectorSize.Columns > this.Columns)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sectorSize),
                    "sectorSize must be within array bounds along with startIndex.");
            }
            var result = new Array2D<T>(sectorSize);

            CopyToInternal(startIndex, result, sectorSize, new Index2D());
            return result;
        }

        /// <summary>
        ///     Returns an element stored at specified one-dimensional index. Indexing start at
        ///     zero and follows row-major ordering.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="IndexOutOfRangeException"/>: Index cannot exceed
        ///         <see cref="Count"/>.
        ///     </para>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                try
                {
                    return ref items[index];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException("Index cannot exceed Count.");
                }
            }
        }

        /// <summary>
        ///     Returns an element stored at specified row and column.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="IndexOutOfRangeException"/>: Index is out of array bounds.
        ///     </para>
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public ref T this[int row, int column]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (!IsValidIndex(row, column))
                {
                    throw new IndexOutOfRangeException("Index is out of array bounds.");
                }
                return ref items[row * Columns + column];
            }
        }

        /// <summary>
        ///     Returns an element stored at specified index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="IndexOutOfRangeException"/>: Index is out of array bounds.
        ///     </para>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ref T this[Index2D index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (!IsValidIndex(index))
                {
                    throw new IndexOutOfRangeException("Index is out of array bounds.");
                }
                return ref items[index.Dimension1Index * Columns + index.Dimension2Index];
            }
        }

        /// <summary>
        ///     Constructs a two-dimensional rectangular array with specified quantity of rows and
        ///     columns.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: All the arguments must be
        ///         greater than zero.
        ///     </para>
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        public Array2D(int rows, int columns)
        {
            try
            {
                bounds = new Bounds2D(rows, columns);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
            items = new T[rows * columns];
        }

        /// <summary>
        ///     Constructs a two-dimensional rectangular array with specified boundaries.
        /// </summary>
        /// <param name="boundaries"></param>
        public Array2D(Bounds2D boundaries)
        {
            bounds = boundaries;
            items = new T[boundaries.Length1 * boundaries.Length2];
        }

        /// <summary>
        ///     Determines whether specified item exists in the current instance.<br/>
        ///     Use <see cref="ContainsEquatable{U}(U)"/> or <see cref="ContainsComparable{U}(U)"/>
        ///     to avoid unnecessary boxing if stored type is <see cref="IEquatable{T}"/> or
        ///     <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item) => Index1DOf(item).IsSuccess;

        /// <summary>
        ///     Determines whether specified item exists in the current instance.<br/>
        ///     To search for <see langword="null"/> use <see cref="Contains(T)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsComparable<U>(U item) where U : T, IComparable<T>
        {
            try
            {
                return Index1DOfComparable(item).IsSuccess;
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("item cannot be null.");
            }
        }

        /// <summary>
        ///     Determines whether specified item exists in the current instance.<br/>
        ///     To search for <see langword="null"/> use <see cref="Contains(T)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsEquatable<U>(U item) where U : T, IEquatable<T>
        {
            try
            {
                return Index1DOfEquatable(item).IsSuccess;
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("item cannot be null.");
            }
        }

        /// <summary>
        ///     Returns a new <see cref="Array2D{TOutput}"/> instance containing items from this
        ///     instance converted to another type.
        /// </summary>
        /// <typeparam name="TOutput">
        ///     Target type of resulting <see cref="Array2D{T}"/>.
        /// </typeparam>
        /// <typeparam name="TConverter">
        ///     <typeparamref name = "TConverter"/> is <see cref="IConverter{T, TOutput}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="converter">
        ///     An <see langword="struct"/> implementing <see cref="IConverter{T, TOutput}"/> that
        ///     converts each element from one type to another type.
        /// </param>
        /// <returns></returns>
        public Array2D<TOutput> ConvertAll<TOutput, TConverter>(TConverter converter)
            where TConverter : struct, IConverter<T, TOutput>
        {
            var result = new Array2D<TOutput>(this.bounds);

            for (int i = 0, length = Count; i < length; i++)
            {
                result.items[i] = converter.Invoke(items[i]);
            }
            return result;
        }

        /// <summary>
        ///     Returns a new <see cref="Array2D{TOutput}"/> instance containing items from this
        ///     instance converted to another type.<br/>
        ///     Use <see cref="ConvertAll{TOutput, TConverter}(TConverter)"/> to avoid virtual
        ///     call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="converter"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///     </para>
        /// </summary>
        /// <typeparam name="TOutput">
        ///     Target type of resulting <see cref="Array2D{T}"/>.
        /// </typeparam>
        /// <param name="converter">
        ///    A <see cref="Converter{TInput, TOutput}"/> delegate that converts each element from
        ///    one type to another type.
        /// </param>
        /// <returns></returns>
        public Array2D<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter), "converter cannot be null.");
            }
            return ConvertAll<TOutput, BoxedConverter<T, TOutput>>(
                new BoxedConverter<T, TOutput>(converter));
        }

        /// <summary>
        ///     Copies all the elements of the current two-dimensional array to the specified
        ///     one-dimensional array starting at the specified destination array index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: Array is null.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Index is less than the lower
        ///         bound of array.<br/>
        ///         <see cref="ArgumentException"/>: array is multidimensional. -or- The number of
        ///         elements in the source array is greater than the available number of elements
        ///         from index to the end of the destination array.<br/>
        ///         <see cref="ArrayTypeMismatchException"/>: The type of the source array cannot
        ///         be cast automatically to the type of the destination array.<br/>
        ///         <see cref="RankException"/>: The source array is multidimensional.<br/>
        ///         <see cref="InvalidCastException"/>: At least one element in the source array
        ///         cannot be cast to the type of destination array.<br/>
        ///     </para>
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="destIndex">Represents the index in array at which copying begins.</param>
        public void CopyTo(Array destination, int destIndex)
        {
            try
            {
                items.CopyTo(destination, destIndex);
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (ArrayTypeMismatchException)
            {
                throw;
            }
            catch (RankException)
            {
                throw;
            }
            catch (InvalidCastException)
            {
                throw;
            }
        }

        internal void CopyToInternal(
            Index2D srcIndex, T[,] dest, Bounds2D sectorSize, Index2D destIndex)
        {
            int dr = destIndex.Row;
            int sr = srcIndex.Row;
            int totalRows = destIndex.Row + sectorSize.Rows;
            int totalCols = destIndex.Column + sectorSize.Columns;
            int srcIndex1D = RowMajorIndex2DToInt(new Index2D(sr, srcIndex.Column), Columns);
            int stepsToNextSrcIndex = Columns - sectorSize.Columns;

            for (; dr < totalRows; dr++, sr++, srcIndex1D += stepsToNextSrcIndex)
            {
                int dc = destIndex.Column;
                int sc = srcIndex.Column;

                for (; dc < totalCols; dc++, sc++, srcIndex1D++)
                {
                    dest[dr, dc] = items[srcIndex1D];
                }
            }
        }

        /// <summary>
        ///     Copies specified sector of the current instance to the particular two-dimensional
        ///     array at the specified destination index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="destination"/> cannot be
        ///         null.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="sourceIndex"/> must
        ///         be within source array bounds;<br/>
        ///         <paramref name="destIndex"/> must be within destination array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within source and destination array
        ///         bounds along with specified indices.
        ///     </para>
        /// </summary>
        /// <param name="sourceIndex">
        ///     The zero-based index from which copying items of source array begin. 
        /// </param>
        /// <param name="destination">The array to which elements are copied.</param>
        /// <param name="sectorSize">The size of the sector of items to be copied.</param>
        /// <param name="destIndex">
        ///     The zero-based index of destination array from which items begin to be copied.
        /// </param>
        public void CopyTo(
            Index2D sourceIndex, T[,] destination, Bounds2D sectorSize, Index2D destIndex)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(
                    nameof(destination), "destination cannot be null.");
            }
            if (!IsValidIndex(sourceIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sourceIndex),"sourceIndex must be within source array bounds.");
            }
            var destBounds = new Bounds2D(
                new Box<int>(destination.GetLength(0)), new Box<int>(destination.GetLength(1)));

            if (!destBounds.IsValidIndex(destIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(destIndex), "destIndex must be within destination array bounds.");
            }
            if (sourceIndex.Row + sectorSize.Rows > this.Rows ||
                sourceIndex.Column + sectorSize.Columns > this.Columns ||
                destIndex.Row + sectorSize.Rows > destBounds.Rows ||
                destIndex.Column + sectorSize.Columns > destBounds.Columns)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sectorSize),
                    "sectorSize must be within source and destination array bounds along with " +
                    "specified indices.");
            }
            CopyToInternal(sourceIndex, destination, sectorSize, destIndex);
        }

        /// <summary>
        ///     Copies specified sector from the beginning index of the current instance to the
        ///     particular two-dimensional array at the specified destination index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="destination"/> cannot be
        ///         null.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="destIndex"/> must be
        ///         within destination array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within source and destination array
        ///         bounds along with specified <paramref name="destIndex"/>.
        ///     </para>
        /// </summary>
        /// <param name="destination">The array to which elements are copied.</param>
        /// <param name="sectorSize">The size of the sector of items to be copied.</param>
        /// <param name="destIndex">
        ///     The zero-based index of destination array from which items begin to be copied.
        /// </param>
        public void CopyTo(T[,] destination, Bounds2D sectorSize, Index2D destIndex)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(
                    nameof(destination), "destination cannot be null.");
            }
            var destBounds = new Bounds2D(
                new Box<int>(destination.GetLength(0)), new Box<int>(destination.GetLength(1)));

            if (!destBounds.IsValidIndex(destIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(destIndex), "destIndex must be within destination array bounds.");
            }
            if (sectorSize.Rows > this.Rows ||
                sectorSize.Columns > this.Columns ||
                destIndex.Row + sectorSize.Rows > destBounds.Rows ||
                destIndex.Column + sectorSize.Columns > destBounds.Columns)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sectorSize),
                    "sectorSize must be within source and destination array bounds along with " +
                    "specified destIndex.");
            }
            CopyToInternal(new Index2D(), destination, sectorSize, destIndex);
        }

        /// <summary>
        ///     Copies specified sector from the beginning index of the current instance to the
        ///     particular two-dimensional array at the starting index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="destination"/> cannot be
        ///         null.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="sectorSize"/> must
        ///         be within source and destination array bounds.
        ///     </para>
        /// </summary>
        /// <param name="destination">The array to which elements are copied.</param>
        /// <param name="sectorSize">The size of the sector of items to be copied.</param>
        public void CopyTo(T[,] destination, Bounds2D sectorSize)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(
                        nameof(destination), "destination cannot be null.");
            }
            var destBounds = new Bounds2D(
                new Box<int>(destination.GetLength(0)), new Box<int>(destination.GetLength(1)));

            if (sectorSize.Rows > this.Rows || sectorSize.Columns > this.Columns ||
                sectorSize.Rows > destBounds.Rows || sectorSize.Columns > destBounds.Columns)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sectorSize),
                    "sectorSize must be within source and destination array bounds.");
            }
            CopyToInternal(new Index2D(), destination, sectorSize, new Index2D());
        }

        /// <summary>
        ///     Copies all elements of the current instance from the beginning index to the
        ///     particular two-dimensional array at the specified destination index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="destination"/> cannot be
        ///         null.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="destIndex"/> must be
        ///         within destination array bounds.<br/>
        ///         <see cref="ArgumentException"/>: <paramref name="destination"/> must be able to
        ///         accommodate all source array elements along with specified
        ///         <paramref name="destIndex"/>.
        ///     </para>
        /// </summary>
        /// <param name="destination">The array to which elements are copied.</param>
        /// <param name="destIndex">
        ///     The zero-based index of destination array from which items begin to be copied.
        /// </param>
        public void CopyTo(T[,] destination, Index2D destIndex)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(
                        nameof(destination), "destination cannot be null.");
            }
            var destBounds = new Bounds2D(
                new Box<int>(destination.GetLength(0)), new Box<int>(destination.GetLength(1)));

            if (!destBounds.IsValidIndex(destIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(destIndex), "destIndex must be within destination array bounds.");
            }
            if (this.Rows + destIndex.Row > destBounds.Rows ||
                this.Columns + destIndex.Column > destBounds.Columns)
            {
                throw new ArgumentException(
                    "destination must be able to accommodate all source array elements along " +
                    "with specified destIndex.",
                    nameof(destination));
            }
            CopyToInternal(new Index2D(), destination, this.bounds, destIndex);
        }

        /// <summary>
        ///     Copies all elements of the current instance beginning index to the beginning to the
        ///     particular two-dimensional array at the starting index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="destination"/> cannot be
        ///         null.<br/>
        ///         <see cref="ArgumentException"/>: <paramref name="destination"/> must be able to
        ///         accommodate all source array elements.
        ///     </para>
        /// </summary>
        /// <param name="destination">The array to which elements are copied.</param>
        public void CopyTo(T[,] destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(
                        nameof(destination), "destination cannot be null.");
            }
            var destBounds = new Bounds2D(
                new Box<int>(destination.GetLength(0)), new Box<int>(destination.GetLength(1)));

            if (this.Rows > destBounds.Rows || this.Columns > destBounds.Columns)
            {
                throw new ArgumentException(
                    "destination must be able to accommodate all source array elements.",
                    nameof(destination));
            }
            CopyToInternal(new Index2D(), destination, destBounds, new Index2D());
        }

        internal void CopyToInternal(
            Index2D srcIndex, Array2D<T> dest, Bounds2D sectorSize, Index2D destIndex)
        {
            int srcCols = Columns;
            int dstCols = dest.Columns;
            int dr = destIndex.Row;
            int sr = srcIndex.Row;
            int totalRows = dr + sectorSize.Rows;
            int srcIndex1D = RowMajorIndex2DToInt(new Index2D(sr, srcIndex.Column), srcCols);
            int dstIndex1D = RowMajorIndex2DToInt(new Index2D(dr, destIndex.Column), dstCols);

            for (; dr < totalRows; dr++, sr++, srcIndex1D += srcCols, dstIndex1D += dstCols)
            {
                Array.Copy(items, srcIndex1D, dest.items, dstIndex1D, sectorSize.Columns);
            }
        }

        /// <summary>
        ///     Copies specified sector of the current instance to the particular
        ///     <see cref="Array2D{T}"/> at the specified destination index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="destination"/> cannot be
        ///         null.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="sourceIndex"/> must
        ///         be within source array bounds;<br/>
        ///         <paramref name="destIndex"/> must be within destination array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within source and destination array
        ///         bounds along with specified indices.
        ///     </para>
        /// </summary>
        /// <param name="sourceIndex">
        ///     The zero-based index from which copying items of source array begin. 
        /// </param>
        /// <param name="destination">The array to which elements are copied.</param>
        /// <param name="sectorSize">The size of the sector of items to be copied.</param>
        /// <param name="destIndex">
        ///     The zero-based index of destination array from which items begin to be copied.
        /// </param>
        public void CopyTo(
            Index2D sourceIndex, Array2D<T> destination, Bounds2D sectorSize, Index2D destIndex)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(
                    nameof(destination), "destination cannot be null.");
            }
            if (!IsValidIndex(sourceIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sourceIndex), "sourceIndex must be within source array bounds.");
            }
            if (!destination.IsValidIndex(destIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(destIndex), "destIndex must be within destination array bounds.");
            }
            if (sourceIndex.Row + sectorSize.Rows > this.Rows ||
                sourceIndex.Column + sectorSize.Columns > this.Columns ||
                destIndex.Row + sectorSize.Rows > destination.Rows ||
                destIndex.Column + sectorSize.Columns > destination.Columns)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sectorSize),
                    "sectorSize must be within source and destination array bounds along with " +
                    "specified indices.");
            }
            CopyToInternal(sourceIndex, destination, sectorSize, destIndex);
        }

        /// <summary>
        ///     Copies specified sector from the beginning index of the current instance to the
        ///     particular <see cref="Array2D{T}"/> at the specified destination index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="destination"/> cannot be
        ///         null.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="destIndex"/> must be
        ///         within destination array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within source and destination array
        ///         bounds along with specified <paramref name="destIndex"/>.
        ///     </para>
        /// </summary>
        /// <param name="destination">The array to which elements are copied.</param>
        /// <param name="sectorSize">The size of the sector of items to be copied.</param>
        /// <param name="destIndex">
        ///     The zero-based index of destination array from which items begin to be copied.
        /// </param>
        public void CopyTo(Array2D<T> destination, Bounds2D sectorSize, Index2D destIndex)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(
                    nameof(destination), "destination cannot be null.");
            }
            if (!destination.IsValidIndex(destIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(destIndex), "destIndex must be within destination array bounds.");
            }
            if (sectorSize.Rows > this.Rows ||
                sectorSize.Columns > this.Columns ||
                destIndex.Row + sectorSize.Rows > destination.Rows ||
                destIndex.Column + sectorSize.Columns > destination.Columns)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sectorSize),
                    "sectorSize must be within source and destination array bounds along with " +
                    "specified destIndex.");
            }
            CopyToInternal(new Index2D(), destination, sectorSize, destIndex);
        }

        /// <summary>
        ///     Copies specified sector from the beginning index of the current instance to the
        ///     particular <see cref="Array2D{T}"/> at the starting index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="destination"/> cannot be
        ///         null.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="sectorSize"/> must
        ///         be within source and destination array bounds.
        ///     </para>
        /// </summary>
        /// <param name="destination">The array to which elements are copied.</param>
        /// <param name="sectorSize">The size of the sector of items to be copied.</param>
        public void CopyTo(Array2D<T> destination, Bounds2D sectorSize)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(
                        nameof(destination), "destination cannot be null.");
            }
            if (sectorSize.Rows > this.Rows || sectorSize.Columns > this.Columns ||
                sectorSize.Rows > destination.Rows || sectorSize.Columns > destination.Columns)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sectorSize),
                    "sectorSize must be within source and destination array bounds.");
            }
            CopyToInternal(new Index2D(), destination, sectorSize, new Index2D());
        }

        /// <summary>
        ///     Copies all elements of the current instance from the beginning index to the
        ///     particular <see cref="Array2D{T}"/> at the specified destination index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="destination"/> cannot be
        ///         null.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="destIndex"/> must be
        ///         within destination array bounds.<br/>
        ///         <see cref="ArgumentException"/>: <paramref name="destination"/> must be able to
        ///         accommodate all source array elements along with specified
        ///         <paramref name="destIndex"/>.
        ///     </para>
        /// </summary>
        /// <param name="destination">The array to which elements are copied.</param>
        /// <param name="destIndex">
        ///     The zero-based index of destination array from which items begin to be copied.
        /// </param>
        public void CopyTo(Array2D<T> destination, Index2D destIndex)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(
                        nameof(destination), "destination cannot be null.");
            }
            if (!destination.IsValidIndex(destIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(destIndex), "destIndex must be within destination array bounds.");
            }
            if (this.Rows + destIndex.Row > destination.Rows ||
                this.Columns + destIndex.Column > destination.Columns)
            {
                throw new ArgumentException(
                    "destination must be able to accommodate all source array elements along " +
                    "with specified destIndex.",
                    nameof(destination));
            }
            CopyToInternal(new Index2D(), destination, this.bounds, destIndex);
        }

        /// <summary>
        ///     Copies all elements of the current instance beginning index to the beginning to the
        ///     particular <see cref="Array2D{T}"/> at the starting index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="destination"/> cannot be
        ///         null.<br/>
        ///         <see cref="ArgumentException"/>: <paramref name="destination"/> must be able to
        ///         accommodate all source array elements.
        ///     </para>
        /// </summary>
        /// <param name="destination">The array to which elements are copied.</param>
        public void CopyTo(Array2D<T> destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(
                        nameof(destination), "destination cannot be null.");
            }
            if (this.Rows > destination.Rows || this.Columns > destination.Columns)
            {
                throw new ArgumentException(
                    "destination must be able to accommodate all source array elements.",
                    nameof(destination));
            }
            CopyToInternal(new Index2D(), destination, destination.bounds, new Index2D());
        }

        /// <summary>
        ///     Determines whether any item that match the conditions defined by the specified
        ///     predicate exists in the current instance.
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Exists<TPredicate>(TPredicate match)
            where TPredicate : struct, IPredicate<T>
            => FindIndex1D(match).IsSuccess;

        /// <summary>
        ///     Determines whether any item that match the conditions defined by the specified
        ///     predicate exists in the current instance.<br/>  
        ///     Use <see cref="Exists{TPredicate}(TPredicate)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/> <paramref name="match"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Exists(Predicate<T> match)
        {
            try
            {
                return FindIndex1D(match).IsSuccess;
            }
            catch (ArgumentNullException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying first
        ///     occurrence searched row by row within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/>
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<T> Find<TPredicate>(TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            for (int i = 0, length = Count; i < length; i++)
            {
                ref readonly T item = ref items[i];

                if (match.Invoke(item))
                {
                    return new ItemRequestResult<T>(item);
                }
            }
            return ItemRequestResult<T>.Fail;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying first
        ///     occurrence searched row by row within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="Find{TPredicate}(TPredicate)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<T> Find(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }
            return Find(new BoxedPredicate<T>(match));
        }

        /// <summary>
        ///     Returns <see cref="ICollection{T}"/> containing all the elements that match the
        ///     conditions defined by the specified predicate.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="NotSupportedException"/>: Provided type must support 
        ///         <see cref="ICollection{T}.Add(T)"/> method.
        ///     </para>
        /// </summary>
        /// <typeparam name="TCollection">
        ///     <typeparamref name = "TCollection"/> is <see cref="ICollection{T}"/> and has a
        ///     parameterless constructor.
        /// </typeparam>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        public TCollection FindAll<TCollection, TPredicate>(TPredicate match)
            where TCollection : ICollection<T>, new()
            where TPredicate : struct, IPredicate<T>
        {
            var resizableCollection = new TCollection();
            try
            {
                for (int i = 0, length = Count; i < length; i++)
                {
                    ref readonly T item = ref items[i];

                    if (match.Invoke(item))
                    {
                        resizableCollection.Add(item);
                    }
                }
            }
            catch (NotSupportedException)
            {
                throw new NotSupportedException("Provided type must support Add(T) method.");
            }
            return resizableCollection;
        }

        /// <summary>
        ///     Returns <see cref="ICollection{T}"/> containing all the elements that match the
        ///     conditions defined by the specified predicate.<br/>
        ///     Use <see cref="FindAll{TCollection, TPredicate}(TPredicate)"/> to avoid virtual
        ///     call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="NotSupportedException"/>: Provided type must support
        ///         <see cref="ICollection{T}.Add(T)"/> method.
        ///     </para>
        /// </summary>
        /// <typeparam name="TCollection">
        ///     <typeparamref name = "TCollection"/> is <see cref="ICollection{T}"/> and has a
        ///     parameterless constructor.
        /// </typeparam>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        public TCollection FindAll<TCollection>(Predicate<T> match)
            where TCollection : ICollection<T>, new()
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }
            try
            {
                return FindAll<TCollection, BoxedPredicate<T>>(new BoxedPredicate<T>(match));
            }
            catch (NotSupportedException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns <see cref="ICollection{T}"/> containing all the indices of which items
        ///     match the conditions defined by the specified predicate.<br/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="NotSupportedException"/>: Provided type must support 
        ///         <see cref="ICollection{T}.Add(T)"/> method.
        ///     </para>
        /// </summary>
        /// <typeparam name="Index2DCollection">
        ///     <typeparamref name = "Index2DCollection"/> is <see cref="ICollection{T}"/> and has a
        ///     parameterless constructor.
        /// </typeparam>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        public Index2DCollection FindAllIndices<Index2DCollection, TPredicate>(TPredicate match)
            where Index2DCollection : ICollection<Index2D>, new()
            where TPredicate : struct, IPredicate<T>
        {
            var resizableCollection = new Index2DCollection();
            try
            {
                for (int i = 0, length = Count, columns = Columns; i < length; i++)
                {
                    if (match.Invoke(items[i]))
                    {
                        resizableCollection.Add(IntToRowMajorIndex2D(i, columns));
                    }
                }
            }
            catch (NotSupportedException)
            {
                throw new NotSupportedException("Provided type must support Add(T) method.");
            }
            return resizableCollection;
        }

        /// <summary>
        ///     Returns <see cref="ICollection{T}"/> containing all the indices of which items
        ///     match the conditions defined by the specified predicate.<br/>
        ///     Use <see cref="FindAllIndices{TCollection, TPredicate}(TPredicate)"/> to avoid
        ///     virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="NotSupportedException"/>: Provided type must support
        ///         <see cref="ICollection{T}.Add(T)"/> method.
        ///     </para>
        /// </summary>
        /// <typeparam name="Index2DCollection">
        ///     <typeparamref name = "Index2DCollection"/> is <see cref="ICollection{T}"/> and has a
        ///     parameterless constructor.
        /// </typeparam>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        public Index2DCollection FindAllIndices<Index2DCollection>(Predicate<T> match)
            where Index2DCollection : ICollection<Index2D>, new()
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }
            try
            {
                return FindAllIndices<Index2DCollection, BoxedPredicate<T>>(
                    new BoxedPredicate<T>(match));
            }
            catch (NotSupportedException)
            {
                throw;
            }
        }

        internal ItemRequestResult<int> FindIndex1DInternal<TPredicate>(
           int startIndex, int indexAfterEnd, TPredicate match)
           where TPredicate : struct, IPredicate<T>
        {
            Debug.Assert(startIndex >= 0);
            Debug.Assert(indexAfterEnd >= 0);
            Debug.Assert(indexAfterEnd <= Count);

            for (int i = startIndex; i < indexAfterEnd; i++)
            {
                if (match.Invoke(items[i]))
                {
                    return new ItemRequestResult<int>(i);
                }
            }
            return ItemRequestResult<int>.Fail;
        }

        /// <summary>
        ///     Searches from the beginning for an item that matches the conditions defined by the
        ///     specified predicate, and returns the <see cref="ItemRequestResult{T}"/> with
        ///     underlying one-dimensional index of the first occurrence searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/>
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> FindIndex1D<TPredicate>(TPredicate match)
            where TPredicate : struct, IPredicate<T>
            => FindIndex1DInternal(0, Count, match);

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the first occurrence searched row by row within the
        ///     specified range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="count">Number of items to search.</param>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<int> FindIndex1D<TPredicate>(
            Index2D startIndex, int count, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            if (!IsValidIndex(startIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startIndex), "startIndex must be within array bounds.");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count), "count must be greater or equal to zero.");
            }
            int startIndex1D = RowMajorIndex2DToInt(startIndex, Columns);
            int indexAfterEnd = startIndex1D + count;

            if (indexAfterEnd > items.Length)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count), "startIndex together with count must not exceed Array2D.Count");
            }
            return FindIndex1DInternal(startIndex1D, indexAfterEnd, match);
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the first occurrence searched within the specified sector.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning from
        ///         <paramref name="startIndex"/>.
        ///     </para>   
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<int> FindIndex1D<TPredicate>(
            Index2D startIndex, Bounds2D sectorSize, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            if (!IsValidIndex(startIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startIndex), "startIndex must be within array bounds.");
            }
            var indexAfterEnd = new Index2D(
                startIndex.Row + sectorSize.Rows,
                startIndex.Column + sectorSize.Columns);

            if (indexAfterEnd.Row > Rows || indexAfterEnd.Column > Columns)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sectorSize),
                    "sectorSize must be within array bounds, beginning from startIndex.");
            }
            int index1D = RowMajorIndex2DToInt(startIndex, Columns);
            int stepsToNextIndex = Columns - sectorSize.Columns;

            for (int i = startIndex.Row; i < indexAfterEnd.Row; i++, index1D += stepsToNextIndex)
            {
                for (int j = startIndex.Column; j < indexAfterEnd.Column; j++, index1D++)
                {
                    if (match.Invoke(items[index1D]))
                    {
                        return new ItemRequestResult<int>(index1D);
                    }
                }
            }
            return ItemRequestResult<int>.Fail;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the first occurrence searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindIndex1D{TPredicate}(TPredicate)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<int> FindIndex1D(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }
            return FindIndex1DInternal(0, Count, new BoxedPredicate<T>(match));
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the first occurrence searched row by row within the
        ///     specified range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindIndex1D{TPredicate}(Index2D, int, TPredicate)"/> to avoid virtual
        ///     call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="count">Number of items to search.</param>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<int> FindIndex1D(Index2D startIndex, int count, Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            try
            {
                return FindIndex1D(startIndex, count, new BoxedPredicate<T>(match));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the first occurrence searched within the specified sector.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindIndex1D{TPredicate}(Index2D, Bounds2D, TPredicate)"/> to avoid
        ///     virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning from
        ///         <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<int> FindIndex1D(
            Index2D startIndex, Bounds2D sectorSize, Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            try
            {
                return FindIndex1D(startIndex, sectorSize, new BoxedPredicate<T>(match));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        private ItemRequestResult<Index2D> RequestedIntToRequested2DIndex(
            ItemRequestResult<int> possibleIndex)
        {
            if (!possibleIndex.IsSuccess)
            {
                return ItemRequestResult<Index2D>.Fail;
            }
            return new ItemRequestResult<Index2D>(
                IntToRowMajorIndex2D(possibleIndex.ItemOrDefault, Columns));
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence searched within the entire <see cref="Array2D{T}"/> if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/>
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> FindIndex<TPredicate>(TPredicate match)
            where TPredicate : struct, IPredicate<T>
            => RequestedIntToRequested2DIndex(FindIndex1D(match));

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="count">Number of items to search.</param>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> FindIndex<TPredicate>(
            Index2D startIndex, int count, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindIndex1D(startIndex, count, match));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence searched within the specified sector. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning from
        ///         <paramref name="startIndex"/>.
        ///     </para>   
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> FindIndex<TPredicate>(
            Index2D startIndex, Bounds2D sectorSize, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindIndex1D(startIndex, sectorSize, match));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence searched within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindIndex{TPredicate}(TPredicate)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/> <paramref name="match"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> FindIndex(Predicate<T> match)
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindIndex1D(match));
            }
            catch (ArgumentNullException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindIndex{TPredicate}(Index2D, int, TPredicate)"/> to avoid
        ///     virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="count">Number of items to search.</param>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> FindIndex(
            Index2D startIndex, int count, Predicate<T> match)
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindIndex1D(startIndex, count, match));
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence searched within the specified sector. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindIndex{TPredicate}(Index2D, Bounds2D, TPredicate)"/> to avoid
        ///     virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning from
        ///         <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> FindIndex(
            Index2D startIndex, Bounds2D sectorSize, Predicate<T> match)
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindIndex1D(startIndex, sectorSize, match));
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying last
        ///     occurrence searched row by row within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/>
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<T> FindLast<TPredicate>(TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                ref readonly T item = ref items[i];

                if (match.Invoke(item))
                {
                    return new ItemRequestResult<T>(item);
                }
            }
            return ItemRequestResult<T>.Fail;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying last
        ///     occurrence searched row by row within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindLast{TPredicate}(TPredicate)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<T> FindLast(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }
            return FindLast(new BoxedPredicate<T>(match));
        }

        internal ItemRequestResult<int> FindLastIndex1DInternal<TPredicate>(
            int startIndex, int indexAfterEnd, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            Debug.Assert(startIndex < Count);
            Debug.Assert(indexAfterEnd >= -1);

            for (int i = startIndex; i > indexAfterEnd; i--)
            {
                if (match.Invoke(items[i]))
                {
                    return new ItemRequestResult<int>(i);
                }
            }
            return ItemRequestResult<int>.Fail;
        }

        /// <summary>
        ///     Searches from the beginning for an item that matches the conditions defined by the
        ///     specified predicate, and returns the <see cref="ItemRequestResult{T}"/> with
        ///     underlying one-dimensional index of the last occurrence searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/>
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> FindLastIndex1D<TPredicate>(TPredicate match)
            where TPredicate : struct, IPredicate<T>
            => FindLastIndex1DInternal(Count - 1, -1, match);

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the last occurrence searched row by row within the
        ///     specified range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="count">Number of items to search.</param>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<int> FindLastIndex1D<TPredicate>(
            Index2D startIndex, int count, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            if (!IsValidIndex(startIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startIndex), "startIndex must be within array bounds.");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count), "count must be greater or equal to zero.");
            }
            int startIndex1D = RowMajorIndex2DToInt(startIndex, Columns);
            int indexAfterEnd = startIndex1D - count;

            if (indexAfterEnd < -1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count), "startIndex together with count must not exceed Array2D.Count");
            }
            return FindLastIndex1DInternal(startIndex1D, indexAfterEnd, match);
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the last occurrence searched within the specified sector.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning backwardly
        ///         from <paramref name="startIndex"/>.
        ///     </para>   
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<int> FindLastIndex1D<TPredicate>(
            Index2D startIndex, Bounds2D sectorSize, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            if (!IsValidIndex(startIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startIndex), "startIndex must be within array bounds.");
            }
            var indexAfterEnd = new Index2D(
                startIndex.Row - sectorSize.Rows,
                startIndex.Column - sectorSize.Columns);

            if (indexAfterEnd.Row < -1 || indexAfterEnd.Column < -1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sectorSize),
                    "sectorSize must be within array bounds, beginning backwardly from " +
                    "startIndex.");
            }
            int index1D = RowMajorIndex2DToInt(startIndex, Columns);
            int stepsToNextIndex = Columns - sectorSize.Columns;

            for (int i = startIndex.Row; i > indexAfterEnd.Row; i--, index1D -= stepsToNextIndex)
            {
                for (int j = startIndex.Column; j > indexAfterEnd.Column; j--, index1D--)
                {
                    if (match.Invoke(items[index1D]))
                    {
                        return new ItemRequestResult<int>(index1D);
                    }
                }
            }
            return ItemRequestResult<int>.Fail;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the last occurrence searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindLastIndex1D{TPredicate}(TPredicate)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<int> FindLastIndex1D(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }
            return FindLastIndex1DInternal(Count - 1, -1, new BoxedPredicate<T>(match));
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the last occurrence searched row by row within the
        ///     specified range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindLastIndex1D{TPredicate}(Index2D, int, TPredicate)"/> to avoid
        ///     virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="count">Number of items to search.</param>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<int> FindLastIndex1D(
            Index2D startIndex, int count, Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            try
            {
                return FindLastIndex1D(startIndex, count, new BoxedPredicate<T>(match));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the last occurrence searched within the specified sector.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindLastIndex1D{TPredicate}(Index2D, Bounds2D, TPredicate)"/> to avoid
        ///     virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning backwardly
        ///         from <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<int> FindLastIndex1D(
            Index2D startIndex, Bounds2D sectorSize, Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            try
            {
                return FindLastIndex1D(startIndex, sectorSize, new BoxedPredicate<T>(match));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches from the beginning for an item that matches the conditions defined by the
        ///     specified predicate, and returns the <see cref="ItemRequestResult{T}"/> with
        ///     underlying index of the last occurrence searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/>
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> FindLastIndex<TPredicate>(TPredicate match)
            where TPredicate : struct, IPredicate<T>
            => RequestedIntToRequested2DIndex(FindLastIndex1D(match));

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the last occurrence searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="count">Number of items to search.</param>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<Index2D> FindLastIndex<TPredicate>(
            Index2D startIndex, int count, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindLastIndex1D(startIndex, count, match));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the last occurrence searched within the specified sector. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning backwardly
        ///         from <paramref name="startIndex"/>.
        ///     </para>   
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<Index2D> FindLastIndex<TPredicate>(
            Index2D startIndex, Bounds2D sectorSize, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            try
            {
                return RequestedIntToRequested2DIndex(
                    FindLastIndex1D(startIndex, sectorSize, match));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the last occurrence searched within the entire <see cref="Array2D{T}"/> if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindLastIndex{TPredicate}(TPredicate)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<Index2D> FindLastIndex(Predicate<T> match)
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindLastIndex1D(match));
            }
            catch (ArgumentNullException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the last occurrence searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindLastIndex{TPredicate}(Index2D, int, TPredicate)"/> to avoid
        ///     virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="count">Number of items to search.</param>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<Index2D> FindLastIndex(
            Index2D startIndex, int count, Predicate<T> match)
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindLastIndex1D(startIndex, count, match));
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the last occurrence searched within the specified sector. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindLastIndex{TPredicate}(Index2D, Bounds2D, TPredicate)"/> to avoid
        ///     virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning backwardly
        ///         from <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        public ItemRequestResult<Index2D> FindLastIndex(
            Index2D startIndex, Bounds2D sectorSize, Predicate<T> match)
        {
            try
            {
                return RequestedIntToRequested2DIndex(
                    FindLastIndex1D(startIndex, sectorSize, match));
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Performs the specified action on each element of the <see cref="Array2D{T}"/>.
        /// </summary>
        /// <typeparam name="TAction">
        ///     <typeparamref name="TAction"/> is <see cref="IAction{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="action">
        ///     A <see langword="struct"/> implementing <see cref="IAction{T}"/> that defines
        ///     an action to perform on each element.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ForEach<TAction>(TAction action) where TAction : IAction<T>
            => ForEach(ref action);

        /// <summary>
        ///     Performs the specified action on each element of the <see cref="Array2D{T}"/>.
        ///     <paramref name="action"/> modifications are reflected to the caller as it is passed
        ///     by <see langword="ref"/>.
        /// </summary>
        /// <typeparam name="TAction">
        ///     <typeparamref name="TAction"/> is <see cref="IAction{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="action">
        ///     A <see langword="struct"/> implementing <see cref="IAction{T}"/> that defines
        ///     an action to perform on each element.
        /// </param>
        public void ForEach<TAction>(ref TAction action) where TAction : IAction<T>
        {
            for (int i = 0, length = Count; i < length; i++)
            {
                action.Invoke(items[i]);
            }
        }

        /// <summary>
        ///     Performs the specified action on each element of the <see cref="Array2D{T}"/>.<br/>
        ///     Use <see cref="ForEach{TAction}(TAction)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="action"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///     </para>
        /// </summary>
        /// <param name="action">
        ///     The <see cref="Action{T}"/> delegate to perform on each element of the
        ///     <see cref="List2D{T}"/>.
        /// </param>
        public void ForEach(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action cannot be null.");
            }
            ForEach(new BoxedAction<T>(action));
        }

        /// <summary>
        /// Creates a <see cref="Array2D{T}"/> from <typeparamref name = "T"/>[,] instance.
        /// <para>
        ///     Exceptions:<br/>
        ///     <see cref="ArgumentNullException"/>: Array cannot be null.
        /// </para>
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static Array2D<T> FromSystem2DArray(T[,] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Array cannot be null.");
            }
            Bounds2D bounds = new Bounds2D(
                new Box<int>(array.GetLength(0)), new Box<int>(array.GetLength(1)));
            var result = new Array2D<T>(bounds);
            int index1D = 0;

            for (int i = 0; i < bounds.Rows; i++)
            {
                for (int j = 0; j < bounds.Columns; j++, index1D++)
                {
                    result.items[index1D] = array[i, j];
                }
            }
            return result;
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="Index1DOfEquatable{U}(U)"/> or <see cref="Index1DOfComparable{U}(U)"/>
        ///     to avoid unnecessary boxing if stored type is <see cref="IEquatable{T}"/> or
        ///     <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> Index1DOf(T item)
            => FindIndex1D(new EqualsObjectPredicate<T>(item));

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched row by row within the specified
        ///     range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="Index1DOfEquatable{U}(U, Index2D, int)"/> or
        ///     <see cref="Index1DOfComparable{U}(U, Index2D, int)"/> to avoid unnecessary boxing if
        ///     stored type is <see cref="IEquatable{T}"/> or <see cref="IComparable{T}"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="count">Number of items to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> Index1DOf(T item, Index2D startIndex, int count)
        {
            try
            {
                return FindIndex1D(startIndex, count, new EqualsObjectPredicate<T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched searched within the specified sector
        ///     if found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="Index1DOfEquatable{U}(U, Index2D, Bounds2D)"/> or
        ///     <see cref="Index1DOfComparable{U}(U, Index2D, Bounds2D)"/> to avoid unnecessary
        ///     boxing if stored type is <see cref="IEquatable{T}"/> or
        ///     <see cref="IComparable{T}"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning from
        ///         <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> Index1DOf(T item, Index2D startIndex, Bounds2D sectorSize)
        {
            try
            {
                return FindIndex1D(startIndex, sectorSize, new EqualsObjectPredicate<T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use <see cref="Index1DOf(T)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> Index1DOfEquatable<U>(U item)
            where U : T, IEquatable<T>
        {
            try
            {
                return FindIndex1D(new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched row by row within the specified
        ///     range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use <see cref="Index1DOf(T, Index2D, int)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="count">Number of items to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> Index1DOfEquatable<U>(U item, Index2D startIndex, int count)
             where U : T, IEquatable<T>
        {
            try
            {
                return FindIndex1D(startIndex, count, new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched searched within the specified sector
        ///     if found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="Index1DOf(T, Index2D, Bounds2D)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning from
        ///         <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> Index1DOfEquatable<U>(
            U item, Index2D startIndex, Bounds2D sectorSize)
             where U : T, IEquatable<T>
        {
            try
            {
                return FindIndex1D(startIndex, sectorSize, new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use <see cref="Index1DOf(T)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> Index1DOfComparable<U>(U item) where U : T, IComparable<T>
        {
            try
            {
                return FindIndex1D(new EqualsComparablePredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched row by row within the specified
        ///     range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use <see cref="Index1DOf(T, Index2D, int)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="count">Number of items to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> Index1DOfComparable<U>(U item, Index2D startIndex, int count)
             where U : T, IComparable<T>
        {
            try
            {
                return FindIndex1D(startIndex, count, new EqualsComparablePredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched searched within the specified sector
        ///     if found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="Index1DOf(T, Index2D, Bounds2D)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning from
        ///         <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> Index1DOfComparable<U>(
            U item, Index2D startIndex, Bounds2D sectorSize)
            where U : T, IComparable<T>
        {
            try
            {
                return FindIndex1D(startIndex, sectorSize, new EqualsComparablePredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="IndexOfEquatable{U}(U)"/> or
        ///     <see cref="IndexOfComparable{U}(U)"/> to avoid unnecessary boxing if stored type
        ///     is <see cref="IEquatable{T}"/> or <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> IndexOf(T item)
             => FindIndex(new EqualsObjectPredicate<T>(item));

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="IndexOfEquatable{U}(U, Index2D, int)"/> or
        ///     <see cref="IndexOfComparable{U}(U, Index2D, int)"/> to avoid unnecessary boxing
        ///     if stored type is <see cref="IEquatable{T}"/> or <see cref="IComparable{T}"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="count">Number of items to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> IndexOf(T item, Index2D startIndex, int count)
        {
            try
            {
                return FindIndex(startIndex, count, new EqualsObjectPredicate<T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched searched within the specified sector if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="IndexOfEquatable{U}(U, Index2D, Bounds2D)"/> or
        ///     <see cref="IndexOfComparable{U}(U, Index2D, Bounds2D)"/> to avoid unnecessary
        ///     boxing if stored type is <see cref="IEquatable{T}"/> or
        ///     <see cref="IComparable{T}"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning from
        ///         <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> IndexOf(
            T item, Index2D startIndex, Bounds2D sectorSize)
        {
            try
            {
                return FindIndex(startIndex, sectorSize, new EqualsObjectPredicate<T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use <see cref="IndexOf(T)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> IndexOfEquatable<U>(U item)
            where U : T, IEquatable<T>
        {
            try
            {
                return FindIndex(new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use <see cref="IndexOf(T, Index2D, int)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="count">Number of items to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> IndexOfEquatable<U>(
            U item, Index2D startIndex, int count)
            where U : T, IEquatable<T>
        {
            try
            {
                return FindIndex(startIndex, count, new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched searched within the specified sector if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="IndexOf(T, Index2D, Bounds2D)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning from
        ///         <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> IndexOfEquatable<U>(
            U item, Index2D startIndex, Bounds2D sectorSize)
             where U : T, IEquatable<T>
        {
            try
            {
                return FindIndex(startIndex, sectorSize, new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use <see cref="IndexOf(T)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> IndexOfComparable<U>(U item)
            where U : T, IComparable<T>
        {
            try
            {
                return FindIndex(new EqualsComparablePredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use <see cref="IndexOf(T, Index2D, int)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="count">Number of items to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> IndexOfComparable<U>(
            U item, Index2D startIndex, int count)
            where U : T, IComparable<T>
        {
            try
            {
                return FindIndex(startIndex, count, new EqualsComparablePredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched searched within the specified sector if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="IndexOf(T, Index2D, Bounds2D)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning from
        ///         <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> IndexOfComparable<U>(
            U item, Index2D startIndex, Bounds2D sectorSize)
            where U : T, IComparable<T>
        {
            try
            {
                return FindIndex(startIndex, sectorSize, new EqualsComparablePredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Determines whether specified index exists in this <see cref="Array2D{T}"/>
        ///     instance.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidIndex(int row, int column)
            => bounds.IsValidIndex(row, column);

        /// <summary>
        ///     Determines whether specified index exists in this <see cref="Array2D{T}"/>
        ///     instance.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidIndex(Index2D index)
            => bounds.IsValidIndex(index);

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="LastIndex1DOfEquatable{U}(U)"/> or
        ///     <see cref="LastIndex1DOfComparable{U}(U)"/> to avoid unnecessary boxing if stored
        ///     type is <see cref="IEquatable{T}"/> or <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> LastIndex1DOf(T item)
            => FindLastIndex1D(new EqualsObjectPredicate<T>(item));

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched row by row within the specified
        ///     range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="LastIndex1DOfEquatable{U}(U, Index2D, int)"/> or
        ///     <see cref="LastIndex1DOfComparable{U}(U, Index2D, int)"/> to avoid unnecessary boxing
        ///     if stored type is <see cref="IEquatable{T}"/> or <see cref="IComparable{T}"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="count">Number of items to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> LastIndex1DOf(T item, Index2D startIndex, int count)
        {
            try
            {
                return FindLastIndex1D(startIndex, count, new EqualsObjectPredicate<T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched searched within the specified sector
        ///     if found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="LastIndex1DOfEquatable{U}(U, Index2D, Bounds2D)"/> or
        ///     <see cref="LastIndex1DOfComparable{U}(U, Index2D, Bounds2D)"/> to avoid unnecessary
        ///     boxing if stored type is <see cref="IEquatable{T}"/> or
        ///     <see cref="IComparable{T}"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning backwardly
        ///         from <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> LastIndex1DOf(T item, Index2D startIndex, Bounds2D sectorSize)
        {
            try
            {
                return FindLastIndex1D(startIndex, sectorSize, new EqualsObjectPredicate<T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use <see cref="LastIndex1DOf(T)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> LastIndex1DOfEquatable<U>(U item)
            where U : T, IEquatable<T>
        {
            try
            {
                return FindLastIndex1D(new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched row by row within the specified range
        ///     of items if found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use <see cref="LastIndex1DOf(T, Index2D, int)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="count">Number of items to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> LastIndex1DOfEquatable<U>(
            U item, Index2D startIndex, int count)
             where U : T, IEquatable<T>
        {
            try
            {
                return FindLastIndex1D(startIndex, count, new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched searched within the specified sector
        ///     if found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="LastIndex1DOf(T, Index2D, Bounds2D)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning backwardly
        ///         from <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> LastIndex1DOfEquatable<U>(
            U item, Index2D startIndex, Bounds2D sectorSize)
             where U : T, IEquatable<T>
        {
            try
            {
                return FindLastIndex1D(startIndex, sectorSize, new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use <see cref="LastIndex1DOf(T)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> LastIndex1DOfComparable<U>(U item) where U : T, IComparable<T>
        {
            try
            {
                return FindLastIndex1D(new EqualsComparablePredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched row by row within the specified range
        ///     of items if found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use <see cref="LastIndex1DOf(T, Index2D, int)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="count">Number of items to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> LastIndex1DOfComparable<U>(
            U item, Index2D startIndex, int count)
             where U : T, IComparable<T>
        {
            try
            {
                return FindLastIndex1D(startIndex, count, new EqualsComparablePredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched searched within the specified sector
        ///     if found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="LastIndex1DOf(T, Index2D, Bounds2D)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning backwardly
        ///         from <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> LastIndex1DOfComparable<U>(
            U item, Index2D startIndex, Bounds2D sectorSize)
            where U : T, IComparable<T>
        {
            try
            {
                return FindLastIndex1D(
                    startIndex, sectorSize, new EqualsComparablePredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="LastIndexOfEquatable{U}(U)"/> or
        ///     <see cref="LastIndexOfComparable{U}(U)"/> to avoid unnecessary boxing if stored
        ///     type is <see cref="IEquatable{T}"/> or <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> LastIndexOf(T item)
            => FindLastIndex(new EqualsObjectPredicate<T>(item));

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="LastIndexOfEquatable{U}(U, Index2D, int)"/> or
        ///     <see cref="LastIndexOfComparable{U}(U, Index2D, int)"/> to avoid unnecessary
        ///     boxing if stored type is <see cref="IEquatable{T}"/> or
        ///     <see cref="IComparable{T}"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="count">Number of items to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> LastIndexOf(T item, Index2D startIndex, int count)
        {
            try
            {
                return FindLastIndex(startIndex, count, new EqualsObjectPredicate<T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched searched within the specified sector if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="LastIndexOfEquatable{U}(U, Index2D, Bounds2D)"/> or
        ///     <see cref="LastIndexOfComparable{U}(U, Index2D, Bounds2D)"/> to avoid unnecessary
        ///     boxing if stored type is <see cref="IEquatable{T}"/> or
        ///     <see cref="IComparable{T}"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning backwardly
        ///         from <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> LastIndexOf(T item, Index2D startIndex, Bounds2D sectorSize)
        {
            try
            {
                return FindLastIndex(startIndex, sectorSize, new EqualsObjectPredicate<T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use <see cref="LastIndexOf(T)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> LastIndexOfEquatable<U>(U item)
            where U : T, IEquatable<T>
        {
            try
            {
                return FindLastIndex(new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="LastIndexOf(T, Index2D, int)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="count">Number of items to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> LastIndexOfEquatable<U>(
            U item, Index2D startIndex, int count)
             where U : T, IEquatable<T>
        {
            try
            {
                return FindLastIndex(startIndex, count, new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched searched within the specified sector if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="LastIndexOf(T, Index2D, Bounds2D)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning backwardly
        ///         from <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> LastIndexOfEquatable<U>(
            U item, Index2D startIndex, Bounds2D sectorSize)
             where U : T, IEquatable<T>
        {
            try
            {
                return FindLastIndex(startIndex, sectorSize, new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use <see cref="LastIndexOf(T)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> LastIndexOfComparable<U>(U item) where U : T, IComparable<T>
        {
            try
            {
                return FindLastIndex(new EqualsComparablePredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="LastIndexOf(T, Index2D, int)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="count"/> must be greater or equal to zero;<br/>
        ///         <paramref name="startIndex"/> together with <paramref name="count"/> must not
        ///         exceed <see cref="Count"/>
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="count">Number of items to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> LastIndexOfComparable<U>(
            U item, Index2D startIndex, int count)
             where U : T, IComparable<T>
        {
            try
            {
                return FindLastIndex(startIndex, count, new EqualsComparablePredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched searched within the specified sector if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="LastIndexOf(T, Index2D, Bounds2D)"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="item"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within array bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within array bounds, beginning backwardly
        ///         from <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> LastIndexOfComparable<U>(
            U item, Index2D startIndex, Bounds2D sectorSize)
            where U : T, IComparable<T>
        {
            try
            {
                return FindLastIndex(
                    startIndex, sectorSize, new EqualsComparablePredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Creates a <typeparamref name = "T"/>[,] from this <see cref="Array2D{T}"/>
        ///     instance.
        /// </summary>
        /// <returns></returns>
        public T[,] ToSystem2DArray()
        {
            var result = new T[Rows, Columns];

            CopyToInternal(new Index2D(), result, bounds, new Index2D());
            return result;
        }

        /// <summary>
        ///     Determines whether every item matches the conditions defined by the specified
        ///     predicate. If the current instance contains no items the return value is
        ///     <see langword="true"/>.
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions to check against the items.
        /// </param>
        /// <returns>true</returns>
        public bool TrueForAll<TPredicate>(TPredicate match)
             where TPredicate : struct, IPredicate<T>
        {
            for (int i = 0, length = Count; i < length; i++)
            {
                if (!match.Invoke(items[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///     Determines whether every item matches the conditions defined by the specified
        ///     predicate. If the current instance contains no items the return value is
        ///     <see langword="true"/>.<br/>
        ///     Use <see cref="TrueForAll{TPredicate}(TPredicate)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions to check
        ///     against the items.
        /// </param>
        /// <returns></returns>
        public bool TrueForAll(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            return TrueForAll(new BoxedPredicate<T>(match));
        }
    }
}