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
        public static void TestIndexOf<T>(Array2D<T> array, T valueToFind, int? expected)
            => Assert.AreEqual(expected, array.IndexOf(valueToFind));

        [TestCaseSource(typeof(Array2DTests), nameof(IndexOfEquatableTestCases))]
        public static void TestIndexOfEquatable<T>(Array2D<T> array, T valueToFind, int? expected)
            where T : IEquatable<T>
            => Assert.AreEqual(expected, array.IndexOfEquatable(valueToFind));

        [TestCaseSource(typeof(Array2DTests), nameof(IndexOfComparableTestCases))]
        public static void TestIndexOfComparable<T>(Array2D<T> array, T valueToFind, int? expected)
             where T : IComparable<T>
            => Assert.AreEqual(expected, array.IndexOfComparable(valueToFind));

        private static IEnumerable<TestCaseData> IndexOfTestCases()
        {
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3, 5 },
                                    { 4, 9, 1 } }),
                9,
                new int?(4));
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } }),
                3,
                new int?(1));
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } }),
                8,
                new int?());
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } }),
                7,
                new int?());
        }

        private static IEnumerable<TestCaseData> IndexOfEquatableTestCases()
        {
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "4", "9", "1" } }),
                "9",
                new int?(4));
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                "3",
                new int?(1));
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                "8",
                new int?());
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                "7",
                new int?());
        }

        private static IEnumerable<TestCaseData> IndexOfComparableTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } }),
                9,
                new int?(4));
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                3,
                new int?(1));
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                8,
                new int?());
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                7,
                new int?());
        }
    }
}
