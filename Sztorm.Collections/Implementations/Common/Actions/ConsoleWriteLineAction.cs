using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents an action which takes an object and writes a text representation of it,
    ///     followed by line terminator, to the standart output stream.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct ConsoleWriteLineAction<T> : IAction<T>
    {
        /// <summary>
        ///     Takes an object and writes a text representation of it, followed by line
        ///     terminator, to the standart output stream.
        /// </summary>
        /// <param name="obj"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T obj) => Console.WriteLine(obj);
    }
}
