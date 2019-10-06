using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    partial class Array2D<T>
    {
        /// <summary>
        /// Represents X dimension subarray of two-dimensional array.
        /// </summary>
        public readonly struct ElementsOfX : IEnumerable<T>
        {
            private readonly Array2D<T> array;

            /// <summary>
            /// Returns count of elements in this subarray.
            /// </summary>
            public int Length => array.X;

            /// <summary>
            /// Returns an element stored in the X dimension subarray.
            /// <para>
            /// Throws <see cref="IndexOutOfRangeException"></see> if index is out of X dimension
            /// subarray bounds.
            /// </para>
            public ref T this[int index]
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    try
                    {
                        return ref array[Index, index];
                    }
                    catch(IndexOutOfRangeException)
                    {
                        throw new IndexOutOfRangeException(
                            "Index is out of X dimension subarray bounds.");
                    }
                }
            }

            /// <summary>
            /// Returns index of subarray in provided two-dimensional array.
            /// </summary>
            public readonly int Index;

            /// <summary>
            /// Constructs a reference to X dimension subarray of two-dimensional array with
            /// specified index. 
            /// <para>
            /// Note: Changes done in provided array are reflected in this instance which may 
            /// result in unexpected behaviour.<br/> In example array dimension change may cause
            /// this object's index no longer valid, which results in exception during enumeration.
            /// <br/> To avoid such situations it is best to reconstruct this object on array
            /// dimension alteration.
            /// </para>
            /// <para>Exceptions:</para>
            /// <para>
            /// <see cref="ArgumentOutOfRangeException"></see>: Index is out of bounds of the X 
            /// dimension.
            /// </para>
            /// </summary>
            /// <param name="array">An array from which this instance uses reference.</param>
            /// <param name="index">
            /// A zero-based index that determines which subarray is to take.
            /// </param>
            public ElementsOfX(Array2D<T> array, int index)
            {
                if ((uint)index >= array.X)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), "Index argument is out of bounds of the X dimension.");
                }

                this.array = array;
                this.Index = index;
            }

            /// <summary>
            /// Returns an enumerator for X dimension elements of specified index;
            /// </summary>
            /// <returns></returns>
            public IEnumerator<T> GetEnumerator()
            {
                for (int i = 0; i < Length; i++)
                {
                    yield return this[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
