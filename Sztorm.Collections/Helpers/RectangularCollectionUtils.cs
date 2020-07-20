using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    public static class RectangularCollectionUtils
    {
        /// <summary>
        ///      Returns the one-dimensional index mapped from two-dimensional index taken from a
        ///      row-major ordered contiguous rectangular collection.
        /// </summary>
        /// <param name="index">
        ///     A two-dimensional position in a contiguous rectangular collection.
        /// </param>
        /// <param name="columns">
        ///     Total number of columns in a rectangular collection. (Length2)
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RowMajorIndex2DToInt(Index2D index, int columns)
            => index.Row * columns + index.Column;

        /// <summary>
        ///      Returns the one-dimensional index mapped from two-dimensional index taken from a
        ///      column-major ordered contiguous rectangular collection.
        /// </summary>
        /// <param name="index">
        ///     A two-dimensional position in a contiguous rectangular collection.
        /// </param>
        /// <param name="rows">Total number of rows in a rectangular collection. (Length1)</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ColumnMajorIndex2DToInt(Index2D index, int rows)
            => index.Column * rows + index.Row;

        /// <summary>
        ///      Returns the two-dimensional index mapped from one-dimensional index taken from a
        ///      row-major ordered contiguous rectangular collection.
        /// </summary>
        /// <param name="index">
        ///     A one-dimensional position in a contiguous rectangular collection.
        /// </param>
        /// <param name="columns">
        ///     Total number of columns in a rectangular collection. (Length2)
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Index2D IntToRowMajorIndex2D(int index, int columns) 
            => new Index2D(index / columns, index % columns);

        /// <summary>
        ///      Returns the two-dimensional index mapped from one-dimensional index taken from a
        ///      column-major ordered contiguous rectangular collection.
        /// </summary>
        /// <param name="index">
        ///     A one-dimensional position in a contiguous rectangular collection.
        /// </param>
        /// <param name="rows">Total number of rows in a rectangular collection. (Length1)</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Index2D IntToColumnMajorIndex2D(int index, int rows)
            => new Index2D(index % rows, index / rows);
    }
}
