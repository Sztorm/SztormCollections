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

        public static class FindIndexTests
        {
            #region FindIndex(Predicate<T>)

            [Test]
            public static void FindIndexThrowsExceptionIfMatchIsNull()
            {
                var array = new Array2D<int>(0, 0);
                void findIndexNull() => array.FindIndex(match: null);

                Assert.Throws<ArgumentNullException>(findIndexNull);
            }

            [TestCaseSource(typeof(FindIndexTests), nameof(FindIndexTestCases))]
            public static ItemRequestResult<int> TestFindIndex<T>(
                Array2D<T> array, Predicate<T> match)
                => array.FindIndex(match);

            #endregion

            #region FindIndex(Index2D, int, Predicate<T>)

            [Test]
            public static void FindIndexIndex2DIntThrowsExceptionIfMatchIsNull()
            {
                var array = new Array2D<int>(0, 0);
                void findIndexNull() => array.FindIndex((0, 0), 0, match: null);

                Assert.Throws<ArgumentNullException>(findIndexNull);
            }

            [TestCaseSource(typeof(FindIndexTests), nameof(FindIndexTestInvalidStartIndexCases))]
            public static void FindIndexIndex2DIntThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                Array2D<T> array, Index2D startIndex)
            {
                void findIndex() => array.FindIndex(startIndex, 0, o => true);

                Assert.Throws<ArgumentOutOfRangeException>(findIndex);
            }

            [TestCaseSource(typeof(FindIndexTests), nameof(FindIndexInvalidCountTestCases))]
            public static void FindIndexIndex2DIntThrowsExceptionIfCountExceedsArray2DCount<T>(
                Array2D<T> array, Index2D startIndex, int count)
            {
                void findIndex() => array.FindIndex(startIndex, count, o => true);

                Assert.Throws<ArgumentOutOfRangeException>(findIndex);
            }

            [TestCaseSource(typeof(FindIndexTests), nameof(FindIndexIndex2DIntTestCases))]
            public static ItemRequestResult<int> TestFindIndex<T>(
                Array2D<T> array, Index2D startIndex, int count, Predicate<T> match)
                => array.FindIndex(startIndex, count, match);

            #endregion

            #region FindIndex(Index2D, Bounds2D, Predicate<T>)

            [Test]
            public static void FindIndexIndex2DBounds2DThrowsExceptionIfMatchIsNull()
            {
                var array = new Array2D<int>(0, 0);
                void findIndexNull() => array.FindIndex((0, 0), array.Boundaries, match: null);

                Assert.Throws<ArgumentNullException>(findIndexNull);
            }

            [TestCaseSource(typeof(FindIndexTests), nameof(FindIndexTestInvalidStartIndexCases))]
            public static void FindIndexIndex2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                Array2D<T> array, Index2D startIndex)
            {
                void findIndex() => array.FindIndex(startIndex, new Bounds2D(), o => true);
                Assert.Throws<ArgumentOutOfRangeException>(findIndex);
            }

            [TestCaseSource(typeof(FindIndexTests), nameof(FindIndexTestInvalidSectorTestCases))]
            public static void FindIndexIndex2DBounds2DThrowsExceptionIfSectorIsOutOfBounds<T>(
                Array2D<T> array, Index2D startIndex, Bounds2D sector)
            {
                void findIndex() => array.FindIndex(startIndex, sector, o => true);

                Assert.Throws<ArgumentOutOfRangeException>(findIndex);
            }

            [TestCaseSource(typeof(FindIndexTests), nameof(FindIndexIndex2DBounds2DTestCases))]
            public static ItemRequestResult<int> TestFindIndex<T>(
                Array2D<T> array, Index2D startIndex, Bounds2D sector, Predicate<T> match)
                => array.FindIndex(startIndex, sector, match);

            #endregion

            #region FindIndex<TPredicate>(TPredicate)

            [TestCaseSource(typeof(FindIndexTests), nameof(FindIndexIPredicateTestCases))]
            public static ItemRequestResult<int> TestFindIndex<T, TPredicate>(
                Array2D<T> array, TPredicate match)
                where TPredicate : struct, IPredicate<T>
                => array.FindIndex(match);

            #endregion

            #region FindIndex<TPredicate>(Index2D, int, TPredicate)

            [TestCaseSource(typeof(FindIndexTests), nameof(FindIndexTestInvalidStartIndexCases))]
            public static void FindIndexIndex2DIntIPredicateThrowsExceptionIfStartIndexIsOutOfBounds
                <T>(Array2D<T> array, Index2D startIndex)
            {
                void findIndex() => array.FindIndex(
                    startIndex, 0, new TestUtils.AlwaysTruePredicate<T>());

                Assert.Throws<ArgumentOutOfRangeException>(findIndex);
            }

            [TestCaseSource(typeof(FindIndexTests), nameof(FindIndexInvalidCountTestCases))]
            public static void FindIndexIndex2DIntIPredicateThrowsExceptionIfCountExceedsArray2DCount
                <T>(Array2D<T> array, Index2D startIndex, int count)
            {
                void findIndex() => array.FindIndex(
                    startIndex, count, new TestUtils.AlwaysTruePredicate<T>());

                Assert.Throws<ArgumentOutOfRangeException>(findIndex);
            }

            [TestCaseSource(
                typeof(FindIndexTests), nameof(FindIndexIndex2DIntIPredicateTestCases))]
            public static ItemRequestResult<int> TestFindIndex<T, TPredicate>(
                Array2D<T> array, Index2D startIndex, int count, TPredicate match)
                where TPredicate : struct, IPredicate<T>
                => array.FindIndex(startIndex, count, match);

            #endregion

            #region FindIndex<TPredicate>(Index2D, Bounds2D, TPredicate)

            [TestCaseSource(typeof(FindIndexTests), nameof(FindIndexTestInvalidStartIndexCases))]
            public static void FindIndexIndex2DBounds2DIPredicateThrowsExceptionIfStartIndexIsOutOfBounds
                <T>(Array2D<T> array, Index2D startIndex)
            {
                void findIndex() => array.FindIndex(
                    startIndex, new Bounds2D(), new TestUtils.AlwaysTruePredicate<T>());
                Assert.Throws<ArgumentOutOfRangeException>(findIndex);
            }

            [TestCaseSource(typeof(FindIndexTests), nameof(FindIndexTestInvalidSectorTestCases))]
            public static void FindIndexIndex2DBounds2DIPredicateThrowsExceptionIfSectorIsOutOfBounds
                <T>(Array2D<T> array, Index2D startIndex, Bounds2D sector)
            {
                void findIndex() => array.FindIndex(
                    startIndex, sector, new TestUtils.AlwaysTruePredicate<T>());

                Assert.Throws<ArgumentOutOfRangeException>(findIndex);
            }

            [TestCaseSource(
                typeof(FindIndexTests), nameof(FindIndexIndex2DBounds2DIPredicateTestCases))]
            public static ItemRequestResult<int> TestFindIndex<T, TPredicate>(
                Array2D<T> array, Index2D startIndex, Bounds2D sector, TPredicate match)
                where TPredicate : struct, IPredicate<T>
                => array.FindIndex(startIndex, sector, match);

            #endregion

            private static IEnumerable<TestCaseData> FindIndexTestInvalidStartIndexCases()
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

            private static IEnumerable<TestCaseData> FindIndexInvalidCountTestCases()
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

            private static IEnumerable<TestCaseData> FindIndexTestInvalidSectorTestCases()
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

            private static IEnumerable<TestCaseData> FindIndexTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> FindIndexIndex2DIntTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    6,
                    new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 2),
                    0,
                    new Predicate<int>(o => true))
                    .Returns(ItemRequestResult<int>.Failed);
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 1),
                    2,
                    new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> FindIndexIndex2DBounds2DTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    new Bounds2D(2, 3),
                    new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    new Bounds2D(0, 0),
                    new Predicate<int>(o => true))
                    .Returns(ItemRequestResult<int>.Failed);
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 1),
                    new Bounds2D(1, 2),
                    new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> FindIndexIPredicateTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> FindIndexIndex2DIntIPredicateTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    6,
                    new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 2),
                    0,
                    new TestUtils.AlwaysTruePredicate<int>())
                    .Returns(ItemRequestResult<int>.Failed);
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 1),
                    2,
                    new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> FindIndexIndex2DBounds2DIPredicateTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    new Bounds2D(2, 3),
                    new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<int>(4));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(0, 0),
                    new Bounds2D(0, 0),
                    new TestUtils.AlwaysTruePredicate<int>())
                    .Returns(ItemRequestResult<int>.Failed);
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 } }),
                    new Index2D(1, 1),
                    new Bounds2D(1, 2),
                    new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<int>.Failed);
            }
        }    
    }
}
