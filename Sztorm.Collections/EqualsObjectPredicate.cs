using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents a predicate which determines whether any <typeparamref name="T"/> object
    ///     equals <see cref="InnerObject"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct EqualsObjectPredicate<T> : IPredicate<T>
    {
        private static readonly bool TIsValueType = typeof(T).IsValueType;

        private readonly object innerObj;

        /// <summary>
        ///     The already boxed object which may be used in comparisons.
        /// </summary>
        public object InnerObject
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => innerObj;
        }

        /// <summary>
        ///     Constructs a predicate that takes an object which may be used to determine whether
        ///     any other <typeparamref name="T"/> object equals the object passed in the constructor.
        /// </summary>
        /// <param name="obj">The already boxed object which may be used in comparisons.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EqualsObjectPredicate(object obj) => this.innerObj = obj;

        /// <summary>
        ///     Returns a value indicating whether <paramref name="other"/> equals
        ///     <see cref="InnerObject"/>
        /// </summary>
        /// <param name="other">
        ///     The object to be compared with <see cref="InnerObject"/>.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(T other)
        {
            if (TIsValueType)
            {
                return other.Equals(innerObj);
            }
            if (innerObj == null || other == null)
            {
                return ReferenceEquals(other, innerObj);
            }
            return other.Equals(innerObj);
        }
    }
}
