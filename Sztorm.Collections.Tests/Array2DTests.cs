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
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public static void TestRows(int rows)
        {
            var array = new Array2D<int>(rows, 1);

            Assert.AreEqual(array.Rows, rows);
            Assert.AreEqual(array.Length1, rows);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public static void TestColumns(int columns)
        {
            var array = new Array2D<int>(1, columns);

            Assert.AreEqual(array.Columns, columns);
            Assert.AreEqual(array.Length2, columns);
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(4, 2)]
        [TestCase(1, 6)]
        public static void TestCount(int rows, int columns)
        {
            var array = new Array2D<int>(rows, columns);

            Assert.AreEqual(array.Count, rows * columns);
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(4, 2)]
        [TestCase(1, 6)]
        public static void TestBoundaries(int rows, int columns)
        {
            var array = new Array2D<int>(rows, columns);

            Assert.AreEqual(array.Boundaries, new Bounds2D(rows, columns));
        }

        [TestCaseSource(typeof(Array2DTests), nameof(IndexerTestCases))]
        public static void TestIndexer<T>(Array2D<T> array, Index2D index, T expected)
            => Assert.AreEqual(array[index], expected);

        [TestCaseSource(typeof(Array2DTests), nameof(IndexerInvalidTestCases))]
        public static void IndexerThrowsExceptionIfIndexIsOutOfBounds<T>(
            Array2D<T> array, Index2D index)
        {
            TestDelegate testMethod = () => { T value = array[index]; };

            Assert.Throws<IndexOutOfRangeException>(testMethod);
        }

        [TestCaseSource(typeof(Array2DTests), nameof(GetRowInvalidTestCases))]
        public static void GetRowThrowExceptionIfIndexExceedsRows<T>(Array2D<T> array, int index)
        {
            TestDelegate testMethod = () => array.GetRow(index);

            Assert.Throws<ArgumentOutOfRangeException>(testMethod);
        }

        [TestCaseSource(typeof(Array2DTests), nameof(GetColumnInvalidTestCases))]
        public static void GetColumnThrowExceptionIfIndexExceedsColumns<T>(
            Array2D<T> array, int index)
        {
            TestDelegate testMethod = () => array.GetColumn(index);

            Assert.Throws<ArgumentOutOfRangeException>(testMethod);
        }

        private static IEnumerable<TestCaseData> IndexerTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 },
                                                            { 4, 9, 1 },
                                                            { 8, 2, 3 } }),
                                               new Index2D(1, 1),
                                               9);
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5, 8 },
                                                            { 4, 9, 1, 5 },
                                                            { 8, 2, 3, 0 } }),
                                               new Index2D(2, 0),
                                               8);
        }

        private static IEnumerable<TestCaseData> IndexerInvalidTestCases()
        {
            // Index has row component lesser than zero.
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 },
                                                                { 4, 9, 1 },
                                                                { 8, 2, 3 } }),
                                               new Index2D(-1, 0));

            // Index has column component lesser than zero.
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 },
                                                                { 4, 9, 1 },
                                                                { 8, 2, 3 } }),
                                               new Index2D(0, -1));

            // Index exceeding rows count.
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5, 8 },
                                                            { 4, 9, 1, 5 },
                                                            { 8, 2, 3, 0 } }),
                                               new Index2D(3, 0));

            // Index exceeding column count.
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5, 8 },
                                                            { 4, 9, 1, 5 },
                                                            { 8, 2, 3, 0 } }),
                                               new Index2D(0, 4));
        }

        private static IEnumerable<TestCaseData> GetRowInvalidTestCases()
        {
            // Row index lesser than zero.
            yield return new TestCaseData(
                new Array2D<int>(2, 1),
                -1);

            // Row index greater or equal to array rows property.
            yield return new TestCaseData(
                new Array2D<int>(2, 1),
                2);

            // Row index greater or equal to array rows property.
            yield return new TestCaseData(
                new Array2D<int>(0, 1),
                0);
        }

        private static IEnumerable<TestCaseData> GetColumnInvalidTestCases()
        {
            // Column index lesser than zero.
            yield return new TestCaseData(
                new Array2D<int>(1, 2),
                -1);

            // Column index greater or equal to array columns property.
            yield return new TestCaseData(
                new Array2D<int>(1, 2),
                2);

            // Column index greater or equal to array columns property.
            yield return new TestCaseData(
                new Array2D<int>(1, 0),
                0);
        }
    }
}
