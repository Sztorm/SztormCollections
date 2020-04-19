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
        public static class ContainsTests
        {
            [TestCaseSource(typeof(ContainsTests), nameof(ContainsTestCases))]
            [TestCaseSource(typeof(ContainsTests), nameof(ContainsEquatableTestCases))]
            [TestCaseSource(typeof(ContainsTests), nameof(ContainsComparableTestCases))]
            public static bool TestContains<T>(Array2D<T> array, T value) => array.Contains(value);

            [Test]
            public static void ContainsEquatableThrowsExceptionIfItemIsNull()
                => Assert.Throws<ArgumentNullException>(
                    () => new Array2D<string>(0, 0).ContainsEquatable<string>(item: null));

            [TestCaseSource(typeof(ContainsTests), nameof(ContainsEquatableTestCases))]
            public static bool TestContainsEquatable<T>(Array2D<T> array, T value)
                where T : IEquatable<T>
                => array.ContainsEquatable(value);

            [Test]
            public static void ContainsComparableThrowsExceptionIfItemIsNull()
                => Assert.Throws<ArgumentNullException>(
                    () => new Array2D<string>(0, 0).ContainsComparable<string>(item: null));

            [TestCaseSource(typeof(ContainsTests), nameof(ContainsComparableTestCases))]
            public static bool TestContainsComparable<T>(Array2D<T> array, T value)
                where T : IComparable<T>
                => array.ContainsComparable(value);

            private static IEnumerable<TestCaseData> ContainsTestCases()
            {
                var array3x2 = Array2D<object>.FromSystem2DArray(
                        new object[,] { { 2, 3 },
                                        { 4, 9 },
                                        { 3, 6 } });

                yield return new TestCaseData(
                    Array2D<object>.FromSystem2DArray(
                        new object[,] { { 2, 3, 5 },
                                        { 4, 9, 1 } }), 9).Returns(true);
                yield return new TestCaseData(array3x2, 3).Returns(true);
                yield return new TestCaseData(array3x2, 8).Returns(false);
                yield return new TestCaseData(array3x2, 7).Returns(false);
            }

            private static IEnumerable<TestCaseData> ContainsEquatableTestCases()
            {
                var array3x2 = Array2D<string>.FromSystem2DArray(
                        new string[,] { { "2", "3" },
                                        { "4", "9" },
                                        { "3", "6" } });

                yield return new TestCaseData(
                    Array2D<string>.FromSystem2DArray(
                        new string[,] { { "2", "3", "5" },
                                        { "4", "9", "1" } }), "9").Returns(true);
                yield return new TestCaseData(array3x2, "3").Returns(true);
                yield return new TestCaseData(array3x2, "8").Returns(false);
                yield return new TestCaseData(array3x2, "7").Returns(false);
            }

            private static IEnumerable<TestCaseData> ContainsComparableTestCases()
            {
                var array3x2 = Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3 },
                                     { 4, 9 },
                                     { 3, 6 } });

                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }), 9).Returns(true);
                yield return new TestCaseData(array3x2, 3).Returns(true);
                yield return new TestCaseData(array3x2, 8).Returns(false);
                yield return new TestCaseData(array3x2, 7).Returns(false);
            }
        }   
    }
}
