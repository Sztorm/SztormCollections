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

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents an interface which is alternative to <see cref="System.Predicate{T}"/> but
    ///     if implemented by a struct type and constrained in generic method result in direct call
    ///     instead of virtual and avoids the garbage collector work.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPredicate<in T>
    {
        /// <summary>
        ///     Represents the method that defines a set of criteria and determines whether the
        ///     specified object meets those criteria.
        /// </summary>
        /// <param name="obj">
        ///     The object to compare against the criteria defined within the implemented method.
        /// </param>
        /// <returns></returns>
        bool Invoke(T obj);
    }
}