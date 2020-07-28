// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //
// File is auto-generated. Do not modify as changes may be overwritten.
// If you want to modify this file, edit template file with the same name and .tt extension.
// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections.Extensions
{
    public static partial class ReadOnlyRectangularCollectionExtensions
    {
        /// <summary>
        ///     Returns a row at specified index. Indexing start at zero.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Index is out of boundaries of the row
        ///         count.
        ///     </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReadOnlyRectCollection">
        ///     <typeparamref name="TReadOnlyRectCollection"/> is 
        ///     <see cref="IReadOnlyRectangularCollection{T}"/>
        /// </typeparam>
        /// <param name="index">A zero-based index that determines which row is to take.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyRow<T, TReadOnlyRectCollection> GetRow<T, TReadOnlyRectCollection>(
            this TReadOnlyRectCollection source, int index)
            where TReadOnlyRectCollection : IReadOnlyRectangularCollection<T>
        {
            try
            {
                return new ReadOnlyRow<T, TReadOnlyRectCollection>(source, index);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }
    }
}
