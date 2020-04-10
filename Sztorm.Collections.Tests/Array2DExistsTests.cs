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
        public static void ExistsThrowsExceptionIfMatchIsNull()
        {
            var array = new Array2D<int>(0, 0);
            void ExistsNull() => array.Exists(match: null);

            Assert.Throws<ArgumentNullException>(ExistsNull);
        }

        [TestCaseSource(typeof(Array2DTests), nameof(ExistsTestCases))]
        public static bool TestExists<T>(Array2D<T> array, Predicate<T> match)
            => array.Exists(match);

        [TestCaseSource(typeof(Array2DTests), nameof(ExistsIPredicateTestCases))]
        public static bool TestExists<T, TPredicate>(
            Array2D<T> array, TPredicate match)
            where TPredicate : struct, IPredicate<T>
            => array.Exists(match);

        private static IEnumerable<TestCaseData> ExistsTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }),
                new Predicate<int>(o => o > 5))
                .Returns(true);
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }),
                new Predicate<int>(o => o == 10))
                .Returns(false);
        }

        private static IEnumerable<TestCaseData> ExistsIPredicateTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }),
                new GreaterThanPredicate<int>(5))
                .Returns(true);
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }),
                new EqualsPredicate<int>(10))
                .Returns(false);
        }
    }
}
