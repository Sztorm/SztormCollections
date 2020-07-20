using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents a predicate which determines whether any <typeparamref name="T"/> object
    ///     is less than <see cref="InnerObject"/>.
    /// </summary>
    /// <typeparam name="T"><typeparamref name = "T"/> is <see cref="IComparable{T}"/></typeparam>
    public readonly struct LessThanPredicate<T> : IPredicate<T> where T : IComparable<T>
    {
        private readonly T innerObj;

        /// <summary>
        ///     The object which may be used in comparisons.
        /// </summary>
        public T InnerObject
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => innerObj;
        }

        /// <summary>
        ///     Constructs a predicate that takes an object which may be used to determine
        ///     whether any other <typeparamref name="T"/> object is less than object passed in
        ///     constructor.
        /// </summary>
        /// <param name="obj">The object which may be used in comparisons.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LessThanPredicate(T obj) => innerObj = obj;

        /// <summary>
        ///     Returns a value indicating whether <paramref name="other"/> is less than
        ///     <see cref="InnerObject"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(T other) => other.CompareTo(innerObj) < 0;
    }
}
