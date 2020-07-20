using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    using static TestUtils;

    public partial class List2DTests
    {
        public static class FindLastIndex2D
        {
            public static class Predicate
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new List2D<int>(0, 0).FindLastIndex2D(match: null));

                [TestCaseSource(typeof(FindLastIndex2D), nameof(PredicateTestCases))]
                public static ItemRequestResult<Index2D> Test<T>(
                    List2D<T> list, Predicate<T> match)
                    => list.FindLastIndex2D(match);

                [Test]
                public static void Index2DIntThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new List2D<int>(0, 0).FindLastIndex2D((0, 0), 0, match: null));

                [TestCaseSource(typeof(FindLastIndex2D), nameof(InvalidStartIndexCases))]
                public static void Index2DIntThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    List2D<T> list, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.FindLastIndex2D(startIndex, 0, o => true));

                [TestCaseSource(typeof(FindLastIndex2D), nameof(InvalidCountTestCases))]
                public static void Index2DIntThrowsExceptionIfCountExceedsList2DCount<T>(
                    List2D<T> list, Index2D startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.FindLastIndex2D(startIndex, count, o => true));

                [TestCaseSource(typeof(FindLastIndex2D), nameof(PredicateIndex2DIntTestCases))]
                public static ItemRequestResult<Index2D> Test<T>(
                    List2D<T> list, Index2D startIndex, int count, Predicate<T> match)
                    => list.FindLastIndex2D(startIndex, count, match);

                [Test]
                public static void Index2DBounds2DThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new List2D<int>(0, 0).FindLastIndex2D((0, 0), new Bounds2D(), match: null));

                [TestCaseSource(typeof(FindLastIndex2D), nameof(InvalidStartIndexCases))]
                public static void Index2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    List2D<T> list, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.FindLastIndex2D(startIndex, new Bounds2D(), o => true));

                [TestCaseSource(typeof(FindLastIndex2D), nameof(InvalidSectorTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfSectorIsOutOfBounds<T>(
                    List2D<T> list, Index2D startIndex, Bounds2D sector)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.FindLastIndex2D(startIndex, sector, o => true));

                [TestCaseSource(typeof(FindLastIndex2D), nameof(PredicateIndex2DBounds2DTestCases))]
                public static ItemRequestResult<Index2D> Test<T>(
                    List2D<T> list, Index2D startIndex, Bounds2D sector, Predicate<T> match)
                    => list.FindLastIndex2D(startIndex, sector, match);
            }

            public static class IPredicate
            {
                [TestCaseSource(typeof(FindLastIndex2D), nameof(IPredicateTestCases))]
                public static ItemRequestResult<Index2D> Test<T, TPredicate>(
                    List2D<T> list, TPredicate match)
                    where TPredicate : struct, IPredicate<T>
                    => list.FindLastIndex2D(match);

                [TestCaseSource(typeof(FindLastIndex2D), nameof(InvalidStartIndexCases))]
                public static void Index2DIntIPredicateThrowsExceptionIfStartIndexIsOutOfBounds<T>
                    (List2D<T> list, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.FindLastIndex2D(startIndex, 0, new AlwaysTruePredicate<T>()));

                [TestCaseSource(typeof(FindLastIndex2D), nameof(InvalidCountTestCases))]
                public static void Index2DIntIPredicateThrowsExceptionIfCountExceedsList2DCount<T>
                    (List2D<T> list, Index2D startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.FindLastIndex2D(
                            startIndex, count, new AlwaysTruePredicate<T>()));

                [TestCaseSource(typeof(FindLastIndex2D), nameof(Index2DIntIPredicateTestCases))]
                public static ItemRequestResult<Index2D> Test<T, TPredicate>(
                    List2D<T> list, Index2D startIndex, int count, TPredicate match)
                    where TPredicate : struct, IPredicate<T>
                    => list.FindLastIndex2D(startIndex, count, match);

                [TestCaseSource(typeof(FindLastIndex2D), nameof(InvalidStartIndexCases))]
                public static void Index2DBounds2DIPredicateThrowsExceptionIfStartIndexIsOutOfBounds
                    <T>(List2D<T> list, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.FindLastIndex2D(
                            startIndex, new Bounds2D(), new AlwaysTruePredicate<T>()));

                [TestCaseSource(typeof(FindLastIndex2D), nameof(InvalidSectorTestCases))]
                public static void Index2DBounds2DIPredicateThrowsExceptionIfSectorIsOutOfBounds<T>
                    (List2D<T> list, Index2D startIndex, Bounds2D sector)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.FindLastIndex2D(
                            startIndex, sector, new AlwaysTruePredicate<T>()));

                [TestCaseSource(
                    typeof(FindLastIndex2D), nameof(Index2DBounds2DIPredicateTestCases))]
                public static ItemRequestResult<Index2D> Test<T, TPredicate>(
                    List2D<T> list, Index2D startIndex, Bounds2D sector, TPredicate match)
                    where TPredicate : struct, IPredicate<T>
                    => list.FindLastIndex2D(startIndex, sector, match);
            }

            private static IEnumerable<TestCaseData> InvalidStartIndexCases()
            {
                var list2x3 = new List2D<byte>(10, 5);
                list2x3.AddRows(2);
                list2x3.AddColumns(3);

                var list0x0 = new List2D<byte>(10, 5);
                var list1x0 = new List2D<byte>(10, 5);
                list1x0.AddRow();

                var list0x1 = new List2D<byte>(10, 5);
                list0x1.AddColumn();

                yield return new TestCaseData(list2x3, new Index2D(2, 0));
                yield return new TestCaseData(list2x3, new Index2D(0, 3));
                yield return new TestCaseData(list2x3, new Index2D(-1, 0));
                yield return new TestCaseData(list2x3, new Index2D(0, -1));
                yield return new TestCaseData(list0x0, new Index2D(0, 0));
                yield return new TestCaseData(list1x0, new Index2D(0, 0));
                yield return new TestCaseData(list0x1, new Index2D(0, 0));
            }

            private static IEnumerable<TestCaseData> InvalidCountTestCases()
            {
                var list2x3 = new List2D<byte>(10, 5);
                list2x3.AddRows(2);
                list2x3.AddColumns(3);

                yield return new TestCaseData(list2x3, new Index2D(1, 2), -1);
                yield return new TestCaseData(list2x3, new Index2D(0, 0), -1);
                yield return new TestCaseData(list2x3, new Index2D(0, 1), 3);
                yield return new TestCaseData(list2x3, new Index2D(1, 2), 7);
            }

            private static IEnumerable<TestCaseData> InvalidSectorTestCases()
            {
                var list2x3 = new List2D<byte>(10, 5);
                list2x3.AddRows(2);
                list2x3.AddColumns(3);

                yield return new TestCaseData(list2x3, new Index2D(1, 2), new Bounds2D(3, 0));
                yield return new TestCaseData(list2x3, new Index2D(1, 2), new Bounds2D(0, 4));
                yield return new TestCaseData(list2x3, new Index2D(0, 1), new Bounds2D(2, 0));
                yield return new TestCaseData(list2x3, new Index2D(0, 1), new Bounds2D(0, 3));
            }
      
            private static IEnumerable<TestCaseData> PredicateTestCases()
            {
                var list3x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 0, 3 } });
                list3x3.IncreaseCapacity(new Bounds2D(10, 5));

                yield return new TestCaseData(list3x3, new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<Index2D>((2, 0)));
                yield return new TestCaseData(list3x3, new Predicate<int>(o => o == 2))
                    .Returns(new ItemRequestResult<Index2D>((0, 0)));
                yield return new TestCaseData(list3x3, new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }

            private static IEnumerable<TestCaseData> PredicateIndex2DIntTestCases()
            {
                var list2x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 0 },
                                 { 8, 9, 1 } });
                list2x3.IncreaseCapacity(new Bounds2D(10, 5));

                yield return new TestCaseData(
                    list2x3, new Index2D(1, 2), 6, new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(
                    list2x3, new Index2D(0, 0), 0, new Predicate<int>(o => true))
                    .Returns(ItemRequestResult<Index2D>.Fail);
                yield return new TestCaseData(
                    list2x3, new Index2D(0, 1), 2, new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
                yield return new TestCaseData(
                    list2x3, new Index2D(1, 0), 4, new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }

            private static IEnumerable<TestCaseData> PredicateIndex2DBounds2DTestCases()
            {
                var list2x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 0 },
                                 { 8, 9, 1 } });
                list2x3.IncreaseCapacity(new Bounds2D(10, 5));

                yield return new TestCaseData(
                    list2x3,
                    new Index2D(1, 2),
                    new Bounds2D(2, 3),
                    new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(
                    list2x3,
                    new Index2D(1, 2),
                    new Bounds2D(0, 0),
                    new Predicate<int>(o => true))
                    .Returns(ItemRequestResult<Index2D>.Fail);
                yield return new TestCaseData(
                    list2x3,
                    new Index2D(0, 1),
                    new Bounds2D(1, 2),
                    new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }   
            
            private static IEnumerable<TestCaseData> IPredicateTestCases()
            {
                var list3x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 0, 3 } });
                list3x3.IncreaseCapacity(new Bounds2D(10, 5));

                yield return new TestCaseData(list3x3, new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<Index2D>((2, 0)));
                yield return new TestCaseData(list3x3, new EqualsPredicate<int>(2))
                    .Returns(new ItemRequestResult<Index2D>((0, 0)));
                yield return new TestCaseData(list3x3, new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }

            private static IEnumerable<TestCaseData> Index2DIntIPredicateTestCases()
            {
                var list2x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 0 },
                                 { 8, 9, 1 } });
                list2x3.IncreaseCapacity(new Bounds2D(10, 5));

                yield return new TestCaseData(
                    list2x3, new Index2D(1, 2), 6, new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(
                    list2x3, new Index2D(0, 0), 0, new AlwaysTruePredicate<int>())
                    .Returns(ItemRequestResult<Index2D>.Fail);
                yield return new TestCaseData(
                    list2x3, new Index2D(0, 1), 2, new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
                yield return new TestCaseData(
                    list2x3, new Index2D(1, 0), 4, new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }

            private static IEnumerable<TestCaseData> Index2DBounds2DIPredicateTestCases()
            {
                var list2x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 0 },
                                 { 8, 9, 1 } });
                list2x3.IncreaseCapacity(new Bounds2D(10, 5));

                yield return new TestCaseData(
                    list2x3,
                    new Index2D(1, 2),
                    new Bounds2D(2, 3),
                    new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(
                    list2x3,
                    new Index2D(1, 2),
                    new Bounds2D(0, 0),
                    new AlwaysTruePredicate<int>())
                    .Returns(ItemRequestResult<Index2D>.Fail);
                yield return new TestCaseData(
                    list2x3,
                    new Index2D(0, 1),
                    new Bounds2D(1, 2),
                    new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }
        }    
    }
}
