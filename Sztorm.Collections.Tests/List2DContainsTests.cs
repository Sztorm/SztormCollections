using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public partial class List2DTests
    {
        [TestCaseSource(typeof(List2DTests), nameof(ContainsTestCases))]
        [TestCaseSource(typeof(List2DTests), nameof(ContainsEquatableTestCases))]
        [TestCaseSource(typeof(List2DTests), nameof(ContainsComparableTestCases))]
        public static void TestContains<T>(List2D<T> list, T value, bool expected)
            => Assert.AreEqual(expected, list.Contains(value));

        private static IEnumerable<TestCaseData> ContainsTestCases()
        {
            yield return new TestCaseData(
                List2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3, 5 },
                                    { 4, 9, 1 } }),
                9,
                true);
            yield return new TestCaseData(
                List2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } }),
                3,
                true);
            yield return new TestCaseData(
                List2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } }),
                8,
                false);
            yield return new TestCaseData(
                List2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } }),
                7,
                false);
        }

        private static IEnumerable<TestCaseData> ContainsEquatableTestCases()
        {
            yield return new TestCaseData(
                List2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "4", "9", "1" } }),
                    "9",
                    true);
            yield return new TestCaseData(
                List2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                    "3",
                    true);
            yield return new TestCaseData(
                List2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                    "8",
                    false);
            yield return new TestCaseData(
                List2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                    "7",
                    false);
        }

        private static IEnumerable<TestCaseData> ContainsComparableTestCases()
        {
            yield return new TestCaseData(
                List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } }),
                    9,
                    true);
            yield return new TestCaseData(
                List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                    3,
                    true);
            yield return new TestCaseData(
                List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                    8,
                    false);
            yield return new TestCaseData(
                List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                    7,
                    false);
        }
    }
}
