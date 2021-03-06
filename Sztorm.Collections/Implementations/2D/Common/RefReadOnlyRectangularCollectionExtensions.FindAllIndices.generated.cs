// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //
// File is auto-generated. Do not modify as changes may be overwritten.
// If you want to modify this file, edit template file with the same name and .tt extension.
// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

using System;
using System.Collections.Generic;

namespace Sztorm.Collections.Extensions
{
    public static partial class RefReadOnlyRectangularCollectionExtensions
    {
        /// <summary>
        ///     Returns <see cref="ICollection{T}"/> containing all the indices of which items
        ///     match the conditions defined by the specified predicate.<br/>
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="NotSupportedException"/>: Provided type must support 
        ///         <see cref="ICollection{T}.Add(T)"/> method.
        ///     </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Index2DCollection">
        ///     <typeparamref name = "Index2DCollection"/> is <see cref="ICollection{T}"/> and has a
        ///     parameterless constructor.
        /// </typeparam>
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
        public static Index2DCollection FindAllIndices<
            T, Index2DCollection, TRefReadOnlyRectCollection, TPredicate>(
            this TRefReadOnlyRectCollection source, TPredicate match)
            where Index2DCollection : ICollection<Index2D>, new()
            where TRefReadOnlyRectCollection : IRefReadOnlyRectangularCollection<T>
            where TPredicate : struct, IPredicate<T>
        {
            var resizableCollection = new Index2DCollection();

            try
            {
                Bounds2D bounds = source.Boundaries;

                for (int i = 0; i < bounds.Length1; i++)
                {
                    for (int j = 0; j < bounds.Length2; j++)
                    {
                        var index = new Index2D(i, j);

                        if (match.Invoke(source[index]))
                        {
                            resizableCollection.Add(index);
                        }
                    }
                }
            }
            catch (NotSupportedException)
            {
                throw new NotSupportedException("Provided type must support Add(T) method.");
            }
            return resizableCollection;
        }

        /// <summary>
        ///     Returns <see cref="ICollection{T}"/> containing all the indices of which items
        ///     match the conditions defined by the specified predicate.<br/>
        ///     Use <see cref="FindAllIndices{T, Index2DCollection, TRefReadOnlyRectCollection,
        ///     TPredicate}(TRefReadOnlyRectCollection, TPredicate)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="NotSupportedException"/>: Provided type must support
        ///         <see cref="ICollection{T}.Add(T)"/> method.
        ///     </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Index2DCollection">
        ///     <typeparamref name = "Index2DCollection"/> is <see cref="ICollection{T}"/> and has a
        ///     parameterless constructor.
        /// </typeparam>
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
        public static Index2DCollection FindAllIndices<
            T, Index2DCollection, TRefReadOnlyRectCollection>(
            this TRefReadOnlyRectCollection source, Predicate<T> match)
            where Index2DCollection : ICollection<Index2D>, new()
            where TRefReadOnlyRectCollection : IRefReadOnlyRectangularCollection<T>
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }
            try
            {
                return source.FindAllIndices<
                    T, Index2DCollection, TRefReadOnlyRectCollection, BoxedPredicate<T>>(
                    new BoxedPredicate<T>(match));
            }
            catch (NotSupportedException)
            {
                throw;
            }
        }
    }
}
