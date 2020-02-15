using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

namespace Sztorm.Collections
{
    partial class Array2D<T>
    {
        /// <summary>
        ///     Represents specific row of two-dimensional array.
        /// </summary>
        public readonly struct Row : IEnumerable<T>
        {
            private readonly Array2D<T> array;

            /// <summary>
            ///     Returns index of row in provided two-dimensional array.
            /// </summary>
            public readonly int Index;

            /// <summary>
            ///     Returns count of elements stored int this row.
            /// </summary>
            public int Count
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => array.Columns;
            }

            /// <summary>
            ///     Returns an element stored at given index.
            ///     <para>
            ///         Exceptions:<br/>
            ///         <see cref="IndexOutOfRangeException"/>: Index is out of row bounds.
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
            ///     Returns an element stored at given index.<br/>
            ///     Argument is not checked. For internal purposes only.
            /// </summary>
            /// <param name="index">Range: ([0, <see cref="Count"/>).</param>
            /// <returns></returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal ref T NotCheckedGetItem(int index)
            {
                return ref array.NotCheckedGetItem(Index, index);
            }

            /// <summary>
            ///     Constructs a reference to index-specified row of two-dimensional array.<br/>
            ///     Changes done in provided array are reflected in this instance.
            ///     <para>
            ///         Exceptions:<br/>
            ///         <see cref="ArgumentOutOfRangeException"/>: Index is out of boundaries of
            ///         the row count.
            ///     </para>
            /// </summary>
            /// <param name="array">An array from which this instance uses reference.</param>
            /// <param name="index">
            ///     A zero-based index that determines which row is to take.<br/>
            ///     Range: [0, <see cref="Array2D{T}.Rows"/>).
            /// </param>
            internal Row(Array2D<T> array, int index)
            {
                if ((uint)index >= array.Rows)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(index), "Index is out of boundaries of the row count.");
                }
                this.array = array;
                this.Index = index;
            }

            /// <summary>
            ///     Assigns the given value to each element of this instance.
            ///     Changes are reflected in provided array.
            /// </summary>
            /// <param name="value"></param>
            public void FillWith(T value)
            {
                for (int i = 0, length = Count; i < length; i++)
                {
                    NotCheckedGetItem(i) = value;
                }
            }

            /// <summary>
            ///     Inverts the order of the elements in this instance.
            ///     Changes are reflected in provided array.
            /// </summary>
            public void Reverse()
            {
                int lastIndex = Count - 1;
                int halfLength = Count / 2;

                for (int i = 0; i < halfLength; i++)
                {
                    T item = NotCheckedGetItem(i);
                    NotCheckedGetItem(i) = NotCheckedGetItem(lastIndex - i);
                    NotCheckedGetItem(lastIndex - i) = item;
                }
            }

            /// <summary>
            ///     Returns an <see cref="IEnumerator{T}"/> for the <see cref="Row"/>.
            /// </summary>
            /// <returns></returns>
            public IEnumerator<T> GetEnumerator()
            {
                for (int i = 0, length = Count; i < length; i++)
                {
                    yield return NotCheckedGetItem(i);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <summary>
            ///     Creates a <typeparamref name = "T"/>[] from this <see cref="Row"/> intance.
            /// </summary>
            /// <returns></returns>
            public T[] ToArray()
            {
                int length = Count;
                var result = new T[length];

                for (int i = 0; i < length; i++)
                {
                    result[i] = NotCheckedGetItem(i);
                }
                return result;
            }
        }
    }
}
