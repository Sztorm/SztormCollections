using System;
using System.Collections.Generic;
using System.Text;

namespace Sztorm.Collections.Tests
{
    public static class TestsExtensions
    {
        public static void CopyTo<T>(this T[,] source, Array2D<T> dest)
        {
            if (dest != null &&
                source != null &&
                source.GetLength(0) != dest.Length1 &&
                source.GetLength(1) != dest.Length2)
            {
                throw new ArgumentException("None of arrays can be null or have different lengths.");
            }

            for (int i = 0, len1 = dest.Length1; i < len1; i++)
            {
                for (int j = 0, len2 = dest.Length2; j < len2; j++)
                {
                    dest[i, j] = source[i, j];
                }
            }
        }
    }
}
