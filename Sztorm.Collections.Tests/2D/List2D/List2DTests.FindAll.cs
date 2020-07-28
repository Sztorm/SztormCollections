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
        public static class FindAll
        {
            public static class Items
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new List2D<object>(0, 0).FindAll<List<object>>(match: null));

                [TestCaseSource(typeof(FindAll), nameof(ItemsTestCases))]
                public static void TestFindAll<T, TCollection>(
                    List2D<T> list, Predicate<T> match, TCollection expected)
                    where TCollection : ICollection<T>, new()
                    => CollectionAssert.AreEqual(expected, list.FindAll<TCollection>(match));

                [TestCaseSource(typeof(FindAll), nameof(ItemsIPredicateTestCases))]
                public static void TestFindAll<T, TCollection, TPredicate>(
                    List2D<T> list, TPredicate match, TCollection expected)
                    where TCollection : ICollection<T>, new()
                    where TPredicate : struct, IPredicate<T>
                    => CollectionAssert.AreEqual(
                        expected, list.FindAll<TCollection, TPredicate>(match));
            }

            public static class Indices
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new List2D<int>(0, 0).FindAllIndices<List<Index2D>>(match: null));

                [TestCaseSource(typeof(FindAll), nameof(IndicesTestCases))]
                public static void TestFindAll<T, Index2DCollection>(
                    List2D<T> list, Predicate<T> match, Index2DCollection expected)
                    where Index2DCollection : ICollection<Index2D>, new()
                    => CollectionAssert.AreEqual(
                        expected, list.FindAllIndices<Index2DCollection>(match));

                [TestCaseSource(typeof(FindAll), nameof(IndicesIPredicateTestCases))]
                public static void TestFindAll<T, Index2DCollection, TPredicate>(
                    List2D<T> list, TPredicate match, Index2DCollection expected)
                    where Index2DCollection : ICollection<Index2D>, new()
                    where TPredicate : struct, IPredicate<T>
                    => CollectionAssert.AreEqual(
                        expected, list.FindAllIndices<Index2DCollection, TPredicate>(match));
            }

            private static IEnumerable<TestCaseData> ItemsTestCases()
            {
                var list3x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });
                list3x3.IncreaseCapacity(list3x3.Boundaries);

                yield return new TestCaseData(
                    list3x3,
                    new Predicate<int>(o => o > 5),
                    new List<int>() { 9, 8 });
                yield return new TestCaseData(
                    list3x3,
                    new Predicate<int>(o => o == 10),
                    new List<int>());
            }

            private static IEnumerable<TestCaseData> ItemsIPredicateTestCases()
            {
                var list3x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });
                list3x3.IncreaseCapacity(list3x3.Boundaries);

                yield return new TestCaseData(
                    list3x3,
                    new GreaterThanPredicate<int>(5),
                    new List<int>() { 9, 8 });
                yield return new TestCaseData(
                    list3x3,
                    new EqualsPredicate<int>(10),
                    new List<int>());
            }

            private static IEnumerable<TestCaseData> IndicesTestCases()
            {
                var list3x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });
                list3x3.IncreaseCapacity(list3x3.Boundaries);

                yield return new TestCaseData(
                    list3x3,
                    new Predicate<int>(o => o > 5),
                    new List<Index2D>() { (1, 1), (2, 0) });
                yield return new TestCaseData(
                    list3x3,
                    new Predicate<int>(o => o == 10),
                    new List<Index2D>());
            }

            private static IEnumerable<TestCaseData> IndicesIPredicateTestCases()
            {
                var list3x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });
                list3x3.IncreaseCapacity(list3x3.Boundaries);

                yield return new TestCaseData(
                    list3x3,
                    new GreaterThanPredicate<int>(5),
                    new List<Index2D>() { (1, 1), (2, 0) });
                yield return new TestCaseData(
                    list3x3,
                    new EqualsPredicate<int>(10),
                    new List<Index2D>());
            }
        }
    }
}
