using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public partial class Array2DTests
    {
        public static class GetRefIndexable
        {
            [TestCaseSource(typeof(GetRefIndexable), nameof(GetRowInvalidTestCases))]
            public static void GetRowThrowExceptionIfIndexExceedsRows<T>(
                Array2D<T> array, int index)
                => Assert.Throws<ArgumentOutOfRangeException>(() => array.GetRow(index));

            [TestCaseSource(typeof(GetRefIndexable), nameof(GetRowTestCases))]
            public static void GetRowTest<T>(
                Array2D<T> array, int index, RefRow<T, Array2D<T>> expected)
                => CollectionAssert.AreEqual(expected, array.GetRow(index));

            [TestCaseSource(typeof(GetRefIndexable), nameof(GetColumnInvalidTestCases))]
            public static void GetColumnThrowExceptionIfIndexExceedsColumns<T>(
                Array2D<T> array, int index)
                => Assert.Throws<ArgumentOutOfRangeException>(() => array.GetColumn(index));

            [TestCaseSource(typeof(GetRefIndexable), nameof(GetColumnTestCases))]
            public static void GetColumnTest<T>(
                Array2D<T> array, int index, RefColumn<T, Array2D<T>> expected)
                => CollectionAssert.AreEqual(expected, array.GetColumn(index));

            private static IEnumerable<TestCaseData> GetRowInvalidTestCases()
            {
                yield return new TestCaseData(new Array2D<byte>(2, 1), -1);
                yield return new TestCaseData(new Array2D<byte>(2, 1), 2);
                yield return new TestCaseData(new Array2D<byte>(0, 1), 0);
            }

            private static IEnumerable<TestCaseData> GetRowTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 4, 3, 7 },
                                 { 8, 9, 2 } });

                var firstRow = new RefRow<int, Array2D<int>>(new Array2D<int>(1, 3), 0);
                firstRow[0] = 4;
                firstRow[1] = 3;
                firstRow[2] = 7;

                var lastRow = new RefRow<int, Array2D<int>>(new Array2D<int>(1, 3), 0);
                lastRow[0] = 8;
                lastRow[1] = 9;
                lastRow[2] = 2;

                yield return new TestCaseData(array2x3, 0, firstRow);
                yield return new TestCaseData(array2x3, 1, lastRow);
            }

            private static IEnumerable<TestCaseData> GetColumnInvalidTestCases()
            {
                yield return new TestCaseData(new Array2D<byte>(1, 2), -1);
                yield return new TestCaseData(new Array2D<byte>(1, 2), 2);
                yield return new TestCaseData(new Array2D<byte>(1, 0), 0);
            }

            private static IEnumerable<TestCaseData> GetColumnTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 4, 8 },
                                 { 3, 9 },
                                 { 7, 2 } });

                var firstColumn = new RefColumn<int, Array2D<int>>(new Array2D<int>(3, 1), 0);
                firstColumn[0] = 4;
                firstColumn[1] = 3;
                firstColumn[2] = 7;

                var lastColumn = new RefColumn<int, Array2D<int>>(new Array2D<int>(3, 1), 0);
                lastColumn[0] = 8;
                lastColumn[1] = 9;
                lastColumn[2] = 2;

                yield return new TestCaseData(array2x3, 0, firstColumn);
                yield return new TestCaseData(array2x3, 1, lastColumn);
            }
        }      
    }
}
