using System;
using System.Collections;
using System.Collections.Generic;

namespace Sztorm.Collections.Tests
{
    using static RectangularCollectionUtils;

    /// <summary>
    ///     Row-major indexed ReadOnlyMatrix4x4 of floats.
    /// </summary>
    public class ReadOnlyMatrix4x4 : IReadOnlyRectangularCollection<float>
    {
        private static readonly Bounds2D Matrix4x4Boundaries = new Bounds2D(4, 4);

        internal readonly float[] numbers;

        public ReadOnlyMatrix4x4() => numbers = new float[Rows * Columns];

        public ReadOnlyMatrix4x4(Matrix4x4 matrix) => numbers = matrix.numbers;

        public int Rows => 4;

        public int Columns => 4;

        public Bounds2D Boundaries => Matrix4x4Boundaries;

        public float this[Index2D index]
        {
            get
            {
                if (!IsValidIndex(index))
                {
                    throw new IndexOutOfRangeException();
                }
                return numbers[RowMajorIndex2DToInt(index, Columns)];
            }
        }

        public float this[int row, int column] => this[(row, column)];

        public ReadOnlyColumn<float, ReadOnlyMatrix4x4> GetColumn(int index)
            => new ReadOnlyColumn<float, ReadOnlyMatrix4x4>(this, index);

        public ReadOnlyRow<float, ReadOnlyMatrix4x4> GetRow(int index)
            => new ReadOnlyRow<float, ReadOnlyMatrix4x4>(this, index);

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

        public static ReadOnlyMatrix4x4 CreateIdentityMatrix() 
            => Matrix4x4.CreateIdentityMatrix().AsReadOnly();

        /// <summary>
        ///     Creates a <see cref="ReadOnlyMatrix4x4"/> from <see cref="float"/>[,] instance.
        /// <para>
        ///     Exceptions:<br/>
        ///     <see cref="ArgumentNullException"/>: Array cannot be null.<br/>
        ///     <see cref="ArgumentException"/>: Array must have length of 4 in both dimensions.
        /// </para>
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static ReadOnlyMatrix4x4 FromSystem2DArray(float[,] array)
            => Matrix4x4.FromSystem2DArray(array).AsReadOnly();
    }
}
