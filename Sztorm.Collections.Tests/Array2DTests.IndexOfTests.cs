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
        public static class IndexOfTests
        {
            public static class IndexOfAnyTests
            {
                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfTestCases))]
                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfEquatableTestCases))]
                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfComparableTestCases))]
                public static ItemRequestResult<int> TestIndexOf<T>(Array2D<T> array, T item)
                => array.IndexOf(item);

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfTestInvalidStartIndexCases))]
                public static void IndexOfIndex2DIntThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.IndexOf(default, startIndex, 0));

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfInvalidCountTestCases))]
                public static void IndexOfIndex2DIntThrowsExceptionIfCountExceedsArray2DCount<T>(
                    Array2D<T> array, Index2D startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.IndexOf(default, startIndex, count));

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfIndex2DIntTestCases))]
                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfEquatableIndex2DIntTestCases))]
                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfComparableIndex2DIntTestCases))]
                public static ItemRequestResult<int> TestIndexOf<T>(
                    Array2D<T> array, T item, Index2D startIndex, int count)
                    => array.IndexOf(item, startIndex, count);

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfTestInvalidStartIndexCases))]
                public static void IndexOfIndex2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.IndexOf(default, startIndex, new Bounds2D()));

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfTestInvalidSectorTestCases))]
                public static void IndexOfIndex2DBounds2DThrowsExceptionIfSectorIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex, Bounds2D sector)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.IndexOf(default, startIndex, sector));

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfIndex2DBounds2DTestCases))]
                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfEquatableIndex2DBounds2DTestCases))]
                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfComparableIndex2DBounds2DTestCases))]
                public static ItemRequestResult<int> TestIndexOf<T>(
                    Array2D<T> array, T item, Index2D startIndex, Bounds2D sector)
                    => array.IndexOf(item, startIndex, sector);
            }

            public static class IndexOfEquatableTests
            {
                [Test]
                public static void IndexOfEquatableThrowsExceptionIfItemIsNull()
                {
                    var array = new Array2D<string>(0, 0);

                    Assert.Throws<ArgumentNullException>(
                        () => array.IndexOfEquatable<string>(item: null));
                }

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfEquatableTestCases))]
                public static ItemRequestResult<int> TestIndexOfEquatable<T>(
                    Array2D<T> array, T item)
                    where T : IEquatable<T>
                    => array.IndexOfEquatable(item);

                [Test]
                public static void IndexOfEquatableIndex2DIntThrowsExceptionIfItemIsNull()
                {
                    var array = new Array2D<string>(1, 1);

                    Assert.Throws<ArgumentNullException>(
                        () => array.IndexOfEquatable<string>(null, (0, 0), 0));
                }

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfTestInvalidStartIndexCases))]
                public static void IndexOfEquatableIndex2DIntThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    where T : IEquatable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.IndexOfEquatable(new T(), startIndex, 0));

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfInvalidCountTestCases))]
                public static void IndexOfEquatableIndex2DIntThrowsExceptionIfCountExceedsArray2DCount<T>(
                    Array2D<T> array, Index2D startIndex, int count)
                    where T : IEquatable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.IndexOfEquatable(new T(), startIndex, count));

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfEquatableIndex2DIntTestCases))]
                public static ItemRequestResult<int> TestIndexOfEquatable<T>(
                    Array2D<T> array, T item, Index2D startIndex, int count)
                    where T : IEquatable<T>
                    => array.IndexOfEquatable(item, startIndex, count);

                [Test]
                public static void IndexOfEquatableIndex2DBounds2DThrowsExceptionIfItemIsNull()
                {
                    var array = new Array2D<string>(1, 1);

                    Assert.Throws<ArgumentNullException>(
                        () => array.IndexOfEquatable<string>(null, (0, 0), array.Boundaries));
                }

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfTestInvalidStartIndexCases))]
                public static void IndexOfEquatableIndex2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    where T : IEquatable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.IndexOfEquatable(new T(), startIndex, new Bounds2D()));

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfTestInvalidSectorTestCases))]
                public static void IndexOfEquatableIndex2DBounds2DThrowsExceptionIfSectorIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex, Bounds2D sector)
                    where T : IEquatable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.IndexOfEquatable(new T(), startIndex, sector));

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfEquatableIndex2DBounds2DTestCases))]
                public static ItemRequestResult<int> TestIndexOfEquatable<T>(
                    Array2D<T> array, T item, Index2D startIndex, Bounds2D sector)
                    where T : IEquatable<T>
                    => array.IndexOfEquatable(item, startIndex, sector);
            }

            public static class IndexOfComparableTests
            {
                [Test]
                public static void IndexOfComparableThrowsExceptionIfItemIsNull()
                {
                    var array = new Array2D<string>(0, 0);

                    Assert.Throws<ArgumentNullException>(
                        () => array.IndexOfComparable<string>(item: null));
                }

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfComparableTestCases))]
                public static ItemRequestResult<int> TestIndexOfComparable<T>(
                    Array2D<T> array, T item)
                    where T : IComparable<T>
                    => array.IndexOfComparable(item);

                [Test]
                public static void IndexOfComparableIndex2DIntThrowsExceptionIfItemIsNull()
                {
                    var array = new Array2D<string>(1, 1);

                    Assert.Throws<ArgumentNullException>(
                        () => array.IndexOfComparable<string>(null, (0, 0), 0));
                }

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfTestInvalidStartIndexCases))]
                public static void IndexOfComparableIndex2DIntThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    where T : IComparable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.IndexOfComparable(new T(), startIndex, 0));

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfInvalidCountTestCases))]
                public static void IndexOfComparableIndex2DIntThrowsExceptionIfCountExceedsArray2DCount<T>(
                    Array2D<T> array, Index2D startIndex, int count)
                    where T : IComparable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.IndexOfComparable(new T(), startIndex, count));

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfComparableIndex2DIntTestCases))]
                public static ItemRequestResult<int> TestIndexOfComparable<T>(
                    Array2D<T> array, T item, Index2D startIndex, int count)
                    where T : IComparable<T>
                    => array.IndexOfComparable(item, startIndex, count);

                [Test]
                public static void IndexOfComparableIndex2DBounds2DThrowsExceptionIfItemIsNull()
                {
                    var array = new Array2D<string>(1, 1);

                    Assert.Throws<ArgumentNullException>(
                        () => array.IndexOfComparable<string>(null, (0, 0), array.Boundaries));
                }

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfTestInvalidStartIndexCases))]
                public static void IndexOfComparableIndex2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    where T : IComparable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.IndexOfComparable(new T(), startIndex, new Bounds2D()));

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfTestInvalidSectorTestCases))]
                public static void IndexOfComparableIndex2DBounds2DThrowsExceptionIfSectorIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex, Bounds2D sector)
                    where T : IComparable<T>, new()
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.IndexOfComparable(new T(), startIndex, sector));

                [TestCaseSource(typeof(IndexOfTests), nameof(IndexOfComparableIndex2DBounds2DTestCases))]
                public static ItemRequestResult<int> TestIndexOfComparable<T>(
                    Array2D<T> array, T item, Index2D startIndex, Bounds2D sector)
                    where T : IComparable<T>
                    => array.IndexOfComparable(item, startIndex, sector);
            }

            private static IEnumerable<TestCaseData> IndexOfTestInvalidStartIndexCases()
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

            private static IEnumerable<TestCaseData> IndexOfInvalidCountTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } });

                yield return new TestCaseData(array2x3, new Index2D(0, 0), -1);
                yield return new TestCaseData(array2x3, new Index2D(1, 2), -1);
                yield return new TestCaseData(array2x3, new Index2D(1, 1), 3);
                yield return new TestCaseData(array2x3, new Index2D(0, 0), 7);
            }

            private static IEnumerable<TestCaseData> IndexOfTestInvalidSectorTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } });

                yield return new TestCaseData(array2x3, new Index2D(0, 0), new Bounds2D(3, 0));
                yield return new TestCaseData(array2x3, new Index2D(0, 0), new Bounds2D(0, 4));
                yield return new TestCaseData(array2x3, new Index2D(1, 1), new Bounds2D(2, 0));
                yield return new TestCaseData(array2x3, new Index2D(1, 1), new Bounds2D(0, 3));
            }

            private static IEnumerable<TestCaseData> IndexOfTestCases()
            {
                var array3x2 = Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },
                                    { 4, null },
                                    { 9, null } });

                yield return new TestCaseData(array3x2, 9)
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array3x2, 3)
                    .Returns(new ItemRequestResult<int>(1));
                yield return new TestCaseData(array3x2, 8)
                    .Returns(ItemRequestResult<int>.Failed);
                yield return new TestCaseData(array3x2, null)
                    .Returns(new ItemRequestResult<int>(3));
            }

            private static IEnumerable<TestCaseData> IndexOfIndex2DIntTestCases()
            {
                var array2x3 = Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3, 5 },
                                    { 4, 9, 1 } });

                yield return new TestCaseData(array2x3, 9, new Index2D(0, 0), 6)
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array2x3, 1, new Index2D(1, 2), 0)
                    .Returns(ItemRequestResult<int>.Failed);
                yield return new TestCaseData(array2x3, 10, new Index2D(1, 1), 2)
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> IndexOfIndex2DBounds2DTestCases()
            {
                var array2x3 = Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3, 5 },
                                    { 4, 9, 1 } });

                yield return new TestCaseData(array2x3, 9, new Index2D(0, 0), new Bounds2D(2, 3))
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array2x3, 2, new Index2D(0, 0), new Bounds2D(0, 0))
                    .Returns(ItemRequestResult<int>.Failed);
                yield return new TestCaseData(array2x3, 10, new Index2D(1, 1), new Bounds2D(1, 2))
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> IndexOfEquatableTestCases()
            {
                var array2x3 = Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "4", "9", "1" } });

                yield return new TestCaseData(array2x3, "9")
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array2x3, "3")
                    .Returns(new ItemRequestResult<int>(1));
                yield return new TestCaseData(array2x3, "8")
                    .Returns(ItemRequestResult<int>.Failed);
                yield return new TestCaseData(array2x3, "7")
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> IndexOfEquatableIndex2DIntTestCases()
            {
                var array2x3 = Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "4", "9", "1" } });

                yield return new TestCaseData(array2x3, "9", new Index2D(0, 0), 6)
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array2x3, "1", new Index2D(1, 2), 0)
                    .Returns(ItemRequestResult<int>.Failed);
                yield return new TestCaseData(array2x3, "10", new Index2D(1, 1), 2)
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> IndexOfEquatableIndex2DBounds2DTestCases()
            {
                var array2x3 = Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "4", "9", "1" } });

                yield return new TestCaseData(array2x3, "9", new Index2D(0, 0), new Bounds2D(2, 3))
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array2x3, "2", new Index2D(0, 0), new Bounds2D(0, 0))
                    .Returns(ItemRequestResult<int>.Failed);
                yield return new TestCaseData(
                    array2x3, "10", new Index2D(1, 1), new Bounds2D(1, 2))
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> IndexOfComparableTestCases()
            {
                var array3x2 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } });

                yield return new TestCaseData(array3x2, 9)
                    .Returns(new ItemRequestResult<int>(3));
                yield return new TestCaseData(array3x2, 3)
                    .Returns(new ItemRequestResult<int>(1));
                yield return new TestCaseData(array3x2, 8)
                    .Returns(ItemRequestResult<int>.Failed);
                yield return new TestCaseData(array3x2, 7)
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> IndexOfComparableIndex2DIntTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } });

                yield return new TestCaseData(array2x3, 9, new Index2D(0, 0), 6)
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array2x3, 1, new Index2D(1, 2), 0)
                    .Returns(ItemRequestResult<int>.Failed);
                yield return new TestCaseData(array2x3, 10, new Index2D(1, 1), 2)
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> IndexOfComparableIndex2DBounds2DTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } });

                yield return new TestCaseData(array2x3, 9, new Index2D(0, 0), new Bounds2D(2, 3))
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(array2x3, 2, new Index2D(0, 0), new Bounds2D(0, 0))
                    .Returns(ItemRequestResult<int>.Failed);
                yield return new TestCaseData(array2x3, 10, new Index2D(1, 1), new Bounds2D(1, 2))
                    .Returns(ItemRequestResult<int>.Failed);
            }
        }
    }
}
