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
        [TestCaseSource(nameof(RemoveRowsTestCases))]
        public static void RemoveRowsTest<T>(
            List2D<T> list, int startIndex, int count, List2D<T> expected)
        {
            list.RemoveRows(startIndex, count);

            Assert.AreEqual(expected.Rows, list.Rows);
            CollectionAssert.AreEqual(expected, list);
        }

        [TestCaseSource(nameof(RemoveRowsReleasesUnusedReferencesTestCases))]
        public static void RemoveRowsReleasesUnusedReferences<T>(
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

            Assert.AreEqual(expected.Columns, list.Columns);
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
    }
}
