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
    partial class Array2D<T>
    {
        /// <summary>
        ///     Represents specific column of two-dimensional array.
        /// </summary>
        public readonly struct Column : IEnumerable<T>
        {
            private readonly Array2D<T> array;

            /// <summary>
            ///     Returns index of column in provided two-dimensional array.
            /// </summary>
            public readonly int Index;

            /// <summary>
            ///     Returns count of elements stored in this column.
            /// </summary>
            public int Count
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => array.Rows;
            }

            /// <summary>
            ///     Returns an element stored at given index.
            ///     <para>
            ///         Exceptions:<br/>
            ///         <see cref="IndexOutOfRangeException"/>: Index is out of column bounds.
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
                        return ref array[index, Index];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new IndexOutOfRangeException(
                            "Index is out of column bounds.");
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
            internal ref T GetItemInternal(int index) => ref array.GetItemInternal(index, Index);

            /// <summary>
            ///     Constructs a reference to index-specified column of two-dimensional array.<br/>
            ///     Changes done in provided array are reflected in this instance.
            ///     <para>
            ///         Exceptions:<br/>
            ///         <see cref="ArgumentOutOfRangeException"/>: Index is out of boundaries of
            ///         the column count.
            ///     </para>
            /// </summary>
            /// <param name="array">An array from which this instance uses reference.</param>
            /// <param name="index">
            ///     A zero-based index that determines which column is to take.<br/>
            ///     Range: [0, <see cref="Array2D{T}.Columns"/>).
            /// </param>
            internal Column(Array2D<T> array, int index)
            {
                if ((uint)index >= array.Columns)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(index), "Index is out of boundaries of the column count.");
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
                    GetItemInternal(i) = value;
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
                    T item = GetItemInternal(i);
                    GetItemInternal(i) = GetItemInternal(lastIndex - i);
                    GetItemInternal(lastIndex - i) = item;
                }
            }

            /// <summary>
            ///     Returns an <see cref="IEnumerator{T}"/> for the <see cref="Column"/>.
            /// </summary>
            /// <returns></returns>
            public IEnumerator<T> GetEnumerator()
            {
                for (int i = 0, length = Count; i < length; i++)
                {
                    yield return this[i];
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            /// <summary>
            ///     Creates a <typeparamref name = "T"/>[] from this <see cref="Column"/> instance.
            /// </summary>
            /// <returns></returns>
            public T[] ToArray()
            {
                int length = Count;
                var result = new T[length];

                for (int i = 0; i < length; i++)
                {
                    result[i] = GetItemInternal(i);
                }
                return result;
            }
        }
    }
}
