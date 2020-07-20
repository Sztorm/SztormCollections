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
    ///     Represents specific column of rectangular collection.
    /// </summary>
    public readonly partial struct Column<T, TCollection> :
        IEnumerable<T>, IIndexable<T>
        where TCollection : IRectangularCollection<T>
    {
        private readonly TCollection collection;

        /// <summary>
        ///     Returns index of column in provided <typeparamref name="TCollection"/>.
        /// </summary>
        public int Index { get; }

        /// <summary>
        ///     Returns number of elements stored in this column.
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
        ///     Gets or sets an item stored at given index.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="IndexOutOfRangeException"/>: Index is out of column bounds.
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
                    throw new IndexOutOfRangeException("Index is out of column bounds.");
                }
                return collection[new Index2D(index, Index)];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (!IsValidIndex(index))
                {
                    throw new IndexOutOfRangeException("Index is out of column bounds.");
                }
                collection[new Index2D(index, Index)] = value;
            }
        }

        /// <summary>
        ///     Constructs index-specified indexable column of indexable rectangular collection.<br/>
        ///     Changes done in provided <typeparamref name="TCollection"/> are reflected in this
        ///     instance.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Index is out of boundaries of the
        ///         column count.
        ///     </para>
        /// </summary>
        /// <param name="collection">A collection from which this instance uses reference.</param>
        /// <param name="index">An index that determines which column is to take.</param>
        public Column(TCollection collection, int index)
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
        ///     Returns an <see cref="IEnumerator{T}"/> for the <see cref="Column{T, TCollection}"/>.
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
        ///     Creates a <typeparamref name = "T"/>[] from this <see cref="Column{T, TCollection}"/> instance.
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
