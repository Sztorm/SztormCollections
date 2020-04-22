/*
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
    public sealed class Array2D<T> : IRefRectangularCollection<T>, ICollection
    {
        private readonly T[] items;
        private readonly Bounds2D bounds;

        /// <summary>
        ///     Returns total amount of rows in this two-dimensional array instance. This
        ///     property is equal to <see cref="Length1"/>.
        /// </summary>
        public int Rows
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.Rows;
        }

        /// <summary>
        ///     Returns total amount of columns in this two-dimensional array instance. This
        ///     property is equal to <see cref="Length2"/>.
        /// </summary>
        public int Columns
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.Columns;
        }

        /// <summary>
        ///     Returns length of the first dimension in this two-dimensional array instance. This
        ///     property is equal to <see cref="Rows"/>.
        /// </summary>
        public int Length1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.Length1;
        }

        /// <summary>
        ///     Returns length of the second dimension in this two-dimensional array instance. This
        ///     property is equal to <see cref="Columns"/>.
        /// </summary>
        public int Length2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.Length2;
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
        ///     Copies all the elements of the current two-dimensional array to the specified
        ///     two-dimensional array starting at the specified destination array index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: Array is null.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Index is less than the lower bound
        ///         of array.<br/>
        ///         <see cref="ArgumentException"/>: The number of elements in the source array is
        ///         greater than the available number of elements from index to the end of the 
        ///         destination array.<br/>
        ///         <see cref="InvalidCastException"/>: All the elements in the source array must
        ///         be able to be casted to the type of destination array.<br/>
        ///     </para>
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="index">
        /// A 32-bit integer that represents the index in array at which copying begins.
        /// </param>
        public void CopyTo(Array2D<T> destination, int index)
        {
            try
            {
                items.CopyTo(destination.items, index);
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
            catch (InvalidCastException)
            {
                throw;
            }
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
        /// <param name="index">Represents the index in array at which copying begins.</param>
        public void CopyTo(Array destination, int index)
        {
            try
            {
                items.CopyTo(destination, index);
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

        /// <summary>
        ///     Copies specified quantity of elements of the current <see cref="Array2D{T}"/>
        ///     instance from the specified source index to the particular two-dimensional array at
        ///     the exact destination index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: Destination array cannot be null.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: SourceIndex argument must consist of
        ///         numbers remaining within this Array2D instance;<br/>
        ///         DestIndex argument must consist of numbers remaining within destination
        ///         array instance;<br/> 
        ///         Quantity argument must remain within this and destination array boundaries
        ///         including specified indices.<br/>
        ///         <see cref="InvalidCastException"/>: All the elements in the source array must
        ///         be able to be casted to the type of destination array.<br/>
        ///     </para>
        /// </summary>
        /// <param name="sourceIndex">
        ///     Index from which copying elements of source array start. Indexing is zero-based.   
        /// </param>
        /// <param name="destination">The array to which elements are copied.</param>
        /// <param name="quantity">Total amount of elements copied.</param>
        /// <param name="destIndex">
        ///     Index of destination array into which the elements begin to be copied. Indexing is
        ///     zero-based.
        /// </param>
        public void CopyTo(
            Index2D sourceIndex, T[,] destination, Bounds2D quantity, Index2D destIndex)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(
                    nameof(destination), "Destination array cannot be null.");
            }
            if (!this.IsValidIndex(sourceIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sourceIndex),
                    "SourceIndex argument must consist of numbers remaining within this " +
                    "Array2D instance.");
            }
            int destRows = destination.GetLength(0);
            int destCols = destination.GetLength(1);
            var destBounds = new Bounds2D(new Box<int>(destRows), new Box<int>(destCols));

            if (!destBounds.IsValidIndex(destIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(destIndex),
                    "DestIndex argument must consist of numbers remaining within destination " +
                    "array instance.");
            }
            if (sourceIndex.Row + quantity.Rows > this.Rows ||
                sourceIndex.Column + quantity.Columns > this.Columns ||
                destIndex.Row + quantity.Rows > destRows ||
                destIndex.Column + quantity.Columns > destCols)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    "Quantity argument must remain within this and destination array " +
                    "boundaries including specified indices.");
            }
            try
            {
                CopyToInternal(sourceIndex, destination, quantity, destIndex);
            }
            catch (InvalidCastException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Copies specified quantity of elements of the current <see cref="Array2D{T}"/>
        ///     instance from the beginning to the particular two-dimensional array at
        ///     the exact destination index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: Destination array cannot be null.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Index argument must consist of
        ///         numbers remaining within destination array instance;<br/> 
        ///         Quantity argument must remain within this and destination array boundaries
        ///         including specified index.<br/>
        ///         <see cref="InvalidCastException"/>: All the elements in the source array must
        ///         be able to be casted to the type of destination array.<br/>
        ///     </para>
        /// </summary>
        /// <param name="destination">The array to which elements are copied.</param>
        /// <param name="quantity">Total amount of elements copied.</param>
        /// <param name="index">
        ///     Index of destination array into which the elements begin to be copied. Indexing is
        ///     zero-based.
        /// </param>
        public void CopyTo(T[,] destination, Bounds2D quantity, Index2D index)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(
                    nameof(destination), "Destination array cannot be null.");
            }
            int destRows = destination.GetLength(0);
            int destCols = destination.GetLength(1);
            var destBounds = new Bounds2D(new Box<int>(destRows), new Box<int>(destCols));

            if (!destBounds.IsValidIndex(index))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(index),
                    "Index argument must consist of numbers remaining within destination " +
                    "array instance.");
            }
            if (this.Rows < quantity.Rows ||
                this.Columns < quantity.Columns ||
                index.Row + quantity.Rows > destRows ||
                index.Column + quantity.Columns > destCols)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    "Quantity argument must remain within this and destination array " +
                    "boundaries including specified index.");
            }
            try
            {
                CopyToInternal(new Index2D(0, 0), destination, quantity, index);
            }
            catch (InvalidCastException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Copies specified quantity of elements of the current <see cref="Array2D{T}"/>
        ///     instance from the beginning to the particular two-dimensional array at
        ///     the start index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: Destination array cannot be null.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Quantity argument must remain within
        ///         this and destination array boundaries.<br/>
        ///         <see cref="InvalidCastException"/>: All the elements in the source array must
        ///         be able to be casted to the type of destination array.<br/>
        ///     </para>
        /// </summary>
        /// <param name="destination">The array to which elements are copied.</param>
        /// <param name="quantity">Total amount of elements copied.</param>
        public void CopyTo(T[,] destination, Bounds2D quantity)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(
                    nameof(destination), "Destination array cannot be null.");
            }
            int destRows = destination.GetLength(0);
            int destCols = destination.GetLength(1);

            if (this.Rows < quantity.Rows ||
                this.Columns < quantity.Columns ||
                destRows < quantity.Rows ||
                destCols < quantity.Columns)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    "Quantity argument must remain within this and destination array " +
                    "boundaries.");
            }
            try
            {
                CopyToInternal(new Index2D(0, 0), destination, quantity, new Index2D(0, 0));
            }
            catch (InvalidCastException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Copies all elements of the current <see cref="Array2D{T}"/> instance from the
        ///     beginning to the particular two-dimensional array at the exact destination index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: Destination array cannot be null.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Index argument must consist of
        ///         numbers remaining within destination array instance.<br/>
        ///         <see cref="ArgumentException"/>: Destination array boundaries components must
        ///         be greater or equal to ones in the source array.<br/>
        ///         <see cref="InvalidCastException"/>: All the elements in the source array must
        ///         be able to be casted to the type of destination array.<br/>
        ///     </para>
        /// </summary>
        /// <param name="destination">The array to which elements are copied.</param>
        /// <param name="index">
        ///     Index of destination array into which the elements begin to be copied. Indexing is
        ///     zero-based.
        /// </param>
        public void CopyTo(T[,] destination, Index2D index)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(
                    nameof(destination), "Destination array cannot be null.");
            }
            int destRows = destination.GetLength(0);
            int destCols = destination.GetLength(1);
            var destBounds = new Bounds2D(new Box<int>(destRows), new Box<int>(destCols));

            if (!destBounds.IsValidIndex(index))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(index),
                    "Index argument must consist of numbers remaining within destination " +
                    "array instance.");
            }
            if (this.Rows + index.Row > destRows ||
                this.Columns + index.Column > destCols)
            {
                throw new ArgumentException(
                    "Destination array boundaries components must be greater or equal to ones " +
                    "in the source array.",
                    nameof(destination));
            }
            try
            {
                CopyToInternal(new Index2D(0, 0), destination, this.bounds, index);
            }
            catch (InvalidCastException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Copies all elements of the current <see cref="Array2D{T}"/> instance from the
        ///     beginning to the particular two-dimensional array at the start index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: Destination array cannot be null.<br/>
        ///         <see cref="ArgumentException"/>: Destination array boundaries components must
        ///         be greater or equal to ones in the source array.<br/>
        ///         <see cref="InvalidCastException"/>: All the elements in the source array must
        ///         be able to be casted to the type of destination array.<br/>
        ///     </para>
        /// </summary>
        /// <param name="destination">The array to which elements are copied.</param>
        public void CopyTo(T[,] destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(
                    nameof(destination), "Destination array cannot be null.");
            }
            int rows = destination.GetLength(0);
            int columns = destination.GetLength(1);

            if (rows < this.Rows || columns < this.Columns)
            {
                throw new ArgumentException(
                    "Destination array boundaries components must be greater or equal to ones " +
                    "in the source array.",
                    nameof(destination));
            }
            try
            {
                CopyToInternal(new Index2D(0, 0), destination, this.bounds, new Index2D(0, 0));
            }
            catch (InvalidCastException)
            {
                throw;
            }
        }

        internal void CopyToInternal(
            Index2D sourceIndex, T[,] destination, Bounds2D quantity, Index2D destIndex)
        {
            int dr = destIndex.Row;
            int sr = sourceIndex.Row;
            int totalRows = destIndex.Row + quantity.Rows;
            int totalCols = destIndex.Column + quantity.Columns;

            for (; dr < totalRows; dr++, sr++)
            {
                int dc = destIndex.Column;
                int sc = sourceIndex.Column;

                for (; dc < totalCols; dc++, sc++)
                {
                    destination[dr, dc] = this[sr, sc];
                }
            }
        }

        /// <summary>
        ///     Returns an enumerator for all elements of two-dimensional array.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < items.Length; i++)
            {
                yield return items[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        ///     Determines whether specified item exists in the current instance.<br/>
        ///     Use <see cref="ContainsEquatable{U}(U)"/> or <see cref="ContainsComparable{U}(U)"/>
        ///     to avoid unnecessary boxing if stored type is <see cref="IEquatable{T}"/> or
        ///     <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item) => IndexOf(item).IsSuccess;

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
                return IndexOfEquatable(item).IsSuccess;
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
                return IndexOfComparable(item).IsSuccess;
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("item cannot be null.");
            }
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
            => FindIndex(match).IsSuccess;

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
                return FindIndex(match).IsSuccess;
            }
            catch (ArgumentNullException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="IndexOfEquatable{U}(U)"/> or <see cref="IndexOfComparable{U}(U)"/>
        ///     to avoid unnecessary boxing if stored type is <see cref="IEquatable{T}"/> or
        ///     <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> IndexOf(T item)
            => FindIndex(new EqualsObjectPredicate<T>(item));

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched row by row within the specified
        ///     range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="IndexOfEquatable{U}(U, Index2D, int)"/> or
        ///     <see cref="IndexOfComparable{U}(U, Index2D, int)"/> to avoid unnecessary boxing if
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
        public ItemRequestResult<int> IndexOf(T item, Index2D startIndex, int count)
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched searched within the specified sector
        ///     if found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> IndexOf(T item, Index2D startIndex, Bounds2D sectorSize)
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> IndexOfEquatable<U>(U item)
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched row by row within the specified
        ///     range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> IndexOfEquatable<U>(U item, Index2D startIndex, int count)
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched searched within the specified sector
        ///     if found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> IndexOfEquatable<U>(
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> IndexOfComparable<U>(U item) where U : T, IComparable<T>
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched row by row within the specified
        ///     range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> IndexOfComparable<U>(U item, Index2D startIndex, int count)
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the first occurrence of item searched searched within the specified sector
        ///     if found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> IndexOfComparable<U>(
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="Index2DOfEquatable{U}(U)"/> or
        ///     <see cref="Index2DOfComparable{U}(U)"/> to avoid unnecessary boxing if stored type
        ///     is <see cref="IEquatable{T}"/> or <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> Index2DOf(T item)
             => FindIndex2D(new EqualsObjectPredicate<T>(item));

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="Index2DOfEquatable{U}(U, Index2D, int)"/> or
        ///     <see cref="Index2DOfComparable{U}(U, Index2D, int)"/> to avoid unnecessary boxing
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
        public ItemRequestResult<Index2D> Index2DOf(T item, Index2D startIndex, int count)
        {
            try
            {
                return FindIndex2D(startIndex, count, new EqualsObjectPredicate<T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched searched within the specified sector if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="Index2DOfEquatable{U}(U, Index2D, Bounds2D)"/> or
        ///     <see cref="Index2DOfComparable{U}(U, Index2D, Bounds2D)"/> to avoid unnecessary
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
        public ItemRequestResult<Index2D> Index2DOf(
            T item, Index2D startIndex, Bounds2D sectorSize)
        {
            try
            {
                return FindIndex2D(startIndex, sectorSize, new EqualsObjectPredicate<T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     To search for <see langword="null"/> use <see cref="Index2DOf(T)"/>
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
        public ItemRequestResult<Index2D> Index2DOfEquatable<U>(U item)
            where U : T, IEquatable<T>
        {
            try
            {
                return FindIndex2D(new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     To search for <see langword="null"/> use <see cref="Index2DOf(T, Index2D, int)"/>
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
        public ItemRequestResult<Index2D> Index2DOfEquatable<U>(
            U item, Index2D startIndex, int count)
            where U : T, IEquatable<T>
        {
            try
            {
                return FindIndex2D(startIndex, count, new EqualsPredicate<U, T>(item));
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
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="Index2DOf(T, Index2D, Bounds2D)"/>
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
        public ItemRequestResult<Index2D> Index2DOfEquatable<U>(
            U item, Index2D startIndex, Bounds2D sectorSize)
             where U : T, IEquatable<T>
        {
            try
            {
                return FindIndex2D(startIndex, sectorSize, new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     To search for <see langword="null"/> use <see cref="Index2DOf(T)"/>
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
        public ItemRequestResult<Index2D> Index2DOfComparable<U>(U item)
            where U : T, IComparable<T>
        {
            try
            {
                return FindIndex2D(new EqualsComparablePredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     To search for <see langword="null"/> use <see cref="Index2DOf(T, Index2D, int)"/>
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
        public ItemRequestResult<Index2D> Index2DOfComparable<U>(
            U item, Index2D startIndex, int count)
            where U : T, IComparable<T>
        {
            try
            {
                return FindIndex2D(startIndex, count, new EqualsComparablePredicate<U, T>(item));
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
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="Index2DOf(T, Index2D, Bounds2D)"/>
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
        public ItemRequestResult<Index2D> Index2DOfComparable<U>(
            U item, Index2D startIndex, Bounds2D sectorSize)
            where U : T, IComparable<T>
        {
            try
            {
                return FindIndex2D(startIndex, sectorSize, new EqualsComparablePredicate<U, T>(item));
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
        ///     index of the last occurrence of item searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="LastIndexOfEquatable{U}(U)"/> or
        ///     <see cref="LastIndexOfComparable{U}(U)"/> to avoid unnecessary boxing if stored
        ///     type is <see cref="IEquatable{T}"/> or <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<int> LastIndexOf(T item)
            => FindLastIndex(new EqualsObjectPredicate<T>(item));

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched row by row within the specified
        ///     range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="LastIndexOfEquatable{U}(U, Index2D, int)"/> or
        ///     <see cref="LastIndexOfComparable{U}(U, Index2D, int)"/> to avoid unnecessary boxing
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
        public ItemRequestResult<int> LastIndexOf(T item, Index2D startIndex, int count)
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched searched within the specified sector
        ///     if found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> LastIndexOf(T item, Index2D startIndex, Bounds2D sectorSize)
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> LastIndexOfEquatable<U>(U item)
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched row by row within the specified range
        ///     of items if found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     To search for <see langword="null"/> use <see cref="LastIndexOf(T, Index2D, int)"/>
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
        public ItemRequestResult<int> LastIndexOfEquatable<U>(
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched searched within the specified sector
        ///     if found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> LastIndexOfEquatable<U>(
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> LastIndexOfComparable<U>(U item) where U : T, IComparable<T>
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched row by row within the specified range
        ///     of items if found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     To search for <see langword="null"/> use <see cref="LastIndexOf(T, Index2D, int)"/>
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
        public ItemRequestResult<int> LastIndexOfComparable<U>(
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying one-dimensional
        ///     index of the last occurrence of item searched searched within the specified sector
        ///     if found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> LastIndexOfComparable<U>(
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="LastIndex2DOfEquatable{U}(U)"/> or
        ///     <see cref="LastIndex2DOfComparable{U}(U)"/> to avoid unnecessary boxing if stored
        ///     type is <see cref="IEquatable{T}"/> or <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> LastIndex2DOf(T item)
            => FindLastIndex2D(new EqualsObjectPredicate<T>(item));

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="LastIndex2DOfEquatable{U}(U, Index2D, int)"/> or
        ///     <see cref="LastIndex2DOfComparable{U}(U, Index2D, int)"/> to avoid unnecessary
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
        public ItemRequestResult<Index2D> LastIndex2DOf(T item, Index2D startIndex, int count)
        {
            try
            {
                return FindLastIndex2D(startIndex, count, new EqualsObjectPredicate<T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched searched within the specified sector if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="LastIndex2DOfEquatable{U}(U, Index2D, Bounds2D)"/> or
        ///     <see cref="LastIndex2DOfComparable{U}(U, Index2D, Bounds2D)"/> to avoid unnecessary
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
        public ItemRequestResult<Index2D> LastIndex2DOf(T item, Index2D startIndex, Bounds2D sectorSize)
        {
            try
            {
                return FindLastIndex2D(startIndex, sectorSize, new EqualsObjectPredicate<T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     To search for <see langword="null"/> use <see cref="LastIndex2DOf(T)"/>
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
        public ItemRequestResult<Index2D> LastIndex2DOfEquatable<U>(U item)
            where U : T, IEquatable<T>
        {
            try
            {
                return FindLastIndex2D(new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="LastIndex2DOf(T, Index2D, int)"/>
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
        public ItemRequestResult<Index2D> LastIndex2DOfEquatable<U>(
            U item, Index2D startIndex, int count)
             where U : T, IEquatable<T>
        {
            try
            {
                return FindLastIndex2D(startIndex, count, new EqualsPredicate<U, T>(item));
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
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="LastIndex2DOf(T, Index2D, Bounds2D)"/>
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
        public ItemRequestResult<Index2D> LastIndex2DOfEquatable<U>(
            U item, Index2D startIndex, Bounds2D sectorSize)
             where U : T, IEquatable<T>
        {
            try
            {
                return FindLastIndex2D(startIndex, sectorSize, new EqualsPredicate<U, T>(item));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     To search for <see langword="null"/> use <see cref="LastIndex2DOf(T)"/>
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
        public ItemRequestResult<Index2D> LastIndex2DOfComparable<U>(U item) where U : T, IComparable<T>
        {
            try
            {
                return FindLastIndex2D(new EqualsComparablePredicate<U, T>(item));
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            }
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="LastIndex2DOf(T, Index2D, int)"/>
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
        public ItemRequestResult<Index2D> LastIndex2DOfComparable<U>(
            U item, Index2D startIndex, int count)
             where U : T, IComparable<T>
        {
            try
            {
                return FindLastIndex2D(startIndex, count, new EqualsComparablePredicate<U, T>(item));
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
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     To search for <see langword="null"/> use
        ///     <see cref="LastIndex2DOf(T, Index2D, Bounds2D)"/>
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
        public ItemRequestResult<Index2D> LastIndex2DOfComparable<U>(
            U item, Index2D startIndex, Bounds2D sectorSize)
            where U : T, IComparable<T>
        {
            try
            {
                return FindLastIndex2D(
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
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying first
        ///     occurrence searched row by row within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/>
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
            return ItemRequestResult<T>.Failed;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying first
        ///     occurrence searched row by row within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying last
        ///     occurrence searched row by row within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/>
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
            return ItemRequestResult<T>.Failed;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying last
        ///     occurrence searched row by row within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<T> FindLast(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }
            return FindLast(new BoxedPredicate<T>(match));
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

        internal ItemRequestResult<int> FindIndexInternal<TPredicate>(
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
            return ItemRequestResult<int>.Failed;
        }

        /// <summary>
        ///     Searches from the beginning for an item that matches the conditions defined by the
        ///     specified predicate, and returns the <see cref="ItemRequestResult{T}"/> with
        ///     underlying one-dimensional index of the first occurrence searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/>
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
        public ItemRequestResult<int> FindIndex<TPredicate>(TPredicate match)
            where TPredicate : struct, IPredicate<T>
            => FindIndexInternal(0, Count, match);

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the first occurrence searched row by row within the
        ///     specified range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/>
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
        public ItemRequestResult<int> FindIndex<TPredicate>(
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
            return FindIndexInternal(startIndex1D, indexAfterEnd, match);
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the first occurrence searched within the specified sector.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/>
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
        public ItemRequestResult<int> FindIndex<TPredicate>(
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
            for (int i = startIndex.Row; i < indexAfterEnd.Row; i++)
            {
                for (int j = startIndex.Column; j < indexAfterEnd.Column; j++)
                {
                    if (match.Invoke(GetItemInternal(i, j)))
                    {
                        return new ItemRequestResult<int>(
                            RowMajorIndex2DToInt(new Index2D(i, j), Columns));
                    }
                }
            }
            return ItemRequestResult<int>.Failed;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the first occurrence searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="FindIndex{TPredicate}(TPredicate)"/> to avoid virtual call.
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
        public ItemRequestResult<int> FindIndex(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }
            return FindIndexInternal(0, Count, new BoxedPredicate<T>(match));
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the first occurrence searched row by row within the
        ///     specified range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="FindIndex{TPredicate}(Index2D, int, TPredicate)"/> to avoid virtual
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
        public ItemRequestResult<int> FindIndex(Index2D startIndex, int count, Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            try
            {
                return FindIndex(startIndex, count, new BoxedPredicate<T>(match));
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
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> FindIndex(
            Index2D startIndex, Bounds2D sectorSize, Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            try
            {
                return FindIndex(startIndex, sectorSize, new BoxedPredicate<T>(match));
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
                return ItemRequestResult<Index2D>.Failed;
            }
            return new ItemRequestResult<Index2D>(
                IntToRowMajorIndex2D(possibleIndex.ItemOrDefault, Columns));
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence searched within the entire <see cref="Array2D{T}"/> if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/>
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
        public ItemRequestResult<Index2D> FindIndex2D<TPredicate>(TPredicate match)
            where TPredicate : struct, IPredicate<T>
            => RequestedIntToRequested2DIndex(FindIndex(match));

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/>
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
        public ItemRequestResult<Index2D> FindIndex2D<TPredicate>(
            Index2D startIndex, int count, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindIndex(startIndex, count, match));
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
        ///     <see cref="ItemRequestResult{T}.Failed"/>
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
        public ItemRequestResult<Index2D> FindIndex2D<TPredicate>(
            Index2D startIndex, Bounds2D sectorSize, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindIndex(startIndex, sectorSize, match));
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
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="FindIndex2D{TPredicate}(TPredicate)"/> to avoid virtual call.
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
        public ItemRequestResult<Index2D> FindIndex2D(Predicate<T> match)
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindIndex(match));
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
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="FindIndex2D{TPredicate}(Index2D, int, TPredicate)"/> to avoid
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
        public ItemRequestResult<Index2D> FindIndex2D(
            Index2D startIndex, int count, Predicate<T> match)
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindIndex(startIndex, count, match));
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
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="FindIndex2D{TPredicate}(Index2D, Bounds2D, TPredicate)"/> to avoid
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
        public ItemRequestResult<Index2D> FindIndex2D(
            Index2D startIndex, Bounds2D sectorSize, Predicate<T> match)
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindIndex(startIndex, sectorSize, match));
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

        internal ItemRequestResult<int> FindLastIndexInternal<TPredicate>(
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
            return ItemRequestResult<int>.Failed;
        }

        /// <summary>
        ///     Searches from the beginning for an item that matches the conditions defined by the
        ///     specified predicate, and returns the <see cref="ItemRequestResult{T}"/> with
        ///     underlying one-dimensional index of the last occurrence searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/>
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
        public ItemRequestResult<int> FindLastIndex<TPredicate>(TPredicate match)
            where TPredicate : struct, IPredicate<T>
            => FindLastIndexInternal(Count - 1, -1, match);

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the last occurrence searched row by row within the
        ///     specified range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/>
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
        public ItemRequestResult<int> FindLastIndex<TPredicate>(
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
            return FindLastIndexInternal(startIndex1D, indexAfterEnd, match);
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the last occurrence searched within the specified sector.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/>
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
        public ItemRequestResult<int> FindLastIndex<TPredicate>(
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
            for (int i = startIndex.Row; i > indexAfterEnd.Row; i--)
            {
                for (int j = startIndex.Column; j > indexAfterEnd.Column; j--)
                {
                    if (match.Invoke(GetItemInternal(i, j)))
                    {
                        return new ItemRequestResult<int>(
                            RowMajorIndex2DToInt(new Index2D(i, j), Columns));
                    }
                }
            }
            return ItemRequestResult<int>.Failed;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the last occurrence searched within the entire
        ///     <see cref="Array2D{T}"/> if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> FindLastIndex(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }
            return FindLastIndexInternal(Count - 1, -1, new BoxedPredicate<T>(match));
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying
        ///     one-dimensional index of the last occurrence searched row by row within the
        ///     specified range of items if found. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> FindLastIndex(
            Index2D startIndex, int count, Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            try
            {
                return FindLastIndex(startIndex, count, new BoxedPredicate<T>(match));
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
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<int> FindLastIndex(
            Index2D startIndex, Bounds2D sectorSize, Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            try
            {
                return FindLastIndex(startIndex, sectorSize, new BoxedPredicate<T>(match));
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
        ///     <see cref="ItemRequestResult{T}.Failed"/>
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
        public ItemRequestResult<Index2D> FindLastIndex2D<TPredicate>(TPredicate match)
            where TPredicate : struct, IPredicate<T>
            => RequestedIntToRequested2DIndex(FindLastIndex(match));

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the last occurrence searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/>
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
        public ItemRequestResult<Index2D> FindLastIndex2D<TPredicate>(
            Index2D startIndex, int count, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindLastIndex(startIndex, count, match));
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
        ///     <see cref="ItemRequestResult{T}.Failed"/>
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
        public ItemRequestResult<Index2D> FindLastIndex2D<TPredicate>(
            Index2D startIndex, Bounds2D sectorSize, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            try
            {
                return RequestedIntToRequested2DIndex(
                    FindLastIndex(startIndex, sectorSize, match));
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
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="FindLastIndex2D{TPredicate}(TPredicate)"/> to avoid virtual call.
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
        public ItemRequestResult<Index2D> FindLastIndex2D(Predicate<T> match)
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindLastIndex(match));
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
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="FindLastIndex2D{TPredicate}(Index2D, int, TPredicate)"/> to avoid
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
        public ItemRequestResult<Index2D> FindLastIndex2D(
            Index2D startIndex, int count, Predicate<T> match)
        {
            try
            {
                return RequestedIntToRequested2DIndex(FindLastIndex(startIndex, count, match));
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
        ///     <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="FindLastIndex2D{TPredicate}(Index2D, Bounds2D, TPredicate)"/> to avoid
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
        public ItemRequestResult<Index2D> FindLastIndex2D(
            Index2D startIndex, Bounds2D sectorSize, Predicate<T> match)
        {
            try
            {
                return RequestedIntToRequested2DIndex(
                    FindLastIndex(startIndex, sectorSize, match));
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
            var result = new Array2D<TOutput>(this.bounds);

            for (int i = 0, length = Count; i < length; i++)
            {
                result.items[i] = converter(items[i]);
            }
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

        /// <summary>
        ///     Creates a <typeparamref name = "T"/>[,] from this <see cref="Array2D{T}"/>
        ///     instance.
        /// </summary>
        /// <returns></returns>
        public T[,] ToSystem2DArray()
        {
            var result = new T[Rows, Columns];
            var zero = new Index2D(0, 0);

            CopyToInternal(zero, result, bounds, zero);
            return result;
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
                for (int j = 0; j < bounds.Columns; j++)
                {
                    result.items[index1D] = array[i, j];
                    index1D++;
                }
            }
            return result;
        }
    }
}