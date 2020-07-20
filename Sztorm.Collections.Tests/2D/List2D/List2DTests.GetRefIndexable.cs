using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    using static TestUtils;

    [TestFixture]
    public partial class List2DTests
    {
        public static class GetRefIndexable
        {
            [TestCaseSource(typeof(GetRefIndexable), nameof(GetRowInvalidTestCases))]
            public static void GetRowThrowExceptionIfIndexExceedsRows<T>(
                List2D<T> list, int index)
                => Assert.Throws<ArgumentOutOfRangeException>(() => list.GetRow(index));

            [TestCaseSource(typeof(GetRefIndexable), nameof(GetRowTestCases))]
            public static void GetRowTest<T>(
                List2D<T> list, int index, RefRow<T, List2D<T>> expected)
                => CollectionAssert.AreEqual(expected, list.GetRow(index));

            [TestCaseSource(typeof(GetRefIndexable), nameof(GetColumnInvalidTestCases))]
            public static void GetColumnThrowExceptionIfIndexExceedsColumns<T>(
                List2D<T> list, int index)
                => Assert.Throws<ArgumentOutOfRangeException>(() => list.GetColumn(index));

            [TestCaseSource(typeof(GetRefIndexable), nameof(GetColumnTestCases))]
            public static void GetColumnTest<T>(
                List2D<T> list, int index, RefColumn<T, List2D<T>> expected)
                => CollectionAssert.AreEqual(expected, list.GetColumn(index));

            private static IEnumerable<TestCaseData> GetRowInvalidTestCases()
            {
                yield return new TestCaseData(CreateList2DWithBounds<byte>(2, 1), -1);
                yield return new TestCaseData(CreateList2DWithBounds<byte>(2, 1), 2);
                yield return new TestCaseData(CreateList2DWithBounds<byte>(0, 1), 0);
            }

            private static IEnumerable<TestCaseData> GetRowTestCases()
            {
                var list2x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 4, 3, 7 },
                                 { 8, 9, 2 } });
                list2x3.IncreaseCapacity(list2x3.Boundaries);

                var firstRow = new RefRow<int, List2D<int>>(
                    CreateList2DWithBounds<int>(1, 3), 0);
                firstRow[0] = 4;
                firstRow[1] = 3;
                firstRow[2] = 7;

                var lastRow = new RefRow<int, List2D<int>>(
                    CreateList2DWithBounds<int>(1, 3), 0);
                lastRow[0] = 8;
                lastRow[1] = 9;
                lastRow[2] = 2;

                yield return new TestCaseData(list2x3, 0, firstRow);
                yield return new TestCaseData(list2x3, 1, lastRow);
            }

            private static IEnumerable<TestCaseData> GetColumnInvalidTestCases()
            {
                yield return new TestCaseData(CreateList2DWithBounds<byte>(1, 2), -1);
                yield return new TestCaseData(CreateList2DWithBounds<byte>(1, 2), 2);
                yield return new TestCaseData(CreateList2DWithBounds<byte>(1, 0), 0);
            }

            private static IEnumerable<TestCaseData> GetColumnTestCases()
            {
                var list2x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 4, 8 },
                                 { 3, 9 },
                                 { 7, 2 } });
                list2x3.IncreaseCapacity(list2x3.Boundaries);

                var firstColumn = new RefColumn<int, List2D<int>>(
                    CreateList2DWithBounds<int>(3, 1), 0);
                firstColumn[0] = 4;
                firstColumn[1] = 3;
                firstColumn[2] = 7;

                var lastColumn = new RefColumn<int, List2D<int>>(
                    CreateList2DWithBounds<int>(3, 1), 0);
                lastColumn[0] = 8;
                lastColumn[1] = 9;
                lastColumn[2] = 2;

                yield return new TestCaseData(list2x3, 0, firstColumn);
                yield return new TestCaseData(list2x3, 1, lastColumn);
            }
        }      
    }
}
