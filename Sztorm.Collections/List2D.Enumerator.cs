using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    public sealed partial class List2D<T>
    {
        /// <summary>
        ///     Enumerates the items of a <see cref="List2D{T}"/> row by row from the (0, 0)
        ///     position.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public struct Enumerator : IEnumerator<T>
        {
            private readonly List2D<T> list;
            private readonly T[] items;
            private readonly int version;
            private readonly int cols;
            private readonly int gapPerRow;
            private readonly int indexAfterEnd;
            private T current;
            private int index1D;
            private int colIndex;

            /// <summary>
            ///    Returns the item at the current position of the enumerator.
            /// </summary>
            public T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => current;
            }

            object IEnumerator.Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => current;
            }

            internal Enumerator(List2D<T> list)
            {
                this.list = list;
                items = list.items;
                version = list.version;
                cols = list.bounds.Columns;
                gapPerRow = list.capacity.Columns - cols;
                indexAfterEnd = list.bounds.Rows * (cols + gapPerRow) - gapPerRow;
                current = default;
                index1D = 0;
                colIndex = 0;
            }

            /// <summary>
            ///     Releases all resources used by this instance.<br/>
            ///     (This instance does not use any unmanaged resources, this method is implemented
            ///     to fulfill the <see cref="IEnumerator{T}"/> contract.)
            /// </summary>
            public void Dispose() {}

            /// <summary>
            ///     Advances the enumerator to the next element of the <see cref="List2D{T}"/> and
            ///     returns <see langword="true"/> if the advancement was successfull,
            ///     <see langword="false"/> otherwise.
            ///     <para>
            ///         Exceptions:<br/>
            ///         <see cref="InvalidOperationException"/>: Enumerator cannot be used upon
            ///         list modification.
            ///     </para>
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                if (list.version != version)
                {
                    throw new InvalidOperationException(
                        "Enumerator cannot be used upon list modification.");
                }
                for (; index1D < indexAfterEnd; index1D += gapPerRow, colIndex = 0)
                {
                    if (colIndex < cols)
                    {
                        current = items[index1D];
                        colIndex++;
                        index1D++;
                        return true;
                    }
                }
                return false;
            }

            /// <summary>
            ///     Sets the enumerator to its initial position, which is before the first element
            ///     in the collection.
            /// </summary>
            public void Reset()
            {
                current = default;
                index1D = 0;
                colIndex = 0;
            }
        }
    }
}
