using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    partial class Array2D<T>
    {
        /// <summary>
        /// Represents specific row of two-dimensional array.
        /// </summary>
        public readonly struct Row : IEnumerable<T>
        {
            private readonly Array2D<T> array;

            /// <summary>
            /// Returns index of row in provided two-dimensional array.
            /// </summary>
            public readonly int Index;

            /// <summary>
            /// Returns count of elements stored int this row.
            /// </summary>
            public int Count => array.Rows;

            /// <summary>
            /// Returns an element stored at given index.
            /// <para>
            /// Throws <see cref="IndexOutOfRangeException"></see> if index is out of row bounds.
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
                    catch (IndexOutOfRangeException)
                    {
                        throw new IndexOutOfRangeException(
                            "Index is out of row bounds.");
                    }
                }
            }

            /// <summary>
            /// Constructs a reference to index-specified row of two-dimensional array.
            /// <para>
            /// Note: Changes done in provided array are reflected in this instance which may 
            /// result in unexpected behaviour.<br/> In example array size change may cause this
            /// object's index no longer valid, which results in exception during enumeration.<br/>
            /// To avoid such situations it is best to reconstruct this object on array dimension
            /// alteration.
            /// </para>
            /// <para>Exceptions:</para>
            /// <para>
            /// <see cref="ArgumentOutOfRangeException"></see>: Index is out of bounds of the row.
            /// </para>
            /// </summary>
            /// <param name="array">An array from which this instance uses reference.</param>
            /// <param name="index">
            /// A zero-based index that determines which row is to take.
            /// </param>
            internal Row(Array2D<T> array, int index)
            {
                if ((uint)index >= array.Rows)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), "Index argument is out of bounds of the row.");
                }

                this.array = array;
                this.Index = index;
            }

            /// <summary>
            /// Returns an <see cref="IEnumerator{T}"></see> for the <see cref="Row"></see>.
            /// </summary>
            /// <returns></returns>
            public IEnumerator<T> GetEnumerator()
            {
                for (int i = 0, length = Count; i < length; i++)
                {
                    yield return this[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
