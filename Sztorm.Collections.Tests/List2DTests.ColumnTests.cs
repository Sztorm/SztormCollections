using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    partial class List2DTests
    {
        public class ColumnTests
        {
            [TestCase(0)]
            [TestCase(1)]
            [TestCase(5)]
            public static void TestCount(int listRows)
            {
                var list = new List2D<int>(listRows, 3);
                list.AddRows(listRows);
                list.AddColumns(3);

                Assert.AreEqual(listRows, list.GetColumn(0).Count);
                Assert.AreEqual(listRows, list.GetColumn(1).Count);
                Assert.AreEqual(listRows, list.GetColumn(2).Count);
            }

            [TestCaseSource(nameof(IndexerTestCases))]
            public static void TestIndexer(List2D<int>.Column column, int index, int expected)
            {
                Assert.AreEqual(column[index], expected);
            }

            [TestCaseSource(nameof(IndexerInvalidTestCases))]
            public static void IndexerThrowsExceptionIfIndexIsOutOfBounds(
                List2D<int>.Column column, int index)
            {
                TestDelegate testMethod = () => { int value = column[index]; };

                Assert.Throws<IndexOutOfRangeException>(testMethod);
            }

            [TestCaseSource(nameof(EqualityTestCases))]
            public static void TestEquality(List2D<int>.Column actual, int[] expected)
            {
                CollectionAssert.AreEqual(expected, actual);
            }

            [TestCaseSource(nameof(ReverseTestCases))]
            public static void TestReverse(List2D<int>.Column actual, int[] expected)
            {
                actual.Reverse();
                CollectionAssert.AreEqual(expected, actual);
            }

            [TestCaseSource(nameof(FillWithTestCases))]
            public static void TestFillWith(
                List2D<int>.Column actual, int value, int[] expected)
            {
                actual.FillWith(value);
                CollectionAssert.AreEqual(expected, actual);
            }

            private static IEnumerable<TestCaseData> IndexerTestCases()
            {
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5 },
                                         { 4, 9, 1 },
                                         { 8, 2, 3 } })).GetColumn(1),
                    2,
                    2);
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5, 8 },
                                         { 4, 9, 1, 5 },
                                         { 8, 2, 3, 0 } })).GetColumn(0),
                    2,
                    8);
            }

            private static IEnumerable<TestCaseData> IndexerInvalidTestCases()
            {
                // Index lesser than zero.
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5 },
                                         { 4, 9, 1 },
                                         { 8, 2, 3 } })).GetColumn(1),
                    -1);
                // Index exceeding column length.
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5, 8 },
                                         { 4, 9, 1, 5 },
                                         { 8, 2, 3, 0 } })).GetColumn(0),
                    3);
            }

            private static IEnumerable<TestCaseData> EqualityTestCases()
            {
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5 },
                                         { 4, 9, 1 },
                                         { 8, 2, 3 } })).GetColumn(1),
                    new int[] { 3, 9, 2 });
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5, 8 },
                                         { 4, 9, 1, 5 },
                                         { 8, 2, 3, 0 } })).GetColumn(0),
                    new int[] { 2, 4, 8 });
            }

            private static IEnumerable<TestCaseData> ReverseTestCases()
            {
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5 },
                                         { 4, 9, 1 },
                                         { 8, 2, 3 } })).GetColumn(1),
                    new int[] { 2, 9, 3 });
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 5, 4 } })).GetColumn(1),
                    new int[] { 4 });
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 5, 3 },
                                         { 4, 2} })).GetColumn(0),
                    new int[] { 4, 5 });
            }

            private static IEnumerable<TestCaseData> FillWithTestCases()
            {
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5 },
                                         { 4, 9, 1 },
                                         { 8, 2, 3 } })).GetColumn(1),
                    4,
                    new int[] { 4, 4, 4 });
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 6, },
                                         { 1, } })).GetColumn(0),
                    2,
                    new int[] { 2, 2 });
            }
        }
    }
}
