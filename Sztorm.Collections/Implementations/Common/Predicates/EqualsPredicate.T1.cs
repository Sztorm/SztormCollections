using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents a predicate which determines whether any <typeparamref name="TEquatable"/>
    ///     object equals <see cref="InnerObject"/>
    /// </summary>
    /// <typeparam name="TEquatable">
    ///     <typeparamref name = "TEquatable"/> is <see cref="IEquatable{T}"/>
    /// </typeparam>
    public readonly struct EqualsPredicate<TEquatable> : IPredicate<TEquatable> 
        where TEquatable : IEquatable<TEquatable>
    {
        private readonly TEquatable innerObj;

        /// <summary>
        ///     The object which may be used in comparisons.
        /// </summary>
        public TEquatable InnerObject
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => innerObj;
        }

        /// <summary>
        ///     Constructs a predicate that takes an object which may be used to determine whether
        ///     any other <typeparamref name="TEquatable"/> object equals object passed in
        ///     constructor.
        /// </summary>
        /// <param name="obj">The object which may be used in comparisons.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EqualsPredicate(TEquatable obj) => innerObj = obj;

        /// <summary>
        ///     Returns a value indicating whether <paramref name="other"/> equals
        ///     <see cref="InnerObject"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(TEquatable other) => other.Equals(innerObj);
    }
}
