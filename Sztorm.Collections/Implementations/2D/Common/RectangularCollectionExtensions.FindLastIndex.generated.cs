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
        internal static ItemRequestResult<Index2D> FindLastIndexInternal<
            T, TRectCollection, TPredicate>(
            this TRectCollection source,
            Index2D startIndex,
            Index2D indexAfterEnd,
            TPredicate match)
            where TRectCollection : IRectangularCollection<T>
            where TPredicate : struct, IPredicate<T>
        {
            for (int i = startIndex.Dimension1Index; i > indexAfterEnd.Dimension1Index; i--)
            {
                for (int j = startIndex.Dimension2Index; j > indexAfterEnd.Dimension2Index; j--)
                {
                    var index = new Index2D(i, j);

                    if (match.Invoke(source[index]))
                    {
                        return new ItemRequestResult<Index2D>(index);
                    }
                }
            }
            return ItemRequestResult<Index2D>.Fail;
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the last occurrence searched within the entire
        ///     <typeparamref name="TRectCollection"/> if found. Otherwise
        ///     returns <see cref="ItemRequestResult{T}.Fail"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRectCollection">
        ///     <typeparamref name="TRectCollection"/> is 
        ///     <see cref="IRectangularCollection{T}"/>
        /// </typeparam>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name="TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="source">The collection in which the operation takes place.</param>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ItemRequestResult<Index2D> FindLastIndex<
            T, TRectCollection, TPredicate>(
            this TRectCollection source, TPredicate match)
            where TRectCollection : IRectangularCollection<T>
            where TPredicate : struct, IPredicate<T>
            => source.FindLastIndexInternal<T, TRectCollection, TPredicate>(
                new Index2D(source.Boundaries.Rows - 1, source.Boundaries.Columns - 1),
                new Index2D(-1, -1),
                match);

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the last occurrence searched within the specified sector. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within collection bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within collection bounds, beginning
        ///         backwardly from <paramref name="startIndex"/>.
        ///     </para>  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRectCollection">
        ///     <typeparamref name="TRectCollection"/> is 
        ///     <see cref="IRectangularCollection{T}"/>
        /// </typeparam>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name="TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="source">The collection in which the operation takes place.</param>
        /// <param name="startIndex">Zero-based starting index of the backward search.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        public static ItemRequestResult<Index2D> FindLastIndex<
            T, TRectCollection, TPredicate>(
            this TRectCollection source,
            Index2D startIndex,
            Bounds2D sectorSize,
            TPredicate match)
            where TRectCollection : IRectangularCollection<T>
            where TPredicate : struct, IPredicate<T>
        {
            if (!source.IsValidIndex(startIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startIndex), "startIndex must be within collection bounds.");
            }
            var indexAfterEnd = new Index2D(
                startIndex.Row - sectorSize.Rows,
                startIndex.Column - sectorSize.Columns);

            if (indexAfterEnd.Row < -1 || indexAfterEnd.Column < -1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sectorSize),
                    "sectorSize must be within collection bounds, beginning backwardly from " +
                    "startIndex.");
            }

            return source.FindLastIndexInternal<T, TRectCollection, TPredicate>(
                startIndex, indexAfterEnd, match);
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the last occurrence searched within the entire
        ///     <typeparamref name="TRectCollection"/> if found. Otherwise
        ///     returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindLastIndex{T, TRectCollection, TPredicate}
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ItemRequestResult<Index2D> FindLastIndex<
            T, TRectCollection>(
            this TRectCollection source, Predicate<T> match)
            where TRectCollection : IRectangularCollection<T>
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            return source.FindLastIndex<T, TRectCollection, BoxedPredicate<T>>(
                new BoxedPredicate<T>(match));
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the last occurrence searched within the specified sector. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindLastIndex{T, TRectCollection, TPredicate}
        ///     (TRectCollection, Index2D, Bounds2D, TPredicate)"/> to avoid
        ///     virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within list bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within list bounds, beginning backwardly
        ///         from <paramref name="startIndex"/>.
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ItemRequestResult<Index2D> FindLastIndex<
            T, TRectCollection>(
            this TRectCollection source,
            Index2D startIndex,
            Bounds2D sectorSize,
            Predicate<T> match)
            where TRectCollection : IRectangularCollection<T>
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            try
            {
                return source.FindLastIndex<T, TRectCollection, BoxedPredicate<T>>(
                    startIndex, sectorSize, new BoxedPredicate<T>(match));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }
    }
}
