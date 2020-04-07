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
        public static ItemRequestResult<Index2D> TestIndex2DOf<T>(Array2D<T> array, T valueToFind)
            => array.Index2DOf(valueToFind);

        [TestCaseSource(typeof(Array2DTests), nameof(Index2DOfEquatableTestCases))]
        public static ItemRequestResult<Index2D> TestIndex2DOfEquatable<T>(
            Array2D<T> array, T valueToFind)
            where T : IEquatable<T>
            => array.Index2DOfEquatable(valueToFind);

        [TestCaseSource(typeof(Array2DTests), nameof(Index2DOfComparableTestCases))]
        public static ItemRequestResult<Index2D> TestIndex2DOfComparable<T>(
            Array2D<T> array, T valueToFind)
            where T : IComparable<T>
            => array.Index2DOfComparable(valueToFind);

        private static IEnumerable<TestCaseData> Index2DOfTestCases()
        {
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3, 5 },
                                    { 4, 9, 1 } }),
                9)
                .Returns(new ItemRequestResult<Index2D>(new Index2D(1, 1)));
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } }),
                3)
                .Returns(new ItemRequestResult<Index2D>(new Index2D(0, 1)));
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } }),
                8)
                .Returns(ItemRequestResult<Index2D>.Failed);
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } }),
                7)
                .Returns(ItemRequestResult<Index2D>.Failed);
        }

        private static IEnumerable<TestCaseData> Index2DOfEquatableTestCases()
        {
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "4", "9", "1" } }),
                "9")
                .Returns(new ItemRequestResult<Index2D>(new Index2D(1, 1)));
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                "3")
                .Returns(new ItemRequestResult<Index2D>(new Index2D(0, 1)));
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                "8")
                .Returns(ItemRequestResult<Index2D>.Failed);
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                "7")
                .Returns(ItemRequestResult<Index2D>.Failed);
        }

        private static IEnumerable<TestCaseData> Index2DOfComparableTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } }),
                9)
                .Returns(new ItemRequestResult<Index2D>(new Index2D(1, 1)));
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                3)
                .Returns(new ItemRequestResult<Index2D>(new Index2D(0, 1)));
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                8)
                .Returns(ItemRequestResult<Index2D>.Failed);
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                7)
                .Returns(ItemRequestResult<Index2D>.Failed);
        }
    }
}
