﻿using System;
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
        public static class FindIndex2D
        {
            public static class Predicate
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new List2D<int>(0, 0).FindIndex2D(match: null));

                [TestCaseSource(typeof(FindIndex2D), nameof(PredicateTestCases))]
                public static ItemRequestResult<Index2D> Test<T>(
                    List2D<T> list, Predicate<T> match)
                    => list.FindIndex2D(match);

                [Test]
                public static void Index2DIntThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new List2D<int>(0, 0).FindIndex2D((0, 0), 0, match: null));

                [TestCaseSource(typeof(FindIndex2D), nameof(InvalidStartIndexTestCases))]
                public static void Index2DIntThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    List2D<T> list, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.FindIndex2D(startIndex, 0, o => true));

                [TestCaseSource(typeof(FindIndex2D), nameof(InvalidCountTestCases))]
                public static void Index2DIntThrowsExceptionIfCountExceedsList2DCount<T>(
                    List2D<T> list, Index2D startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.FindIndex2D(startIndex, count, o => true));

                [TestCaseSource(typeof(FindIndex2D), nameof(PredicateIndex2DIntTestCases))]
                public static ItemRequestResult<Index2D> Test<T>(
                    List2D<T> list, Index2D startIndex, int count, Predicate<T> match)
                    => list.FindIndex2D(startIndex, count, match);

                [Test]
                public static void Index2DBounds2DThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new List2D<int>(0, 0).FindIndex2D(
                            (0, 0), new Bounds2D(), match: null));

                [TestCaseSource(typeof(FindIndex2D), nameof(InvalidStartIndexTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                    List2D<T> list, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.FindIndex2D(startIndex, new Bounds2D(), o => true));

                [TestCaseSource(typeof(FindIndex2D), nameof(InvalidSectorTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfSectorIsOutOfBounds<T>(
                    List2D<T> list, Index2D startIndex, Bounds2D sector)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.FindIndex2D(startIndex, sector, o => true));

                [TestCaseSource(typeof(FindIndex2D), nameof(PredicateIndex2DBounds2DTestCases))]
                public static ItemRequestResult<Index2D> Test<T>(
                    List2D<T> list, Index2D startIndex, Bounds2D sector, Predicate<T> match)
                    => list.FindIndex2D(startIndex, sector, match);
            }

            public static class IPredicate
            {
                [TestCaseSource(typeof(FindIndex2D), nameof(IPredicateTestCases))]
                public static ItemRequestResult<Index2D> Test<T, TPredicate>(
                    List2D<T> list, TPredicate match)
                    where TPredicate : struct, IPredicate<T>
                    => list.FindIndex2D(match);

                [TestCaseSource(typeof(FindIndex2D), nameof(InvalidStartIndexTestCases))]
                public static void Index2DIntThrowsExceptionIfStartIndexIsOutOfBounds
                    <T>(List2D<T> list, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(() => list.FindIndex2D(
                        startIndex, 0, new AlwaysTruePredicate<T>()));

                [TestCaseSource(typeof(FindIndex2D), nameof(InvalidCountTestCases))]
                public static void Index2DIntThrowsExceptionIfCountExceedsList2DCount
                    <T>(List2D<T> list, Index2D startIndex, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(() => list.FindIndex2D(
                        startIndex, count, new AlwaysTruePredicate<T>()));

                [TestCaseSource(typeof(FindIndex2D), nameof(IPredicateIndex2DIntTestCases))]
                public static ItemRequestResult<Index2D> Test<T, TPredicate>(
                    List2D<T> list, Index2D startIndex, int count, TPredicate match)
                    where TPredicate : struct, IPredicate<T>
                    => list.FindIndex2D(startIndex, count, match);

                [TestCaseSource(typeof(FindIndex2D), nameof(InvalidStartIndexTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds<T>
                    (List2D<T> list, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(() => list.FindIndex2D(
                        startIndex, new Bounds2D(), new AlwaysTruePredicate<T>()));

                [TestCaseSource(typeof(FindIndex2D), nameof(InvalidSectorTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfSectorIsOutOfBounds<T>
                    (List2D<T> list, Index2D startIndex, Bounds2D sector)
                    => Assert.Throws<ArgumentOutOfRangeException>(() => list.FindIndex2D(
                        startIndex, sector, new AlwaysTruePredicate<T>()));

                [TestCaseSource(typeof(FindIndex2D), nameof(IPredicateIndex2DBounds2DTestCases))]
                public static ItemRequestResult<Index2D> Test<T, TPredicate>(
                    List2D<T> list, Index2D startIndex, Bounds2D sector, TPredicate match)
                    where TPredicate : struct, IPredicate<T>
                    => list.FindIndex2D(startIndex, sector, match);
            }

            private static IEnumerable<TestCaseData> InvalidStartIndexTestCases()
            {
                var list2x3 = new List2D<byte>(10, 5);
                list2x3.AddRows(2);
                list2x3.AddColumns(3);

                var list0x0 = new List2D<byte>(10, 5);
                var list1x0 = new List2D<byte>(10, 5);
                list1x0.AddRow();

                var list0x1 = new List2D<byte>(10, 5);
                list0x1.AddColumn();

                yield return new TestCaseData(list2x3, new Index2D(-1, 0));
                yield return new TestCaseData(list2x3, new Index2D(0, -1));
                yield return new TestCaseData(list2x3, new Index2D(2, 0));
                yield return new TestCaseData(list2x3, new Index2D(0, 3));
                yield return new TestCaseData(list0x0, new Index2D(0, 0));
                yield return new TestCaseData(list1x0, new Index2D(0, 0));
                yield return new TestCaseData(list0x1, new Index2D(0, 0));
            }

            private static IEnumerable<TestCaseData> InvalidCountTestCases()
            {
                var list2x3 = new List2D<byte>(10, 5);
                list2x3.AddRows(2);
                list2x3.AddColumns(3);

                yield return new TestCaseData(list2x3, new Index2D(0, 0), -1);
                yield return new TestCaseData(list2x3, new Index2D(1, 2), -1);
                yield return new TestCaseData(list2x3, new Index2D(1, 1), 3);
                yield return new TestCaseData(list2x3, new Index2D(0, 0), 7);
            }

            private static IEnumerable<TestCaseData> InvalidSectorTestCases()
            {
                var list2x3 = new List2D<byte>(10, 5);
                list2x3.AddRows(2);
                list2x3.AddColumns(3);

                yield return new TestCaseData(list2x3, new Index2D(0, 0), new Bounds2D(3, 0));
                yield return new TestCaseData(list2x3, new Index2D(0, 0), new Bounds2D(0, 4));
                yield return new TestCaseData(list2x3, new Index2D(1, 1), new Bounds2D(2, 0));
                yield return new TestCaseData(list2x3, new Index2D(1, 1), new Bounds2D(0, 3));
            }

            private static IEnumerable<TestCaseData> PredicateTestCases()
            {
                var list3x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 9 },
                                 { 8, 2, 3 } });
                list3x3.IncreaseCapacity(new Bounds2D(10, 5));

                yield return new TestCaseData(list3x3, new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(list3x3, new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> PredicateIndex2DIntTestCases()
            {
                var list2x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 9, 9, 1 } });
                list2x3.IncreaseCapacity(new Bounds2D(10, 5));

                yield return new TestCaseData(
                    list2x3, new Index2D(0, 0), 6, new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<Index2D>((1, 0)));
                yield return new TestCaseData(
                    list2x3, new Index2D(1, 2), 0, new Predicate<int>(o => true))
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(
                    list2x3, new Index2D(1, 1), 2, new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(
                    list2x3, new Index2D(0, 2), 4, new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> PredicateIndex2DBounds2DTestCases()
            {
                var list2x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 9, 9, 1 } });
                list2x3.IncreaseCapacity(new Bounds2D(10, 5));

                yield return new TestCaseData(
                    list2x3,
                    new Index2D(0, 0),
                    new Bounds2D(2, 3),
                    new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<Index2D>((1, 0)));
                yield return new TestCaseData(
                    list2x3,
                    new Index2D(0, 0),
                    new Bounds2D(0, 0),
                    new Predicate<int>(o => true))
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(
                    list2x3,
                    new Index2D(1, 1),
                    new Bounds2D(1, 2),
                    new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> IPredicateTestCases()
            {
                var list3x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 9 },
                                 { 8, 2, 3 } });
                list3x3.IncreaseCapacity(new Bounds2D(10, 5));

                yield return new TestCaseData(list3x3, new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<Index2D>((1, 1)));
                yield return new TestCaseData(list3x3, new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> IPredicateIndex2DIntTestCases()
            {
                var list2x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 9, 9, 1 } });
                list2x3.IncreaseCapacity(new Bounds2D(10, 5));

                yield return new TestCaseData(
                    list2x3, new Index2D(0, 0), 6, new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<Index2D>((1, 0)));
                yield return new TestCaseData(
                    list2x3, new Index2D(1, 2), 0, new AlwaysTruePredicate<int>())
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(
                    list2x3, new Index2D(1, 1), 2, new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(
                    list2x3, new Index2D(0, 2), 4, new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }

            private static IEnumerable<TestCaseData> IPredicateIndex2DBounds2DTestCases()
            {
                var list2x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 9, 9, 1 } });
                list2x3.IncreaseCapacity(new Bounds2D(10, 5));

                yield return new TestCaseData(
                    list2x3,
                    new Index2D(0, 0),
                    new Bounds2D(2, 3),
                    new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<Index2D>((1, 0)));
                yield return new TestCaseData(
                    list2x3,
                    new Index2D(0, 0),
                    new Bounds2D(0, 0),
                    new AlwaysTruePredicate<int>())
                    .Returns(ItemRequestResult<Index2D>.Failed);
                yield return new TestCaseData(
                    list2x3,
                    new Index2D(1, 1),
                    new Bounds2D(1, 2),
                    new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<Index2D>.Failed);
            }
        }
    }
}