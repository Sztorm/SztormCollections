using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents a box to put an item in. It may be used for internal purposes like
    ///     arguments that are not checked by method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal struct Box<T>
    {
        public T Item;

        /// <summary>
        ///     Constructs a new <see cref="Box{T}"/> instance which will store item passed in.
        /// </summary>
        /// <param name="item"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Box(T item) => Item = item;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T(Box<T> box) => box.Item;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Box<T>(T item) => new Box<T>(item);
    }
}