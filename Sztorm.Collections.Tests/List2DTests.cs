using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public partial class List2DTests
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public static void TestRows(int rows)
        {
            var list = new List2D<int>();
            list.AddRows(rows);

            Assert.AreEqual(list.Rows, rows);
            Assert.AreEqual(list.Length1, rows);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public static void TestColumns(int columns)
        {
            var list = new List2D<int>();
            list.AddColumns(columns);

            Assert.AreEqual(list.Columns, columns);
            Assert.AreEqual(list.Length2, columns);
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(4, 2)]
        [TestCase(1, 6)]
        public static void TestCount(int rows, int columns)
        {
            var list = new List2D<int>();
            list.AddRows(rows);
            list.AddColumns(columns);

            Assert.AreEqual(list.Count, rows * columns);
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(4, 2)]
        [TestCase(1, 6)]
        public static void TestCapacity(int rows, int columns)
        {
            var capacity = new Bounds2D(rows, columns);
            var list = new List2D<int>(capacity);

            Assert.AreEqual(list.Capacity, capacity);
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(4, 2)]
        [TestCase(1, 6)]
        public static void TestBoundaries(int rows, int columns)
        {
            var list = new List2D<int>(rows, columns);
            list.AddRows(rows);
            list.AddColumns(columns);

            Assert.AreEqual(list.Boundaries, new Bounds2D(rows, columns));
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(4, 2)]
        [TestCase(1, 6)]
        public static void IsEmptyReturnsTrueWhenListIsInitializedWithSpecificCapacity(
            int rowsCap, int colsCap)
        {
            var list = new List2D<int>(rowsCap, colsCap);

            Assert.True(list.IsEmpty);
        }

        [Test]
        public static void IsEmptyReturnsTrueWhenListIsInitializedWithParameterlessConstructor()
        {
            var list = new List2D<int>();

            Assert.True(list.IsEmpty);
        }

        [TestCaseSource(nameof(NonEmptyListsOfValues))]
        [TestCaseSource(nameof(NonEmptyListsOfReferences))]
        public static void IsEmptyReturnsTrueWhenListIsCleared<T>(List2D<T> nonEmptyList)
        {
            nonEmptyList.Clear();

            Assert.True(nonEmptyList.IsEmpty);
        }

        [TestCaseSource(nameof(IndexerTestCases))]
        public static void TestIndexer(List2D<int> list, Index2D index, int expected)
        {
            Assert.AreEqual(expected, list[index]);
        }

        [TestCaseSource(nameof(IndexerInvalidTestCases))]
        public static void IndexerThrowsExceptionIfIndexIsOutOfBounds(
            List2D<int> list, Index2D index)
        {
            TestDelegate testMethod = () => { int value = list[index]; };

            Assert.Throws<IndexOutOfRangeException>(testMethod);
        }

        [TestCaseSource(nameof(AddRowsTestCases))]
        public static void TestAddRows(List2D<int> list, int count, List2D<int> expected)
        {
            list.AddRows(count);

            Assert.AreEqual(list.Boundaries, expected.Boundaries);
            Assert.That(list.Capacity.Rows, Is.GreaterThanOrEqualTo(list.Rows));
            Assert.That(list.Capacity.Columns, Is.GreaterThanOrEqualTo(list.Columns));
            CollectionAssert.AreEqual(expected, list);
        }

        [TestCaseSource(nameof(AddColumnsTestCases))]
        public static void TestAddColumns(List2D<int> list, int count, List2D<int> expected)
        {
            list.AddColumns(count);

            Assert.AreEqual(list.Boundaries, expected.Boundaries);
            Assert.That(list.Capacity.Rows, Is.GreaterThanOrEqualTo(list.Rows));
            Assert.That(list.Capacity.Columns, Is.GreaterThanOrEqualTo(list.Columns));
            CollectionAssert.AreEqual(expected, list);
        }

        [TestCaseSource(nameof(NonEmptyListsOfValues))]
        [TestCaseSource(nameof(NonEmptyListsOfReferences))]
        public static void TestClear<T>(List2D<T> list)
        {
            list.Clear();

            Assert.AreEqual(list.Boundaries, new Bounds2D(0, 0));
        }

        [TestCaseSource(nameof(NonEmptyListsOfReferences))]
        public static void ClearReleasesReferences<T>(List2D<T> list) where T: class
        {
            if (list.IsEmpty)
            {
                Assert.Pass("Empty list passed in. List<T>.Clear will do nothing.");
            }
            list.Clear();

            for (int i = 0; i < list.Capacity.Rows; i++)
            {
                for (int j = 0; j < list.Capacity.Columns; j++)
                {
                    ref T clearedReference = ref list.GetItemInternal(i, j);
                    Assert.AreEqual(null, clearedReference);
                }
            }         
        }

        [TestCaseSource(nameof(RemoveRowsTestCases))]
        public static void RemoveRowsTest<T>(
            List2D<T> list, int startIndex, int count, List2D<T> expected)
        {
            list.RemoveRows(startIndex, count);
            CollectionAssert.AreEqual(expected, list);
        }

        [TestCaseSource(nameof(RemoveRowsReleasesUnusedReferencesTestCases))]
        public static void RemoveRowsReleasesUnusedReferences<T>(
            List2D<T> list,
            int startIndex,
            int count,
            Index2D firstUnusedRefIndex,
            Bounds2D unusedRefsQuantity
            ) where T: class
        {
            if (list.IsEmpty)
            {
                Assert.Pass("Empty list passed in. List<T>.Remove will do nothing.");
            }
            list.RemoveRows(startIndex, count);

            for (int i = firstUnusedRefIndex.Row,
                     rows = i + unusedRefsQuantity.Rows; i < rows; i++)
            {
                for (int j = firstUnusedRefIndex.Column,
                         cols = j + unusedRefsQuantity.Columns; j < cols; j++)
                {
                    ref T unusedRef = ref list.GetItemInternal(i, j);

                    Assert.AreEqual(null, unusedRef, $"Actual differs at {new Index2D(i, j)}.");
                }
            }
        }

        [TestCaseSource(nameof(RemoveRowsInvalidStartIndexTestCases))]
        public static void RemoveRowsThrowsExceptionIfStartIndexIsInvalid<T>(
            List2D<T> list, int startIndex, int count)
        {
            TestDelegate testMethod = () => list.RemoveRows(startIndex, count);

            Assert.Throws<ArgumentOutOfRangeException>(testMethod);
        }

        [TestCaseSource(nameof(RemoveRowsInvalidCountTestCases))]
        public static void RemoveRowsThrowsExceptionIfCountIsInvalid<T>(
            List2D<T> list, int startIndex, int count)
        {
            TestDelegate testMethod = () => list.RemoveRows(startIndex, count);

            Assert.Throws<ArgumentOutOfRangeException>(testMethod);
        }

        [TestCaseSource(nameof(RemoveColumnsTestCases))]
        public static void RemoveColumnsTest<T>(
            List2D<T> list, int startIndex, int count, List2D<T> expected)
        {
            list.RemoveColumns(startIndex, count);
            CollectionAssert.AreEqual(expected, list);
        }

        [TestCaseSource(nameof(RemoveColumnsInvalidStartIndexTestCases))]
        public static void RemoveColumnsThrowsExceptionIfStartIndexIsInvalid<T>(
            List2D<T> list, int startIndex, int count)
        {
            TestDelegate testMethod = () => list.RemoveColumns(startIndex, count);

            Assert.Throws<ArgumentOutOfRangeException>(testMethod);
        }

        [TestCaseSource(nameof(RemoveColumnsInvalidCountTestCases))]
        public static void RemoveColumnsThrowsExceptionIfCountIsInvalid<T>(
            List2D<T> list, int startIndex, int count)
        {
            TestDelegate testMethod = () => list.RemoveColumns(startIndex, count);

            Assert.Throws<ArgumentOutOfRangeException>(testMethod);
        }

        [TestCaseSource(nameof(RemoveColumnsReleasesUnusedReferencesTestCases))]
        public static void RemoveColumnsReleasesUnusedReferences<T>(
            List2D<T> list,
            int startIndex,
            int count,
            Index2D firstUnusedRefIndex,
            Bounds2D unusedRefsQuantity
            ) where T : class
        {
            if (list.IsEmpty)
            {
                Assert.Pass("Empty list passed in. List<T>.Remove will do nothing.");
            }
            list.RemoveColumns(startIndex, count);

            for (int i = firstUnusedRefIndex.Row,
                     rows = i + unusedRefsQuantity.Rows; i < rows; i++)
            {
                for (int j = firstUnusedRefIndex.Column,
                         cols = j + unusedRefsQuantity.Columns; j < cols; j++)
                {
                    ref T unusedRef = ref list.GetItemInternal(i, j);

                    Assert.AreEqual(null, unusedRef, $"Actual differs at {new Index2D(i, j)}.");
                }
            }
        }

        private static IEnumerable<TestCaseData> IndexerTestCases()
        {
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },                              
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } })),                        
                new Index2D(1, 1),
                9);
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5, 8 },
                                     { 4, 9, 1, 5 },
                                     { 8, 2, 3, 0 } })),
                new Index2D(2, 0),
                8);
        }

        private static IEnumerable<TestCaseData> IndexerInvalidTestCases()
        {
            // Index has row component less than zero.
            yield return new TestCaseData(
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 },
                                                                            { 4, 9, 1 },
                                                                            { 8, 2, 3 } })),
                                                               new Index2D(-1, 0));

            // Index has column component less than zero.
            yield return new TestCaseData(
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 },
                                                                            { 4, 9, 1 },
                                                                            { 8, 2, 3 } })),
                                                               new Index2D(0, -1));

            // Index exceeding rows count.
            yield return new TestCaseData(
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5, 8 },
                                                                            { 4, 9, 1, 5 },
                                                                            { 8, 2, 3, 0 } })),
                                                               new Index2D(3, 0));

            // Index exceeding column count.
            yield return new TestCaseData(
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5, 8 },
                                                                            { 4, 9, 1, 5 },
                                                                            { 8, 2, 3, 0 } })),
                                                               new Index2D(0, 4));
        }

        private static IEnumerable<TestCaseData> AddRowsTestCases()
        {
            yield return new TestCaseData(
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 } })),
                                                               2,
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 },
                                                                            { 0, 0, 0 },
                                                                            { 0, 0, 0 } })));
            yield return new TestCaseData(
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 } })),
                                                               0,
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 } })));
        }

        private static IEnumerable<TestCaseData> AddColumnsTestCases()
        {
            yield return new TestCaseData(
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2 },
                                                                            { 4 },
                                                                            { 8 } })),
                                                               2,
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2, 0, 0 },
                                                                            { 4, 0, 0 },
                                                                            { 8, 0, 0 } })));
            yield return new TestCaseData(
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2 },
                                                                            { 4 },
                                                                            { 8 } })),
                                                               0,
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2 },
                                                                            { 4 },
                                                                            { 8 } })));
        }

        private static IEnumerable<TestCaseData> RemoveRowsTestCases()
        {
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } })),
                1,
                1,
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 8, 2, 3 } })));
            yield return new TestCaseData(
                new List2D<string>(
                    Array2D<string>.FromSystem2DArray(
                        new string[,] { { "2", "3" },
                                        { "4", "8" },
                                        { "1", "0" },
                                        { "8", "2" } })),
                0,
                2,
                new List2D<string>(
                    Array2D<string>.FromSystem2DArray(
                        new string[,] { { "1", "0" },
                                        { "8", "2" } })));
            yield return new TestCaseData(
                new List2D<int>(TestUtils.IncrementedIntArray2D(2, 2)),
                0,
                2,
                new List2D<int>());
            yield return new TestCaseData(
                new List2D<int>(TestUtils.IncrementedIntArray2D(2, 2)),
                0,
                0,
                new List2D<int>(TestUtils.IncrementedIntArray2D(2, 2)));
        }

        private static IEnumerable<TestCaseData> RemoveRowsReleasesUnusedReferencesTestCases()
        {
            yield return new TestCaseData(
                new List2D<string>(
                    Array2D<string>.FromSystem2DArray(
                        new string[,] { { "2", "3" },
                                        { "4", "8" },
                                        { "1", "0" },
                                        { "8", "2" } })),
                0,
                2,
                new Index2D(2, 0),
                new Bounds2D(2, 2));
            yield return new TestCaseData(
                new List2D<string>(
                    Array2D<string>.FromSystem2DArray(
                        new string[,] { { "2", "3" },
                                        { "4", "8" },
                                        { "1", "0" },
                                        { "8", "2" } })),
                1,
                3,
                new Index2D(1, 0),
                new Bounds2D(3, 2));
        }       

        private static IEnumerable<TestCaseData> RemoveRowsInvalidStartIndexTestCases()

        {
            // StartIndex less than zero.
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } })),
                -1,
                0);
            // StartIndex greater or equal to list.Rows.
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } })),
                3,
                0);
        }

        private static IEnumerable<TestCaseData> RemoveRowsInvalidCountTestCases()
        {
            // Count less than zero.
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } })),
                0,
                -1);
            // Count greater than (list.Rows - StartIndex).
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } })),
                2,
                2);
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } })),
                0,
                4);
        }

        private static IEnumerable<TestCaseData> RemoveColumnsTestCases()
        {
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } })),
                1,
                1,
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 5 },
                                     { 4, 1 },
                                     { 8, 3 } })));
            yield return new TestCaseData(
                new List2D<string>(
                    Array2D<string>.FromSystem2DArray(
                        new string[,] { { "2", "0", "3", "1" },
                                        { "4", "2", "8", "7" } })),
                0,
                2,
                new List2D<string>(
                    Array2D<string>.FromSystem2DArray(
                        new string[,] { { "3", "1" },
                                        { "8", "7" } })));
            yield return new TestCaseData(
                new List2D<int>(TestUtils.IncrementedIntArray2D(2, 2)),
                0,
                2,
                new List2D<int>());
            yield return new TestCaseData(
                new List2D<int>(TestUtils.IncrementedIntArray2D(2, 2)),
                0,
                0,
                new List2D<int>(TestUtils.IncrementedIntArray2D(2, 2)));
        }

        private static IEnumerable<TestCaseData> RemoveColumnsInvalidStartIndexTestCases()

        {
            // StartIndex less than zero.
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } })),
                -1,
                0);
            // StartIndex greater or equal to list.Columns.
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } })),
                3,
                0);
        }

        private static IEnumerable<TestCaseData> RemoveColumnsInvalidCountTestCases()
        {
            // Count less than zero.
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } })),
                0,
                -1);
            // Count greater than (list.Columns - StartIndex).
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } })),
                2,
                2);
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } })),
                0,
                4);
        }

        private static IEnumerable<TestCaseData> RemoveColumnsReleasesUnusedReferencesTestCases()
        {
            yield return new TestCaseData(
                new List2D<string>(
                    Array2D<string>.FromSystem2DArray(
                        new string[,] { { "2", "3", "1", "0" },
                                        { "4", "8", "8", "2" } })),
                0,
                2,
                new Index2D(0, 2),
                new Bounds2D(2, 2));
            yield return new TestCaseData(
                new List2D<string>(
                    Array2D<string>.FromSystem2DArray(
                        new string[,] { { "2", "3", "1", "7" },
                                        { "4", "8", "5", "2" },
                                        { "2", "6", "3", "9" } })),
                1,
                2,
                new Index2D(0, 2),
                new Bounds2D(3, 2));
        }

        private static IEnumerable<TestCaseData> NonEmptyListsOfValues()
        {
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 },
                                                                { 4, 9, 1 },
                                                                { 8, 2, 3 } })));
            yield return new TestCaseData(
                new List2D<int>(TestUtils.IncrementedIntArray2D(2, 5)));
        }

        private static IEnumerable<TestCaseData> NonEmptyListsOfReferences()
        {
            yield return new TestCaseData(
                new List2D<string>(
                    Array2D<string>.FromSystem2DArray(
                        new string[,] { { "2", "3", "5" },
                                        { "4", "9", "1" },
                                        { "8", "2", "3" } })));
        }
    }
}
