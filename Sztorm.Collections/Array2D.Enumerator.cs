﻿/*
 * MIT License
 * 
 * Copyright (c) 2020 Sztorm
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
    public sealed partial class Array2D<T>
    {
        /// <summary>
        ///     Enumerates the items of an <see cref="Array2D{T}"/> row by row from the (0, 0)
        ///     position.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public struct Enumerator : IEnumerator<T>
        {
            private readonly T[] items;
            private readonly int length;
            private int index1D;

            /// <summary>
            ///    Returns the item at the current position of the enumerator.
            /// </summary>
            public T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => items[index1D];
            }

            object IEnumerator.Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => items[index1D];
            }

            internal Enumerator(Array2D<T> array)
            {
                items = array.items;
                length = items.Length;
                index1D = -1;
            }

            /// <summary>
            ///     Releases all resources used by this instance.<br/>
            ///     (This instance does not use any unmanaged resources, this method is implemented
            ///     to fulfill the <see cref="IEnumerator{T}"/> contract.)
            /// </summary>
            public void Dispose() { }

            /// <summary>
            ///     Advances the enumerator to the next element of the <see cref="Array2D{T}"/> and
            ///     returns <see langword="true"/> if the advancement was successfull,
            ///     <see langword="false"/> otherwise.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                index1D++;

                if (index1D < length)
                {
                    return true;
                }
                return false;
            }

            /// <summary>
            ///     Sets the enumerator to its initial position, which is before the first element
            ///     in the collection.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Reset() => index1D = -1;
        }
    }
}
