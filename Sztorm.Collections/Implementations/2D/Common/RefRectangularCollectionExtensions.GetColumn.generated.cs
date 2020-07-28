// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //
// File is auto-generated. Do not modify as changes may be overwritten.
// If you want to modify this file, edit template file with the same name and .tt extension.
// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections.Extensions
{
    public static partial class RefRectangularCollectionExtensions
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
        /// <typeparam name="TRefRectCollection">
        ///     <typeparamref name="TRefRectCollection"/> is 
        ///     <see cref="IRefRectangularCollection{T}"/>
        /// </typeparam>
        /// <param name="index">A zero-based index that determines which column is to take.</param> 
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RefColumn<T, TRefRectCollection> GetColumn<T, TRefRectCollection>(
            this TRefRectCollection source, int index)
            where TRefRectCollection : IRefRectangularCollection<T>
        {
            try
            {
                return new RefColumn<T, TRefRectCollection>(source, index);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }
    }
}
