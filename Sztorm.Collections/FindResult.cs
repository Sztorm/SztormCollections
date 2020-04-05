﻿using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represent result of finding an item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct FindResult<T> : IEquatable<FindResult<T>>
    {
        private readonly T item;
        private readonly bool isFound;

        /// <summary>
        ///     Determines whether desired item is found.
        /// </summary>
        public bool IsFound
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => isFound;
        }

        /// <summary>
        ///     Returns current instance underlying item if it has been found.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="InvalidOperationException"/> Item must be found to be able to use
        ///         it.
        ///     </para>
        /// </summary>
        public T Item
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (isFound)
                {
                    return item;
                }
                throw new InvalidOperationException("Item must be found to be able to use it.");
            }
        }

        /// <summary>
        ///     Returns current instance underlying item or its default value if it has not been
        ///     found.
        /// </summary>
        public T ItemOrDefault
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => item;
        }

        /// <summary>
        ///     Returns current instance underlying item or specified default value if the item has
        ///     not been found.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetItemOrDefault(T defaultValue) => isFound ? item : defaultValue;

        /// <summary>
        ///     Initializes a new instance of <see cref="FindResult{T}"/> indicating that the item
        ///     has been found.
        /// </summary>
        /// <param name="item">Found item.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FindResult(T item)
        {
            this.item = item;
            this.isFound = true;
        }

        /// <summary>
        ///     Returns a <see cref="Nullable{T}"/> representation of this instance.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public U? ToNullable<U>() where U : struct, T => (U)item;

        /// <summary>
        ///     Returns a <see cref="string"/> representation of current instance underlying item
        ///     if it has been found.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="InvalidOperationException"/> Item must be found to be able to use
        ///         it.
        ///     </para>    
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (!isFound)
            {
                throw new InvalidOperationException("Item must be found to be able to use it.");
            }
            return item.ToString();
        }

        /// <summary>
        ///     Returns the hashcode for this instance.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => isFound ? item.GetHashCode() * 13 : 0;

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified
        ///     <see cref="FindResult{T}"/> value.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            if (other == null || !(other is FindResult<T>))
            {
                return false;
            }
            return Equals((FindResult<T>)other);
        }

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified
        ///     <see cref="FindResult{T}"/> value.<br/>
        ///     Use <see cref="Equals{U}(FindResult{U})"/> if the parameter underlying value is
        ///     <see cref="IEquatable{T}"/> to avoid boxing. 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(FindResult<T> other) 
            => item.Equals(other.item) && this.isFound == other.isFound;

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified
        ///     <see cref="FindResult{T}"/> value.
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals<U>(FindResult<U> other) where U : T, IEquatable<T> 
            => other.item.Equals(item) && this.isFound == other.isFound;

        /// <summary>
        ///     Casts current instance to its underlying item or default value if it has not been
        ///     found.
        /// </summary>
        /// <param name="result"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T(FindResult<T> result) => result.item;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(FindResult<T> left, FindResult<T> right)
            => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(FindResult<T> left, FindResult<T> right)
            => !left.Equals(right);
    }
}
