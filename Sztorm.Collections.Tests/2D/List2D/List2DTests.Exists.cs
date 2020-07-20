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
        public static class Exists
        {
            [Test]
            public static void ThrowsExceptionIfMatchIsNull()
                => Assert.Throws<ArgumentNullException>(
                    () => new List2D<int>(0, 0).Exists(match: null));

            [TestCaseSource(typeof(Exists), nameof(PredicateTestCases))]
            public static bool Test<T>(List2D<T> list, Predicate<T> match)
                => list.Exists(match);

            [TestCaseSource(typeof(Exists), nameof(IPredicateTestCases))]
            public static bool Test<T, TPredicate>(
                List2D<T> list, TPredicate match)
                where TPredicate : struct, IPredicate<T>
                => list.Exists(match);

            private static IEnumerable<TestCaseData> PredicateTestCases()
            {
                var list3x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });
                list3x3.IncreaseCapacity(list3x3.Boundaries);

                yield return new TestCaseData(list3x3, new Predicate<int>(o => o > 5))
                    .Returns(true);
                yield return new TestCaseData(list3x3, new Predicate<int>(o => o == 10))
                    .Returns(false);
            }

            private static IEnumerable<TestCaseData> IPredicateTestCases()
            {
                var list3x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });
                list3x3.IncreaseCapacity(list3x3.Boundaries);

                yield return new TestCaseData(list3x3, new GreaterThanPredicate<int>(5))
                    .Returns(true);
                yield return new TestCaseData(list3x3, new EqualsPredicate<int>(10))
                    .Returns(false);
            }
        }
    }
}
