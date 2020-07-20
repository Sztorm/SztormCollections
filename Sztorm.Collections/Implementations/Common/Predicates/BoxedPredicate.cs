using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents a predicate which uses another predicate to determine criteria.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal readonly struct BoxedPredicate<T> : IPredicate<T>
    {
        private readonly Predicate<T> predicate;

        /// <summary>
        ///     The predicate which defines specific criteria.
        /// </summary>
        public Predicate<T> InnerPredicate
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => predicate;
        }

        /// <summary>
        ///     Constructs a predicate that takes another predicate which may be used to determine
        ///     criteria.
        /// </summary>
        /// <param name="predicate">The predicate which may be used to determine criteria.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoxedPredicate(Predicate<T> predicate) => this.predicate = predicate;

        /// <summary>
        ///     Returns a value indicating whether specified object meets criteria defined by
        ///     <see cref="InnerPredicate"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(T obj) => predicate(obj);
    }
}
