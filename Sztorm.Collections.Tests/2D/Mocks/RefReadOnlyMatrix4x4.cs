using System;
using System.Collections;
using System.Collections.Generic;

namespace Sztorm.Collections.Tests
{
    using static RectangularCollectionUtils;

    /// <summary>
    ///     Row-major ref indexed read-only Matrix4x4 of floats.
    /// </summary>
    public class RefReadOnlyMatrix4x4 : IRefReadOnlyRectangularCollection<float>
    {
        private static readonly Bounds2D Matrix4x4Boundaries = new Bounds2D(4, 4);

        internal readonly float[] numbers;

        public RefReadOnlyMatrix4x4() => numbers = new float[Rows * Columns];

        public RefReadOnlyMatrix4x4(RefMatrix4x4 matrix) => numbers = matrix.numbers;

        public int Rows => 4;

        public int Columns => 4;

        public Bounds2D Boundaries => Matrix4x4Boundaries;

        public ref readonly float this[Index2D index]
        {
            get
            {
                if (!IsValidIndex(index))
                {
                    throw new IndexOutOfRangeException();
                }
                return ref numbers[RowMajorIndex2DToInt(index, Columns)];
            }
        }

        public ref readonly float this[int row, int column] => ref this[(row, column)];

        public RefReadOnlyColumn<float, RefReadOnlyMatrix4x4> GetColumn(int index)
            => new RefReadOnlyColumn<float, RefReadOnlyMatrix4x4>(this, index);

        public RefReadOnlyRow<float, RefReadOnlyMatrix4x4> GetRow(int index)
            => new RefReadOnlyRow<float, RefReadOnlyMatrix4x4>(this, index);

        public IEnumerator<float> GetEnumerator()
        {
            for (int i = 0; i < Rows * Columns; i++)
            {
                yield return numbers[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool IsValidIndex(Index2D index)
            => index.Row >= 0 && index.Row < Rows &&
               index.Column >= 0 && index.Column < Columns;

        public static RefReadOnlyMatrix4x4 CreateIdentityMatrix()
            => RefMatrix4x4.CreateIdentityMatrix().AsReadOnly();

        /// <summary>
        ///     Creates a <see cref="RefReadOnlyMatrix4x4"/> from <see cref="float"/>[,] instance.
        /// <para>
        ///     Exceptions:<br/>
        ///     <see cref="ArgumentNullException"/>: Array cannot be null.<br/>
        ///     <see cref="ArgumentException"/>: Array must have length of 4 in both dimensions.
        /// </para>
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static RefReadOnlyMatrix4x4 FromSystem2DArray(float[,] array)
            => RefMatrix4x4.FromSystem2DArray(array).AsReadOnly();
    }
}
