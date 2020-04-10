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
        public static void FindIndexThrowsExceptionIfMatchIsNull()
        {
            var array = new Array2D<int>(0, 0);
            void findIndexNull() => array.FindIndex(match: null);

            Assert.Throws<ArgumentNullException>(findIndexNull);
        }

        [Test]
        public static void FindIndex2DThrowsExceptionIfMatchIsNull()
        {
            var array = new Array2D<int>(0, 0);
            void findIndex2DNull() => array.FindIndex2D(match: null);

            Assert.Throws<ArgumentNullException>(findIndex2DNull);
        }

        [TestCaseSource(typeof(Array2DTests), nameof(FindIndexTestCases))]
        public static ItemRequestResult<int> TestFindIndex<T>(Array2D<T> array, Predicate<T> match)
            => array.FindIndex(match);

        [TestCaseSource(typeof(Array2DTests), nameof(FindIndexIPredicateTestCases))]
        public static ItemRequestResult<int> TestFindIndex<T, TPredicate>(
            Array2D<T> array, TPredicate match)
            where TPredicate : struct, IPredicate<T>
            => array.FindIndex(match);

        [TestCaseSource(typeof(Array2DTests), nameof(FindIndex2DTestCases))]
        public static ItemRequestResult<Index2D> TestFindIndex2D<T>(Array2D<T> array, Predicate<T> match)
            => array.FindIndex2D(match);

        [TestCaseSource(typeof(Array2DTests), nameof(FindIndex2DIPredicateTestCases))]
        public static ItemRequestResult<Index2D> TestFindIndex2D<T, TPredicate>(
            Array2D<T> array, TPredicate match)
            where TPredicate : struct, IPredicate<T>
            => array.FindIndex2D(match);

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
    }
}
