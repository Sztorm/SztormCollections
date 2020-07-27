// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //
// File is auto-generated. Do not modify as changes may be overwritten.
// If you want to modify this file, edit template file with the same name and .tt extension.
// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

using System;
using System.Collections.Generic;

namespace Sztorm.Collections.Extensions
{
    public static partial class RectangularCollectionExtensions
    {
        /// <summary>
        ///     Returns <see cref="ICollection{T}"/> containing all the elements that match the
        ///     conditions defined by the specified predicate.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="NotSupportedException"/>: Provided type must support 
        ///         <see cref="ICollection{T}.Add(T)"/> method.
        ///     </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TCollection">
        ///     <typeparamref name = "TCollection"/> is <see cref="ICollection{T}"/> and has a
        ///     parameterless constructor.
        /// </typeparam>
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
        public static TCollection FindAll<
            T, TCollection, TRectCollection, TPredicate>(
            this TRectCollection source, TPredicate match)
            where TCollection : ICollection<T>, new()
            where TRectCollection : IRectangularCollection<T>
            where TPredicate : struct, IPredicate<T>
        {
            var resizableCollection = new TCollection();

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
                            resizableCollection.Add(source[index]);
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
        ///     Returns <see cref="ICollection{T}"/> containing all the elements that match the
        ///     conditions defined by the specified predicate.<br/>
        ///     Use <see cref="FindAll{T, TCollection, TRectCollection, TPredicate}
        ///     (TRectCollection, TPredicate)"/> to avoid virtual
        ///     call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
        ///         <see langword="null"/>.<br/>
        ///         <see cref="NotSupportedException"/>: Provided type must support
        ///         <see cref="ICollection{T}.Add(T)"/> method.
        ///     </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TCollection">
        ///     <typeparamref name = "TCollection"/> is <see cref="ICollection{T}"/> and has a
        ///     parameterless constructor.
        /// </typeparam>
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
        public static TCollection FindAll<T, TCollection, TRectCollection>(
            this TRectCollection source, Predicate<T> match)
            where TCollection : ICollection<T>, new()
            where TRectCollection : IRectangularCollection<T>
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }
            try
            {
                return source.FindAll<
                    T, TCollection, TRectCollection, BoxedPredicate<T>>(
                    new BoxedPredicate<T>(match));
            }
            catch (NotSupportedException)
            {
                throw;
            }
        }
    }
}
