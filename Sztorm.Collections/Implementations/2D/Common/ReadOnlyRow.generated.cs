
// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //
// File is auto-generated. Do not modify as changes may be overwritten.
// If you want to modify this file, edit template file with the same name and .tt extension.
// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents specific row of read-only rectangular collection.
    /// </summary>
    public readonly partial struct ReadOnlyRow<T, TCollection> :
        IEnumerable<T>, IReadOnlyIndexable<T>
        where TCollection : IReadOnlyRectangularCollection<T>
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
            get => collection.Boundaries.Length2;
        }

        /// <summary>
        ///     Returns value indicating whether specified index exists in this instance.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidIndex(int index) => collection.IsValidIndex(new Index2D(Index, index));

        /// <summary>
        ///     Gets an item stored at given index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="IndexOutOfRangeException"/>: Index is out of row bounds.
        ///     </para>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (!IsValidIndex(index))
                {
                    throw new IndexOutOfRangeException("Index is out of row bounds.");
                }
                return collection[new Index2D(Index, index)];
            }
        }

        /// <summary>
        ///     Constructs index-specified indexable read-only row of indexable read-only
        ///     rectangular collection.<br/>
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
        public ReadOnlyRow(TCollection collection, int index)
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
        ///     Returns an <see cref="IEnumerator{T}"/> for the <see cref="ReadOnlyRow{T, TCollection}"/>.
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
        ///     Creates a <typeparamref name = "T"/>[] from this <see cref="ReadOnlyRow{T, TCollection}"/> instance.
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
