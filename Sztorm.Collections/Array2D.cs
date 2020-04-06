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
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents two-dimensional row-major ordered rectangular array of single type allocated
    ///     within single contiguous block of memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public sealed partial class Array2D<T> : IRefRectangularCollection<T>, ICollection
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
        ///     An one dimensional indexer that can be used to iterate through all the stored
        ///     elements. Indexing start at zero.
        ///     <para>
        ///         Throws <see cref="IndexOutOfRangeException"/> if index exceeds
        ///         <see cref="Count"/> property.
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
                    throw;
                }
            }
        }

        /// <summary>
        ///     Returns an element stored at specified row and column.
        ///     <para>
        ///         Throws <see cref="IndexOutOfRangeException"/> if any of indices is out of array
        ///         bounds.
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
                    throw new IndexOutOfRangeException(
                        "At least one of indices is out of array bounds.");
                }
                return ref items[row * Columns + column];
            }
        }

        /// <summary>
        ///     Returns an element stored at specified index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="IndexOutOfRangeException"/>: At least one of indices is out of array
        ///         bounds.
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
                    throw new IndexOutOfRangeException(
                        "At least one of indices is out of array bounds.");
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
        ///     Returns true if specified index exists in this <see cref="Array2D{T}"/> instance,
        ///     false otherwise.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidIndex(int row, int column)
            => bounds.IsValidIndex(row, column);

        /// <summary>
        ///     Returns true if specified index exists in this <see cref="Array2D{T}"/> instance,
        ///     false otherwise.
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
            var destBounds = Bounds2D.NotCheckedConstructor(destRows, destCols);

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
            var destBounds = Bounds2D.NotCheckedConstructor(destRows, destCols);

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
            var destBounds = Bounds2D.NotCheckedConstructor(destRows, destCols);

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
        public bool Contains(T item) => IndexOf(item).HasValue;

        /// <summary>
        ///     Determines whether specified item exists in the current instance.
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsEquatable<U>(U item) where U : T, IEquatable<T>
            => IndexOfEquatable(item).HasValue;

        /// <summary>
        ///     Determines whether specified item exists in the current instance.
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsComparable<U>(U item) where U : T, IComparable<T>
            => IndexOfComparable(item).HasValue;

        /// <summary>
        ///     Searches for the specified item and returns the one-dimensional index of its first
        ///     occurrence in this intance if found; returns <see langword="null"/> otherwise.<br/>
        ///     Use <see cref="IndexOfEquatable{U}(U)"/> or <see cref="IndexOfComparable{U}(U)"/>
        ///     to avoid unnecessary boxing if stored type is <see cref="IEquatable{T}"/> or
        ///     <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        public int? IndexOf(T item)
        {
            for (int i = 0, length = Count; i < length; i++)
            {
                if (item.Equals(items[i]))
                {
                    return i;
                }
            }
            return null;
        }

        /// <summary>
        ///     Searches for the specified item and returns the one-dimensional index of its first
        ///     occurrence in this intance if found; returns <see langword="null"/> otherwise.
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        public int? IndexOfEquatable<U>(U item) where U : T, IEquatable<T>
        {
            for (int i = 0, length = Count; i < length; i++)
            {
                if (item.Equals(items[i]))
                {
                    return i;
                }
            }
            return null;
        }

        /// <summary>
        ///     Searches for the specified item and returns the one-dimensional index of its first
        ///     occurrence in this intance if found; returns <see langword="null"/> otherwise.
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        public int? IndexOfComparable<U>(U item) where U : T, IComparable<T>
        {
            for (int i = 0, length = Count; i < length; i++)
            {
                if (item.CompareTo(items[i]) == 0)
                {
                    return i;
                }
            }
            return null;
        }

        private Index2D? Index2DOfHelper(int? possibleIndex)
        {
            if (!possibleIndex.HasValue)
            {
                return null;
            }
            int oneDimIndex = possibleIndex.Value;
            int row = oneDimIndex / Columns;
            int column = oneDimIndex % Columns;

            return new Index2D(row, column);
        }

        /// <summary>
        ///     Searches for the specified item and returns the 2D index of its first occurrence
        ///     in this instance if found; returns <see langword="null"/> otherwise.<br/>
        ///     Use <see cref="Index2DOfEquatable{U}(U)"/> or
        ///     <see cref="Index2DOfComparable{U}(U)"/> to avoid unnecessary boxing if stored type
        ///     is <see cref="IEquatable{T}"/> or <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Index2D? Index2DOf(T item) => Index2DOfHelper(IndexOf(item));

        /// <summary>
        ///     Searches for the specified item and returns the 2D index of its first occurrence
        ///     in this instance if found; returns <see langword="null"/> otherwise.
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Index2D? Index2DOfEquatable<U>(U item) where U : IEquatable<T>, T
            => Index2DOfHelper(IndexOfEquatable(item));

        /// <summary>
        ///     Searches for the specified item and returns the 2D index of its first occurrence
        ///     in this instance if found; returns <see langword="null"/> otherwise.
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IComparable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Index2D? Index2DOfComparable<U>(U item) where U : IComparable<T>, T
            => Index2DOfHelper(IndexOfComparable(item));

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying first 
        ///     occurrence of item searched within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        ///     Use <see cref="Find{TPredicate}(TPredicate)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:
        ///         <see cref="ArgumentNullException"/> Match cannot be null.
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
            for (int i = 0, length = Count; i < length; i++)
            {
                ref readonly T item = ref items[i];

                if (match(item))
                {
                    return new ItemRequestResult<T>(item);
                }
            }
            return ItemRequestResult<T>.Failed;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying first 
        ///     occurrence of item searched within the entire <see cref="Array2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
        /// </summary>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="match">
        ///     An <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
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
        /// Creates a <see cref="Array2D{T}"/> from <typeparamref name = "T"/>[,] intance.
        /// <para>
        ///     Exceptions:<br/>
        ///     <see cref="ArgumentNullException"/>: Array argument cannot be null.
        /// </para>
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static Array2D<T> FromSystem2DArray(T[,] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Array argument cannot be null.");
            }
            Bounds2D bounds =
                Bounds2D.NotCheckedConstructor(array.GetLength(0), array.GetLength(1));
            var result = new Array2D<T>(bounds);

            for (int i = 0; i < bounds.Rows; i++)
            {
                for (int j = 0; j < bounds.Columns; j++)
                {
                    result[i, j] = array[i, j];
                }
            }
            return result;
        }
    }
}