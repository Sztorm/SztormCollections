using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents a predicate which determines whether any <typeparamref name="TComparable"/>
    ///     object equals <see cref="InnerObject"/>
    /// </summary>
    /// <typeparam name="TComparable">
    ///     <typeparamref name = "TComparable"/> is <see cref="IComparable{T}"/>
    /// </typeparam>
    public readonly struct EqualsComparablePredicate<TComparable> : IPredicate<TComparable>
        where TComparable : IComparable<TComparable>
    {
        private readonly TComparable innerObj;

        /// <summary>
        ///     The object which may be used in comparisons.
        /// </summary>
        public TComparable InnerObject
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => innerObj;
        }

        /// <summary>
        ///     Constructs a predicate that takes an object which may be used to determine whether
        ///     any other <typeparamref name="TComparable"/> object equals object passed in
        ///     constructor.
        /// </summary>
        /// <param name="obj">The object which may be used in comparisons.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EqualsComparablePredicate(TComparable obj) => innerObj = obj;

        /// <summary>
        ///     Returns a value indicating whether <paramref name="other"/> equals
        ///     <see cref="InnerObject"/>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(TComparable other) => other.CompareTo(innerObj) == 0;
    }
}
