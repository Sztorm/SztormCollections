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
        [TestCaseSource(nameof(InsertRowsTestCases))]
        public static void InsertRowsTest<T>(
            List2D<T> list, int startIndex, int count, List2D<T> expected)
        {
            list.InsertRows(startIndex, count);

            Assert.AreEqual(expected.Boundaries, list.Boundaries);
            CollectionAssert.AreEqual(expected, list);
        }

        [TestCaseSource(nameof(InsertRowsTestCases))]
        public static void InsertRowsNoAllocationTest<T>(
            List2D<T> list, int startIndex, int count, List2D<T> expected)
        {
            list.IncreaseCapacity(expected.Boundaries);
            list.InsertRows(startIndex, count);

            Assert.AreEqual(expected.Boundaries, list.Boundaries);
            CollectionAssert.AreEqual(expected, list);
        }

        [TestCaseSource(nameof(InsertRowsInvalidStartIndexTestCases))]
        public static void InsertRowsThrowsExceptionIfStartIndexIsInvalid<T>(
            List2D<T> list, int startIndex, int count)
        {
            TestDelegate testMethod = () => list.InsertRows(startIndex, count);

            Assert.Throws<ArgumentOutOfRangeException>(testMethod);
        }

        [TestCaseSource(nameof(InsertRowsInvalidCountTestCases))]
        public static void InsertRowsThrowsExceptionIfCountIsInvalid<T>(
            List2D<T> list, int startIndex, int count)
        {
            TestDelegate testMethod = () => list.InsertRows(startIndex, count);

            Assert.Throws<ArgumentOutOfRangeException>(testMethod);
        }

        [TestCaseSource(nameof(InsertColumnsTestCases))]
        public static void InsertColumnsTest<T>(
            List2D<T> list, int startIndex, int count, List2D<T> expected)
        {
            Console.WriteLine("start");
            TestUtils.WriteTable(list);
            Console.WriteLine();

            list.InsertColumns(startIndex, count);

            TestUtils.WriteTable(list);
            Console.WriteLine();
            Console.WriteLine("exp");

            TestUtils.WriteTable(expected);

            Console.WriteLine("end");

            Assert.AreEqual(expected.Boundaries, list.Boundaries);
            CollectionAssert.AreEqual(expected, list);
        }

        [TestCaseSource(nameof(InsertColumnsTestCases))]
        public static void InsertColumnsNoAllocationTest<T>(
            List2D<T> list, int startIndex, int count, List2D<T> expected)
        {
            Console.WriteLine("start");
            TestUtils.WriteTable(list);
            Console.WriteLine();

            list.IncreaseCapacity(expected.Boundaries);
            list.InsertColumns(startIndex, count);

            TestUtils.WriteTable(list);
            Console.WriteLine();
            Console.WriteLine("exp");

            TestUtils.WriteTable(expected);

            Console.WriteLine("end");

            Assert.AreEqual(expected.Boundaries, list.Boundaries);
            CollectionAssert.AreEqual(expected, list);
        }

        [TestCaseSource(nameof(InsertColumnsInvalidStartIndexTestCases))]
        public static void InsertColumnsThrowsExceptionIfStartIndexIsInvalid<T>(
            List2D<T> list, int startIndex, int count)
        {
            TestDelegate testMethod = () => list.InsertColumns(startIndex, count);

            Assert.Throws<ArgumentOutOfRangeException>(testMethod);
        }

        [TestCaseSource(nameof(InsertColumnsInvalidCountTestCases))]
        public static void InsertColumnsThrowsExceptionIfCountIsInvalid<T>(
            List2D<T> list, int startIndex, int count)
        {
            TestDelegate testMethod = () => list.InsertColumns(startIndex, count);

            Assert.Throws<ArgumentOutOfRangeException>(testMethod);
        }

        private static IEnumerable<TestCaseData> InsertRowsTestCases()
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
                                     { default, default, default },
                                     { 4, 9, 1 },
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
                        new string[,] { { default, default },
                                        { default, default },
                                        { "2", "3" },
                                        { "4", "8" },
                                        { "1", "0" },
                                        { "8", "2" } })));
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 4, 9, 1 },
                                     { 8, 2, 3 } })),
                2,
                3,
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 4, 9, 1 },
                                     { 8, 2, 3 },
                                     { default, default, default },
                                     { default, default, default },
                                     { default, default, default } })));
            yield return new TestCaseData(
                new List2D<int>(TestUtils.IncrementedIntArray2D(2, 2)),
                0,
                0,
                new List2D<int>(TestUtils.IncrementedIntArray2D(2, 2)));
        }

        private static IEnumerable<TestCaseData> InsertRowsInvalidStartIndexTestCases()
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
            // StartIndex greater than list.Rows.
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } })),
                4,
                0);
        }

        private static IEnumerable<TestCaseData> InsertRowsInvalidCountTestCases()
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
        }

        private static IEnumerable<TestCaseData> InsertColumnsTestCases()
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
                        new int[,] { { 2, default, 3, 5 },
                                     { 4, default, 9, 1 },
                                     { 8, default, 2, 3 } })));
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
                        new string[,] { { default, default, "2", "3" },
                                        { default, default, "4", "8" },
                                        { default, default, "1", "0" },
                                        { default, default, "8", "2" } })));
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 4, 9 },
                                     { 8, 2 } })),
                2,
                3,
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 4, 9, default, default, default },
                                     { 8, 2, default, default, default } })));
            yield return new TestCaseData(
                new List2D<int>(TestUtils.IncrementedIntArray2D(2, 2)),
                0,
                0,
                new List2D<int>(TestUtils.IncrementedIntArray2D(2, 2)));
        }

        private static IEnumerable<TestCaseData> InsertColumnsInvalidStartIndexTestCases()
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
            // StartIndex greater than list.Columns.
            yield return new TestCaseData(
                new List2D<int>(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } })),
                4,
                0);
        }

        private static IEnumerable<TestCaseData> InsertColumnsInvalidCountTestCases()
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
        }
    }
}
