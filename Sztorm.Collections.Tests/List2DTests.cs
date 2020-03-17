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

            Assert.AreEqual(rows, list.Rows);
            Assert.AreEqual(rows, list.Length1);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public static void TestColumns(int columns)
        {
            var list = new List2D<int>();
            list.AddColumns(columns);

            Assert.AreEqual(columns, list.Columns);
            Assert.AreEqual(columns, list.Length2);
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

            Assert.AreEqual(rows * columns, list.Count);
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

            Assert.AreEqual(capacity, list.Capacity);
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

            Assert.AreEqual(new Bounds2D(rows, columns), list.Boundaries);
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

        [TestCaseSource(nameof(IncreaseCapacityTestCases))]
        public static void TestIncreaseCapacity(
            Bounds2D initialCapacity, Bounds2D quantity, Bounds2D expected)
        {
            var list = new List2D<int>(initialCapacity);
            list.IncreaseCapacity(quantity);

            Assert.AreEqual(expected, list.Capacity);
        }

        [TestCaseSource(nameof(AddRowsTestCases))]
        public static void TestAddRows(List2D<int> list, int count, List2D<int> expected)
        {
            list.AddRows(count);

            Assert.AreEqual(expected.Boundaries, list.Boundaries);
            Assert.That(list.Capacity.Rows, Is.GreaterThanOrEqualTo(list.Rows));
            Assert.That(list.Capacity.Columns, Is.GreaterThanOrEqualTo(list.Columns));
            CollectionAssert.AreEqual(expected, list);
        }

        [TestCaseSource(nameof(AddColumnsTestCases))]
        public static void TestAddColumns(List2D<int> list, int count, List2D<int> expected)
        {
            list.AddColumns(count);

            Assert.AreEqual(expected.Boundaries, list.Boundaries);
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
        public static void ClearReleasesReferences<T>(List2D<T> list) where T : class
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

        private static IEnumerable<TestCaseData> IncreaseCapacityTestCases()
        {
            yield return new TestCaseData(
                new Bounds2D(0, 0),
                new Bounds2D(2, 3),
                new Bounds2D(2, 3));
            yield return new TestCaseData(
                new Bounds2D(1, 6),
                new Bounds2D(4, 2),
                new Bounds2D(5, 8));
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
