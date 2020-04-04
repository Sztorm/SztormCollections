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
using System.Diagnostics;

namespace Sztorm.Collections
{
    /// <summary>
    /// Represents two-dimensional row-major ordered rectangular list of specific type allocated
    /// within single contiguous block of memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed partial class List2D<T> : IRefRectangularCollection<T>, ICollection
    {
        private static readonly Bounds2D DefaultInitialCapacity = new Bounds2D(16, 16);

        private T[] items;
        private Bounds2D bounds;
        private Bounds2D capacity;

        /// <summary>
        ///     Returns total count of rows in this <see cref="List2D{T}"/> instance. This
        ///     property is equal to <see cref="Length1"/>
        /// </summary>
        public int Rows
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.Rows;
        }

        /// <summary>
        ///     Returns total count of columns in this <see cref="List2D{T}"/> instance. This
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
        ///     This collection is not synchronized. To synchronize access use
        ///     <see langword="lock"/> statement with <see cref="SyncRoot"/> property.
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
        ///     Arguments are not checked on release build.
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
        {
            Debug.Assert(row >= 0);
            Debug.Assert(row < capacity.Rows);
            Debug.Assert(column >= 0);
            Debug.Assert(column < capacity.Columns);

            return ref items[row * capacity.Columns + column];
        }

        /// <summary>
        ///     Returns an element stored at specified index.<br/>
        ///     Argument is not checked on release build.
        /// </summary>
        /// <param name="index">
        ///     Range: ([0, <see cref="Rows"/>), [0, <see cref="Columns"/>)).
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T GetItemInternal(Index2D index)
        {
            Debug.Assert(index.Row >= 0);
            Debug.Assert(index.Row < Rows);
            Debug.Assert(index.Column >= 0);
            Debug.Assert(index.Column < Columns);

            return ref items[index.Dimension1Index * capacity.Columns + index.Dimension2Index];
        }

        /// <summary>
        ///     Returns a row at specified index. Indexing starts at zero.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Index is out of boundaries of the row
        ///         count.
        ///     </para>
        /// </summary>
        /// <param name="index">A zero-based index that determines which row is to take.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RefRow<T, List2D<T>> GetRow(int index)
        {
            try
            {
                return new RefRow<T, List2D<T>>(this, index);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns a column at specified index. Indexing starts at zero.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Index is out of boundaries of the
        ///         column count.
        ///     </para>
        /// </summary>
        /// <param name="index">A zero-based index that determines which column is to take.</param> 
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RefColumn<T, List2D<T>> GetColumn(int index)
        {
            try
            {
                return new RefColumn<T, List2D<T>>(this, index);
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

        /// <summary>
        /// Increases current instance capacity with specified quantity.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="OutOfMemoryException"/>: Insufficient memory to continue the
        ///         execution of the program.
        ///     </para>
        /// </summary>
        /// <param name="quantity"></param>
        public void IncreaseCapacity(Bounds2D quantity)
        {
            int newRows;
            int newCols;

            try
            {
                newRows = checked(quantity.Rows + capacity.Rows);
            }
            catch (OverflowException)
            {
                newRows = int.MaxValue;
            }
            try
            {
                newCols = checked(quantity.Columns + capacity.Columns);
            }
            catch (OverflowException)
            {
                newCols = int.MaxValue;
            }
            try
            {
                Reallocate(Bounds2D.NotCheckedConstructor(newRows, newCols));
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
        }

        /// <summary>
        /// Determines whether current instance capacity can accommodate specified count of rows
        /// and columns.
        /// </summary>
        /// <param name="newRows"></param>
        /// <param name="newCols"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsReallocationNeeded(int newRows, int newCols)
            => newRows > capacity.Rows || newCols > capacity.Columns;

        /// <summary>
        /// Determines whether current instance capacity can accommodate specified boundaries.
        /// </summary>
        /// <param name="newBounds"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsReallocationNeeded(Bounds2D newBounds)
            => IsReallocationNeeded(newBounds.Rows, newBounds.Columns);

        /// <summary>
        ///     Returns a capacity that can accommodate at least number of rows and columns passed
        ///     in arguments. Every boundary that is not sufficient in current instance capacity
        ///     gets doubled. Arguments are not checked on release build. This method does not
        ///     mutate current instance.
        /// </summary>
        /// <param name="newRows">Must be &gt;= 0</param>
        /// <param name="newCols">Must be &gt;= 0</param>
        /// <returns></returns>
        private Bounds2D EnsuredCapacity(int newRows, int newCols)
        {
            Debug.Assert(newRows >= 0);
            Debug.Assert(newCols >= 0);

            int oldRows = capacity.Rows;
            int oldCols = capacity.Columns;
            int newCapRows = oldRows;
            int newCapCols = oldCols;

            if (newRows > oldRows)
            {
                newCapRows = unchecked(newRows * 2);

                if (newCapRows < newRows)
                {
                    newCapRows = int.MaxValue;
                }
            }
            if (newCols > oldCols)
            {
                newCapCols = unchecked(newCols * 2);

                if (newCapCols < newCols)
                {
                    newCapCols = int.MaxValue;
                }
            }
            return Bounds2D.NotCheckedConstructor(newCapRows, newCapCols);
        }

        /// <summary>
        ///     Returns a capacity that can accommodate bounds passed arguments. Every boundary
        ///     that is not sufficient in current instance capacity gets doubled. This method does
        ///     not mutate current instance.
        /// </summary>
        /// <param name="newBounds"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Bounds2D EnsuredCapacity(Bounds2D newBounds)
            => EnsuredCapacity(newBounds.Rows, newBounds.Columns);

        /// <summary>
        /// Maps two-dimensional index into one-dimensional integer with row-major order.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Index2DMappedToInt(int row, int column, int columns)
            => row * columns + column;

        /// <summary>
        ///     Reallocates internal buffer for items. Contents of newCapacity must be greater or 
        ///     equal to current bounds as all current items are copied to a new array. Arguments
        ///     are not checked on release build.
        /// </summary>
        /// <param name="newCapacity">
        ///     Rows &gt;= <see cref="Rows"/>, Columns &gt;= <see cref="Columns"/>
        /// </param>
        private void Reallocate(Bounds2D newCapacity)
        {
            Debug.Assert(newCapacity.Rows >= Rows);
            Debug.Assert(newCapacity.Columns >= Columns);

            T[] newItems;

            try
            {
                newItems = new T[Math.BigMul(newCapacity.Rows, newCapacity.Columns)];
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            int rows = Rows;
            int cols = Columns;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    newItems[Index2DMappedToInt(i, j, newCapacity.Columns)] =
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
        ///     Adds specified count of rows to the end of the <see cref="List2D{T}"/>. This
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
                catch (OutOfMemoryException)
                {
                    throw;
                }
            }
            bounds = Bounds2D.NotCheckedConstructor(newRows, newCols);
        }

        /// <summary>
        ///     Adds specified count of rows to the end of the <see cref="List2D{T}"/>. This
        ///     method does the same as <see cref="AddRows(int)"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Value argument must greater or equal
        ///         to zero.
        ///     </para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddLength1(int count) => AddRows(count);

        /// <summary>
        ///     Adds specified count of columns to the end of the <see cref="List2D{T}"/>. This
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
        ///     Adds specified count of columns to the end of the <see cref="List2D{T}"/>. This
        ///     method does the same as <see cref="AddColumns(int)"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Value argument must greater or equal
        ///         to zero.
        ///     </para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddLength2(int count) => AddColumns(count);

        /// <summary>
        /// Sets items of specified count of rows at given starting index to default values.
        /// Arguments are not checked on release build.
        /// </summary>
        /// <param name="startIndex">Range: [0, <see cref="capacity.Rows"/>].</param>
        /// <param name="count">Range: [0, <see cref="capacity.Rows"/> - startIndex).</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ClearRows(int startIndex, int count)
        {
            Debug.Assert(startIndex >= 0);
            Debug.Assert(startIndex <= capacity.Rows);
            Debug.Assert(count >= 0);
            Debug.Assert(count <= (capacity.Rows - startIndex));

            int capCols = capacity.Columns;

            Array.Clear(items, startIndex * capCols, count * capCols);
        }

        /// <summary>
        /// Sets items of specified count of columns at given starting index to default values.
        /// Arguments are not checked on release build.
        /// </summary>
        /// <param name="startIndex">Range: [0, <see cref="capacity.Columns"/>].</param>
        /// <param name="count">Range: [0, <see cref="capacity.Columns"/> - startIndex).</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ClearColumns(int startIndex, int count)
        {
            Debug.Assert(startIndex >= 0);
            Debug.Assert(startIndex <= capacity.Columns);
            Debug.Assert(count >= 0);
            Debug.Assert(count <= (capacity.Columns - startIndex));

            int rows = bounds.Rows;
            int capCols = capacity.Columns;

            for (int i = 0; i < rows; i++)
            {
                Array.Clear(items, Index2DMappedToInt(i, startIndex, capCols), count);
            }
        }

        /// <summary>
        ///     Removes specified count of rows from this instance starting at the given index.
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
            bool isInvalidRowIndex = unchecked((uint)startIndex >= (uint)Rows);
            bool isInvalidCount = unchecked((uint)count > (uint)(Rows - startIndex));

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

            for (int i0 = startIndex, i1 = i0 + count; i0 < newRows; i0++, i1++)
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
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException(
                    "Index must be a number in the range of (Rows - 1)");
            }
        }

        /// <summary>
        ///     Removes specified count of columns from this instance starting at the given index.
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
                for (int j0 = startIndex, j1 = j0 + count; j0 < newCols; j0++, j1++)
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
        ///     Inserts specified count of rows into this instance starting at the given index by 
        ///     copying. Inserted rows are initialized with default value. newBounds should be a
        ///     cached value equal to (<see cref="Rows"/> + count, <see cref="Columns"/>).
        ///     Arguments are not checked on release build.
        /// </summary>
        /// <param name="startIndex">Range: [0, <see cref="Rows"/>]</param>
        /// <param name="count">Range: [0, (<see cref="capacity.Rows"/> - startIndex)]</param>
        /// <param name="newBounds">newBounds.Columns &lt;= <see cref="Columns"/>
        /// </param>
        internal void InsertRowsNotAlloc(int startIndex, int count, Bounds2D newBounds)
        {
            Debug.Assert(startIndex >= 0);
            Debug.Assert(startIndex <= Rows);
            Debug.Assert(count >= 0);
            Debug.Assert(count <= capacity.Rows - startIndex);
            Debug.Assert(newBounds.Columns <= Columns);

            for (int i0 = newBounds.Rows - 1, i1 = i0 - count; i1 >= startIndex; i0--, i1--)
            {
                for (int j = 0; j < newBounds.Columns; j++)
                {
                    GetItemInternal(i0, j) = GetItemInternal(i1, j);
                }
            }
            ClearRows(startIndex, count);
        }

        /// <summary>
        ///     Inserts specified count of rows into this instance starting at the given index by 
        ///     allocating. Inserted rows are initialized with default value. newBounds should be a
        ///     cached value equal to (<see cref="Rows"/> + count, <see cref="Columns"/>).
        ///     Arguments are not checked on release build.
        /// </summary>
        /// <param name="startIndex">Range: [0, <see cref="Rows"/>]</param>
        /// <param name="count">Must be &gt;= 0</param>
        /// <param name="newBounds">newBounds.Columns &gt;= <see cref="Columns"/>
        /// </param>
        internal void InsertRowsAlloc(int startIndex, int count, Bounds2D newBounds)
        {
            Debug.Assert(startIndex >= 0);
            Debug.Assert(startIndex <= Rows);
            Debug.Assert(count >= 0);
            Debug.Assert(newBounds.Columns >= Columns);

            T[] newItems;
            Bounds2D newCapacity = EnsuredCapacity(newBounds);

            try
            {
                newItems = new T[Math.BigMul(newCapacity.Rows, newCapacity.Columns)];
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            for (int i = 0; i < startIndex; i++)
            {
                for (int j = 0; j < newBounds.Columns; j++)
                {
                    newItems[Index2DMappedToInt(i, j, newCapacity.Columns)] =
                        GetItemInternal(i, j);
                }
            }
            for (int i0 = startIndex, i1 = i0 + count; i1 < newBounds.Rows; i0++, i1++)
            {
                for (int j = 0; j < newBounds.Columns; j++)
                {
                    newItems[Index2DMappedToInt(i1, j, newCapacity.Columns)] =
                        GetItemInternal(i0, j);
                }
            }
            capacity = newCapacity;
            items = newItems;
        }

        /// <summary>
        ///     Inserts specified count of rows into this instance starting at the given index.
        ///     Inserted rows are initialized with default value.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: 
        ///             StartIndex must be a number in the range of <see cref="Rows"/>;<br/>
        ///             Count must be greater or equal to zero.
        ///     </para>
        /// </summary>
        /// <param name="startIndex">The zero-based index at which inserting starts.</param>
        /// <param name="count">Number of rows to insert.</param>
        public void InsertRows(int startIndex, int count)
        {
            bool isInvalidRowIndex = unchecked((uint)startIndex > (uint)Rows);

            if (isInvalidRowIndex)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startIndex), "StartIndex must be a number in the range of Rows.");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count), "Count must be greater or equal to zero.");
            }
            Bounds2D newBounds = Bounds2D.NotCheckedConstructor(Rows + count, Columns);

            if (IsReallocationNeeded(newBounds))
            {
                try
                {
                    InsertRowsAlloc(startIndex, count, newBounds);
                }
                catch (OutOfMemoryException)
                {
                    throw;
                }
            }
            else
            {
                InsertRowsNotAlloc(startIndex, count, newBounds);
            }
            bounds = newBounds;
        }

        /// <summary>
        ///     Inserts row at given index into this instance. Inserted row is initialized with
        ///     default values.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>:
        ///         Index must be a number in the range of <see cref="Rows"/>.
        ///     </para>
        /// </summary>
        /// <param name="index">
        ///     Index indicating where the row is inserted. Indexing is zero-based.
        /// </param>
        public void InsertRow(int index)
        {
            try
            {
                InsertRows(index, 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException(
                    "Index must be a number in the range of Rows.");
            }
        }

        /// <summary>
        ///     Inserts specified count of columns into this instance starting at the given index by 
        ///     copying. Inserted rows are initialized with default value. newBounds should be a
        ///     cached value equal to (<see cref="Rows"/>, <see cref="Columns"/> + count).
        ///     Arguments are not checked on release build.
        /// </summary>
        /// <param name="startIndex">Range: [0, <see cref="Columns"/>]</param>
        /// <param name="count">Range: [0, (<see cref="capacity.Columns"/> - startIndex)]</param>
        /// <param name="newBounds">newBounds.Rows &lt;= <see cref="Rows"/>
        /// </param>
        internal void InsertColumnsNotAlloc(int startIndex, int count, Bounds2D newBounds)
        {
            Debug.Assert(startIndex >= 0);
            Debug.Assert(startIndex <= Columns);
            Debug.Assert(count >= 0);
            Debug.Assert(count <= capacity.Columns - startIndex);
            Debug.Assert(newBounds.Rows <= Rows);

            int capCols = capacity.Columns;

            for (int i = 0; i < newBounds.Rows; i++)
            {
                for (int j0 = newBounds.Columns - 1, j1 = j0 - count; j1 >= startIndex; j0--, j1--)
                {
                    GetItemInternal(i, j0) = GetItemInternal(i, j1);
                }
                Array.Clear(items, Index2DMappedToInt(i, startIndex, capCols), count);
            }
        }

        /// <summary>
        ///     Inserts specified count of columns into this instance starting at the given index
        ///     by allocating. Inserted columns are initialized with default value. newBounds
        ///     should be a cached value equal to
        ///     (<see cref="Rows"/>, <see cref="Columns"/> + count). Arguments are not checked on
        ///     release build.
        /// </summary>
        /// <param name="startIndex">Range: [0, <see cref="Columns"/>]</param>
        /// <param name="count">Must be &gt;= 0</param>
        /// <param name="newBounds">newBounds.Rows &gt;= <see cref="Rows"/>
        /// </param>
        internal void InsertColumnsAlloc(int startIndex, int count, Bounds2D newBounds)
        {
            Debug.Assert(startIndex >= 0);
            Debug.Assert(startIndex <= Columns);
            Debug.Assert(count >= 0);
            Debug.Assert(newBounds.Rows >= Rows);

            T[] newItems;
            Bounds2D newCapacity = EnsuredCapacity(newBounds);

            try
            {
                newItems = new T[Math.BigMul(newCapacity.Rows, newCapacity.Columns)];
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            for (int i = 0; i < newBounds.Rows; i++)
            {
                for (int j = 0; j < startIndex; j++)
                {
                    newItems[Index2DMappedToInt(i, j, newCapacity.Columns)] =
                        GetItemInternal(i, j);
                }
            }
            for (int i = 0; i < newBounds.Rows; i++)
            {
                for (int j0 = startIndex, j1 = j0 + count; j1 < newBounds.Columns; j0++, j1++)
                {
                    newItems[Index2DMappedToInt(i, j1, newCapacity.Columns)] =
                        GetItemInternal(i, j0);
                }
            }
            capacity = newCapacity;
            items = newItems;
        }

        /// <summary>
        ///     Inserts specified count of columns into this instance starting at the given index.
        ///     Inserted columns are initialized with default value.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: 
        ///             StartIndex must be a number in the range of <see cref="Columns"/>;<br/>
        ///             Count must be greater or equal to zero.
        ///     </para>
        /// </summary>
        /// <param name="startIndex">The zero-based index at which inserting starts.</param>
        /// <param name="count">Number of columns to insert.</param>
        public void InsertColumns(int startIndex, int count)
        {
            bool isInvalidColumnIndex = unchecked((uint)startIndex > (uint)Columns);

            if (isInvalidColumnIndex)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startIndex), "StartIndex must be a number in the range of Columns.");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count), "Count must be greater or equal to zero.");
            }
            Bounds2D newBounds = Bounds2D.NotCheckedConstructor(Rows, Columns + count);

            if (IsReallocationNeeded(newBounds))
            {
                try
                {
                    InsertColumnsAlloc(startIndex, count, newBounds);
                }
                catch (OutOfMemoryException)
                {
                    throw;
                }
            }
            else
            {
                InsertColumnsNotAlloc(startIndex, count, newBounds);
            }
            bounds = newBounds;
        }

        /// <summary>
        ///     Inserts column at given index into this instance. Inserted column is initialized
        ///     with default values.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>:
        ///         Index must be a number in the range of <see cref="Columns"/>.
        ///     </para>
        /// </summary>
        /// <param name="index">
        ///     Index indicating where the column is inserted. Indexing is zero-based.
        /// </param>
        public void InsertColumn(int index)
        {
            try
            {
                InsertColumns(index, 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException(
                    "Index must be a number in the range of Columns.");
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

        /// <summary>
        ///     Determines whether specified item exists in the current instance.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            int rows = Rows;
            int cols = Columns;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (GetItemInternal(i, j).Equals(item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        internal void CopyToInternal(
            Index2D sourceIndex, Array2D<T> destination, Bounds2D quantity, Index2D destIndex)
        {
            Debug.Assert(destination != null);
            Debug.Assert(IsValidIndex(sourceIndex));
            Debug.Assert(destination.IsValidIndex(destIndex));
            Debug.Assert(sourceIndex.Row + quantity.Rows <= this.Rows);
            Debug.Assert(sourceIndex.Column + quantity.Columns <= this.Columns);
            Debug.Assert(destIndex.Row + quantity.Rows <= destination.Rows);
            Debug.Assert(destIndex.Column + quantity.Columns <= destination.Columns);

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

        /// <summary>
        ///     Constructs a new <see cref="List2D{T}"/> object that contains elements copied from
        ///     specified <typeparamref name = "T"/>[,] instance and has capacity equal to number
        ///     of elements stored in given array.
        /// <para>
        ///     Exceptions:<br/>
        ///     <see cref="ArgumentNullException"/>: Array argument cannot be null.
        /// </para>
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static List2D<T> FromSystem2DArray(T[,] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Array argument cannot be null.");
            }
            Bounds2D bounds =
                Bounds2D.NotCheckedConstructor(array.GetLength(0), array.GetLength(1));
            var result = new List2D<T>(bounds);

            result.AddRows(bounds.Rows);
            result.AddColumns(bounds.Columns);

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
