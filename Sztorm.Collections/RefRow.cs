using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents specific row of reference-indexable two-dimensional rectangular collection.
    /// </summary>
    public readonly struct RefRow<T, TCollection> : IEnumerable<T>, IRefIndexable<T>
        where TCollection : IRefRectangularCollection<T>
    {
        private readonly TCollection collection;

        /// <summary>
        ///     Returns index of row in provided <typeparamref name="TCollection"/>.
        /// </summary>
        public int Index { get; }

        /// <summary>
        ///     Returns number of elements stored in this row.
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => collection.Length2;
        }

        /// <summary>
        ///     Returns value indicating whether specified index exists in this instance.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidIndex(int index) => collection.IsValidIndex(new Index2D(Index, index));

        /// <summary>
        ///     Returns an item stored at given index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="IndexOutOfRangeException"/>: Index is out of row bounds.
        ///     </para>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                try
                {
                    return ref collection[new Index2D(Index, index)];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException(
                        "Index is out of row bounds.");
                }
            }
        }

        /// <summary>
        ///     Constructs index-specified reference-indexable row of two-dimensional
        ///     reference-indexable rectangular collection.<br/>
        ///     Changes done in provided <typeparamref name="TCollection"/> are reflected in this
        ///     instance.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Index is out of boundaries of the
        ///         row count.
        ///     </para>
        /// </summary>
        /// <param name="collection">A collection from which this instance uses reference.</param>
        /// <param name="index">An index that determines which row is to take.</param>
        public RefRow(TCollection collection, int index)
        {
            if (!collection.IsValidIndex(new Index2D(index, 0)))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(index), "Index is out of boundaries of the row count.");
            }
            this.collection = collection;
            this.Index = index;
        }

        /// <summary>
        ///     Assigns the given value to each element of this instance.
        ///     Changes are reflected in provided <typeparamref name="TCollection"/>.
        /// </summary>
        /// <param name="value"></param>
        public void FillWith(T value)
        {
            for (int i = 0, length = Count; i < length; i++)
            {
                this[i] = value;
            }
        }

        /// <summary>
        ///     Inverts the order of the elements in this instance.
        ///     Changes are reflected in provided <typeparamref name="TCollection"/>.
        /// </summary>
        public void Reverse()
        {
            int lastIndex = Count - 1;
            int halfLength = Count / 2;

            for (int i = 0; i < halfLength; i++)
            {
                T item = this[i];
                this[i] = this[lastIndex - i];
                this[lastIndex - i] = item;
            }
        }

        /// <summary>
        ///     Returns an <see cref="IEnumerator{T}"/> for the <see cref="RefRow{T, TCollection}"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0, length = Count; i < length; i++)
            {
                yield return this[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        ///     Creates a <typeparamref name = "T"/>[] from this <see cref="RefRow{T, TCollection}"/> instance.
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            int length = Count;
            var result = new T[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = this[i];
            }
            return result;
        }
    }
}