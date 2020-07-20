using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    public partial class Array2DTest
    {
        public static class FindAll
        {
            public static class Items
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Array2D<object>(0, 0).FindAll<List<object>>(match: null));

                [TestCaseSource(typeof(FindAll), nameof(ItemsTestCases))]
                public static void TestFindAll<T, TCollection>(
                    Array2D<T> array, Predicate<T> match, TCollection expected)
                    where TCollection : ICollection<T>, new()
                    => CollectionAssert.AreEqual(expected, array.FindAll<TCollection>(match));

                [TestCaseSource(typeof(FindAll), nameof(ItemsIPredicateTestCases))]
                public static void TestFindAll<T, TCollection, TPredicate>(
                    Array2D<T> array, TPredicate match, TCollection expected)
                    where TCollection : ICollection<T>, new()
                    where TPredicate : struct, IPredicate<T>
                    => CollectionAssert.AreEqual(
                        expected, array.FindAll<TCollection, TPredicate>(match));
            }

            public static class Indices
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Array2D<int>(0, 0).FindAllIndices<List<Index2D>>(match: null));

                [TestCaseSource(typeof(FindAll), nameof(IndicesTestCases))]
                public static void TestFindAll<T, Index2DCollection>(
                    Array2D<T> array, Predicate<T> match, Index2DCollection expected)
                    where Index2DCollection : ICollection<Index2D>, new()
                    => CollectionAssert.AreEqual(
                        expected, array.FindAllIndices<Index2DCollection>(match));

                [TestCaseSource(typeof(FindAll), nameof(IndicesIPredicateTestCases))]
                public static void TestFindAll<T, Index2DCollection, TPredicate>(
                    Array2D<T> array, TPredicate match, Index2DCollection expected)
                    where Index2DCollection : ICollection<Index2D>, new()
                    where TPredicate : struct, IPredicate<T>
                    => CollectionAssert.AreEqual(
                        expected, array.FindAllIndices<Index2DCollection, TPredicate>(match));
            }

            private static IEnumerable<TestCaseData> ItemsTestCases()
            {
                var array3x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });

                yield return new TestCaseData(
                    array3x3,
                    new Predicate<int>(o => o > 5),
                    new List<int>() { 9, 8 });
                yield return new TestCaseData(
                    array3x3,
                    new Predicate<int>(o => o == 10),
                    new List<int>());
            }

            private static IEnumerable<TestCaseData> ItemsIPredicateTestCases()
            {
                var array3x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });

                yield return new TestCaseData(
                    array3x3,
                    new GreaterThanPredicate<int>(5),
                    new List<int>() { 9, 8 });
                yield return new TestCaseData(
                    array3x3,
                    new EqualsPredicate<int>(10),
                    new List<int>());
            }

            private static IEnumerable<TestCaseData> IndicesTestCases()
            {
                var array3x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });

                yield return new TestCaseData(
                    array3x3,
                    new Predicate<int>(o => o > 5),
                    new List<Index2D>() { (1, 1), (2, 0) });
                yield return new TestCaseData(
                    array3x3,
                    new Predicate<int>(o => o == 10),
                    new List<Index2D>());
            }

            private static IEnumerable<TestCaseData> IndicesIPredicateTestCases()
            {
                var array3x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });

                yield return new TestCaseData(
                    array3x3,
                    new GreaterThanPredicate<int>(5),
                    new List<Index2D>() { (1, 1), (2, 0) });
                yield return new TestCaseData(
                    array3x3,
                    new EqualsPredicate<int>(10),
                    new List<Index2D>());
            }
        }
    }
}
