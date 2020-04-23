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
        public static class Exists
        {
            [Test]
            public static void ThrowsExceptionIfMatchIsNull()
                => Assert.Throws<ArgumentNullException>(
                    () => new Array2D<int>(0, 0).Exists(match: null));

            [TestCaseSource(typeof(Exists), nameof(PredicateTestCases))]
            public static bool Test<T>(Array2D<T> array, Predicate<T> match)
                => array.Exists(match);

            [TestCaseSource(typeof(Exists), nameof(IPredicateTestCases))]
            public static bool Test<T, TPredicate>(
                Array2D<T> array, TPredicate match)
                where TPredicate : struct, IPredicate<T>
                => array.Exists(match);

            private static IEnumerable<TestCaseData> PredicateTestCases()
            {
                var array3x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });

                yield return new TestCaseData(array3x3, new Predicate<int>(o => o > 5))
                    .Returns(true);
                yield return new TestCaseData(array3x3, new Predicate<int>(o => o == 10))
                    .Returns(false);
            }

            private static IEnumerable<TestCaseData> IPredicateTestCases()
            {
                var array3x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });

                yield return new TestCaseData(array3x3, new GreaterThanPredicate<int>(5))
                    .Returns(true);
                yield return new TestCaseData(array3x3, new EqualsPredicate<int>(10))
                    .Returns(false);
            }
        }
    }
}
