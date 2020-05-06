using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    partial class List2DTests
    {
        public static class Insert
        {
            public static class Rows
            {
                [TestCaseSource(typeof(Insert), nameof(RowsInvalidStartIndexTestCases))]
                public static void ThrowsExceptionIfStartIndexIsInvalid<T>(
                    List2D<T> list, int startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.InsertRows(startIndex, count));

                [TestCaseSource(typeof(Insert), nameof(RowsInvalidCountTestCases))]
                public static void ThrowsExceptionIfCountIsInvalid<T>(
                    List2D<T> list, int startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.InsertRows(startIndex, count));

                [TestCaseSource(typeof(Insert), nameof(RowsTestCases))]
                public static void RowsTest<T>(
                    List2D<T> list, int startIndex, int count, List2D<T> expected)
                {
                    list.InsertRows(startIndex, count);

                    Assert.AreEqual(expected.Boundaries, list.Boundaries);
                    CollectionAssert.AreEqual(expected, list);
                }

                [TestCaseSource(typeof(Insert), nameof(RowsTestCases))]
                public static void NoAllocationTest<T>(
                    List2D<T> list, int startIndex, int count, List2D<T> expected)
                {
                    list.IncreaseCapacity(expected.Boundaries);
                    list.InsertRows(startIndex, count);

                    Assert.AreEqual(expected.Boundaries, list.Boundaries);
                    CollectionAssert.AreEqual(expected, list);
                }
            }

            public static class Columns
            {
                [TestCaseSource(typeof(Insert), nameof(ColumnsTestCases))]
                public static void Test<T>(
                    List2D<T> list, int startIndex, int count, List2D<T> expected)
                {
                    list.InsertColumns(startIndex, count);

                    Assert.AreEqual(expected.Boundaries, list.Boundaries);
                    CollectionAssert.AreEqual(expected, list);
                }

                [TestCaseSource(typeof(Insert), nameof(ColumnsTestCases))]
                public static void NoAllocationTest<T>(
                    List2D<T> list, int startIndex, int count, List2D<T> expected)
                {
                    list.IncreaseCapacity(expected.Boundaries);
                    list.InsertColumns(startIndex, count);

                    Assert.AreEqual(expected.Boundaries, list.Boundaries);
                    CollectionAssert.AreEqual(expected, list);
                }

                [TestCaseSource(typeof(Insert), nameof(ColumnsInvalidStartIndexTestCases))]
                public static void ThrowsExceptionIfStartIndexIsInvalid<T>(
                    List2D<T> list, int startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.InsertColumns(startIndex, count));

                [TestCaseSource(typeof(Insert), nameof(ColumnsInvalidCountTestCases))]
                public static void ThrowsExceptionIfCountIsInvalid<T>(
                    List2D<T> list, int startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.InsertColumns(startIndex, count));
            }

            private static IEnumerable<TestCaseData> RowsInvalidStartIndexTestCases()
            {
                var list3x2 = new List2D<byte>(7, 8);
                list3x2.AddRows(3);
                list3x2.AddColumns(2);

                yield return new TestCaseData(list3x2, -1, 0);
                yield return new TestCaseData(list3x2, 4, 0);
            }

            private static IEnumerable<TestCaseData> RowsInvalidCountTestCases()
            {
                var list3x2 = new List2D<byte>(7, 8);
                list3x2.AddRows(3);
                list3x2.AddColumns(2);

                yield return new TestCaseData(list3x2, 0, -1);
                yield return new TestCaseData(list3x2, 0, 4);
                yield return new TestCaseData(list3x2, 1, 3);
            }

            private static IEnumerable<TestCaseData> RowsTestCases()
            {
                yield return new TestCaseData(
                    List2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    1,
                    1,
                    List2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5 },
                                         { default, default, default },
                                         { 4, 9, 1 },
                                         { 8, 2, 3 } }));
                yield return new TestCaseData(
                    List2D<string>.FromSystem2DArray(
                        new string[,] { { "2", "3" },
                                        { "4", "8" },
                                        { "1", "0" },
                                        { "8", "2" } }),
                    0,
                    2,
                    List2D<string>.FromSystem2DArray(
                        new string[,] { { default, default },
                                        { default, default },
                                        { "2", "3" },
                                        { "4", "8" },
                                        { "1", "0" },
                                        { "8", "2" } }));
                yield return new TestCaseData(
                    List2D<int>.FromSystem2DArray(
                        new int[,] { { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    2,
                    3,
                    List2D<int>.FromSystem2DArray(
                        new int[,] { { 4, 9, 1 },
                                     { 8, 2, 3 },
                                     { default, default, default },
                                     { default, default, default },
                                     { default, default, default } }));
                yield return new TestCaseData(
                    List2D<int>.FromSystem2DArray(
                        new int[,] { { 1, 2 },
                                     { 3, 4 } }),
                    0,
                    0,
                    List2D<int>.FromSystem2DArray(
                        new int[,] { { 1, 2 },
                                     { 3, 4 } }));
            }

            private static IEnumerable<TestCaseData> ColumnsInvalidStartIndexTestCases()
            {
                var list2x3 = new List2D<byte>(8, 7);
                list2x3.AddRows(2);
                list2x3.AddColumns(3);

                yield return new TestCaseData(list2x3, -1, 0);
                yield return new TestCaseData(list2x3, 4, 0);
            }

            private static IEnumerable<TestCaseData> ColumnsInvalidCountTestCases()
            {
                var list2x3 = new List2D<byte>(8, 7);
                list2x3.AddRows(2);
                list2x3.AddColumns(3);

                yield return new TestCaseData(list2x3, 0, -1);
                yield return new TestCaseData(list2x3, 0, 4);
                yield return new TestCaseData(list2x3, 1, 3);
            }

            private static IEnumerable<TestCaseData> ColumnsTestCases()
            {
                yield return new TestCaseData(
                    List2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    1,
                    1,
                    List2D<int>.FromSystem2DArray(
                        new int[,] { { 2, default, 3, 5 },
                                     { 4, default, 9, 1 },
                                     { 8, default, 2, 3 } }));
                yield return new TestCaseData(
                    List2D<string>.FromSystem2DArray(
                        new string[,] { { "2", "3" },
                                        { "4", "8" },
                                        { "1", "0" },
                                        { "8", "2" } }),
                    0,
                    2,
                    List2D<string>.FromSystem2DArray(
                        new string[,] { { default, default, "2", "3" },
                                        { default, default, "4", "8" },
                                        { default, default, "1", "0" },
                                        { default, default, "8", "2" } }));
                yield return new TestCaseData(
                    List2D<int>.FromSystem2DArray(
                        new int[,] { { 4, 9 },
                                     { 8, 2 } }),
                    2,
                    3,
                    List2D<int>.FromSystem2DArray(
                        new int[,] { { 4, 9, default, default, default },
                                     { 8, 2, default, default, default } }));
                yield return new TestCaseData(
                    List2D<int>.FromSystem2DArray(
                        new int[,] { { 1, 2 },
                                     { 3, 4 } }),
                    0,
                    0,
                    List2D<int>.FromSystem2DArray(
                        new int[,] { { 1, 2 },
                                     { 3, 4 } }));
            }
        }     
    }
}
