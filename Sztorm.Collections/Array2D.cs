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
        /// Returns count of elements in X dimension;
        /// </summary>
        public int X
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => dim0;
        }

        /// <summary>
        /// Returns count of elements in Y dimension;
        /// </summary>
        public int Y
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
        /// Returns a subarray that represents elements from X dimension with specified index.
        /// <para>Exceptions:</para>
        /// <para>
        /// <see cref="ArgumentOutOfRangeException"></see>: Index is out of bounds of the X 
        /// dimension.
        /// </para>
        /// </summary>
        /// <param name="index">A zero-based index that determines which subarray is to take.</param>
        /// <returns></returns>
        public ElementsOfX GetElementsOfX(int index)
        {
            return new ElementsOfX(this, index);
        }

        /// <summary>
        /// Returns a subarray that represents elements from Y dimension with specified index.
        /// <para>Exceptions:</para>
        /// <para>
        /// <see cref="ArgumentOutOfRangeException"></see>: Index is out of bounds of the X 
        /// dimension.
        /// </para>
        /// </summary>
        /// <param name="index">A zero-based index that determines which subarray is to take.</param>
        /// <returns></returns>
        public ElementsOfY GetElementsOfY(int index)
        {
            return new ElementsOfY(this, index);
        }

        /// <summary>
        /// One dimensional indexer can be used to iterate through all the stored elements without
        /// specific order.
        /// <para>
        /// Throws <see cref="IndexOutOfRangeException"></see> if index exceeds array bounds.
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
        /// Returns an element stored in two-dimensional array.
        /// <para>
        /// Throws <see cref="IndexOutOfRangeException"></see> if any of indices is out of array
        /// bounds.
        /// </para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public ref T this[int x, int y]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)x >= dim0 || (uint)y >= dim1)
                {
                    throw new IndexOutOfRangeException("At least one of indices is out of array bounds.");
                }

                return ref elements[x * Y + y];
            }
        }

        /// <summary>
        /// Constructs a two-dimensional rectangular array with specified X and Y dimension sizes.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Array2D(int x, int y)
        {
            elements = new T[x * y];
            dim0 = x;
            dim1 = y;
        }

        /// <summary>
        /// Copies all the elements of the current two-dimensional array to the specified
        /// two-dimensional array starting at the specified destination array index.
        /// <para>Exceptions:</para>
        /// <para>
        /// <see cref="ArgumentNullException"></see>: Array is null.
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
        /// <see cref="ArrayTypeMismatchException"></see>: The type of the source array cannot be
        /// cast automatically to the type of the destination array.
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(Array2D<T> destination, int index)
        {
            this.elements.CopyTo(destination.elements, index);
        }

        /// <summary>
        /// Not implemented yet.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="index"></param>
        public void CopyTo(Array destination, int index)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            int rank = destination.Rank;

            if (rank == 1)
            {
                elements.CopyTo(destination, index);
                return;
            }
            else if (rank == 2)
            {
                /*if (destination.Length - index < Count)
                {
                    throw new ArgumentException("Not enough elements after index in the destination array.");
                }*/
                throw new NotImplementedException();
            }
            else
            {
                throw new ArgumentException("array has invalid number of dimensions.", nameof(destination));
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }     
    }
}