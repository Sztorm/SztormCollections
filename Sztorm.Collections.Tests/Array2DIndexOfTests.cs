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
        [TestCaseSource(typeof(Array2DTests), nameof(IndexOfTestCases))]
        [TestCaseSource(typeof(Array2DTests), nameof(IndexOfEquatableTestCases))]
        [TestCaseSource(typeof(Array2DTests), nameof(IndexOfComparableTestCases))]
        public static ItemRequestResult<int> TestIndexOf<T>(Array2D<T> array, T valueToFind)
            => array.IndexOf(valueToFind);

        [TestCaseSource(typeof(Array2DTests), nameof(IndexOfEquatableTestCases))]
        public static ItemRequestResult<int> TestIndexOfEquatable<T>(
            Array2D<T> array, T valueToFind)
            where T : IEquatable<T>
            => array.IndexOfEquatable(valueToFind);

        [TestCaseSource(typeof(Array2DTests), nameof(IndexOfComparableTestCases))]
        public static ItemRequestResult<int> TestIndexOfComparable<T>(
            Array2D<T> array, T valueToFind)
            where T : IComparable<T>
            => array.IndexOfComparable(valueToFind);

        private static IEnumerable<TestCaseData> IndexOfTestCases()
        {
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3, 5 },
                                    { 4, 9, 1 } }),
                9)
                .Returns(new ItemRequestResult<int>(4));
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } }),
                3)
                .Returns(new ItemRequestResult<int>(1));
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } }),
                8)
                .Returns(ItemRequestResult<int>.Failed);
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } }),
                7)
                .Returns(ItemRequestResult<int>.Failed);
        }

        private static IEnumerable<TestCaseData> IndexOfEquatableTestCases()
        {
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "4", "9", "1" } }),
                "9")
                .Returns(new ItemRequestResult<int>(4));
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                "3")
                .Returns(new ItemRequestResult<int>(1));
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                "8")
                .Returns(ItemRequestResult<int>.Failed);
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                "7")
                .Returns(ItemRequestResult<int>.Failed);
        }

        private static IEnumerable<TestCaseData> IndexOfComparableTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } }),
                9)
                .Returns(new ItemRequestResult<int>(4));
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                3)
                .Returns(new ItemRequestResult<int>(1));
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                8)
                .Returns(ItemRequestResult<int>.Failed);
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                7)
                .Returns(ItemRequestResult<int>.Failed);
        }
    }
}
