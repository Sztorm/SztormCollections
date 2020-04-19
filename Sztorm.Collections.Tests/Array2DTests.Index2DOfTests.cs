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
        public static class Index2DOfTests
        {
            public static class Index2DOfAnyTests
            {
                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfTestCases))]
                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfEquatableTestCases))]
                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfComparableTestCases))]
                public static ItemRequestResult<Index2D> TestIndex2DOf<T>(Array2D<T> array, T item)
                => array.Index2DOf(item);

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfTestInvalidStartIndexCases))]
                public static void Index2DOfIndex2DIntThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.Index2DOf(default, startIndex, 0));

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfInvalidCountTestCases))]
                public static void Index2DOfIndex2DIntThrowsExceptionIfCountExceedsArray2DCount<T>(
                    Array2D<T> array, Index2D startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.Index2DOf(default, startIndex, count));

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfIndex2DIntTestCases))]
                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfEquatableIndex2DIntTestCases))]
                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfComparableIndex2DIntTestCases))]
                public static ItemRequestResult<Index2D> TestIndex2DOf<T>(
                    Array2D<T> array, T item, Index2D startIndex, int count)
                    => array.Index2DOf(item, startIndex, count);

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfTestInvalidStartIndexCases))]
                public static void Index2DOfIndex2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.Index2DOf(default, startIndex, new Bounds2D()));

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfTestInvalidSectorTestCases))]
                public static void Index2DOfIndex2DBounds2DThrowsExceptionIfSectorIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex, Bounds2D sector)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.Index2DOf(default, startIndex, sector));

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfIndex2DBounds2DTestCases))]
                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfEquatableIndex2DBounds2DTestCases))]
                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfComparableIndex2DBounds2DTestCases))]
                public static ItemRequestResult<Index2D> TestIndex2DOf<T>(
                    Array2D<T> array, T item, Index2D startIndex, Bounds2D sector)
                    => array.Index2DOf(item, startIndex, sector);
            }

            public static class Index2DOfEquatableTests
            {
                [Test]
                public static void Index2DOfEquatableThrowsExceptionIfItemIsNull()
                {
                    var array = new Array2D<string>(0, 0);

                    Assert.Throws<ArgumentNullException>(
                        () => array.Index2DOfEquatable<string>(item: null));
                }

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfEquatableTestCases))]
                public static ItemRequestResult<Index2D> TestIndex2DOfEquatable<T>(
                    Array2D<T> array, T item)
                    where T : IEquatable<T>
                    => array.Index2DOfEquatable(item);

                [Test]
                public static void Index2DOfEquatableIndex2DIntThrowsExceptionIfItemIsNull()
                {
                    var array = new Array2D<string>(1, 1);

                    Assert.Throws<ArgumentNullException>(
                        () => array.Index2DOfEquatable<string>(null, (0, 0), 0));
                }

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfTestInvalidStartIndexCases))]
                public static void Index2DOfEquatableIndex2DIntThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    where T : IEquatable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.Index2DOfEquatable(new T(), startIndex, 0));

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfInvalidCountTestCases))]
                public static void Index2DOfEquatableIndex2DIntThrowsExceptionIfCountExceedsArray2DCount<T>(
                    Array2D<T> array, Index2D startIndex, int count)
                    where T : IEquatable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.Index2DOfEquatable(new T(), startIndex, count));

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfEquatableIndex2DIntTestCases))]
                public static ItemRequestResult<Index2D> TestIndex2DOfEquatable<T>(
                    Array2D<T> array, T item, Index2D startIndex, int count)
                    where T : IEquatable<T>
                    => array.Index2DOfEquatable(item, startIndex, count);

                [Test]
                public static void Index2DOfEquatableIndex2DBounds2DThrowsExceptionIfItemIsNull()
                {
                    var array = new Array2D<string>(1, 1);

                    Assert.Throws<ArgumentNullException>(
                        () => array.Index2DOfEquatable<string>(null, (0, 0), array.Boundaries));
                }

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfTestInvalidStartIndexCases))]
                public static void Index2DOfEquatableIndex2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    where T : IEquatable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.Index2DOfEquatable(new T(), startIndex, new Bounds2D()));

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfTestInvalidSectorTestCases))]
                public static void Index2DOfEquatableIndex2DBounds2DThrowsExceptionIfSectorIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex, Bounds2D sector)
                    where T : IEquatable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.Index2DOfEquatable(new T(), startIndex, sector));

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfEquatableIndex2DBounds2DTestCases))]
                public static ItemRequestResult<Index2D> TestIndex2DOfEquatable<T>(
                    Array2D<T> array, T item, Index2D startIndex, Bounds2D sector)
                    where T : IEquatable<T>
                    => array.Index2DOfEquatable(item, startIndex, sector);
            }

            public static class Index2DOfComparableTests
            {
                [Test]
                public static void Index2DOfComparableThrowsExceptionIfItemIsNull()
                {
                    var array = new Array2D<string>(0, 0);

                    Assert.Throws<ArgumentNullException>(
                        () => array.Index2DOfComparable<string>(item: null));
                }

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfComparableTestCases))]
                public static ItemRequestResult<Index2D> TestIndex2DOfComparable<T>(
                    Array2D<T> array, T item)
                    where T : IComparable<T>
                    => array.Index2DOfComparable(item);

                [Test]
                public static void Index2DOfComparableIndex2DIntThrowsExceptionIfItemIsNull()
                {
                    var array = new Array2D<string>(1, 1);

                    Assert.Throws<ArgumentNullException>(
                        () => array.Index2DOfComparable<string>(null, (0, 0), 0));
                }

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfTestInvalidStartIndexCases))]
                public static void Index2DOfComparableIndex2DIntThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    where T : IComparable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.Index2DOfComparable(new T(), startIndex, 0));

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfInvalidCountTestCases))]
                public static void Index2DOfComparableIndex2DIntThrowsExceptionIfCountExceedsArray2DCount<T>(
                    Array2D<T> array, Index2D startIndex, int count)
                    where T : IComparable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.Index2DOfComparable(new T(), startIndex, count));

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfComparableIndex2DIntTestCases))]
                public static ItemRequestResult<Index2D> TestIndex2DOfComparable<T>(
                    Array2D<T> array, T item, Index2D startIndex, int count)
                    where T : IComparable<T>
                    => array.Index2DOfComparable(item, startIndex, count);

                [Test]
                public static void Index2DOfComparableIndex2DBounds2DThrowsExceptionIfItemIsNull()
                {
                    var array = new Array2D<string>(1, 1);

                    Assert.Throws<ArgumentNullException>(
                        () => array.Index2DOfComparable<string>(null, (0, 0), array.Boundaries));
                }

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfTestInvalidStartIndexCases))]
                public static void Index2DOfComparableIndex2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    where T : IComparable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.Index2DOfComparable(new T(), startIndex, new Bounds2D()));

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfTestInvalidSectorTestCases))]
                public static void Index2DOfComparableIndex2DBounds2DThrowsExceptionIfSectorIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex, Bounds2D sector)
                    where T : IComparable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.Index2DOfComparable(new T(), startIndex, sector));

                [TestCaseSource(typeof(Index2DOfTests), nameof(Index2DOfComparableIndex2DBounds2DTestCases))]
                public static ItemRequestResult<Index2D> TestIndex2DOfComparable<T>(
                    Array2D<T> array, T item, Index2D startIndex, Bounds2D sector)
                    where T : IComparable<T>
                    => array.Index2DOfComparable(item, startIndex, sector);
            }

            private static IEnumerable<TestCaseData> Index2DOfTestInvalidStartIndexCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } });

                yield return new TestCaseData(array2x3, new Index2D(-1, 0));
                yield return new TestCaseData(array2x3, new Index2D(0, -1));
                yield return new TestCaseData(array2x3, new Index2D(2, 0));
                yield return new TestCaseData(array2x3, new Index2D(0, 3));
                yield return new TestCaseData(new Array2D<int>(0, 0), new Index2D(0, 0));
                yield return new TestCaseData(new Array2D<int>(1, 0), new Index2D(0, 0));
                yield return new TestCaseData(new Array2D<int>(0, 1), new Index2D(0, 0));
            }

            private static IEnumerable<TestCaseData> Index2DOfInvalidCountTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } });

                yield return new TestCaseData(array2x3, new Index2D(0, 0), -1);
                yield return new TestCaseData(array2x3, new Index2D(1, 2), -1);
                yield return new TestCaseData(array2x3, new Index2D(1, 1), 3);
                yield return new TestCaseData(array2x3, new Index2D(0, 0), 7);
            }

            private static IEnumerable<TestCaseData> Index2DOfTestInvalidSectorTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } });

                yield return new TestCaseData(array2x3, new Index2D(0, 0), new Bounds2D(3, 0));
                yield return new TestCaseData(array2x3, new Index2D(0, 0), new Bounds2D(0, 4));
                yield return new TestCaseData(array2x3, new Index2D(1, 1), new Bounds2D(2, 0));
                yield return new TestCaseData(array2x3, new Index2D(1, 1), new Bounds2D(0, 3));
            }

            private static IEnumerable<TestCaseData> Index2DOfTestCases()
            {
                var array3x2 = Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, null },
                                    { 9, null } });

                yield return new TestCaseData(array3x2, 9)
                    .Returns(new ItemRequestResult<Index2D>((2, 0)));
                yield return new TestCaseData(array3x2, 3)
                    .Returns(new ItemRequestResult<Index2D>((0, 1)));
                yield return new TestCaseData(array3x2, 8)
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(array3x2, null)
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
            }

            private static IEnumerable<TestCaseData> Index2DOfIndex2DIntTestCases()
            {
                var array2x3 = Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3, 5 },
                                    { 4, 9, 1 } });

                yield return new TestCaseData(array2x3, 9, new Index2D(0, 0), 6)
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(array2x3, 1, new Index2D(1, 2), 0)
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(array2x3, 10, new Index2D(1, 1), 2)
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> Index2DOfIndex2DBounds2DTestCases()
            {
                var array2x3 = Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3, 5 },
                                    { 4, 9, 1 } });

                yield return new TestCaseData(array2x3, 9, new Index2D(0, 0), new Bounds2D(2, 3))
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(array2x3, 2, new Index2D(0, 0), new Bounds2D(0, 0))
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(array2x3, 10, new Index2D(1, 1), new Bounds2D(1, 2))
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> Index2DOfEquatableTestCases()
            {
                var array2x3 = Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "4", "9", "1" } });

                yield return new TestCaseData(array2x3, "9")
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(array2x3, "3")
                    .Returns(new ItemRequestResult<Index2D>((0, 1)));
                yield return new TestCaseData(array2x3, "8")
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(array2x3, "7")
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> Index2DOfEquatableIndex2DIntTestCases()
            {
                var array2x3 = Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "4", "9", "1" } });

                yield return new TestCaseData(array2x3, "9", new Index2D(0, 0), 6)
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(array2x3, "1", new Index2D(1, 2), 0)
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(array2x3, "10", new Index2D(1, 1), 2)
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> Index2DOfEquatableIndex2DBounds2DTestCases()
            {
                var array2x3 = Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "4", "9", "1" } });

                yield return new TestCaseData(array2x3, "9", new Index2D(0, 0), new Bounds2D(2, 3))
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(array2x3, "2", new Index2D(0, 0), new Bounds2D(0, 0))
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(
                    array2x3, "10", new Index2D(1, 1), new Bounds2D(1, 2))
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> Index2DOfComparableTestCases()
            {
                var array3x2 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } });

                yield return new TestCaseData(array3x2, 9)
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(array3x2, 3)
                    .Returns(new ItemRequestResult<Index2D>((0, 1)));
                yield return new TestCaseData(array3x2, 8)
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(array3x2, 7)
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> Index2DOfComparableIndex2DIntTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } });

                yield return new TestCaseData(array2x3, 9, new Index2D(0, 0), 6)
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(array2x3, 1, new Index2D(1, 2), 0)
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(array2x3, 10, new Index2D(1, 1), 2)
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> Index2DOfComparableIndex2DBounds2DTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } });

                yield return new TestCaseData(array2x3, 9, new Index2D(0, 0), new Bounds2D(2, 3))
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(array2x3, 2, new Index2D(0, 0), new Bounds2D(0, 0))
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(array2x3, 10, new Index2D(1, 1), new Bounds2D(1, 2))
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }
        }
    }
}
