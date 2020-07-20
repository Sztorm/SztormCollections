using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents a predicate which determines whether <see cref="InnerObject"/> equals any
    ///     other <typeparamref name="T"/> object.
    /// </summary>
    /// <typeparam name="TComparable">
    ///     <typeparamref name = "TComparable"/> is <see cref="IComparable{T}"/>
    /// </typeparam>
    /// <typeparam name="TOther">
    ///     <typeparamref name = "TOther"/> is type of any other object.
    /// </typeparam>
    public readonly struct EqualsComparablePredicate<TComparable, TOther> : IPredicate<TOther>
        where TComparable : IComparable<TOther>
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
        ///     Does a method call to trigger <see cref="NullReferenceException"/> if object is
        ///     null, then throws <see cref="ArgumentNullException"/>. This avoids unnecessary
        ///     boxing when argument is a value type.
        /// </summary>
        /// <param name="obj"></param>
        private static void ThrowExceptionIfArgumentIsNull(TComparable obj)
        {
            try
            {
                obj.GetHashCode();
            }
            catch (NullReferenceException)
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        ///     Constructs a predicate that takes an object which may be used to determine whether
        ///     object passed in constructor equals any other <typeparamref name="T"/> object.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="obj"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <param name="obj">The object which may be used in comparisons.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EqualsComparablePredicate(TComparable obj)
        {
            try
            {
                ThrowExceptionIfArgumentIsNull(obj);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(obj), "obj cannot be null.");
            }
            this.innerObj = obj;
        }

        /// <summary>
        ///     Returns a value indicating whether <see cref="InnerObject"/> equals 
        ///     <paramref name="other"/>
        /// </summary>
        /// <param name="other">
        ///     The other object that does not need to implement <see cref="IComparable{T}"/>
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(TOther other) => innerObj.CompareTo(other) == 0;
    }
}
