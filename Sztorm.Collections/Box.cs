/*
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

using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents a box to put an item in. It may be used for internal purposes like
    ///     arguments that are not checked by method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal struct Box<T>
    {
        public T Item;

        /// <summary>
        ///     Constructs a new <see cref="Box{T}"/> instance which will store item passed in.
        /// </summary>
        /// <param name="item"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Box(T item) => Item = item;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T(Box<T> box) => box.Item;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Box<T>(T item) => new Box<T>(item);
    }
}