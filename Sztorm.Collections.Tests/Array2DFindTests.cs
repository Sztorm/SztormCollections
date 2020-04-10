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
        [Test]
        public static void FindThrowsExceptionIfMatchIsNull()
        {
            var array = new Array2D<int>(0, 0);
            void findNull() => array.Find(match: null);

            Assert.Throws<ArgumentNullException>(findNull);
        }

        [TestCaseSource(typeof(Array2DTests), nameof(FindTestCases))]
        public static ItemRequestResult<T> TestFind<T>(Array2D<T> array, Predicate<T> match)
            => array.Find(match);

        [TestCaseSource(typeof(Array2DTests), nameof(FindIPredicateTestCases))]
        public static ItemRequestResult<T> TestFind<T, TPredicate>(
            Array2D<T> array, TPredicate match)
            where TPredicate : struct, IPredicate<T>
            => array.Find(match);

        [TestCaseSource(typeof(Array2DTests), nameof(FindAllTestCases))]
        public static void TestFindAll<T, TCollection>(
            Array2D<T> array, Predicate<T> match, TCollection expected)
            where TCollection : ICollection<T>, new()
            => CollectionAssert.AreEqual(expected, array.FindAll<TCollection>(match));


        [TestCaseSource(typeof(Array2DTests), nameof(FindAllIPredicateTestCases))]
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
