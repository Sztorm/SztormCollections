﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    public partial class Array2DTests
    {
        public static class LastIndex1DOf
        {
            public static class Any
            {
                [TestCaseSource(typeof(LastIndex1DOf), nameof(AnyTestCases))]
                [TestCaseSource(typeof(LastIndex1DOf), nameof(EquatableTestCases))]
                [TestCaseSource(typeof(LastIndex1DOf), nameof(ComparableTestCases))]
                public static ItemRequestResult<int> Test<T>(Array2D<T> array, T item)
                    => array.LastIndex1DOf(item);

                [TestCaseSource(typeof(LastIndex1DOf), nameof(InvalidStartIndexCases))]
                public static void Index2DIntThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.LastIndex1DOf(default, startIndex, 0));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(InvalidCountTestCases))]
                public static void Index2DIntThrowsExceptionIfCountExceedsArray2DCount<T>(
                    Array2D<T> array, Index2D startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.LastIndex1DOf(default, startIndex, count));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(AnyIndex2DIntTestCases))]
                [TestCaseSource(typeof(LastIndex1DOf), nameof(EquatableIndex2DIntTestCases))]
                [TestCaseSource(typeof(LastIndex1DOf), nameof(ComparableIndex2DIntTestCases))]
                public static ItemRequestResult<int> Test<T>(
                    Array2D<T> array, T item, Index2D startIndex, int count)
                    => array.LastIndex1DOf(item, startIndex, count);

                [TestCaseSource(typeof(LastIndex1DOf), nameof(InvalidStartIndexCases))]
                public static void Index2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.LastIndex1DOf(default, startIndex, new Bounds2D()));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(InvalidSectorTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfSectorIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex, Bounds2D sector)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.LastIndex1DOf(default, startIndex, sector));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(AnyIndex2DBounds2DTestCases))]
                [TestCaseSource(typeof(LastIndex1DOf), nameof(EquatableIndex2DBounds2DTestCases))]
                [TestCaseSource(typeof(LastIndex1DOf), nameof(ComparableIndex2DBounds2DTestCases))]
                public static ItemRequestResult<int> Test<T>(
                    Array2D<T> array, T item, Index2D startIndex, Bounds2D sector)
                    => array.LastIndex1DOf(item, startIndex, sector);
            }

            public static class Equatable
            {
                [Test]
                public static void ThrowsExceptionIfItemIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Array2D<string>(0, 0).LastIndex1DOfEquatable<string>(item: null));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(EquatableTestCases))]
                public static ItemRequestResult<int> Test<T>(
                    Array2D<T> array, T item)
                    where T : IEquatable<T>
                    => array.LastIndex1DOfEquatable(item);

                [Test]
                public static void Index2DIntThrowsExceptionIfItemIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Array2D<string>(1, 1).LastIndex1DOfEquatable<string>(null, (0, 0), 0));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(InvalidStartIndexCases))]
                public static void Index2DIntThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    where T : IEquatable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.LastIndex1DOfEquatable(new T(), startIndex, 0));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(InvalidCountTestCases))]
                public static void Index2DIntThrowsExceptionIfCountExceedsArray2DCount<T>(
                    Array2D<T> array, Index2D startIndex, int count)
                    where T : IEquatable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.LastIndex1DOfEquatable(new T(), startIndex, count));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(EquatableIndex2DIntTestCases))]
                public static ItemRequestResult<int> Test<T>(
                    Array2D<T> array, T item, Index2D startIndex, int count)
                    where T : IEquatable<T>
                    => array.LastIndex1DOfEquatable(item, startIndex, count);

                [Test]
                public static void Index2DBounds2DThrowsExceptionIfItemIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Array2D<string>(1, 1).LastIndex1DOfEquatable<string>(
                            null, (0, 0), new Bounds2D(1, 1)));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(InvalidStartIndexCases))]
                public static void Index2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    where T : IEquatable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.LastIndex1DOfEquatable(new T(), startIndex, new Bounds2D()));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(InvalidSectorTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfSectorIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex, Bounds2D sector)
                    where T : IEquatable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.LastIndex1DOfEquatable(new T(), startIndex, sector));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(EquatableIndex2DBounds2DTestCases))]
                public static ItemRequestResult<int> Test<T>(
                    Array2D<T> array, T item, Index2D startIndex, Bounds2D sector)
                    where T : IEquatable<T>
                    => array.LastIndex1DOfEquatable(item, startIndex, sector);
            }

            public static class Comparable
            {
                [Test]
                public static void ThrowsExceptionIfItemIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Array2D<string>(0, 0).LastIndex1DOfComparable<string>(item: null));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(ComparableTestCases))]
                public static ItemRequestResult<int> Test<T>(
                    Array2D<T> array, T item)
                    where T : IComparable<T>
                    => array.LastIndex1DOfComparable(item);

                [Test]
                public static void Index2DIntThrowsExceptionIfItemIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Array2D<string>(1, 1).LastIndex1DOfComparable<string>(
                            null, (0, 0), 0));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(InvalidStartIndexCases))]
                public static void Index2DIntThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    where T : IComparable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.LastIndex1DOfComparable(new T(), startIndex, 0));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(InvalidCountTestCases))]
                public static void Index2DIntThrowsExceptionIfCountExceedsArray2DCount<T>(
                    Array2D<T> array, Index2D startIndex, int count)
                    where T : IComparable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.LastIndex1DOfComparable(new T(), startIndex, count));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(ComparableIndex2DIntTestCases))]
                public static ItemRequestResult<int> Test<T>(
                    Array2D<T> array, T item, Index2D startIndex, int count)
                    where T : IComparable<T>
                    => array.LastIndex1DOfComparable(item, startIndex, count);

                [Test]
                public static void Index2DBounds2DThrowsExceptionIfItemIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Array2D<string>(1, 1).LastIndex1DOfComparable<string>(
                            null, (0, 0), new Bounds2D(1, 1)));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(InvalidStartIndexCases))]
                public static void Index2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    where T : IComparable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.LastIndex1DOfComparable(new T(), startIndex, new Bounds2D()));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(InvalidSectorTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfSectorIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex, Bounds2D sector)
                    where T : IComparable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.LastIndex1DOfComparable(new T(), startIndex, sector));

                [TestCaseSource(typeof(LastIndex1DOf), nameof(ComparableIndex2DBounds2DTestCases))]
                public static ItemRequestResult<int> Test<T>(
                    Array2D<T> array, T item, Index2D startIndex, Bounds2D sector)
                    where T : IComparable<T>
                    => array.LastIndex1DOfComparable(item, startIndex, sector);
            }

            private static IEnumerable<TestCaseData> InvalidStartIndexCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 0 },
                                 { 8, 9, 1 } });

                yield return new TestCaseData(array2x3, new Index2D(2, 0));
                yield return new TestCaseData(array2x3, new Index2D(0, 3));
                yield return new TestCaseData(array2x3, new Index2D(-1, 0));
                yield return new TestCaseData(array2x3, new Index2D(0, -1));
                yield return new TestCaseData(new Array2D<int>(0, 0), new Index2D(0, 0));
                yield return new TestCaseData(new Array2D<int>(1, 0), new Index2D(0, 0));
                yield return new TestCaseData(new Array2D<int>(0, 1), new Index2D(0, 0));
            }

            private static IEnumerable<TestCaseData> InvalidCountTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 0 },
                                 { 8, 9, 1 } });

                yield return new TestCaseData(array2x3, new Index2D(1, 2), -1);
                yield return new TestCaseData(array2x3, new Index2D(0, 0), -1);
                yield return new TestCaseData(array2x3, new Index2D(0, 1), 3);
                yield return new TestCaseData(array2x3, new Index2D(1, 2), 7);
            }

            private static IEnumerable<TestCaseData> InvalidSectorTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 0 },
                                 { 8, 9, 1 } });

                yield return new TestCaseData(array2x3, new Index2D(1, 2), new Bounds2D(3, 0));
                yield return new TestCaseData(array2x3, new Index2D(1, 2), new Bounds2D(0, 4));
                yield return new TestCaseData(array2x3, new Index2D(0, 1), new Bounds2D(2, 0));
                yield return new TestCaseData(array2x3, new Index2D(0, 1), new Bounds2D(0, 3));
            }

            private static IEnumerable<TestCaseData> AnyTestCases()
            {
                var array3x2 = Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, null },
                                    { 9, null } });

                yield return new TestCaseData(array3x2, 9)
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array3x2, 2)
                    .Returns(new ItemRequestResult<int>(0));
                yield return new TestCaseData(array3x2, 8)
                    .Returns(ItemRequestResult<int>.Fail);
                yield return new TestCaseData(array3x2, null)
                    .Returns(new ItemRequestResult<int>(5));
            }

            private static IEnumerable<TestCaseData> AnyIndex2DIntTestCases()
            {
                var array2x3 = Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3, 5 },
                                    { 9, 9, 1 } });

                yield return new TestCaseData(array2x3, 9, new Index2D(1, 2), 6)
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array2x3, 2, new Index2D(0, 0), 0)
                    .Returns(ItemRequestResult<int>.Fail);
                yield return new TestCaseData(array2x3, 10, new Index2D(0, 1), 2)
                    .Returns(ItemRequestResult<int>.Fail);
            }

            private static IEnumerable<TestCaseData> AnyIndex2DBounds2DTestCases()
            {
                var array2x3 = Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3, 5 },
                                    { 9, 9, 1 } });

                yield return new TestCaseData(array2x3, 9, new Index2D(1, 2), new Bounds2D(2, 3))
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array2x3, 2, new Index2D(0, 0), new Bounds2D(0, 0))
                    .Returns(ItemRequestResult<int>.Fail);
                yield return new TestCaseData(array2x3, 10, new Index2D(0, 1), new Bounds2D(1, 2))
                    .Returns(ItemRequestResult<int>.Fail);
            }

            private static IEnumerable<TestCaseData> EquatableTestCases()
            {
                var array3x2 = Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "0" },
                                    { "9", "0" } });

                yield return new TestCaseData(array3x2, "9")
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array3x2, "2")
                    .Returns(new ItemRequestResult<int>(0));
                yield return new TestCaseData(array3x2, "8")
                    .Returns(ItemRequestResult<int>.Fail);
                yield return new TestCaseData(array3x2, "0")
                    .Returns(new ItemRequestResult<int>(5));
            }

            private static IEnumerable<TestCaseData> EquatableIndex2DIntTestCases()
            {
                var array2x3 = Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "9", "9", "1" } });

                yield return new TestCaseData(array2x3, "9", new Index2D(1, 2), 6)
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array2x3, "1", new Index2D(1, 2), 0)
                    .Returns(ItemRequestResult<int>.Fail);
                yield return new TestCaseData(array2x3, "10", new Index2D(0, 1), 2)
                    .Returns(ItemRequestResult<int>.Fail);
            }

            private static IEnumerable<TestCaseData> EquatableIndex2DBounds2DTestCases()
            {
                var array2x3 = Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "9", "9", "1" } });

                yield return new TestCaseData(array2x3, "9", new Index2D(1, 2), new Bounds2D(2, 3))
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array2x3, "2", new Index2D(0, 0), new Bounds2D(0, 0))
                    .Returns(ItemRequestResult<int>.Fail);
                yield return new TestCaseData(
                    array2x3, "10", new Index2D(0, 1), new Bounds2D(1, 2))
                    .Returns(ItemRequestResult<int>.Fail);
            }

            private static IEnumerable<TestCaseData> ComparableTestCases()
            {
                var array3x2 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 0 },
                                 { 9, 0 } });

                yield return new TestCaseData(array3x2, 9)
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array3x2, 2)
                    .Returns(new ItemRequestResult<int>(0));
                yield return new TestCaseData(array3x2, 8)
                    .Returns(ItemRequestResult<int>.Fail);
                yield return new TestCaseData(array3x2, 0)
                    .Returns(new ItemRequestResult<int>(5));
            }

            private static IEnumerable<TestCaseData> ComparableIndex2DIntTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 9, 9, 1 } });

                yield return new TestCaseData(array2x3, 9, new Index2D(1, 2), 6)
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array2x3, 2, new Index2D(0, 0), 0)
                    .Returns(ItemRequestResult<int>.Fail);
                yield return new TestCaseData(array2x3, 10, new Index2D(0, 1), 2)
                    .Returns(ItemRequestResult<int>.Fail);
            }

            private static IEnumerable<TestCaseData> ComparableIndex2DBounds2DTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 9, 9, 1 } });

                yield return new TestCaseData(array2x3, 9, new Index2D(1, 2), new Bounds2D(2, 3))
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array2x3, 2, new Index2D(0, 0), new Bounds2D(0, 0))
                    .Returns(ItemRequestResult<int>.Fail);
                yield return new TestCaseData(array2x3, 10, new Index2D(0, 1), new Bounds2D(1, 2))
                    .Returns(ItemRequestResult<int>.Fail);
            }
        }
    }
}
