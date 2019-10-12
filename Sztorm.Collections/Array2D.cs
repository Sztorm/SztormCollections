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
        private int dim0;
        private int dim1;

        /// <summary>
        /// Returns total amount of rows in this two-dimensional array instance.
        /// </summary>
        public int Rows
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => dim0;
        }

        /// <summary>
        /// Returns total amount of columns in this two-dimensional array instance.
        /// </summary>
        public int Columns
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => dim1;
        }

        /// <summary>
        /// Returns total count of elements in all the dimensions.
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => elements.Length;
        }

        /// <summary>
        /// This collection is not synchronized internally.
        /// To synchronize access use lock statement with <see cref="SyncRoot"></see> property.
        /// </summary>
        public bool IsSynchronized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => false;
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the System.Array.
        /// </summary>
        public object SyncRoot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => elements.SyncRoot;
        }

        /// <summary>
        /// Returns a row at specified index.
        /// <para>Exceptions:</para>
        /// <para>
        /// <see cref="IndexOutOfRangeException"></see>: Index is out of bounds of the row count.
        /// </para>
        /// </summary>
        /// <param name="index">A zero-based index that determines which row is to take.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Row GetRow(int index)
        {
            return new Row(this, index);
        }

        /// <summary>
        /// Returns a column at specified index.
        /// <para>Exceptions:</para>
        /// <para>
        /// <see cref="IndexOutOfRangeException"></see>: Index is out of bounds of the column count.
        /// </para>
        /// </summary>
        /// <param name="index">A zero-based index that determines which column is to take.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Column GetColumn(int index)
        {
            return new Column(this, index);
        }

        /// <summary>
        /// An one dimensional indexer that can be used to iterate through all the stored elements.
        /// <para>
        /// Throws <see cref="IndexOutOfRangeException"></see> if index exceeds
        /// <see cref="Count"></see> property.
        /// </para>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ref T this[int index]
        {
            // In fact bounds check is omitted here because it already exists in array indexer
            // and has proper exception.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref elements[index];
        }

        /// <summary>
        /// Returns an element stored at specified row and column.
        /// <para>
        /// Throws <see cref="IndexOutOfRangeException"></see> if any of indices is out of array
        /// bounds.
        /// </para>
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public ref T this[int row, int column]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)row >= dim0 || (uint)column >= dim1)
                {
                    throw new IndexOutOfRangeException("At least one of indices is out of array bounds.");
                }

                return ref elements[row * Columns + column];
            }
        }

        /// <summary>
        /// Constructs a two-dimensional rectangular array with specified quantity of rows and
        /// columns.
        /// <para>
        /// Throws <see cref="ArgumentException"></see> if rows or columns quantity is less than
        /// zero.
        /// </para>
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        public Array2D(int rows, int columns)
        {
            if (rows < 0 || columns < 0)
            {
                throw new ArgumentException(
                    "Rows and columns arguments must be greater or equal to zero.");
            }
            elements = new T[rows * columns];
            dim0 = rows;
            dim1 = columns;
        }

        /// <summary>
        /// Copies all the elements of the current two-dimensional array to the specified
        /// two-dimensional array starting at the specified destination array index.
        /// <para>Exceptions:</para>
        /// <para>
        /// <see cref="NullReferenceException"></see>: Array is null.
        /// </para>
        /// <para>
        /// <see cref="ArgumentOutOfRangeException"></see>: Index is less than the lower bound of
        /// array.
        /// </para>
        /// <para>
        /// <see cref="ArgumentException"></see>: The number of elements in the source array is
        /// greater than the available number of elements from index to the end of the destination
        /// array.
        /// </para>
        /// <para>
        /// <see cref="InvalidCastException"></see>: At least one element in the source array
        /// cannot be cast to the type of destination array.
        /// </para>
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
        /// Copies all the elements of the current two-dimensional array to the specified
        /// one-dimensional array starting at the specified destination array index.
        /// <para>Exceptions:</para>
        /// <para>
        /// <see cref="ArgumentNullException"></see>: Array is null.
        /// </para>
        /// <para>
        /// <see cref="ArgumentOutOfRangeException"></see>: Index is less than the lower bound of
        /// array.
        /// </para>
        /// <para>
        /// <see cref="ArgumentException"></see>: array is multidimensional. -or- The number of 
        /// elements in the source array is greater than the available number of elements from
        /// index to the end of the destination array.
        /// </para>
        /// <para>
        /// <see cref="ArrayTypeMismatchException"></see>: The type of the source array cannot be
        /// cast automatically to the type of the destination array.
        /// </para>
        /// <para>
        /// <see cref="RankException"></see>: The source array is multidimensional.
        /// </para>   
        /// <para>
        /// <see cref="InvalidCastException"></see>: At least one element in the source array
        /// cannot be cast to the type of destination array.
        /// </para>
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="index">
        /// A 32-bit integer that represents the index in array at which copying begins.
        /// </param>
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

        /// <summary>
        /// Copies all the elements of the current two-dimensional array to the specified
        /// two-dimensional array starting at the specified destination array index.
        /// <para>Exceptions:</para>
        /// <para>
        /// <see cref="ArgumentNullException"></see>: Array is null.
        /// </para>
        /// <para>
        /// <see cref="ArgumentOutOfRangeException"></see>: Any of indices is less than the lower
        /// bound of array.
        /// </para>
        /// <para>
        /// <see cref="ArgumentException"></see>: The number of elements in the source array is
        /// greater than the available number of elements from indices to the end of the 
        /// destination array.
        /// </para>
        /// <para>
        /// <see cref="InvalidCastException"></see>: At least one element in the source array
        /// cannot be cast to the type of destination array.
        /// </para>
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="row">
        /// An <see cref="int"></see> that represents the index of row in array at which copying 
        /// begins.
        /// </param>
        /// <param name="column">
        /// An <see cref="int"></see> that represents the index of column in array at which copying
        /// begins.
        /// </param>
        public void CopyTo(T[,] destination, int row, int column)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            if (row < 0 || column < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "Any of indices is less than the lower bound of array.");
            }
            int rows = destination.GetLength(0);
            int columns = destination.GetLength(1);

            if (rows - row < this.Rows || columns - column < this.Columns)
            {

                throw new ArgumentException(
                    "The number of elements in the source array is greater than the available " +
                    "number of elements from indices to the end of the destination array.",
                    nameof(destination));
            }
            try
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        destination[i, j] = this[i, j];
                    }
                }
            }
            catch (InvalidCastException)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns an enumerator for all elements of two-dimensional array.
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
        /// Searches for the specified object and returns the index of its first occurrence
        /// in a one-dimensional array if found; otherwise returns null (<see cref="int"></see>?
        /// with HasValue property set to false).
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
        /// Searches for the specified object and returns the indices in a form of row and column 
        /// <see cref="ValueTuple{int, int}"></see> of its first occurrence in a two-dimensional
        /// array if found; otherwise returns null
        /// ((<see cref="int"></see>, <see cref="int"></see>)? with HasValue property set to 
        /// false).
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="element">An element value to search.</param>
        /// <returns></returns>
        public (int row, int column)? IndicesOf<U>(U element) where U : IEquatable<T>
        {
            int? possibleIndex = IndexOf(element);

            if (!possibleIndex.HasValue)
            {
                return null;
            }
            int oneDimIndex = possibleIndex.Value;
            int row = oneDimIndex / Columns;
            int column = oneDimIndex % Columns;

            return (row, column);
        }
    }
}