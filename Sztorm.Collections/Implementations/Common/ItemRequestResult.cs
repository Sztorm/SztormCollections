using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represent result of requesting an item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct ItemRequestResult<T> : IEquatable<ItemRequestResult<T>>
    {
        private readonly T item;
        private readonly bool isSuccess;

        /// <summary>
        ///     Represents a failed request. The item has not been delivered.
        /// </summary>
        public static readonly ItemRequestResult<T> Fail = new ItemRequestResult<T>();

        /// <summary>
        ///     Determines whether requested item is delivered.
        /// </summary>
        public bool IsSuccess
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => isSuccess;
        }

        /// <summary>
        ///     Returns current instance underlying item if it has been delivered.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="InvalidOperationException"/> Item must be delivered to be able to
        ///         use it.
        ///     </para>
        /// </summary>
        public T Item
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (isSuccess)
                {
                    return item;
                }
                throw new InvalidOperationException(
                    "Item must be delivered to be able to use it.");
            }
        }

        /// <summary>
        ///     Returns current instance underlying item or its default value if it has not been
        ///     delivered.
        /// </summary>
        public T ItemOrDefault
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => item;
        }

        /// <summary>
        ///     Returns current instance underlying item or specified default value if the item has
        ///     not been delivered.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetItemOrDefault(T defaultValue) => isSuccess ? item : defaultValue;

        /// <summary>
        ///     Initializes a new instance of <see cref="ItemRequestResult{T}"/> indicating that the item
        ///     has been delivered.
        /// </summary>
        /// <param name="item">Delivered item.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemRequestResult(T item)
        {
            this.item = item;
            this.isSuccess = true;
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
        ///     if it has been delivered.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="InvalidOperationException"/> Item must be delivered to be able to
        ///         use it.
        ///     </para>    
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (!isSuccess)
            {
                throw new InvalidOperationException(
                    "Item must be delivered to be able to use it.");
            }
            return item.ToString();
        }

        /// <summary>
        ///     Returns the hashcode for this instance.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => isSuccess ? item.GetHashCode() * 13 : 0;

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified
        ///     <see cref="ItemRequestResult{T}"/> value.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            if (other == null || !(other is ItemRequestResult<T>))
            {
                return false;
            }
            return Equals((ItemRequestResult<T>)other);
        }

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified
        ///     <see cref="ItemRequestResult{T}"/> value.<br/>
        ///     Use <see cref="Equals{U}(ItemRequestResult{U})"/> if the parameter underlying value is
        ///     <see cref="IEquatable{T}"/> to avoid boxing. 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ItemRequestResult<T> other) 
            => item.Equals(other.item) && this.isSuccess == other.isSuccess;

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified
        ///     <see cref="ItemRequestResult{T}"/> value.
        /// </summary>
        /// <typeparam name="U">
        ///     <typeparamref name = "U"/> is <see cref="IEquatable{T}"/> and
        ///     <typeparamref name = "T"/>
        /// </typeparam>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals<U>(ItemRequestResult<U> other) where U : T, IEquatable<T> 
            => other.item.Equals(item) && this.isSuccess == other.isSuccess;

        /// <summary>
        ///     Casts current instance to its underlying item or default value if it has not been
        ///     delivered.
        /// </summary>
        /// <param name="result"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T(ItemRequestResult<T> result) => result.item;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ItemRequestResult<T> left, ItemRequestResult<T> right)
            => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ItemRequestResult<T> left, ItemRequestResult<T> right)
            => !left.Equals(right);
    }
}
