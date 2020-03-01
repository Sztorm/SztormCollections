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

        [TestCaseSource(nameof(IndexerTestCases))]
        public static void TestIndexer(List2D<int> list, Index2D index, int expected)
        {
            Assert.AreEqual(list[index], expected);
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
            CollectionAssert.AreEqual(list, expected);
        }

        [TestCaseSource(nameof(AddColumnsTestCases))]
        public static void TestAddColumns(List2D<int> list, int count, List2D<int> expected)
        {
            list.AddColumns(count);

            Assert.AreEqual(list.Boundaries, expected.Boundaries);
            Assert.That(list.Capacity.Rows, Is.GreaterThanOrEqualTo(list.Rows));
            Assert.That(list.Capacity.Columns, Is.GreaterThanOrEqualTo(list.Columns));
            CollectionAssert.AreEqual(list, expected);
        }

        [TestCaseSource(nameof(ClearTestCases))]
        public static void TestClear(List2D<int> list)
        {
            list.Clear();

            Assert.AreEqual(list.Boundaries, new Bounds2D(0, 0));
        }

        private static IEnumerable<TestCaseData> IndexerTestCases()
        {
            yield return new TestCaseData(
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 },
                                                                          { 4, 9, 1 },
                                                                          { 8, 2, 3 } })),
                                                             new Index2D(1, 1),
                                                             9);
            yield return new TestCaseData(
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5, 8 },
                                                                          { 4, 9, 1, 5 },
                                                                          { 8, 2, 3, 0 } })),
                                                             new Index2D(2, 0),
                                                             8);
        }

        private static IEnumerable<TestCaseData> IndexerInvalidTestCases()
        {
            // Index has row component lesser than zero.
            yield return new TestCaseData(
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 },
                                                                            { 4, 9, 1 },
                                                                            { 8, 2, 3 } })),
                                                               new Index2D(-1, 0));

            // Index has column component lesser than zero.
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

        private static IEnumerable<TestCaseData> ClearTestCases()
        {
            yield return new TestCaseData(
                new List2D<int>(Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 },
                                                                            { 4, 9, 1 },
                                                                            { 8, 2, 3 } })));
            yield return new TestCaseData(
                new List2D<int>(TestsUtils.IncrementedIntArray2D(2, 5)));
        }
    }
}
