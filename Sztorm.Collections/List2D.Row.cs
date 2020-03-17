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
    partial class List2D<T>
    {
        /// <summary>
        ///     Represents specific row of <see cref="List2D{T}"/>.
        /// </summary>
        public readonly struct Row : IEnumerable<T>
        {
            private readonly List2D<T> list;

            /// <summary>
            ///     Returns index of row in provided two-dimensional list.
            /// </summary>
            public readonly int Index;

            /// <summary>
            ///     Returns a count of items stored in this row.
            /// </summary>
            public int Count
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => list.Columns;
            }

            /// <summary>
            ///     Returns an element stored at given index.<br/>
            ///     Argument is not checked. For internal purposes only.
            /// </summary>
            /// <param name="index">Range: ([0, <see cref="Count"/>).</param>
            /// <returns></returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal ref T GetItemInternal(int index) => ref list.GetItemInternal(Index, index);

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
                        return ref list[Index, index];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new IndexOutOfRangeException(
                            "Index is out of row bounds.");
                    }
                }
            }

            /// <summary>
            ///     Constructs a reference to index-specified row of two-dimensional list.<br/>
            ///     Changes done in provided list are reflected in this instance.
            ///     <para>
            ///         Exceptions:<br/>
            ///         <see cref="ArgumentOutOfRangeException"/>: Index is out of boundaries of
            ///         the row count.
            ///     </para>
            /// </summary>
            /// <param name="list">A list from which this instance uses reference.</param>
            /// <param name="index">
            ///     A zero-based index that determines which row is to take.<br/>
            ///     Range: [0, <see cref="List2D{T}.Rows"/>).
            /// </param>
            internal Row(List2D<T> list, int index)
            {
                if ((uint)index >= list.Rows)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(index), "Index is out of boundaries of the row count.");
                }
                this.list = list;
                this.Index = index;
            }

            /// <summary>
            ///     Returns an <see cref="IEnumerator{T}"/> for the <see cref="Row"/>.
            /// </summary>
            /// <returns></returns>
            public IEnumerator<T> GetEnumerator()
            {
                for (int i = 0, length = Count; i < length; i++)
                {
                    yield return GetItemInternal(i);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            /// <summary>
            ///     Assigns the given value to each element of this instance.
            ///     Changes are reflected in provided list.
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
            ///     Changes are reflected in provided list.
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
            ///     Creates a <typeparamref name = "T"/>[] from this <see cref="Row"/> instance.
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
