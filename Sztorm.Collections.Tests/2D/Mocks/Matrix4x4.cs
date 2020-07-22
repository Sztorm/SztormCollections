﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Sztorm.Collections.Tests
{
    using static RectangularCollectionUtils;

    /// <summary>
    ///     Row-major indexed Matrix4x4 of floats.
    /// </summary>
    public class Matrix4x4 : IRectangularCollection<float>
    {
        private static readonly Bounds2D Matrix4x4Boundaries = new Bounds2D(4, 4);

        internal readonly float[] numbers;

        public Matrix4x4() => numbers = new float[Rows * Columns];

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
            set
            {
                if (!IsValidIndex(index))
                {
                    throw new IndexOutOfRangeException();
                }
                numbers[RowMajorIndex2DToInt(index, Columns)] = value;
            }
        }

        public float this[int row, int column]
        {
            get => this[(row, column)];
            set => this[(row, column)] = value;
        }

        public Column<float, Matrix4x4> GetColumn(int index)
            => new Column<float, Matrix4x4>(this, index);

        public Row<float, Matrix4x4> GetRow(int index)
            => new Row<float, Matrix4x4>(this, index);

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

        public ReadOnlyMatrix4x4 AsReadOnly() => new ReadOnlyMatrix4x4(this);

        public static Matrix4x4 CreateIdentityMatrix()
        {
            var result = new Matrix4x4();
            result[0, 0] = 1;
            result[1, 1] = 1;
            result[2, 2] = 1;
            result[3, 3] = 1;

            return result;
        }
    }
}
