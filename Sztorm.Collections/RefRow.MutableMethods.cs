using System.Collections.Generic;

namespace Sztorm.Collections
{
    public readonly partial struct RefRow<T, TCollection> :
        IEnumerable<T>, IRefIndexable<T>
        where TCollection : IRefRectangularCollection<T>
    {
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
    }
}
