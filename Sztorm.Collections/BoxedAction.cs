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

using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents an action which uses another action to define specific operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal readonly struct BoxedAction<T> : IAction<T>
    {
        private readonly Action<T> action;

        /// <summary>
        ///     The action which defines specific operation.
        /// </summary>
        public Action<T> InnerAction
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => action;
        }

        /// <summary>
        ///     Constructs an action that takes another action to define specific operation.
        /// </summary>
        /// <param name="Action">The Action defining specific operation.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoxedAction(Action<T> action) => this.action = action;

        /// <summary>
        ///     Invokes an action defined by <see cref="InnerAction"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T obj) => action(obj);
    }
}
