using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    /// Represents two-dimensional rectangular array of single type allocated within single
    /// contiguous block of memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public partial class Array2D<T> : IEnumerable<T>, ICollection
    {
        private T[] elements;
        private Array2DBounds bounds;

        /// <summary>
        ///     Returns total amount of rows in this two-dimensional array instance. This
        ///     property is equal to <see cref="Length1"/>
        /// </summary>
        public int Rows
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.len1;
        }

        /// <summary>
        ///     Returns total amount of columns in this two-dimensional array instance. This
        ///     property is equal to <see cref="Length2"/>
        /// </summary>
        public int Columns
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.len2;
        }

        /// <summary>
        ///     Returns length of the first dimension in this two-dimensional array instance. This
        ///     property is equal to <see cref="Rows"/>.
        /// </summary>
        public int Length1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.len1;
        }

        /// <summary>
        ///     Returns length of the second dimension in this two-dimensional array instance. This
        ///     property is equal to <see cref="Columns"/>
        /// </summary>
        public int Length2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => bounds.len2;
        }

        /// <summary>
        ///     Returns total count of elements in all the dimensions.
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => elements.Length;
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
        ///     <see cref="Array2D{T}"/>
        /// </summary>
        public object SyncRoot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => elements.SyncRoot;
        }

        /// <summary>
        ///     Returns a row at specified index. Indexing start at zero.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="IndexOutOfRangeException"/>: Index is out of boundaries of the row
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
            catch (IndexOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns a column at specified index. Indexing start at zero.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="IndexOutOfRangeException"/>: Index is out of boundaries of the
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
            catch (IndexOutOfRangeException)
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
                    return ref elements[index];
                }
                catch(IndexOutOfRangeException)
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
                if (IsValidIndex(row, column))
                {
                    throw new IndexOutOfRangeException(
                        "At least one of indices is out of array bounds.");
                }

                return ref elements[row * Columns + column];
            }
        }

        /// <summary>
        ///     Returns an element stored at specified row and column.
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
                if (IsValidIndex(index))
                {
                    throw new IndexOutOfRangeException(
                        "At least one of indices is out of array bounds.");
                }
                return ref elements[index.Dimension1Index * Columns + index.Dimension2Index];
            }
        }

        /// <summary>
        ///     Returns true if specified index exists in this <see cref="Array2D{T}"/> instance,
        ///     false otherwise.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidIndex(int row, int column) 
            => (uint)row >= bounds.len1 || (uint)column >= bounds.len2;

        /// <summary>
        ///     Returns true if specified index exists in this <see cref="Array2D{T}"/> instance,
        ///     false otherwise.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidIndex(Index2D index) 
            => IsValidIndex(index.Dimension1Index, index.Dimension2Index);

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
                bounds = new Array2DBounds(rows, columns);
            }
            catch(ArgumentOutOfRangeException)
            {
                throw;
            }
            elements = new T[rows * columns];      
        }

        /// <summary>
        ///     Constructs a two-dimensional rectangular array with specified boundaries.
        /// </summary>
        /// <param name="boundaries"></param>
        public Array2D(Array2DBounds boundaries)
        {
            bounds = boundaries;
            elements = new T[boundaries.len1 * boundaries.len2];
        }

        /// <summary>
        ///     Copies all the elements of the current two-dimensional array to the specified
        ///     two-dimensional array starting at the specified destination array index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="NullReferenceException"/>: Array is null.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Index is less than the lower bound
        ///         of array.<br/>
        ///         <see cref="ArgumentException"/>: The number of elements in the source array is
        ///         greater than the available number of elements from index to the end of the 
        ///         destination array.<br/>
        ///         <see cref="InvalidCastException"/>: At least one element in the source array
        ///         cannot be cast to the type of destination array.<br/>
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
                elements.CopyTo(destination.elements, index);
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
                elements.CopyTo(destination, index);
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

        public void CopyTo(
            Index2D sourceIndex, T[,] destination, Array2DBounds quantity, Index2D destIndex)
        {
            throw new NotImplementedException();
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
        }

        public void CopyTo(T[,] destination, Array2DBounds quantity, Index2D index)
        {
            throw new NotImplementedException();
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            if (index.Row < 0 || index.Column < 0)
            {
                throw new IndexOutOfRangeException(
                    "Index must have all the components greater than zero.");
            }
            int destRows = index.Row + quantity.Rows;
            int destCols = index.Column + quantity.Columns;
            int actualDestRows = destination.GetLength(0);
            int actualDestCols = destination.GetLength(1);

            if (destRows > actualDestRows || destCols > actualDestCols)
            {
                throw new ArgumentOutOfRangeException(
                    "Arguments (index, rows, columns) must stay in the destination array bounds.");
            }
            /*if (rows - index.Row < this.Rows || columns - index.Column < this.Columns)
            {

                throw new IndexOutOfRangeException("");
            }*/
            try
            {
            }
            catch (InvalidCastException)
            {
                throw;
            }
        }

        public void CopyTo(T[,] destination, Array2DBounds quantity)
        {
            throw new NotImplementedException();
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            int actualDestRows = destination.GetLength(0);
            int actualDestCols = destination.GetLength(1);

            if (quantity.Rows > actualDestRows || quantity.Columns > actualDestCols)
            {
                throw new ArgumentOutOfRangeException(
                    "Arguments (rows, columns) must stay in the destination array bounds.");
            }
            try
            {
            }
            catch (InvalidCastException)
            {
                throw;
            }
        }

        /// <summary>
        /// Copies all the elements of the current two-dimensional array to the specified
        /// two-dimensional array starting at the specified destination array index.
        /// <para>Exceptions:</para>
        /// <para>
        /// <see cref="ArgumentNullException"/>: Array is null.
        /// </para>
        /// <para>
        /// <see cref="ArgumentOutOfRangeException"/>: Any of indices is less than the lower
        /// bound of array.
        /// </para>
        /// <para>
        /// <see cref="ArgumentException"/>: The number of elements in the source array is
        /// greater than the available number of elements from indices to the end of the 
        /// destination array.
        /// </para>
        /// <para>
        /// <see cref="InvalidCastException"/>: At least one element in the source array
        /// cannot be cast to the type of destination array.
        /// </para>
        /// </summary>
        /// <param name="destination">
        ///     The array, which elements are copied to.
        /// </param>
        /// <param name="index">
        ///     Represents the index that consist of row and column
        ///     in array at which copying begins. Indexing is zero-based.
        /// </param>
        public void CopyTo(T[,] destination, Index2D index)
        {
            throw new NotImplementedException();
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            if (index.Row < 0 || index.Column < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "Any of indices cannot be less than zero.");
            }
            int rows = destination.GetLength(0);
            int columns = destination.GetLength(1);

            if (rows - index.Row < this.Rows || columns - index.Column < this.Columns)
            {

                throw new ArgumentException(
                    "The number of elements in the source array is greater than the available " +
                    "number of elements from indices to the end of the destination array.",
                    nameof(destination));
            }
            try
            {
            }
            catch (InvalidCastException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Copies all the elements of the current two-dimensional array to the specified
        ///     two-dimensional array.
        ///     <para>Exceptions:</para>
        /// <para>
        ///     <see cref="ArgumentNullException"/>: Destination array is null.
        /// </para>
        /// <para>
        ///     <see cref="ArgumentOutOfRangeException"/>: Destination array does not have equal
        ///     bounds.
        /// </para>
        /// <para>
        ///     <see cref="InvalidCastException"/>: At least one element in the source array cannot
        ///     be cast to the type of destination array.
        /// </para>
        /// </summary>
        /// <param name="destination"></param>
        public void CopyTo(T[,] destination)
        {
            throw new NotImplementedException();
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            int rows = destination.GetLength(0);
            int columns = destination.GetLength(1);

            if (rows != this.Rows || columns != this.Columns)
            {
                throw new ArgumentOutOfRangeException(
                    "Destination array does not have equal bounds.");
            }                  
            try
            {
            }
            catch (InvalidCastException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns an enumerator for all elements of two-dimensional array.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < elements.Length; i++)
            {
                yield return elements[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        ///     Searches for the specified object and returns the index of its first occurrence
        ///     in a one-dimensional array if found; otherwise returns null (<see cref="int"/>?
        ///     with HasValue property set to false).
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="element">An element value to search.</param>
        /// <returns></returns>
        public int? IndexOf<U>(U element) where U : IEquatable<T>
        {
            for (int i = 0, length = Count; i < length; i++)
            {
                if (element.Equals(this.elements[i]))
                {
                    return i;
                }
            }
            return null;
        }

        /// <summary>
        ///     Searches for the specified object and returns the 2D index of its first occurrence
        ///     in a two-dimensional array if found; otherwise returns null
        ///     ((<see cref="int"/>, <see cref="int"/>)? with HasValue property set to false).
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="element">An element value to search.</param>
        /// <returns></returns>
        public Index2D? IndicesOf<U>(U element) where U : IEquatable<T>
        {
            int? possibleIndex = IndexOf(element);

            if (!possibleIndex.HasValue)
            {
                return null;
            }
            int oneDimIndex = possibleIndex.Value;
            int row = oneDimIndex / Columns;
            int column = oneDimIndex % Columns;

            return new Index2D(row, column);
        }
    }
}