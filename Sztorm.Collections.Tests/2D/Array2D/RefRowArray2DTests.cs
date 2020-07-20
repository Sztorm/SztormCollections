using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public class RefRowArray2DTests
    {
        [TestCaseSource(typeof(RefRowArray2DTests), nameof(Array2DArbitraryRows))]
        public static void CountEqualsCollectionLength2<T>(
            Array2D<T> collection, RefRow<T, Array2D<T>> row)
            => RefRowTests.CountEqualsCollectionLength2(collection, row);

        [TestCaseSource(typeof(RefRowArray2DTests), nameof(IndexerInvalidTestCases))]
        public static void IndexerThrowsExceptionIfIndexIsOutOfBounds<T>(
            RefRow<T, Array2D<T>> row, int index)
            => RefRowTests.IndexerThrowsExceptionIfIndexIsOutOfBounds(row, index);

        [TestCaseSource(typeof(RefRowArray2DTests), nameof(EqualityTestCases))]
        public static void TestEquality<T, TEnumerable>(
            RefRow<T, Array2D<T>> actual, TEnumerable expected)
            where TEnumerable : IEnumerable<T>
            => RefRowTests.TestEquality(actual, expected);

        [TestCaseSource(typeof(RefRowArray2DTests), nameof(ReverseTestCases))]
        public static void TestReverse<T, TEnumerable>(
            RefRow<T, Array2D<T>> actual, TEnumerable expected)
            where TEnumerable : IEnumerable<T>
            => RefRowTests.TestReverse(actual, expected);

        [TestCaseSource(typeof(RefRowArray2DTests), nameof(FillWithTestCases))]
        public static void TestFillWith<T, TEnumerable>(
            RefRow<T, Array2D<T>> actual, T value, TEnumerable expected)
            where TEnumerable : IEnumerable<T>
            => RefRowTests.TestFillWith(actual, value, expected);

        [TestCaseSource(typeof(RefRowArray2DTests), nameof(IndexerTestCases))]
        public static void TestIndexer<T>(
            RefRow<T, Array2D<T>> indexable, int index, T expected)
            => RefIndexableTests.TestIndexer(indexable, index, expected);

        [TestCaseSource(typeof(RefRowArray2DTests), nameof(IndexerInvalidTestCases))]
        public static void IsValidIndexReturnsFalseOnInvalidIndexArguments<T>(
            RefRow<T, Array2D<T>> indexable, int index)
            => RefIndexableTests.IsValidIndexReturnsFalseOnInvalidIndexArguments
                <T, RefRow<T, Array2D<T>>>(indexable, index);

        private static IEnumerable<TestCaseData> Array2DArbitraryRows()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }),
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }).GetRow(1));
        }

        private static IEnumerable<TestCaseData> IndexerInvalidTestCases()
        {
            // Index lesser than zero.
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }).GetRow(1),
                -1);
            // Index exceeding row length.
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5, 8 },
                                 { 4, 9, 1, 5 },
                                 { 8, 2, 3, 0 } }).GetRow(0),
                4);
        }

        private static IEnumerable<TestCaseData> EqualityTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }).GetRow(1),
                new int[] { 4, 9, 1 });
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5, 8 },
                                 { 4, 9, 1, 5 },
                                 { 8, 2, 3, 0 } }).GetRow(0),
                new int[] { 2, 3, 5, 8 });
        }

        private static IEnumerable<TestCaseData> ReverseTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }).GetRow(1),
                new int[] { 1, 9, 4 });
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 5 },
                                 { 4 }}).GetRow(1),
                new int[] { 4 });
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 5, 3 },
                                 { 4, 2 } }).GetRow(0),
                new int[] { 3, 5 });
        }

        private static IEnumerable<TestCaseData> FillWithTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }).GetRow(1),
                4,
                new int[] { 4, 4, 4 });
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 6, },
                                 { 2, } }).GetRow(1),
                2,
                new int[] { 2 });
        }

        private static IEnumerable<TestCaseData> IndexerTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }).GetRow(1),
                1,
                9);
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5", "8" },
                                    { "4", "9", "1", "5" },
                                    { "8", "2", "3", "0" } }).GetRow(0),
                2,
                "5");  
        }
    }
}
