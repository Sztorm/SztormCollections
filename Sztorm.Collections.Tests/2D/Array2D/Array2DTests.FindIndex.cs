using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    using static TestUtils;

    public partial class Array2DTests
    {
        public static class FindIndex
        {
            public static class Predicate
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Array2D<int>(0, 0).FindIndex(match: null));

                [TestCaseSource(typeof(FindIndex), nameof(PredicateTestCases))]
                public static ItemRequestResult<Index2D> Test<T>(
                    Array2D<T> array, Predicate<T> match)
                    => array.FindIndex(match);

                [Test]
                public static void Index2DIntThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Array2D<int>(0, 0).FindIndex((0, 0), 0, match: null));

                [TestCaseSource(typeof(FindIndex), nameof(InvalidStartIndexTestCases))]
                public static void Index2DIntThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.FindIndex(startIndex, 0, o => true));

                [TestCaseSource(typeof(FindIndex), nameof(InvalidCountTestCases))]
                public static void Index2DIntThrowsExceptionIfCountExceedsArray2DCount<T>(
                    Array2D<T> array, Index2D startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.FindIndex(startIndex, count, o => true));

                [TestCaseSource(typeof(FindIndex), nameof(PredicateIndex2DIntTestCases))]
                public static ItemRequestResult<Index2D> Test<T>(
                    Array2D<T> array, Index2D startIndex, int count, Predicate<T> match)
                    => array.FindIndex(startIndex, count, match);

                [Test]
                public static void Index2DBounds2DThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Array2D<int>(0, 0).FindIndex(
                            (0, 0), new Bounds2D(), match: null));

                [TestCaseSource(typeof(FindIndex), nameof(InvalidStartIndexTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.FindIndex(startIndex, new Bounds2D(), o => true));

                [TestCaseSource(typeof(FindIndex), nameof(InvalidSectorTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfSectorIsOutOfBounds<T>(
                    Array2D<T> array, Index2D startIndex, Bounds2D sector)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => array.FindIndex(startIndex, sector, o => true));

                [TestCaseSource(typeof(FindIndex), nameof(PredicateIndex2DBounds2DTestCases))]
                public static ItemRequestResult<Index2D> Test<T>(
                    Array2D<T> array, Index2D startIndex, Bounds2D sector, Predicate<T> match)
                    => array.FindIndex(startIndex, sector, match);
            }

            public static class IPredicate
            {
                [TestCaseSource(typeof(FindIndex), nameof(IPredicateTestCases))]
                public static ItemRequestResult<Index2D> Test<T, TPredicate>(
                    Array2D<T> array, TPredicate match)
                    where TPredicate : struct, IPredicate<T>
                    => array.FindIndex(match);

                [TestCaseSource(typeof(FindIndex), nameof(InvalidStartIndexTestCases))]
                public static void Index2DIntThrowsExceptionIfStartIndexIsOutOfBounds
                    <T>(Array2D<T> array, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(() => array.FindIndex(
                        startIndex, 0, new AlwaysTruePredicate<T>()));

                [TestCaseSource(typeof(FindIndex), nameof(InvalidCountTestCases))]
                public static void Index2DIntThrowsExceptionIfCountExceedsArray2DCount
                    <T>(Array2D<T> array, Index2D startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(() => array.FindIndex(
                        startIndex, count, new AlwaysTruePredicate<T>()));

                [TestCaseSource(typeof(FindIndex), nameof(IPredicateIndex2DIntTestCases))]
                public static ItemRequestResult<Index2D> Test<T, TPredicate>(
                    Array2D<T> array, Index2D startIndex, int count, TPredicate match)
                    where TPredicate : struct, IPredicate<T>
                    => array.FindIndex(startIndex, count, match);

                [TestCaseSource(typeof(FindIndex), nameof(InvalidStartIndexTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds
                    <T>(Array2D<T> array, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(() => array.FindIndex(
                        startIndex, new Bounds2D(), new AlwaysTruePredicate<T>()));

                [TestCaseSource(typeof(FindIndex), nameof(InvalidSectorTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfSectorIsOutOfBounds
                    <T>(Array2D<T> array, Index2D startIndex, Bounds2D sector)
                    => Assert.Throws<ArgumentOutOfRangeException>(() => array.FindIndex(
                        startIndex, sector, new AlwaysTruePredicate<T>()));

                [TestCaseSource(typeof(FindIndex), nameof(IPredicateIndex2DBounds2DTestCases))]
                public static ItemRequestResult<Index2D> Test<T, TPredicate>(
                    Array2D<T> array, Index2D startIndex, Bounds2D sector, TPredicate match)
                    where TPredicate : struct, IPredicate<T>
                    => array.FindIndex(startIndex, sector, match);
            }

            private static IEnumerable<TestCaseData> InvalidStartIndexTestCases()
            {
                yield return new TestCaseData(new Array2D<byte>(2, 3), new Index2D(-1, 0));
                yield return new TestCaseData(new Array2D<byte>(2, 3), new Index2D(0, -1));
                yield return new TestCaseData(new Array2D<byte>(2, 3), new Index2D(2, 0));
                yield return new TestCaseData(new Array2D<byte>(2, 3), new Index2D(0, 3));
                yield return new TestCaseData(new Array2D<byte>(0, 0), new Index2D(0, 0));
                yield return new TestCaseData(new Array2D<byte>(1, 0), new Index2D(0, 0));
                yield return new TestCaseData(new Array2D<byte>(0, 1), new Index2D(0, 0));
            }

            private static IEnumerable<TestCaseData> InvalidCountTestCases()
            {
                yield return new TestCaseData(new Array2D<byte>(2, 3), new Index2D(0, 0), -1);
                yield return new TestCaseData(new Array2D<byte>(2, 3), new Index2D(1, 2), -1);
                yield return new TestCaseData(new Array2D<byte>(2, 3), new Index2D(1, 1), 3);
                yield return new TestCaseData(new Array2D<byte>(2, 3), new Index2D(0, 0), 7);
            }

            private static IEnumerable<TestCaseData> InvalidSectorTestCases()
            {
                yield return new TestCaseData(
                    new Array2D<byte>(2, 3), new Index2D(0, 0), new Bounds2D(3, 0));
                yield return new TestCaseData(
                    new Array2D<byte>(2, 3), new Index2D(0, 0), new Bounds2D(0, 4));
                yield return new TestCaseData(
                    new Array2D<byte>(2, 3), new Index2D(1, 1), new Bounds2D(2, 0));
                yield return new TestCaseData(
                    new Array2D<byte>(2, 3), new Index2D(1, 1), new Bounds2D(0, 3));
            }

            private static IEnumerable<TestCaseData> PredicateTestCases()
            {
                var array3x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 9 },
                                 { 8, 2, 3 } });

                yield return new TestCaseData(array3x3, new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(array3x3, new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }

            private static IEnumerable<TestCaseData> PredicateIndex2DIntTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 9, 9, 1 } });

                yield return new TestCaseData(
                    array2x3, new Index2D(0, 0), 6, new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<Index2D>((1, 0)));
                yield return new TestCaseData(
                    array2x3, new Index2D(1, 2), 0, new Predicate<int>(o => true))
                    .Returns(ItemRequestResult<Index2D>.Fail);
                yield return new TestCaseData(
                    array2x3, new Index2D(1, 1), 2, new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }

            private static IEnumerable<TestCaseData> PredicateIndex2DBounds2DTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 9, 9, 1 } });

                yield return new TestCaseData(
                    array2x3,
                    new Index2D(0, 0),
                    new Bounds2D(2, 3),
                    new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<Index2D>((1, 0)));
                yield return new TestCaseData(
                    array2x3,
                    new Index2D(0, 0),
                    new Bounds2D(0, 0),
                    new Predicate<int>(o => true))
                    .Returns(ItemRequestResult<Index2D>.Fail);
                yield return new TestCaseData(
                    array2x3,
                    new Index2D(1, 1),
                    new Bounds2D(1, 2),
                    new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }

            private static IEnumerable<TestCaseData> IPredicateTestCases()
            {
                var array3x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 9 },
                                 { 8, 2, 3 } });

                yield return new TestCaseData(array3x3, new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(array3x3, new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }

            private static IEnumerable<TestCaseData> IPredicateIndex2DIntTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 9, 9, 1 } });

                yield return new TestCaseData(
                    array2x3, new Index2D(0, 0), 6, new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<Index2D>((1, 0)));
                yield return new TestCaseData(
                    array2x3, new Index2D(1, 2), 0, new AlwaysTruePredicate<int>())
                    .Returns(ItemRequestResult<Index2D>.Fail);
                yield return new TestCaseData(
                    array2x3, new Index2D(1, 1), 2, new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }

            private static IEnumerable<TestCaseData> IPredicateIndex2DBounds2DTestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 9, 9, 1 } });

                yield return new TestCaseData(
                    array2x3,
                    new Index2D(0, 0),
                    new Bounds2D(2, 3),
                    new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<Index2D>((1, 0)));
                yield return new TestCaseData(
                    array2x3,
                    new Index2D(0, 0),
                    new Bounds2D(0, 0),
                    new AlwaysTruePredicate<int>())
                    .Returns(ItemRequestResult<Index2D>.Fail);
                yield return new TestCaseData(
                    array2x3,
                    new Index2D(1, 1),
                    new Bounds2D(1, 2),
                    new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }
        }
    }
}
