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
        ///     Determines whether any item that match the conditions defined by the specified
        ///     predicate exists in the current instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRefRectCollection">
        ///     <typeparamref name="TRefRectCollection"/> is 
        ///     <see cref="IRefRectangularCollection{T}"/>
        /// </typeparam>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="source">The collection in which the operation takes place.</param>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Exists<T, TRefRectCollection, TPredicate>(
            this TRefRectCollection source, TPredicate match)
            where TRefRectCollection : IRefRectangularCollection<T>
            where TPredicate : struct, IPredicate<T>
            => source.FindIndex<T, TRefRectCollection, TPredicate>(match).IsSuccess;

        /// <summary>
        ///     Determines whether any item that match the conditions defined by the specified
        ///     predicate exists in the current instance.<br/>  
        ///     Use <see cref="Exists{T, TRefRectCollection, TPredicate}
        ///     (TRefRectCollection, TPredicate)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/> <paramref name="match"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRefRectCollection">
        ///     <typeparamref name="TRefRectCollection"/> is 
        ///     <see cref="IRefRectangularCollection{T}"/>
        /// </typeparam>
        /// <param name="source">The collection in which the operation takes place.</param>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Exists<T, TRefRectCollection>(
            this TRefRectCollection source, Predicate<T> match)
            where TRefRectCollection : IRefRectangularCollection<T>
        {
            try
            {
                return source.FindIndex(match).IsSuccess;
            }
            catch (ArgumentNullException)
            {
                throw;
            }
        }
    }
}
