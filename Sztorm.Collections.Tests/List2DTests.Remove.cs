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
    partial class List2DTests
    {
        public static class Remove
        {
            public static class Rows
            {
                [TestCaseSource(typeof(Remove), nameof(RowsInvalidStartIndexTestCases))]
                public static void ThrowsExceptionIfStartIndexIsInvalid<T>(
                    List2D<T> list, int startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.RemoveRows(startIndex, count));

                [TestCaseSource(typeof(Remove), nameof(RowsInvalidCountTestCases))]
                public static void ThrowsExceptionIfCountIsInvalid<T>(
                    List2D<T> list, int startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.RemoveRows(startIndex, count));

                [TestCaseSource(typeof(Remove), nameof(RowsTestCases))]
                public static void Test<T>(
                    List2D<T> list, int startIndex, int count, List2D<T> expected)
                {
                    list.RemoveRows(startIndex, count);

                    Assert.AreEqual(expected.Rows, list.Rows);
                    Assert.AreEqual(expected.Count, list.Count);
                    CollectionAssert.AreEqual(expected, list);
                }

                [TestCaseSource(typeof(Remove), nameof(RowsReleasesUnusedReferencesTestCases))]
                public static void ReleasesUnusedReferences<T>(
                    List2D<T> list,
                    int startIndex,
                    int count,
                    Index2D firstUnusedRefIndex,
                    Bounds2D unusedRefsQuantity)
                    where T : class
                {
                    if (list.IsEmpty)
                    {
                        Assert.Pass("Empty list passed in. List<T>.Remove will do nothing.");
                    }
                    list.RemoveRows(startIndex, count);
                    CheckUnusedReferences(list, firstUnusedRefIndex, unusedRefsQuantity);
                }
            }

            public static class Columns
            {
                [TestCaseSource(typeof(Remove), nameof(ColumnsInvalidStartIndexTestCases))]
                public static void ThrowsExceptionIfStartIndexIsInvalid<T>(
                    List2D<T> list, int startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.RemoveColumns(startIndex, count));

                [TestCaseSource(typeof(Remove), nameof(ColumnsInvalidCountTestCases))]
                public static void ThrowsExceptionIfCountIsInvalid<T>(
                    List2D<T> list, int startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.RemoveColumns(startIndex, count));

                [TestCaseSource(typeof(Remove), nameof(ColumnsTestCases))]
                public static void Test<T>(
                    List2D<T> list, int startIndex, int count, List2D<T> expected)
                {
                    list.RemoveColumns(startIndex, count);

                    Assert.AreEqual(expected.Columns, list.Columns);
                    Assert.AreEqual(expected.Count, list.Count);
                    CollectionAssert.AreEqual(expected, list);
                }

                [TestCaseSource(typeof(Remove), nameof(ColumnsReleasesUnusedReferencesTestCases))]
                public static void ReleasesUnusedReferences<T>(
                    List2D<T> list,
                    int startIndex,
                    int count,
                    Index2D firstUnusedRefIndex,
                    Bounds2D unusedRefsQuantity)
                    where T : class
                {
                    if (list.IsEmpty)
                    {
                        Assert.Pass("Empty list passed in. List<T>.Remove will do nothing.");
                    }
                    list.RemoveColumns(startIndex, count);
                    CheckUnusedReferences(list, firstUnusedRefIndex, unusedRefsQuantity);
                }
            }

            private static IEnumerable<TestCaseData> RowsInvalidStartIndexTestCases()
            {
                yield return new TestCaseData(CreateList2DWithBounds<byte>(3, 3), -1, 0);
                yield return new TestCaseData(CreateList2DWithBounds<byte>(3, 3), 3, 0);
            }

            private static IEnumerable<TestCaseData> RowsInvalidCountTestCases()
            {
                yield return new TestCaseData(CreateList2DWithBounds<byte>(3, 3), 0, -1);
                yield return new TestCaseData(CreateList2DWithBounds<byte>(3, 3), 0, 4);
                yield return new TestCaseData(CreateList2DWithBounds<byte>(3, 3), 2, 2);
            }

            private static IEnumerable<TestCaseData> RowsTestCases()
            {
                var list1 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });
                list1.IncreaseCapacity(list1.Boundaries);

                var list2 = List2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "8" },
                                    { "1", "0" },
                                    { "8", "2" } });
                list2.IncreaseCapacity(list2.Boundaries);

                var list3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 1, 2 },
                                 { 3, 4 } });
                list3.IncreaseCapacity(list3.Boundaries);

                var list4 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 1, 2 },
                                 { 3, 4 } });
                list4.IncreaseCapacity(list4.Boundaries);

                yield return new TestCaseData(
                    list1, 1, 1, List2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 8, 2, 3 } }));
                yield return new TestCaseData(
                    list2, 0, 2, List2D<string>.FromSystem2DArray(
                        new string[,] { { "1", "0" },
                                        { "8", "2" } }));
                yield return new TestCaseData(
                    list3, 0, 0, List2D<int>.FromSystem2DArray(
                        new int[,] { { 1, 2 },
                                     { 3, 4 } }));
                yield return new TestCaseData(list4, 0, 2, new List2D<int>());
            }

            private static IEnumerable<TestCaseData> RowsReleasesUnusedReferencesTestCases()
            {
                var list1 = List2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "8" },
                                    { "1", "0" },
                                    { "8", "2" } });
                list1.IncreaseCapacity(list1.Boundaries);

                var list2 = List2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "8" },
                                    { "1", "0" },
                                    { "8", "2" } });
                list2.IncreaseCapacity(list2.Boundaries);

                yield return new TestCaseData(list1, 0, 2, new Index2D(2, 0), new Bounds2D(2, 2));
                yield return new TestCaseData(list2, 1, 3, new Index2D(1, 0), new Bounds2D(3, 2));
            }

            private static IEnumerable<TestCaseData> ColumnsInvalidStartIndexTestCases()
            {
                yield return new TestCaseData(CreateList2DWithBounds<byte>(3, 3), -1, 0);
                yield return new TestCaseData(CreateList2DWithBounds<byte>(3, 3), 3, 0);
            }

            private static IEnumerable<TestCaseData> ColumnsInvalidCountTestCases()
            {
                yield return new TestCaseData(CreateList2DWithBounds<byte>(3, 3), 0, -1);
                yield return new TestCaseData(CreateList2DWithBounds<byte>(3, 3), 0, 4);
                yield return new TestCaseData(CreateList2DWithBounds<byte>(3, 3), 2, 2);
            }

            private static IEnumerable<TestCaseData> ColumnsTestCases()
            {
                var list1 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });
                list1.IncreaseCapacity(list1.Boundaries);

                var list2 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });

                var list3 = List2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "0", "3", "1" },
                                    { "4", "2", "8", "7" } });
                list3.IncreaseCapacity(list3.Boundaries);

                var list4 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 1, 2 },
                                 { 3, 4 } });
                list4.IncreaseCapacity(list4.Boundaries);

                var list5 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 1, 2 },
                                 { 3, 4 } });
                list5.IncreaseCapacity(list5.Boundaries);

                yield return new TestCaseData(
                    list1, 1, 1, List2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 5 },
                                     { 4, 1 },
                                     { 8, 3 } }));
                yield return new TestCaseData(
                    list2, 1, 1, List2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 5 },
                                     { 4, 1 },
                                     { 8, 3 } }));
                yield return new TestCaseData(
                    list3, 0, 2, List2D<string>.FromSystem2DArray(
                        new string[,] { { "3", "1" },
                                        { "8", "7" } }));
                yield return new TestCaseData(
                    list4, 0, 0, List2D<int>.FromSystem2DArray(
                        new int[,] { { 1, 2 },
                                     { 3, 4 } }));
                yield return new TestCaseData(list5, 0, 2, new List2D<int>());
            }

            private static IEnumerable<TestCaseData> ColumnsReleasesUnusedReferencesTestCases()
            {
                var list1 = List2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "1", "0" },
                                    { "4", "8", "8", "2" } });
                list1.IncreaseCapacity(list1.Boundaries);

                var list2 = List2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "1", "0" },
                                    { "4", "8", "8", "2" } });

                var list3 = List2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "1", "7" },
                                    { "4", "8", "5", "2" },
                                    { "2", "6", "3", "9" } });
                list3.IncreaseCapacity(list3.Boundaries);

                yield return new TestCaseData(list1, 0, 2, new Index2D(0, 2), new Bounds2D(2, 2));
                yield return new TestCaseData(list2, 0, 2, new Index2D(0, 2), new Bounds2D(2, 2));
                yield return new TestCaseData(list3, 1, 2, new Index2D(0, 2), new Bounds2D(3, 2));
            }
        }
    }
}
