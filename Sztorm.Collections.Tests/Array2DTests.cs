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
