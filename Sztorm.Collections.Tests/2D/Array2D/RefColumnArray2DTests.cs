using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public class RefColumnArray2DTests
    {
        [TestCaseSource(typeof(RefColumnArray2DTests), nameof(Array2DArbitraryColumns))]
        public static void CountEqualsCollectionLength1<T>(
            Array2D<T> collection, RefColumn<T, Array2D<T>> column)
            => RefColumnTests.CountEqualsCollectionLength1(collection, column);

        [TestCaseSource(typeof(RefColumnArray2DTests), nameof(IndexerInvalidTestCases))]
        public static void IndexerThrowsExceptionIfIndexIsOutOfBounds<T>(
            RefColumn<T, Array2D<T>> column, int index)
            => RefColumnTests.IndexerThrowsExceptionIfIndexIsOutOfBounds(column, index);

        [TestCaseSource(typeof(RefColumnArray2DTests), nameof(EqualityTestCases))]
        public static void TestEquality<T, TEnumerable>(
            RefColumn<T, Array2D<T>> actual, TEnumerable expected)
            where TEnumerable : IEnumerable<T>
            => RefColumnTests.TestEquality(actual, expected);

        [TestCaseSource(typeof(RefColumnArray2DTests), nameof(ReverseTestCases))]
        public static void TestReverse<T, TEnumerable>(
            RefColumn<T, Array2D<T>> actual, TEnumerable expected)
            where TEnumerable : IEnumerable<T>
            => RefColumnTests.TestReverse(actual, expected);

        [TestCaseSource(typeof(RefColumnArray2DTests), nameof(FillWithTestCases))]
        public static void TestFillWith<T, TEnumerable>(
            RefColumn<T, Array2D<T>> actual, T value, TEnumerable expected)
            where TEnumerable : IEnumerable<T>
            => RefColumnTests.TestFillWith(actual, value, expected);

        [TestCaseSource(typeof(RefColumnArray2DTests), nameof(IndexerTestCases))]
        public static void TestIndexer<T>(
            RefColumn<T, Array2D<T>> indexable, int index, T expected)
            => RefIndexableTests.TestIndexer(indexable, index, expected);

        [TestCaseSource(typeof(RefColumnArray2DTests), nameof(IndexerInvalidTestCases))]
        public static void IsValidIndexReturnsFalseOnInvalidIndexArguments<T>(
            RefColumn<T, Array2D<T>> indexable, int index)
            => RefIndexableTests.IsValidIndexReturnsFalseOnInvalidIndexArguments
                <T, RefColumn<T, Array2D<T>>>(indexable, index);

        private static IEnumerable<TestCaseData> IndexerInvalidTestCases()
        {
            // Index lesser than zero.
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }).GetColumn(1),
                -1);
            // Index exceeding column length.
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5, 8 },
                                 { 4, 9, 1, 5 },
                                 { 8, 2, 3, 0 } }).GetColumn(0),
                3);
        }

        private static IEnumerable<TestCaseData> Array2DArbitraryColumns()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }),
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }).GetColumn(1));
        }

        private static IEnumerable<TestCaseData> EqualityTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }).GetColumn(1),
                    new int[] { 3, 9, 2 });
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5, 8 },
                                 { 4, 9, 1, 5 },
                                 { 8, 2, 3, 0 } }).GetColumn(0),
                    new int[] { 2, 4, 8 });
        }

        private static IEnumerable<TestCaseData> ReverseTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }).GetColumn(1),
                new int[] { 2, 9, 3 });
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(new int[,] { { 5, 4 } }).GetColumn(1),
                new int[] { 4 });
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 5, 3 },
                                 { 4, 2} }).GetColumn(0),
                new int[] { 4, 5 });
        }

        private static IEnumerable<TestCaseData> FillWithTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }).GetColumn(1),
                4,
                new int[] { 4, 4, 4 });
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 6, },
                                 { 1  }}).GetColumn(0),
                2,
                new int[] { 2, 2 });
        }

        private static IEnumerable<TestCaseData> IndexerTestCases()
        {     
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }).GetColumn(1),
                2,
                2);
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5", "8" },
                                    { "4", "9", "1", "5" },
                                    { "8", "2", "3", "0" } }).GetColumn(0),
                2,
                "8");      
        }
    }
}
