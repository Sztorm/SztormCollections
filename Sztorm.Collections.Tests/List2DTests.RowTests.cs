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
        public class RowTests
        {
            [TestCase(0)]
            [TestCase(1)]
            [TestCase(5)]
            public static void TestCount(int listColumns)
            {
                var list = new List2D<int>(3, listColumns);
                list.AddRows(3);
                list.AddColumns(listColumns);

                Assert.AreEqual(listColumns, list.GetRow(0).Count);
                Assert.AreEqual(listColumns, list.GetRow(1).Count);
                Assert.AreEqual(listColumns, list.GetRow(2).Count);
            }

            [TestCaseSource(nameof(IndexerTestCases))]
            public static void TestIndexer(List2D<int>.Row row, int index, int expected)
            {
                Assert.AreEqual(row[index], expected);
            }

            [TestCaseSource(nameof(IndexerInvalidTestCases))]
            public static void IndexerThrowsExceptionIfIndexIsOutOfBounds(
                List2D<int>.Row row, int index)
            {
                TestDelegate testMethod = () => { int value = row[index]; };

                Assert.Throws<IndexOutOfRangeException>(testMethod);
            }

            [TestCaseSource(nameof(EqualityTestCases))]
            public static void TestEquality(List2D<int>.Row actual, int[] expected)
            {
                CollectionAssert.AreEqual(expected, actual);
            }

            [TestCaseSource(nameof(ReverseTestCases))]
            public static void TestReverse(List2D<int>.Row actual, int[] expected)
            {
                actual.Reverse();
                CollectionAssert.AreEqual(expected, actual);
            }

            [TestCaseSource(nameof(FillWithTestCases))]
            public static void TestFillWith(List2D<int>.Row actual, int value, int[] expected)
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
                                         { 8, 2, 3 } })).GetRow(1),
                    1,
                    9);
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5, 8 },
                                         { 4, 9, 1, 5 },
                                         { 8, 2, 3, 0 } })).GetRow(0),
                    2,
                    5);
            }

            private static IEnumerable<TestCaseData> IndexerInvalidTestCases()
            {
                // Index lesser than zero.
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5 },
                                         { 4, 9, 1 },
                                         { 8, 2, 3 } })).GetRow(1),
                    -1);
                // Index exceeding row length.
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5, 8 },
                                         { 4, 9, 1, 5 },
                                         { 8, 2, 3, 0 } })).GetRow(0),
                    4);
            }

            private static IEnumerable<TestCaseData> EqualityTestCases()
            {
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5 },
                                         { 4, 9, 1 },
                                         { 8, 2, 3 } })).GetRow(1),
                    new int[] { 4, 9, 1 });
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5, 8 },
                                         { 4, 9, 1, 5 },
                                         { 8, 2, 3, 0 } })).GetRow(0),
                    new int[] { 2, 3, 5, 8 });
            }

            private static IEnumerable<TestCaseData> ReverseTestCases()
            {
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5 },
                                         { 4, 9, 1 },
                                         { 8, 2, 3 } })).GetRow(1),
                    new int[] { 1, 9, 4 });
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 5, },
                                         { 4 } })).GetRow(1),
                    new int[] { 4 });
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 5, 3 },
                                         { 4, 2} })).GetRow(0),
                    new int[] { 3, 5 });
            }

            private static IEnumerable<TestCaseData> FillWithTestCases()
            {
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 2, 3, 5 },
                                         { 4, 9, 1 },
                                         { 8, 2, 3 } })).GetRow(1),
                    4,
                    new int[] { 4, 4, 4 });
                yield return new TestCaseData(
                    new List2D<int>(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 6, },
                                         { 2, } })).GetRow(1),
                    2,
                    new int[] { 2 });
            }
        }     
    }
}
