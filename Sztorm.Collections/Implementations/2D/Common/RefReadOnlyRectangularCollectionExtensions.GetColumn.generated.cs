// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //
// File is auto-generated. Do not modify as changes may be overwritten.
// If you want to modify this file, edit template file with the same name and .tt extension.
// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections.Extensions
{
    public static partial class RefReadOnlyRectangularCollectionExtensions
    {
        /// <summary>
        ///     Returns a column at specified index. Indexing start at zero.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: Index is out of boundaries of the
        ///         column count.
        ///     </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRefReadOnlyRectCollection">
        ///     <typeparamref name="TRefReadOnlyRectCollection"/> is 
        ///     <see cref="IRefReadOnlyRectangularCollection{T}"/>
        /// </typeparam>
        /// <param name="index">A zero-based index that determines which column is to take.</param> 
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RefReadOnlyColumn<T, TRefReadOnlyRectCollection> GetColumn<T, TRefReadOnlyRectCollection>(
            this TRefReadOnlyRectCollection source, int index)
            where TRefReadOnlyRectCollection : IRefReadOnlyRectangularCollection<T>
        {
            try
            {
                return new RefReadOnlyColumn<T, TRefReadOnlyRectCollection>(source, index);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }
    }
}
