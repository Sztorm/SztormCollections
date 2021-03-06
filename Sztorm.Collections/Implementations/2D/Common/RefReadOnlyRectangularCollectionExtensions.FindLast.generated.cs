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
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying last
        ///     occurrence searched row by row within the entire
        ///     <typeparamref name="TRefReadOnlyRectCollection"/> if found. Otherwise
        ///     returns <see cref="ItemRequestResult{T}.Fail"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRefReadOnlyRectCollection">
        ///     <typeparamref name="TRefReadOnlyRectCollection"/> is 
        ///     <see cref="IRefReadOnlyRectangularCollection{T}"/>
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
        public static ItemRequestResult<T> FindLast<T, TRefReadOnlyRectCollection, TPredicate>(
            this TRefReadOnlyRectCollection source, TPredicate match)
            where TRefReadOnlyRectCollection : IRefReadOnlyRectangularCollection<T>
            where TPredicate : struct, IPredicate<T>
        {
            ItemRequestResult<Index2D> resultFound = source.FindLastIndex<
                T, TRefReadOnlyRectCollection, TPredicate>(match);

            return resultFound.IsSuccess ? 
                new ItemRequestResult<T>(source[resultFound.ItemOrDefault]) : 
                ItemRequestResult<T>.Fail;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying last
        ///     occurrence searched row by row within the entire 
        ///     <typeparamref name="TRefReadOnlyRectCollection"/> if found. Otherwise 
        ///     returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindLast{T, TRefReadOnlyRectCollection, TPredicate}
        ///     (TRefReadOnlyRectCollection, TPredicate)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRefReadOnlyRectCollection">
        ///     <typeparamref name="TRefReadOnlyRectCollection"/> is 
        ///     <see cref="IRefReadOnlyRectangularCollection{T}"/>
        /// </typeparam>
        /// <param name="source">The collection in which the operation takes place.</param>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        public static ItemRequestResult<T> FindLast<T, TRefReadOnlyRectCollection>(
            this TRefReadOnlyRectCollection source, Predicate<T> match)
            where TRefReadOnlyRectCollection : IRefReadOnlyRectangularCollection<T>
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }
            return source.FindLast<T, TRefReadOnlyRectCollection, BoxedPredicate<T>>(
                new BoxedPredicate<T>(match));
        }
    }
}
