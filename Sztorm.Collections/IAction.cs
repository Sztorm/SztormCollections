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

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents interface which defines a method that has a single parameter and does not
    ///     return a value.<br/>
    ///     This interface mimics <see cref="System.Action{T}"/> behavior. If the interface is
    ///     implemented by a <see langword="struct"/> and is used as constraint in generic method,
    ///     the implemented method call is direct and the garbage collector's work is avoided.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAction<in T>
    {
        /// <summary>
        ///     Represents a method that has a single parameter and does not return a value.
        /// </summary>
        /// <param name="obj"></param>
        void Invoke(T obj);
    }
}
