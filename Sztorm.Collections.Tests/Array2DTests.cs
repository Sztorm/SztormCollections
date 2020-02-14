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
            Array2D<int> array = new Array2D<int>(rows, 1);

            Assert.AreEqual(array.Rows, rows);
            Assert.AreEqual(array.Length1, rows);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public static void TestColumns(int columns)
        {
            Array2D<int> array = new Array2D<int>(1, columns);

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
            Array2D<int> array = new Array2D<int>(rows, columns);

            Assert.AreEqual(array.Count, rows * columns);
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(4, 2)]
        [TestCase(1, 6)]
        public static void TestBoundaries(int rows, int columns)
        {
            Array2D<int> array = new Array2D<int>(rows, columns);

            Assert.AreEqual(array.Boundaries, new Array2DBounds(rows, columns));
        }

        [TestCaseSource(nameof(Array2DRowTestCases))]
        public static void TestArray2DRowEquality(Array2D<int>.Row actual, int[] expected)
        {
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestCaseSource(nameof(Array2DColumnTestCases))]
        public static void TestArray2DColumnEquality(Array2D<int>.Column actual, int[] expected)
        {
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestCaseSource(nameof(IndicesOfTestCases))]
        public static void IndicesOfTest(Array2D<int> array, int valueToFind, Index2D? expected)
        {
            Assert.AreEqual(expected, array.IndicesOf(valueToFind));
        }

        private static IEnumerable<TestCaseData> Array2DRowTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 },
                                                            { 4, 9, 1 },
                                                            { 8, 2, 3 } }).GetRow(1),
                                               new int[] { 4, 9, 1 });
        }

        private static IEnumerable<TestCaseData> Array2DColumnTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 },
                                                            { 4, 9, 1 },
                                                            { 8, 2, 3 } }).GetColumn(1),
                                               new int[] { 3, 9, 2 });
        }

        private static IEnumerable<TestCaseData> IndicesOfTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 5 },
                                                            { 4, 9, 1 } }),
                                               9,
                                               new Nullable<Index2D>(new Index2D(1, 1)));
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3 },
                                                            { 4, 9 },
                                                            { 3, 6 } }),
                                               3,
                                               new Nullable<Index2D>(new Index2D(0, 1)));
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3 },
                                                            { 4, 9 },
                                                            { 3, 6 } }),
                                               8,
                                               new Index2D?());
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3 },
                                                            { 4, 9 },
                                                            { 3, 6 } }),
                                               7,
                                               new Index2D?());
        }
    }
}
