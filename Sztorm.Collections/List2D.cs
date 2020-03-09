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
    /// Represents two-dimensional rectangular list of specific type allocated within single
    /// contiguous block of memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed partial class List2D<T>: IEnumerable<T>, ICollection
    {
        private static readonly Bounds2D DefaultInitialCapacity = new Bounds2D(16, 16);

        private T[] items;
        private Bounds2D bounds;
        private Bounds2D capacity;

        /// <summary>
        ///     Returns total amount of rows in this <see cref="List2D{T}"/> instance. This
        ///     property is equal to <see cref="Length1"/>
        /// </summary>
        public int Rows
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.Rows;
        }

        /// <summary>
        ///     Returns total amount of columns in this <see cref="List2D{T}"/> instance. This
        ///     property is equal to <see cref="Length2"/>
        /// </summary>
        public int Columns
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.Columns;
        }

        /// <summary>
        ///     Returns length of the first dimension of current <see cref="List2D{T}"/> instance.
        ///     This property is equal to <see cref="Rows"/>.
        /// </summary>
        public int Length1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.Length1;
        }

        /// <summary>
        ///     Returns length of the second dimension of current <see cref="List2D{T}"/> instance.
        ///     This property is equal to <see cref="Columns"/>
        /// </summary>
        public int Length2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.Length2;
        }

        /// <summary>
        ///     Returns boundaries of current <see cref="List2D{T}"/> instance.
        /// </summary>
        public Bounds2D Boundaries
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds;
        }

        /// <summary>
        ///     Returns the number of elements that current <see cref="List2D{T}"/> instance can
        ///     contain before resizing is required.
        /// </summary>
        public Bounds2D Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => capacity;
        }

        /// <summary>
        ///     Returns total count of elements in all the dimensions.
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.Rows * bounds.Columns;
        }

        /// <summary>
        ///     This collection is not synchronized. To synchronize access use lock statement with
        ///     <see cref="SyncRoot"/> property.
        /// </summary>
        public bool IsSynchronized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => false;
        }

        /// <summary>
        ///     Determines whether current <see cref="List2D{T}"/> instance has any items in it.
        /// </summary>
        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.Rows == 0 || bounds.Columns == 0;
        }

        /// <summary>
        ///     Gets an object that can be used to synchronize access to the
        ///     <see cref="List2D{T}"/>.
        /// </summary>
        public object SyncRoot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => items.SyncRoot;
        }

        /// <summary>
        ///     Returns an element stored at specified row and column.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="IndexOutOfRangeException"/>: At least one of indices is out of list
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
                        "At least one of indices is out of list bounds.");
                }
                return ref GetItemInternal(row, column);
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
                return ref GetItemInternal(index);
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
            => ref items[row * capacity.Columns + column];

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
            => ref items[index.Dimension1Index * capacity.Columns + index.Dimension2Index];

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
        public Row GetRow(int index)
        {
            try
            {
                return new Row(this, index);
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
        public Column GetColumn(int index)
        {
            try
            {
                return new Column(this, index);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns true if specified index exists in this <see cref="List2D{T}"/> instance,
        ///     false otherwise.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidIndex(int row, int column)
            => bounds.IsValidIndex(row, column);

        /// <summary>
        ///     Returns true if specified index exists in this <see cref="List2D{T}"/> instance,
        ///     false otherwise.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidIndex(Index2D index)
            => bounds.IsValidIndex(index);

        /// <summary>
        ///     Constructs a new instance of <see cref="List2D{T}"/> class that is empty and has
        ///     the default initial capacity.
        /// </summary>
        public List2D()
        {
            bounds = new Bounds2D();
            capacity = DefaultInitialCapacity;
            items = new T[capacity.Length1 * capacity.Length2];
        }

        /// <summary>
        ///     Constructs a new instance of <see cref="List2D{T}"/> class that is empty and has
        ///     specified initial capacity of rows and columns.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: All the arguments must be
        ///         greater or equal to zero.
        ///     </para>
        /// </summary>
        /// <param name="initRowsCap">
        ///     Specifies initial capacity of rows. This argument must be greater or equal to zero.
        /// </param>
        /// <param name="initColsCap">
        ///     Specifies initial capacity of columns. This argument must be greater or equal to
        ///     zero.
        /// </param>
        public List2D(int initRowsCap, int initColsCap)
        {
            try
            {
                capacity = new Bounds2D(initRowsCap, initColsCap);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
            bounds = new Bounds2D();
            items = new T[initRowsCap * initColsCap];        
        }

        /// <summary>
        ///     Constructs a new instance of <see cref="List2D{T}"/> class that is empty and has
        ///     specified initial capacity.
        /// </summary>
        /// <param name="initialCapacity">Specifies two-dimensional initial capacity.</param>
        public List2D(Bounds2D initialCapacity)
        {
            items = new T[initialCapacity.Length1 * initialCapacity.Length2];
            bounds = new Bounds2D();
            capacity = initialCapacity;
        }

        /// <summary>
        ///     Constructs a new <see cref="List2D{T}"/> object that contains elements copied from
        ///     specified <see cref="Array2D{T}"/> instance and has capacity equal to number of
        ///     elements stored in given array.
        /// </summary>
        /// <param name="array">Array from which elements are copied.</param>
        public List2D(Array2D<T> array)
        {
            items = new T[array.Count];
            bounds = array.Boundaries;
            capacity = array.Boundaries;

            array.CopyTo(items, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsReallocationNeeded(int newRows, int newCols)
            => newRows > capacity.Rows || newCols > capacity.Columns;
        
        /// <summary>
        ///     Returns a capacity that can accommodate at least number of rows and columns passed
        ///     in arguments. Every boundary that is not sufficient in current instance capacity
        ///     gets doubled. This method does not mutate current instance.
        /// </summary>
        /// <param name="newRows"></param>
        /// <param name="newCols"></param>
        /// <returns></returns>
        private Bounds2D EnsuredCapacity(int newRows, int newCols)
        {
            int oldRows = capacity.Rows;
            int oldCols = capacity.Columns;
            int newCapRows = oldRows;
            int newCapCols = oldCols;

            if (newRows > oldRows)
            {
                newCapRows = unchecked (newRows * 2);

                if (newCapRows < newRows)
                {
                    newCapRows = int.MaxValue;
                }
            }
            if (newCols > oldCols)
            {
                newCapCols = unchecked (newCols * 2);

                if (newCapCols < newCols)
                {
                    newCapCols = int.MaxValue;
                }
            }
            return Bounds2D.NotCheckedConstructor(newCapRows, newCapCols);
        }

        /// <summary>
        /// Maps two-dimensional index into one-dimensional with row-major order.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Index1DMappedToIndex2D(int row, int column, int columns)
            => row * columns + column;

        /// <summary>
        ///     Reallocates internal buffer for items. Contents of newCapacity must be greater or 
        ///     equal to current bounds as all current items are copied to a new array.
        /// </summary>
        /// <param name="newCapacity"></param>
        private void Reallocate(Bounds2D newCapacity)
        {
            T[] newItems;

            try
            {
                newItems = new T[Math.BigMul(newCapacity.Rows, newCapacity.Columns)];
            }
            catch(OutOfMemoryException)
            {
                throw;
            }
            int rows = Rows;
            int cols = Columns;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    newItems[Index1DMappedToIndex2D(i, j, newCapacity.Columns)] =
                        GetItemInternal(i, j);
                }
            }
            capacity = newCapacity;
            items = newItems;
        }

        /// <summary>
        ///     Adds one row to the end of the <see cref="List2D{T}"/>. This method does the same
        ///     as <see cref="AddLength1(int)"/> with argument of 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRow() => AddRows(1);

        /// <summary>
        ///     Adds one column to the end of the <see cref="List2D{T}"/>. This method does the
        ///     same as <see cref="AddLength2(int)"/> with argument of 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddColumn() => AddColumns(1);

        /// <summary>
        ///     Adds specified amount of rows to the end of the <see cref="List2D{T}"/>. This
        ///     method does the same as <see cref="AddLength1(int)"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Count argument must greater or equal
        ///         to zero.
        ///     </para>
        /// </summary>
        public void AddRows(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count), "Count argument must greater or equal to zero.");
            }
            int newRows = Rows + count;
            int newCols = Columns;

            if (IsReallocationNeeded(newRows, newCols))
            {
                try
                {
                    Reallocate(newCapacity: EnsuredCapacity(newRows, newCols));
                }
                catch(OutOfMemoryException)
                {
                    throw;
                }
            }
            bounds = Bounds2D.NotCheckedConstructor(newRows, newCols);
        }

        /// <summary>
        ///     Adds specified amount of rows to the end of the <see cref="List2D{T}"/>. This
        ///     method does the same as <see cref="AddRows(int)"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Value argument must greater or equal
        ///         to zero.
        ///     </para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddLength1(int value) => AddRows(value);

        /// <summary>
        ///     Adds specified amount of columns to the end of the <see cref="List2D{T}"/>. This
        ///     method does the same as <see cref="AddLength2(int)"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Count argument must greater or equal
        ///         to zero.
        ///     </para>
        /// </summary>
        public void AddColumns(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count), "Count argument must greater or equal to zero.");
            }
            int newRows = Rows;
            int newCols = Columns + count;

            if (IsReallocationNeeded(newRows, newCols))
            {
                try
                {
                    Reallocate(newCapacity: EnsuredCapacity(newRows, newCols));
                }
                catch (OutOfMemoryException)
                {
                    throw;
                }
            }
            bounds = Bounds2D.NotCheckedConstructor(newRows, newCols);
        }

        /// <summary>
        ///     Adds specified amount of columns to the end of the <see cref="List2D{T}"/>. This
        ///     method does the same as <see cref="AddColumns(int)"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Value argument must greater or equal
        ///         to zero.
        ///     </para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddLength2(int value) => AddColumns(value);

        /// <summary>
        /// Sets items of specified amount of rows at given starting index to default values.
        /// Arguments are not checked. For internal purposes only.
        /// </summary>
        /// <param name="startIndex">Range: [0, <see cref="Rows"/>).</param>
        /// <param name="count">Range: [0, <see cref="Rows"/> - startIndex).</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ClearRows(int startIndex, int count)
        {
            int cols = bounds.Columns;

            Array.Clear(items, startIndex * cols, count * cols);
        }

        /// <summary>
        /// Sets items of specified amount of columns at given starting index to default values.
        /// Arguments are not checked. For internal purposes only.
        /// </summary>
        /// <param name="startIndex">Range: [0, <see cref="Rows"/>).</param>
        /// <param name="count">Range: [0, <see cref="Rows"/> - startIndex).</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ClearColumns(int startIndex, int count)
        {
            int rows = bounds.Rows;
            int cols = bounds.Columns;

            for (int i = 0; i < rows; i++)
            {
                Array.Clear(items, Index1DMappedToIndex2D(i, startIndex, cols), count); 
            }
        }

        /// <summary>
        ///     Removes specified amount of rows from this instance starting at the given index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: 
        ///             StartIndex must be a number in the range of (<see cref="Rows"/> - 1);<br/>
        ///             Count must be a number in the range of (<see cref="Rows"/> - startIndex).
        ///     </para>
        /// </summary>
        /// <param name="startIndex">
        ///     Index from which removing starts. Indexing is zero-based.
        /// </param>
        /// <param name="count">Number of rows to remove.</param>
        public void RemoveRows(int startIndex, int count)
        {
            bool isInvalidRowIndex = unchecked ((uint)startIndex >= (uint)Rows);
            bool isInvalidCount = unchecked ((uint)count > (uint)(Rows - startIndex));

            if (isInvalidRowIndex)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startIndex), "StartIndex must be a number in the range of (Rows - 1).");
            }
            if (isInvalidCount)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count), "Count must be a number in the range of (Rows - startIndex).");
            }
            int rows = bounds.Rows;
            int cols = bounds.Columns;
            int newRows = rows - count;

            for (int i0 = startIndex, i1 = startIndex + count; i0 < newRows; i0++, i1++)
            {
                for (int j = 0; j < cols; j++)
                {
                    GetItemInternal(i0, j) = GetItemInternal(i1, j);
                }
            }
            ClearRows(newRows, count);

            bounds = Bounds2D.NotCheckedConstructor(newRows, cols);
        }

        /// <summary>
        ///     Removes specified row from this instance.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>:
        ///         Index must be a number in the range of (<see cref="Rows"/> - 1).
        ///     </para>
        /// </summary>
        /// <param name="index">
        ///     Index indicating which row to remove. Indexing is zero-based.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveRow(int index)
        {
            try
            {
                RemoveRows(index, 1);
            }
            catch(ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException(
                    "Index must be a number in the range of (Rows - 1)");
            }
        }

        /// <summary>
        ///     Removes specified amount of columns from this instance starting at the given index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: 
        ///             StartIndex must be a number in the range of (<see cref="Columns"/> - 1);<br/>
        ///             Count must be a number in the range of (<see cref="Columns"/> - startIndex).
        ///     </para>
        /// </summary>
        /// <param name="startIndex">
        ///     Index from which removing starts. Indexing is zero-based.
        /// </param>
        /// <param name="count">Number of columns to remove.</param>
        public void RemoveColumns(int startIndex, int count)
        {
            bool isInvalidColumnIndex = unchecked((uint)startIndex >= (uint)Columns);
            bool isInvalidCount = unchecked((uint)count > (uint)(Columns - startIndex));

            if (isInvalidColumnIndex)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startIndex),
                    "StartIndex must be a number in the range of (Columns - 1).");
            }
            if (isInvalidCount)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count),
                    "Count must be a number in the range of (Columns - startIndex).");
            }
            int rows = bounds.Rows;
            int cols = bounds.Columns;
            int newCols = cols - count;

            for (int i = 0; i < rows; i++)
            {
                for (int j0 = startIndex, j1 = startIndex + count; j0 < newCols; j0++, j1++)
                {
                    GetItemInternal(i, j0) = GetItemInternal(i, j1);
                }
            }
            ClearColumns(newCols, count);

            bounds = Bounds2D.NotCheckedConstructor(rows, newCols);
        }

        /// <summary>
        ///     Removes specified column from this instance.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>:
        ///         Index must be a number in the range of (<see cref="Columns"/> - 1).
        ///     </para>
        /// </summary>
        /// <param name="index">
        ///     Index indicating which column to remove. Indexing is zero-based.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveColumn(int index)
        {
            try
            {
                RemoveColumns(index, 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException(
                   "Index must be a number in the range of (Columns - 1)");
            }
        }

        /// <summary>
        ///     Removes all elements from the <see cref="List2D{T}"/>. Any stored references are
        ///     released.
        /// </summary>
        public void Clear()
        {
            if (Count > 0)
            {
                Array.Clear(items, 0, items.Length);
                bounds = new Bounds2D();
            }
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        internal void CopyToInternal(
            Index2D sourceIndex, Array2D<T> destination, Bounds2D quantity, Index2D destIndex)
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
        ///     Returns an enumerator for all elements of the <see cref="List2D{T}"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            int rows = Rows;
            int columns = Columns;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    yield return GetItemInternal(i, j);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///     Creates an <see cref="Array2D{T}"/> from this <see cref="List2D{T}"/> instance.
        /// </summary>
        /// <returns></returns>
        public Array2D<T> ToArray2D()
        {
            var result = new Array2D<T>(bounds);

            CopyToInternal(new Index2D(), result, bounds, new Index2D());
            return result;
        }
    }
}
