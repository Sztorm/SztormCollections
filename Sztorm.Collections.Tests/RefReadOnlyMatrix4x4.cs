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
        {
            var result = new RefReadOnlyMatrix4x4();
            result[0, 0] = 1;
            result[1, 1] = 1;
            result[2, 2] = 1;
            result[3, 3] = 1;

            return result;
        }
    }
}
