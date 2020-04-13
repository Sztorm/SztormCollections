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
        // Regions are completely justified considering that unit tests are not really object-oriented
        // so OOP principles do not apply in this case. Also, unit tests do not interact with each
        // other so separation of them (in this case separating tests by different method overloads)
        // is not a bad thing.

        public static class FindIndex2DTests
        {
            #region FindIndex2D(Predicate<T>)

            [Test]
            public static void FindIndex2DThrowsExceptionIfMatchIsNull()
            {
                var array = new Array2D<int>(0, 0);
                void findIndex2DNull() => array.FindIndex2D(match: null);

                Assert.Throws<ArgumentNullException>(findIndex2DNull);
            }

            [TestCaseSource(typeof(FindIndex2DTests), nameof(FindIndex2DTestCases))]
            public static ItemRequestResult<Index2D> TestFindIndex2D<T>(
                Array2D<T> array, Predicate<T> match)
                => array.FindIndex2D(match);

            #endregion

            #region FindIndex2D(Index2D, int, Predicate<T>)

            [Test]
            public static void FindIndex2DIndex2DIntThrowsExceptionIfMatchIsNull()
            {
                var array = new Array2D<int>(0, 0);
                void findIndex2DNull() => array.FindIndex2D((0, 0), 0, match: null);

                Assert.Throws<ArgumentNullException>(findIndex2DNull);
            }

            [TestCaseSource(typeof(FindIndex2DTests), nameof(FindIndex2DTestInvalidStartIndexCases))]
            public static void FindIndex2DIndex2DIntThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                Array2D<T> array, Index2D startIndex)
            {
                void findIndex2D() => array.FindIndex2D(startIndex, 0, o => true);
                Assert.Throws<ArgumentOutOfRangeException>(findIndex2D);
            }

            [TestCaseSource(typeof(FindIndex2DTests), nameof(FindIndex2DInvalidCountTestCases))]
            public static void FindIndex2DIndex2DIntThrowsExceptionIfCountExceedsArray2DCount<T>(
                Array2D<T> array, Index2D startIndex, int count)
            {
                void findIndex2D() => array.FindIndex2D(startIndex, count, o => true);

                Assert.Throws<ArgumentOutOfRangeException>(findIndex2D);
            }

            [TestCaseSource(typeof(FindIndex2DTests), nameof(FindIndex2DIndex2DIntTestCases))]
            public static ItemRequestResult<Index2D> TestFindIndex2D<T>(
                Array2D<T> array, Index2D startIndex, int count, Predicate<T> match)
                => array.FindIndex2D(startIndex, count, match);

            #endregion

            #region FindIndex2D(Index2D, Bounds2D, Predicate<T>)

            [Test]
            public static void FindIndex2DIndex2DBounds2DThrowsExceptionIfMatchIsNull()
            {
                var array = new Array2D<int>(0, 0);
                void findIndex2DNull() => array.FindIndex2D((0, 0), array.Boundaries, match: null);

                Assert.Throws<ArgumentNullException>(findIndex2DNull);
            }

            [TestCaseSource(typeof(FindIndex2DTests), nameof(FindIndex2DTestInvalidStartIndexCases))]
            public static void FindIndex2DIndex2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                Array2D<T> array, Index2D startIndex)
            {
                void findIndex2D() => array.FindIndex2D(startIndex, new Bounds2D(), o => true);

                Assert.Throws<ArgumentOutOfRangeException>(findIndex2D);
            }

            [TestCaseSource(typeof(FindIndex2DTests), nameof(FindIndex2DTestInvalidSectorTestCases))]
            public static void FindIndex2DIndex2DBounds2DThrowsExceptionIfSectorIsOutOfBounds<T>(
                Array2D<T> array, Index2D startIndex, Bounds2D sector)
            {
                void findIndex2D() => array.FindIndex2D(startIndex, sector, o => true);

                Assert.Throws<ArgumentOutOfRangeException>(findIndex2D);
            }

            [TestCaseSource(typeof(FindIndex2DTests), nameof(FindIndex2DIndex2DBounds2DTestCases))]
            public static ItemRequestResult<Index2D> TestFindIndex2D<T>(
                Array2D<T> array, Index2D startIndex, Bounds2D sector, Predicate<T> match)
                => array.FindIndex2D(startIndex, sector, match);

            #endregion

            #region FindIndex2D<TPredicate>(TPredicate)

            [TestCaseSource(typeof(FindIndex2DTests), nameof(FindIndex2DIPredicateTestCases))]
            public static ItemRequestResult<Index2D> TestFindIndex2D<T, TPredicate>(
                Array2D<T> array, TPredicate match)
                where TPredicate : struct, IPredicate<T>
                => array.FindIndex2D(match);

            #endregion

            #region FindIndex2D<TPredicate>(Index2D, int, TPredicate)

            [TestCaseSource(typeof(FindIndex2DTests), nameof(FindIndex2DTestInvalidStartIndexCases))]
            public static void FindIndex2DIndex2DIntIPredicateThrowsExceptionIfStartIndexIsOutOfBounds
                <T>(Array2D<T> array, Index2D startIndex)
            {
                void findIndex2D() => array.FindIndex2D(
                    startIndex, 0, new TestUtils.AlwaysTruePredicate<T>());

                Assert.Throws<ArgumentOutOfRangeException>(findIndex2D);
            }

            [TestCaseSource(typeof(FindIndex2DTests), nameof(FindIndex2DInvalidCountTestCases))]
            public static void FindIndex2DIndex2DIntIPredicateThrowsExceptionIfCountExceedsArray2DCount
                <T>(Array2D<T> array, Index2D startIndex, int count)
            {
                void findIndex2D() => array.FindIndex2D(
                    startIndex, count, new TestUtils.AlwaysTruePredicate<T>());

                Assert.Throws<ArgumentOutOfRangeException>(findIndex2D);
            }

            [TestCaseSource(
                typeof(FindIndex2DTests), nameof(FindIndex2DIndex2DIntIPredicateTestCases))]
            public static ItemRequestResult<Index2D> TestFindIndex2D<T, TPredicate>(
                Array2D<T> array, Index2D startIndex, int count, TPredicate match)
                where TPredicate : struct, IPredicate<T>
                => array.FindIndex2D(startIndex, count, match);

            #endregion

            #region FindIndex2D<TPredicate>(Index2D, Bounds2D, TPredicate)

            [TestCaseSource(typeof(FindIndex2DTests), nameof(FindIndex2DTestInvalidStartIndexCases))]
            public static void FindIndex2DIndex2DBounds2DIPredicateThrowsExceptionIfStartIndexIsOutOfBounds
                <T>(Array2D<T> array, Index2D startIndex)
            {
                void findIndex2D() => array.FindIndex2D(
                    startIndex, new Bounds2D(), new TestUtils.AlwaysTruePredicate<T>());
                Assert.Throws<ArgumentOutOfRangeException>(findIndex2D);
            }

            [TestCaseSource(typeof(FindIndex2DTests), nameof(FindIndex2DTestInvalidSectorTestCases))]
            public static void FindIndex2DIndex2DBounds2DIPredicateThrowsExceptionIfSectorIsOutOfBounds
                <T>(Array2D<T> array, Index2D startIndex, Bounds2D sector)
            {
                void findIndex2D() => array.FindIndex2D(
                    startIndex, sector, new TestUtils.AlwaysTruePredicate<T>());

                Assert.Throws<ArgumentOutOfRangeException>(findIndex2D);
            }

            [TestCaseSource(
                typeof(FindIndex2DTests), nameof(FindIndex2DIndex2DBounds2DIPredicateTestCases))]
            public static ItemRequestResult<Index2D> TestFind2DIndex<T, TPredicate>(
                Array2D<T> array, Index2D startIndex, Bounds2D sector, TPredicate match)
                where TPredicate : struct, IPredicate<T>
                => array.FindIndex2D(startIndex, sector, match);

            #endregion

            private static IEnumerable<TestCaseData> FindIndex2DTestInvalidStartIndexCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(-1, 0));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, -1));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(2, 0));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 3));
                yield return new TestCaseData(
                    new Array2D<int>(0, 0),
                    new Index2D(0, 0));
                yield return new TestCaseData(
                    new Array2D<int>(1, 0),
                    new Index2D(0, 0));
                yield return new TestCaseData(
                    new Array2D<int>(0, 1),
                    new Index2D(0, 0));
            }

            private static IEnumerable<TestCaseData> FindIndex2DInvalidCountTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    -1);
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 2),
                    -1);
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 1),
                    3);
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    7);
            }

            private static IEnumerable<TestCaseData> FindIndex2DTestInvalidSectorTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    new Bounds2D(3, 0));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    new Bounds2D(0, 4));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 1),
                    new Bounds2D(2, 0));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 1),
                    new Bounds2D(0, 3));
            }

            private static IEnumerable<TestCaseData> FindIndex2DTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<Index2D>(new Index2D(1, 1)));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> FindIndex2DIndex2DIntTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    6,
                    new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 2),
                    0,
                    new Predicate<int>(o => true))
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 1),
                    2,
                    new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> FindIndex2DIndex2DBounds2DTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    new Bounds2D(2, 3),
                    new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    new Bounds2D(0, 0),
                    new Predicate<int>(o => true))
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 1),
                    new Bounds2D(1, 2),
                    new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> FindIndex2DIPredicateTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<Index2D>(new Index2D(1, 1)));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> FindIndex2DIndex2DIntIPredicateTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    6,
                    new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 2),
                    0,
                    new TestUtils.AlwaysTruePredicate<int>())
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 1),
                    2,
                    new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> FindIndex2DIndex2DBounds2DIPredicateTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    new Bounds2D(2, 3),
                    new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    new Bounds2D(0, 0),
                    new TestUtils.AlwaysTruePredicate<int>())
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 1),
                    new Bounds2D(1, 2),
                    new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }
        }
    }
}
