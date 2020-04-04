using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    public partial class Array2DTests
    {
        [TestCaseSource(typeof(Array2DTests), nameof(Index2DOfTestCases))]
        [TestCaseSource(typeof(Array2DTests), nameof(Index2DOfEquatableTestCases))]
        [TestCaseSource(typeof(Array2DTests), nameof(Index2DOfComparableTestCases))]
        public static void TestIndex2DOf<T>(Array2D<T> array, T valueToFind, Index2D? expected)
            => Assert.AreEqual(expected, array.Index2DOf(valueToFind));

        [TestCaseSource(typeof(Array2DTests), nameof(Index2DOfEquatableTestCases))]
        public static void TestIndex2DOfEquatable<T>(
            Array2D<T> array, T valueToFind, Index2D? expected)
            where T : IEquatable<T>
            => Assert.AreEqual(expected, array.Index2DOfEquatable(valueToFind));

        [TestCaseSource(typeof(Array2DTests), nameof(Index2DOfComparableTestCases))]
        public static void TestIndex2DOfComparable<T>(
            Array2D<T> array, T valueToFind, Index2D? expected)
            where T : IComparable<T>
            => Assert.AreEqual(expected, array.Index2DOfComparable(valueToFind));

        private static IEnumerable<TestCaseData> Index2DOfTestCases()
        {
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3, 5 },
                                    { 4, 9, 1 } }),
                9,
                new Index2D?(new Index2D(1, 1)));
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } }),
                3,
                new Index2D?(new Index2D(0, 1)));
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } }),
                8,
                new Index2D?());
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } }),
                7,
                new Index2D?());
        }

        private static IEnumerable<TestCaseData> Index2DOfEquatableTestCases()
        {
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "4", "9", "1" } }),
                "9",
                new Index2D?(new Index2D(1, 1)));
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                "3",
                new Index2D?(new Index2D(0, 1)));
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                "8",
                new Index2D?());
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                "7",
                new Index2D?());
        }

        private static IEnumerable<TestCaseData> Index2DOfComparableTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } }),
                9,
                new Index2D?(new Index2D(1, 1)));
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                3,
                new Index2D?(new Index2D(0, 1)));
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                8,
                new Index2D?());
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                7,
                new Index2D?());
        }
    }
}
