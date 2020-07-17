using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents specific column of reference-indexable two-dimensional rectangular
    ///     collection.
    /// </summary>
    public readonly struct RefColumn<T, TCollection> : IEnumerable<T>, IRefIndexable<T>
        where TCollection : IRefRectangularCollection<T>
    {
        private readonly TCollection collection;

        /// <summary>
        ///     Returns index of column in provided <typeparamref name="TCollection"/>.
        /// </summary>
        public int Index { get; }

        /// <summary>
        ///     Returns number of items stored in this column.
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => collection.Boundaries.Length1;
        }

        /// <summary>
        ///     Returns value indicating whether specified index exists in this instance.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidIndex(int index) => collection.IsValidIndex(new Index2D(index, Index));

        /// <summary>
        ///     Returns an item stored at given index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="IndexOutOfRangeException"/>: Index is out of column bounds.
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
                    return ref collection[new Index2D(index, Index)];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException(
                        "Index is out of column bounds.");
                }
            }
        }

        /// <summary>
        ///     Constructs index-specified reference-indexable column of two-dimensional
        ///     reference-indexable rectangular collection.<br/>
        ///     Changes done in provided <typeparamref name="TCollection"/> are reflected in this
        ///     instance.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Index is out of boundaries of
        ///         the column count.
        ///     </para>
        /// </summary>
        /// <param name="collection">A collection from which this instance uses reference.</param>
        /// <param name="index"> An index that determines which column is to take.</param>
        public RefColumn(TCollection collection, int index)
        {
            if (!collection.IsValidIndex(new Index2D(0, index)))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(index), "Index is out of boundaries of the column count.");
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
        ///     Returns an <see cref="IEnumerator{T}"/> for the <see cref="RefColumn{T, TCollection}"/>.
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
        ///     Creates a <typeparamref name = "T"/>[] from this <see cref="RefColumn{T, TCollection}"/> instance.
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