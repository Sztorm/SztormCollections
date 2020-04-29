using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    public partial class List2DTests
    {
        public static class Contains
        {
            [TestCaseSource(typeof(Contains), nameof(AnyTestCases))]
            [TestCaseSource(typeof(Contains), nameof(EquatableTestCases))]
            [TestCaseSource(typeof(Contains), nameof(ComparableTestCases))]
            public static bool TestAny<T>(List2D<T> list, T value) => list.Contains(value);

            [Test]
            public static void EquatableThrowsExceptionIfItemIsNull()
                => Assert.Throws<ArgumentNullException>(
                    () => new List2D<string>(0, 0).ContainsEquatable<string>(item: null));

            [TestCaseSource(typeof(Contains), nameof(EquatableTestCases))]
            public static bool TestEquatable<T>(List2D<T> list, T value)
                where T : IEquatable<T>
                => list.ContainsEquatable(value);

            [Test]
            public static void ComparableThrowsExceptionIfItemIsNull()
                => Assert.Throws<ArgumentNullException>(
                    () => new List2D<string>(0, 0).ContainsComparable<string>(item: null));

            [TestCaseSource(typeof(Contains), nameof(ComparableTestCases))]
            public static bool TestComparable<T>(List2D<T> list, T value)
                where T : IComparable<T>
                => list.ContainsComparable(value);

            private static IEnumerable<TestCaseData> AnyTestCases()
            {
                var list3x2 = List2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, 9 },
                                    { 3, 6 } });
                list3x2.IncreaseCapacity(new Bounds2D(list3x2.Rows, list3x2.Columns));

                var list2x3 = List2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3, 5 },
                                    { 4, 9, 1 } });
                list2x3.IncreaseCapacity(new Bounds2D(list2x3.Rows, list2x3.Columns));

                yield return new TestCaseData(list2x3, 9).Returns(true);
                yield return new TestCaseData(list3x2, 3).Returns(true);
                yield return new TestCaseData(list3x2, 8).Returns(false);
                yield return new TestCaseData(list3x2, 7).Returns(false);
            }

            private static IEnumerable<TestCaseData> EquatableTestCases()
            {
                var list3x2 = List2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } });
                list3x2.IncreaseCapacity(new Bounds2D(list3x2.Rows, list3x2.Columns));

                var list2x3 = List2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "4", "9", "1" } });
                list2x3.IncreaseCapacity(new Bounds2D(list2x3.Rows, list2x3.Columns));

                yield return new TestCaseData(list2x3, "9").Returns(true);
                yield return new TestCaseData(list3x2, "3").Returns(true);
                yield return new TestCaseData(list3x2, "8").Returns(false);
                yield return new TestCaseData(list3x2, "7").Returns(false);
            }

            private static IEnumerable<TestCaseData> ComparableTestCases()
            {
                var list3x2 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } });
                list3x2.IncreaseCapacity(new Bounds2D(list3x2.Rows, list3x2.Columns));

                var list2x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } });
                list2x3.IncreaseCapacity(new Bounds2D(list2x3.Rows, list2x3.Columns));

                yield return new TestCaseData(list2x3, 9).Returns(true);
                yield return new TestCaseData(list3x2, 3).Returns(true);
                yield return new TestCaseData(list3x2, 8).Returns(false);
                yield return new TestCaseData(list3x2, 7).Returns(false);
            }
        }   
    }
}
