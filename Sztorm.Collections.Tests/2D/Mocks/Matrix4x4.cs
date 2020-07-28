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

        public ReadOnlyMatrix4x4 AsReadOnly() => new ReadOnlyMatrix4x4(this);

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

        public static Matrix4x4 CreateIdentityMatrix()
        {
            var result = new Matrix4x4();
            result[0, 0] = 1;
            result[1, 1] = 1;
            result[2, 2] = 1;
            result[3, 3] = 1;

            return result;
        }

        /// <summary>
        ///     Creates a <see cref="Matrix4x4"/> from <see cref="float"/>[,] instance.
        /// <para>
        ///     Exceptions:<br/>
        ///     <see cref="ArgumentNullException"/>: Array cannot be null.<br/>
        ///     <see cref="ArgumentException"/>: Array must have length of 4 in both dimensions.
        /// </para>
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static Matrix4x4 FromSystem2DArray(float[,] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Array cannot be null.");
            }
            if (array.GetLength(0) != Matrix4x4Boundaries.Rows ||
                array.GetLength(1) != Matrix4x4Boundaries.Columns)
            {
                throw new ArgumentException(
                    nameof(array), "Array must have length of 4 in both dimensions");
            }
            var result = new Matrix4x4();

            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    result[i, j] = array[i, j];
                }
            }
            return result;
        }
    }
}
