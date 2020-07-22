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
        internal static ItemRequestResult<Index2D> FindIndexInternal<
            T, TReadOnlyRectCollection, TPredicate>(
            this TReadOnlyRectCollection source,
            Index2D startIndex,
            Bounds2D sectorSize,
            TPredicate match)
            where TReadOnlyRectCollection : IReadOnlyRectangularCollection<T>
            where TPredicate : struct, IPredicate<T>
        {
            for (int i = startIndex.Dimension1Index; i < sectorSize.Length1; i++)
            {
                for (int j = startIndex.Dimension2Index; j < sectorSize.Length2; j++)
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
        ///     of the first occurrence searched within the entire 
        ///     <typeparamref name="TReadOnlyRectCollection"/> if found. Otherwise
        ///     returns <see cref="ItemRequestResult{T}.Fail"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReadOnlyRectCollection">
        ///     <typeparamref name="TReadOnlyRectCollection"/> is 
        ///     <see cref="IReadOnlyRectangularCollection{T}"/>
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
        public static ItemRequestResult<Index2D> FindIndex<
            T, TReadOnlyRectCollection, TPredicate>(
            this TReadOnlyRectCollection source, TPredicate match)
            where TReadOnlyRectCollection : IReadOnlyRectangularCollection<T>
            where TPredicate : struct, IPredicate<T>
            => source.FindIndexInternal<T, TReadOnlyRectCollection, TPredicate>(
                new Index2D(), source.Boundaries, match);

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence searched within the specified sector. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within collection bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within collection bounds, beginning from
        ///         <paramref name="startIndex"/>.
        ///     </para>   
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReadOnlyRectCollection">
        ///     <typeparamref name="TReadOnlyRectCollection"/> is 
        ///     <see cref="IReadOnlyRectangularCollection{T}"/>
        /// </typeparam>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name="TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="source">The collection in which the operation takes place.</param>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions of the element to search for.
        /// </param>
        /// <returns></returns>
        public static ItemRequestResult<Index2D> FindIndex<
            T, TReadOnlyRectCollection, TPredicate>(
            this TReadOnlyRectCollection source,
            Index2D startIndex,
            Bounds2D sectorSize,
            TPredicate match)
            where TReadOnlyRectCollection : IReadOnlyRectangularCollection<T>
            where TPredicate : struct, IPredicate<T>
        {
            if (!source.IsValidIndex(startIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startIndex), "startIndex must be within collection bounds.");
            }
            var indexAfterEnd = new Index2D(
                startIndex.Dimension1Index + sectorSize.Length1,
                startIndex.Dimension2Index + sectorSize.Length2);
            Bounds2D sourceBounds = source.Boundaries;

            if (indexAfterEnd.Dimension1Index > sourceBounds.Length1 ||
                indexAfterEnd.Dimension2Index > sourceBounds.Length2)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sectorSize),
                    "sectorSize must be within collection bounds, beginning from startIndex.");
            }

            return source.FindIndexInternal<T, TReadOnlyRectCollection, TPredicate>(
                startIndex, sectorSize, match);
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence searched within the entire 
        ///     <typeparamref name="TReadOnlyRectCollection"/> if found. Otherwise
        ///     returns <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindIndex{T, TReadOnlyRectCollection, TPredicate}
        ///     (TReadOnlyRectCollection, TPredicate)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/> <paramref name="match"/> cannot be
        ///         <see langword="null"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReadOnlyRectCollection">
        ///     <typeparamref name="TReadOnlyRectCollection"/> is 
        ///     <see cref="IReadOnlyRectangularCollection{T}"/>
        /// </typeparam>
        /// <param name="source">The collection in which the operation takes place.</param>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ItemRequestResult<Index2D> FindIndex<
            T, TReadOnlyRectCollection>(
            this TReadOnlyRectCollection source,
            Index2D startIndex,
            Bounds2D sectorSize,
            Predicate<T> match)
            where TReadOnlyRectCollection : IReadOnlyRectangularCollection<T>
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            try
            {
                return source.FindIndex<T, TReadOnlyRectCollection, BoxedPredicate<T>>(
                    startIndex, sectorSize, new BoxedPredicate<T>(match));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        ///     Searches for an item that matches the conditions defined by the specified
        ///     predicate, and returns the <see cref="ItemRequestResult{T}"/> with underlying index
        ///     of the first occurrence searched within the specified sector. Otherwise returns
        ///     <see cref="ItemRequestResult{T}.Fail"/><br/>
        ///     Use <see cref="FindIndex{T, TReadOnlyRectCollection, TPredicate}
        ///     (TReadOnlyRectCollection, Index2D, Bounds2D, TPredicate)"/> to avoid
        ///     virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="ArgumentOutOfRangeException"/>: <paramref name="startIndex"/> must
        ///         be within collection bounds;<br/>
        ///         <paramref name="sectorSize"/> must be within collection bounds, beginning from
        ///         <paramref name="startIndex"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReadOnlyRectCollection">
        ///     <typeparamref name="TReadOnlyRectCollection"/> is 
        ///     <see cref="IReadOnlyRectangularCollection{T}"/>
        /// </typeparam>
        /// <param name="source">The collection in which the operation takes place.</param>
        /// <param name="startIndex">Zero-based index from which searching starts.</param>
        /// <param name="sectorSize">The rectangular sector size to be searched.</param>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ItemRequestResult<Index2D> FindIndex<
            T, TReadOnlyRectCollection>(
            this TReadOnlyRectCollection source, Predicate<T> match)
            where TReadOnlyRectCollection : IReadOnlyRectangularCollection<T>
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            return source.FindIndex<T, TReadOnlyRectCollection, BoxedPredicate<T>>(
                new BoxedPredicate<T>(match));
        }
    }
}
