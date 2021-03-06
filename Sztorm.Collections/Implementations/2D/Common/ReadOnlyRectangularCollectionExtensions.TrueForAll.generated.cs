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
        ///     Determines whether every item matches the conditions defined by the specified
        ///     predicate. If the current instance contains no items the return value is
        ///     <see langword="true"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReadOnlyRectCollection">
        ///     <typeparamref name="TReadOnlyRectCollection"/> is 
        ///     <see cref="IReadOnlyRectangularCollection{T}"/>
        /// </typeparam>
        /// <typeparam name="TPredicate">
        ///     <typeparamref name = "TPredicate"/> is <see cref="IPredicate{T}"/> and
        ///     <see langword="struct"/>
        /// </typeparam>
        /// <param name="source">The collection in which the operation takes place.</param>
        /// <param name="match">
        ///     A <see langword="struct"/> implementing <see cref="IPredicate{T}"/> that defines
        ///     the conditions to check against the items.
        /// </param>
        /// <returns></returns>
        public static bool TrueForAll<T, TReadOnlyRectCollection, TPredicate>(
            this TReadOnlyRectCollection source, TPredicate match)
            where TReadOnlyRectCollection : IReadOnlyRectangularCollection<T>
            where TPredicate : struct, IPredicate<T>
        {
            Bounds2D bounds = source.Boundaries;

            for (int i = 0; i < bounds.Length1; i++)
            {
                for (int j = 0; j < bounds.Length2; j++)
                {
                    if (!match.Invoke(source[new Index2D(i, j)]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        ///     Determines whether every item matches the conditions defined by the specified
        ///     predicate. If the current instance contains no items the return value is
        ///     <see langword="true"/>.<br/>
        ///     Use <see cref="TrueForAll{T, TReadOnlyRectCollection, TPredicate}
        ///     (TReadOnlyRectCollection, TPredicate)"/> to avoid virtual call.
        ///     <para>
        ///         Exceptions:<br/>
        ///         <see cref="ArgumentNullException"/>: <paramref name="match"/> cannot be
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
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions to check
        ///     against the items.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrueForAll<T, TReadOnlyRectCollection>(
            this TReadOnlyRectCollection source, Predicate<T> match)
            where TReadOnlyRectCollection : IReadOnlyRectangularCollection<T>
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "match cannot be null.");
            }
            return source.TrueForAll<T, TReadOnlyRectCollection, BoxedPredicate<T>>(
                new BoxedPredicate<T>(match));
        }
    }
}
