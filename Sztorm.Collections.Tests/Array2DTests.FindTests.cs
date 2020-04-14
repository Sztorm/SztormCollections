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
        public static class FindTests
        {
            [Test]
            public static void FindThrowsExceptionIfMatchIsNull()
            {
                var array = new Array2D<int>(0, 0);
                void find() => array.Find(match: null);

                Assert.Throws<ArgumentNullException>(find);
            }

            [TestCaseSource(typeof(FindTests), nameof(FindTestCases))]
            public static ItemRequestResult<T> TestFind<T>(Array2D<T> array, Predicate<T> match)
                => array.Find(match);

            [TestCaseSource(typeof(FindTests), nameof(FindIPredicateTestCases))]
            public static ItemRequestResult<T> TestFind<T, TPredicate>(
                Array2D<T> array, TPredicate match)
                where TPredicate : struct, IPredicate<T>
                => array.Find(match);


            [Test]
            public static void FindLastThrowsExceptionIfMatchIsNull()
            {
                var array = new Array2D<int>(0, 0);
                void findLast() => array.FindLast(match: null);

                Assert.Throws<ArgumentNullException>(findLast);
            }

            [TestCaseSource(typeof(FindTests), nameof(FindLastTestCases))]
            public static ItemRequestResult<T> TestFindLast<T>(Array2D<T> array, Predicate<T> match)
                => array.FindLast(match);

            [TestCaseSource(typeof(FindTests), nameof(FindLastIPredicateTestCases))]
            public static ItemRequestResult<T> TestFindLast<T, TPredicate>(
                Array2D<T> array, TPredicate match)
                where TPredicate : struct, IPredicate<T>
                => array.FindLast(match);


            [TestCaseSource(typeof(FindTests), nameof(FindAllTestCases))]
            public static void TestFindAll<T, TCollection>(
                Array2D<T> array, Predicate<T> match, TCollection expected)
                where TCollection : ICollection<T>, new()
                => CollectionAssert.AreEqual(expected, array.FindAll<TCollection>(match));

            [TestCaseSource(typeof(FindTests), nameof(FindAllIPredicateTestCases))]
            public static void TestFindAll<T, TCollection, TPredicate>(
                Array2D<T> array, TPredicate match, TCollection expected)
                where TCollection : ICollection<T>, new()
                where TPredicate : struct, IPredicate<T>
                => CollectionAssert.AreEqual(expected, array.FindAll<TCollection, TPredicate>(match));

            private static IEnumerable<TestCaseData> FindTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<int>(9));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> FindIPredicateTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<int>(9));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> FindLastTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<int>(8));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> FindLastIPredicateTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new GreaterThanPredicate<int>(8))
                    .Returns(new ItemRequestResult<int>(9));
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                     { 4, 9, 1 },
                                     { 8, 2, 3 } }),
                    new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<int>.Failed);
            }

            private static IEnumerable<TestCaseData> FindAllTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }),
                    new Predicate<int>(o => o > 5),
                    new List<int>() { 9, 8 });
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }),
                    new Predicate<int>(o => o == 10),
                    new List<int>());
            }

            private static IEnumerable<TestCaseData> FindAllIPredicateTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }),
                    new GreaterThanPredicate<int>(5),
                    new List<int>() { 9, 8 });
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }),
                    new EqualsPredicate<int>(10),
                    new List<int>());
            }
        }
    }
}
