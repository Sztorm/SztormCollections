// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //
// File is auto-generated. Do not modify as changes may be overwritten.
// If you want to modify this file, edit template file with the same name and .tt extension.
// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections.Extensions
{
    public static partial class RectangularCollectionExtensions
    {
        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying first
        ///     occurrence searched row by row within the entire 
        ///     <typeparamref name="TRectCollection"/> if found. Otherwise
        ///     returns <see cref="ItemRequestResult{T}.Fail"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRectCollection">
        ///     <typeparamref name="TRectCollection"/> is 
        ///     <see cref="IRectangularCollection{T}"/>
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
        public static ItemRequestResult<T> Find<T, TRectCollection, TPredicate>(
            this TRectCollection source, TPredicate match)
            where TRectCollection : IRectangularCollection<T>
            where TPredicate : struct, IPredicate<T>
        {
            ItemRequestResult<Index2D> resultFound = source.FindIndex<
                T, TRectCollection, TPredicate>(match);

            return resultFound.IsSuccess ? 
                new ItemRequestResult<T>(source[resultFound.ItemOrDefault]) : 
                ItemRequestResult<T>.Fail;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying first
        ///     occurrence searched row by row within the entire 
        ///     <typeparamref name="TRectCollection"/> if found. Otherwise 
        ///     returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="Find{T, TRectCollection, TPredicate}
        ///     (TRectCollection, TPredicate)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRectCollection">
        ///     <typeparamref name="TRectCollection"/> is 
        ///     <see cref="IRectangularCollection{T}"/>
        /// </typeparam>
        /// <param name="source">The collection in which the operation takes place.</param>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        public static ItemRequestResult<T> Find<T, TRectCollection>(
            this TRectCollection source, Predicate<T> match)
            where TRectCollection : IRectangularCollection<T>
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }
            return source.Find<T, TRectCollection, BoxedPredicate<T>>(
                new BoxedPredicate<T>(match));
        }
    }
}
