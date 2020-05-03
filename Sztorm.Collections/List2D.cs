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
    using static RectangularCollectionUtils;

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
        private int version;

        /// <summary>
        ///     Returns total number of rows in this <see cref="List2D{T}"/> instance. This
        ///     property is equal to <see cref="Length1"/>
        /// </summary>
        public int Rows
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.Rows;
        }

        /// <summary>
        ///     Returns total number of columns in this <see cref="List2D{T}"/> instance. This
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
        ///     Returns total number of elements in all the dimensions.
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.Rows * bounds.Columns;
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
        ///     This collection is not synchronized. To synchronize access use
        ///     <see langword="lock"/> statement with <see cref="SyncRoot"/> property.
        /// </summary>
        public bool IsSynchronized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => false;
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
        ///         <see cref="IndexOutOfRangeException"/>: At least one of indices is out of list
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
                        "At least one of indices is out of list bounds.");
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
        ///     Returns an enumerator for all elements of the <see cref="List2D{T}"/>, which
        ///     enumerates row by row from the (0, 0) position.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new Enumerator(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///     Constructs a new instance of <see cref="List2D{T}"/> class that is empty and has
        ///     the default initial capacity.
        /// </summary>
        public List2D()
        {
            bounds = new Bounds2D();
            capacity = DefaultInitialCapacity;
            items = new T[capacity.Length1 * capacity.Length2];
            version = 0;
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
            version = 0;
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
            version = 0;
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
            version = 0;

            array.CopyTo(items, 0);
        }

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
            return new Bounds2D(new Box<int>(newCapRows), new Box<int>(newCapCols));
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
                    newItems[RowMajorIndex2DToInt(new Index2D(i, j), newCapacity.Columns)] =
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
        /// Determines whether current instance capacity can accommodate specified number of rows
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
        ///     Adds specified number of columns to the end of the <see cref="List2D{T}"/>. This
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
            bounds = new Bounds2D(new Box<int>(newRows), new Box<int>(newCols));
            version++;
        }

        /// <summary>
        ///     Adds specified number of rows to the end of the <see cref="List2D{T}"/>. This
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
            bounds = new Bounds2D(new Box<int>(newRows), new Box<int>(newCols));
            version++;
        }

        /// <summary>
        ///     Adds specified number of rows to the end of the <see cref="List2D{T}"/>. This
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
        ///     Adds specified number of columns to the end of the <see cref="List2D{T}"/>. This
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
        ///     Removes all elements from the <see cref="List2D{T}"/>. Any stored references are
        ///     released.
        /// </summary>
        public void Clear()
        {
            if (Count > 0)
            {
                Array.Clear(items, 0, items.Length);
                bounds = new Bounds2D();
                version++;
            }
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
        public bool Contains(T item) => Index2DOf(item).IsSuccess;

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
                return Index2DOfEquatable(item).IsSuccess;
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
                return Index2DOfComparable(item).IsSuccess;
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("item cannot be null.");
            }
        }

        internal void CopyToInternal(
            Index2D sourceIndex, Array2D<T> destination, Bounds2D sectorSize, Index2D destIndex)
        {
            Debug.Assert(destination != null);
            Debug.Assert(IsValidIndex(sourceIndex));
            Debug.Assert(destination.IsValidIndex(destIndex));
            Debug.Assert(sourceIndex.Row + sectorSize.Rows <= this.Rows);
            Debug.Assert(sourceIndex.Column + sectorSize.Columns <= this.Columns);
            Debug.Assert(destIndex.Row + sectorSize.Rows <= destination.Rows);
            Debug.Assert(destIndex.Column + sectorSize.Columns <= destination.Columns);

            int dr = destIndex.Row;
            int sr = sourceIndex.Row;
            int totalRows = destIndex.Row + sectorSize.Rows;
            int totalCols = destIndex.Column + sectorSize.Columns;

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

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
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
            => FindIndex2D(match).IsSuccess;

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
                return FindIndex2D(match).IsSuccess;
            }
            catch (ArgumentNullException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying first
        ///     occurrence searched row by row within the entire <see cref="List2D{T}"/> if found.
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
            ItemRequestResult<Index2D> indexRequest = FindIndex2D(match);

            return indexRequest.IsSuccess ? 
                new ItemRequestResult<T>(GetItemInternal(indexRequest.ItemOrDefault)) :
                ItemRequestResult<T>.Failed;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying first
        ///     occurrence searched row by row within the entire <see cref="List2D{T}"/> if found.
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
                int count = Count;
                int capCols = capacity.Columns;
                int gapPerRow = capCols - Columns;

                for (int i = 0, iter = 0; iter < count; i += gapPerRow)
                {
                    int index2DColumn = i % capCols;

                    for (int j = index2DColumn; j < Columns && iter < count; j++, i++, iter++)
                    {
                        ref readonly T item = ref items[i];

                        if (match.Invoke(item))
                        {
                            resizableCollection.Add(item);
                        }
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
                int count = Count;
                int capCols = capacity.Columns;
                int gapPerRow = capCols - Columns;

                for (int i = 0, iter = 0; iter < count; i += gapPerRow)
                {
                    int index2DColumn = i % capCols;

                    for (int j = index2DColumn; j < Columns && iter < count; j++, i++, iter++)
                    {
                        if (match.Invoke(items[i]))
                        {
                            resizableCollection.Add(IntToRowMajorIndex2D(i, capCols));
                        }
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

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying last
        ///     occurrence searched row by row within the entire <see cref="List2D{T}"/> if found.
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
            ItemRequestResult<Index2D> indexRequest = FindLastIndex2D(match);

            return indexRequest.IsSuccess ?
                new ItemRequestResult<T>(GetItemInternal(indexRequest.ItemOrDefault)) :
                ItemRequestResult<T>.Failed;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying last
        ///     occurrence searched row by row within the entire <see cref="List2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
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

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence.<br/>
        ///     Arguments are not validated on release builds.
        /// </summary>
        /// <typeparam name="TPredicate"></typeparam>
        /// <param name="startIndex"> &gt;= 0</param>
        /// <param name="count">
        ///     <paramref name="count"/> + <paramref name="startIndex"/> &lt;= capacity.Rows *
        ///     capacity.Columns
        /// </param>
        /// <param name="match"></param>
        /// <returns></returns>
        internal ItemRequestResult<Index2D> FindIndex2DInternal<TPredicate>(
           int startIndex, int count, TPredicate match)
           where TPredicate : struct, IPredicate<T>
        {
            Debug.Assert(startIndex >= 0);
            Debug.Assert(startIndex + count <= capacity.Rows * capacity.Columns);

            int capCols = capacity.Columns;
            int gapPerRow = capCols - Columns;

            for (int i = startIndex, iter = 0; iter < count; i += gapPerRow)
            {
                int index2DColumn = i % capCols;

                for (int j = index2DColumn; j < Columns && iter < count; j++, i++, iter++)
                {
                    if (match.Invoke(items[i]))
                    {
                        return new ItemRequestResult<Index2D>(IntToRowMajorIndex2D(i, capCols));
                    }
                }
            }
            return ItemRequestResult<Index2D>.Failed;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence searched within the entire <see cref="List2D{T}"/> if
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
            => FindIndex2DInternal(0, Count, match);

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within list bounds;<br/>
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
        public ItemRequestResult<Index2D> FindIndex2D<TPredicate>(
            Index2D startIndex, int count, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            if (!IsValidIndex(startIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startIndex), "startIndex must be within list bounds.");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count), "count must be greater or equal to zero.");
            }
            int startIndex1D = RowMajorIndex2DToInt(startIndex, capacity.Columns);
            int startIndex1DWithoutGap = RowMajorIndex2DToInt(startIndex, Columns);

            if (startIndex1DWithoutGap + count > Count)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count), "startIndex together with count must not exceed List2D.Count");
            }
            return FindIndex2DInternal(startIndex1D, count, match);
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence searched within the specified sector. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within list bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within list bounds, beginning from
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
        public ItemRequestResult<Index2D> FindIndex2D<TPredicate>(
            Index2D startIndex, Bounds2D sectorSize, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            if (!IsValidIndex(startIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startIndex), "startIndex must be within list bounds.");
            }
            var indexAfterEnd = new Index2D(
                startIndex.Row + sectorSize.Rows,
                startIndex.Column + sectorSize.Columns);

            if (indexAfterEnd.Row > Rows || indexAfterEnd.Column > Columns)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sectorSize),
                    "sectorSize must be within list bounds, beginning from startIndex.");
            }
            int capCols = capacity.Columns;

            for (int i = startIndex.Row; i < indexAfterEnd.Row; i++)
            {
                int index1D = RowMajorIndex2DToInt(new Index2D(i, startIndex.Column), capCols);

                for (int j = startIndex.Column; j < indexAfterEnd.Column; j++, index1D++)
                {
                    if (match.Invoke(items[index1D]))
                    {
                        return new ItemRequestResult<Index2D>(new Index2D(i, j));
                    }
                }
            }
            return ItemRequestResult<Index2D>.Failed;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence searched within the entire <see cref="List2D{T}"/> if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
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
        public ItemRequestResult<Index2D> FindIndex2D(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }
            return FindIndex2D(new BoxedPredicate<T>(match));
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
        ///         be within list bounds;<br/>
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
        public ItemRequestResult<Index2D> FindIndex2D(
            Index2D startIndex, int count, Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            try
            {
                return FindIndex2D(startIndex, count, new BoxedPredicate<T>(match));
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
        ///         be within list bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within list bounds, beginning from
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
        public ItemRequestResult<Index2D> FindIndex2D(
            Index2D startIndex, Bounds2D sectorSize, Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            try
            {
                return FindIndex2D(startIndex, sectorSize, new BoxedPredicate<T>(match));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the last occurrence.<br/>
        ///     Arguments are not validated on release builds.
        /// </summary>
        /// <typeparam name="TPredicate"></typeparam>
        /// <param name="startIndex"> &lt; capacity.Rows * capacity.Columns</param>
        /// <param name="count"> &gt;= 0</param>
        /// <param name="match"></param>
        /// <returns></returns>
        internal ItemRequestResult<Index2D> FindLastIndex2DInternal<TPredicate>(
            int startIndex, int count, TPredicate match)
            where TPredicate : struct, IPredicate<T>
        {
            Debug.Assert(startIndex < capacity.Rows * capacity.Columns);
            Debug.Assert(count >= 0);

            int capCols = capacity.Columns;
            int gapPerRow = capCols - Columns;

            for (int i = startIndex, iter = 0; iter < count; i -= gapPerRow)
            {
                int index2DColumn = i % capCols;

                for (int j = index2DColumn; j >= 0 && iter < count; j--, i--, iter++)
                {
                    if (match.Invoke(items[i]))
                    {
                        return new ItemRequestResult<Index2D>(IntToRowMajorIndex2D(i, capCols));
                    }
                }
            }
            return ItemRequestResult<Index2D>.Failed;   
        }

        /// <summary>
        ///     Searches from the beginning for an item that matches the conditions defined by the
        ///     specified predicate, and returns the <see cref="ItemRequestResult{T}"/> with
        ///     underlying index of the last occurrence searched within the entire
        ///     <see cref="List2D{T}"/> if found. Otherwise returns
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
            => FindLastIndex2DInternal(
                RowMajorIndex2DToInt(
                    new Index2D(bounds.Rows - 1, bounds.Columns - 1), capacity.Columns),
                Count,
                match);

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the last occurrence searched row by row within the specified range of items if
        ///     found. Otherwise returns <see cref="ItemRequestResult{T}.Failed"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within list bounds;<br/>
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
            if (!IsValidIndex(startIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startIndex), "startIndex must be within list bounds.");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count), "count must be greater or equal to zero.");
            }
            int startIndex1D = RowMajorIndex2DToInt(startIndex, capacity.Columns);
            int startIndex1DWithoutGap = RowMajorIndex2DToInt(startIndex, Columns);

            if (startIndex1DWithoutGap - count < -1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count), "startIndex together with count must not exceed List2D.Count");
            }
            return FindLastIndex2DInternal(startIndex1D, count, match);
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the last occurrence searched within the specified sector. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Failed"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within list bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within list bounds, beginning backwardly
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
            if (!IsValidIndex(startIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startIndex), "startIndex must be within list bounds.");
            }
            var indexAfterEnd = new Index2D(
                startIndex.Row - sectorSize.Rows,
                startIndex.Column - sectorSize.Columns);

            if (indexAfterEnd.Row < -1 || indexAfterEnd.Column < -1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sectorSize),
                    "sectorSize must be within list bounds, beginning backwardly from " +
                    "startIndex.");
            }
            int capCols = capacity.Columns;

            for (int i = startIndex.Row; i > indexAfterEnd.Row; i--)
            {
                int index1D = RowMajorIndex2DToInt(new Index2D(i, startIndex.Column), capCols);

                for (int j = startIndex.Column; j > indexAfterEnd.Column; j--, index1D--)
                {
                    if (match.Invoke(items[index1D]))
                    {
                        return new ItemRequestResult<Index2D>(new Index2D(i, j));
                    }
                }
            }
            return ItemRequestResult<Index2D>.Failed;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the last occurrence searched within the entire <see cref="List2D{T}"/> if found.
        ///     Otherwise returns <see cref="ItemRequestResult{T}.Failed"/><br/>
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
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }
            return FindLastIndex2DInternal(
                RowMajorIndex2DToInt(
                    new Index2D(bounds.Rows - 1, bounds.Columns - 1), capacity.Columns),
                    Count,
                    new BoxedPredicate<T>(match));
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
        ///         be within list bounds;<br/>
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
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            try
            {
                return FindLastIndex2D(startIndex, count, new BoxedPredicate<T>(match));
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
        ///         be within list bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within list bounds, beginning backwardly
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
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            try
            {
                return FindLastIndex2D(startIndex, sectorSize, new BoxedPredicate<T>(match));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Performs the specified action on each element of the <see cref="List2D{T}"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="InvalidOperationException"/>: List cannot be modified
        ///         during enumeration.
        ///     </para>
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
        ///     Performs the specified action on each element of the <see cref="List2D{T}"/>.
        ///     <paramref name="action"/> modifications are reflected to the caller as it is passed
        ///     by <see langword="ref"/>.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="InvalidOperationException"/>: List cannot be modified
        ///         during enumeration.
        ///     </para>
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
            int cols = Columns;
            int gapPerRow = capacity.Columns - cols;
            int indexAfterEnd = Rows * (cols + gapPerRow) - gapPerRow;
            int version = this.version;

            for (int i = 0; i < indexAfterEnd; i += gapPerRow)
            {
                for (int j = 0; j < cols; j++, i++)
                {
                    action.Invoke(items[i]);

                    if (this.version != version)
                    {
                        throw new InvalidOperationException(
                            "List cannot be modified during enumeration.");
                    }
                }
            }
        }

        /// <summary>
        ///     Performs the specified action on each element of the <see cref="List2D{T}"/>.<br/>
        ///     Use <see cref="ForEach{TAction}(TAction)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="action"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="InvalidOperationException"/>: List cannot be modified
        ///         during enumeration.
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
            Bounds2D bounds = new Bounds2D(
                new Box<int>(array.GetLength(0)), new Box<int>(array.GetLength(1)));
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
                Reallocate(new Bounds2D(new Box<int>(newRows), new Box<int>(newCols)));
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            version++;
        }

        /// <summary>
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the first
        ///     occurrence of item searched within the entire <see cref="List2D{T}"/> if found.
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
        ///         be within list bounds;<br/>
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
        ///         be within list bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within list bounds, beginning from
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
        ///     occurrence of item searched within the entire <see cref="List2D{T}"/> if found.
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
        ///         be within list bounds;<br/>
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
        ///         be within list bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within list bounds, beginning from
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
        ///     occurrence of item searched within the entire <see cref="List2D{T}"/> if found.
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
        ///         be within list bounds;<br/>
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
        ///         be within list bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within list bounds, beginning from
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
        ///     Inserts specified number of columns into this instance starting at the given index
        ///     by copying. Inserted rows are initialized with default value. newBounds should be a
        ///     cached value equal to (<see cref="Rows"/>, <see cref="Columns"/> + count).
        ///     Arguments are not checked on release build.
        /// </summary>
        /// <param name="startIndex">Range: [0, <see cref="Columns"/>]</param>
        /// <param name="count">Range: [0, (<see cref="capacity"/>.Columns - startIndex)]</param>
        /// <param name="newBounds">newBounds.Rows &lt;= <see cref="Rows"/>
        /// </param>
        private void InsertColumnsNotAlloc(int startIndex, int count, Bounds2D newBounds)
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
                Array.Clear(items, RowMajorIndex2DToInt(new Index2D(i, startIndex), capCols), count);
            }
        }

        /// <summary>
        ///     Inserts specified number of columns into this instance starting at the given index
        ///     by allocating. Inserted columns are initialized with default value. newBounds
        ///     should be a cached value equal to
        ///     (<see cref="Rows"/>, <see cref="Columns"/> + count). Arguments are not checked on
        ///     release build.
        /// </summary>
        /// <param name="startIndex">Range: [0, <see cref="Columns"/>]</param>
        /// <param name="count">Must be &gt;= 0</param>
        /// <param name="newBounds">newBounds.Rows &gt;= <see cref="Rows"/>
        /// </param>
        private void InsertColumnsAlloc(int startIndex, int count, Bounds2D newBounds)
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
                    newItems[RowMajorIndex2DToInt(new Index2D(i, j), newCapacity.Columns)] =
                        GetItemInternal(i, j);
                }
            }
            for (int i = 0; i < newBounds.Rows; i++)
            {
                for (int j0 = startIndex, j1 = j0 + count; j1 < newBounds.Columns; j0++, j1++)
                {
                    newItems[RowMajorIndex2DToInt(new Index2D(i, j1), newCapacity.Columns)] =
                        GetItemInternal(i, j0);
                }
            }
            capacity = newCapacity;
            items = newItems;
        }

        /// <summary>
        ///     Inserts specified number of columns into this instance starting at the given index.
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
            Bounds2D newBounds = new Bounds2D(new Box<int>(Rows), new Box<int>(Columns + count));

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
            version++;
        }

        /// <summary>
        /// Sets items of specified number of rows at given starting index to default values.
        /// Arguments are not checked on release build.
        /// </summary>
        /// <param name="startIndex">Range: [0, <see cref="capacity"/>.Rows]</param>
        /// <param name="count">Range: [0, <see cref="capacity"/> - startIndex.Rows)</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ClearRows(int startIndex, int count)
        {
            Debug.Assert(startIndex >= 0);
            Debug.Assert(startIndex <= capacity.Rows);
            Debug.Assert(count >= 0);
            Debug.Assert(count <= (capacity.Rows - startIndex));

            int capCols = capacity.Columns;

            Array.Clear(items, startIndex * capCols, count * capCols);
        }

        /// <summary>
        ///     Inserts specified number of rows into this instance starting at the given index by
        ///     copying. Inserted rows are initialized with default value. newBounds should be a
        ///     cached value equal to (<see cref="Rows"/> + count, <see cref="Columns"/>).
        ///     Arguments are not checked on release build.
        /// </summary>
        /// <param name="startIndex">Range: [0, <see cref="Rows"/>]</param>
        /// <param name="count">Range: [0, (<see cref="capacity"/>.Rows - startIndex)]</param>
        /// <param name="newBounds">newBounds.Columns &lt;= <see cref="Columns"/>
        /// </param>
        private void InsertRowsNotAlloc(int startIndex, int count, Bounds2D newBounds)
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
        ///     Inserts specified number of rows into this instance starting at the given index by 
        ///     allocating. Inserted rows are initialized with default value. newBounds should be a
        ///     cached value equal to (<see cref="Rows"/> + count, <see cref="Columns"/>).
        ///     Arguments are not checked on release build.
        /// </summary>
        /// <param name="startIndex">Range: [0, <see cref="Rows"/>]</param>
        /// <param name="count">Must be &gt;= 0</param>
        /// <param name="newBounds">newBounds.Columns &gt;= <see cref="Columns"/>
        /// </param>
        private void InsertRowsAlloc(int startIndex, int count, Bounds2D newBounds)
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
                    newItems[RowMajorIndex2DToInt(new Index2D(i, j), newCapacity.Columns)] =
                        GetItemInternal(i, j);
                }
            }
            for (int i0 = startIndex, i1 = i0 + count; i1 < newBounds.Rows; i0++, i1++)
            {
                for (int j = 0; j < newBounds.Columns; j++)
                {
                    newItems[RowMajorIndex2DToInt(new Index2D(i1, j), newCapacity.Columns)] =
                        GetItemInternal(i0, j);
                }
            }
            capacity = newCapacity;
            items = newItems;
        }

        /// <summary>
        ///     Inserts specified number of rows into this instance starting at the given index.
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
            Bounds2D newBounds = new Bounds2D(new Box<int>(Rows + count), new Box<int>(Columns));

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
            version++;
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
        ///     Returns the <see cref="ItemRequestResult{T}"/> with underlying index of the last
        ///     occurrence of item searched within the entire <see cref="List2D{T}"/> if found.
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
        ///         be within list bounds;<br/>
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
        ///         be within list bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within list bounds, beginning backwardly
        ///         from <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <param name="item">An element value to search.</param>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult<Index2D> LastIndex2DOf(
            T item, Index2D startIndex, Bounds2D sectorSize)
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
        ///     occurrence of item searched within the entire <see cref="List2D{T}"/> if found.
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
        ///         be within list bounds;<br/>
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
        ///         be within list bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within list bounds, beginning backwardly
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
        ///     occurrence of item searched within the entire <see cref="List2D{T}"/> if found.
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
        ///         be within list bounds;<br/>
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
        ///         be within list bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within list bounds, beginning backwardly
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
        ///     Removes specified number of rows from this instance starting at the given index.
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

            bounds = new Bounds2D(new Box<int>(newRows), new Box<int>(cols));
            version++;
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
        /// Sets items of specified number of columns at given starting index to default values.
        /// Arguments are not checked on release build.
        /// </summary>
        /// <param name="startIndex">Range: [0, <see cref="capacity"/>.Columns]</param>
        /// <param name="count">Range: [0, <see cref="capacity"/>.Columns - startIndex)</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ClearColumns(int startIndex, int count)
        {
            Debug.Assert(startIndex >= 0);
            Debug.Assert(startIndex <= capacity.Columns);
            Debug.Assert(count >= 0);
            Debug.Assert(count <= (capacity.Columns - startIndex));

            int rows = bounds.Rows;
            int capCols = capacity.Columns;

            for (int i = 0; i < rows; i++)
            {
                Array.Clear(items, RowMajorIndex2DToInt(
                    new Index2D(i, startIndex), capCols), count);
            }
        }

        /// <summary>
        ///     Removes specified number of columns from this instance starting at the given index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: 
        ///             StartIndex must be a number in the range of
        ///             (<see cref="Columns"/> - 1);<br/>
        ///             Count must be a number in the range of
        ///             (<see cref="Columns"/> - startIndex).
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

            bounds = new Bounds2D(new Box<int>(rows), new Box<int>(newCols));
            version++;
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
