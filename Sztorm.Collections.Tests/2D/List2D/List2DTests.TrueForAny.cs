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
        public static class TrueForAny
        {
            [Test]
            public static void ThrowsExceptionIfMatchIsNull()
                => Assert.Throws<ArgumentNullException>(
                    () => new List2D<byte>(0, 0).TrueForAny(match: null));

            [TestCaseSource(typeof(TrueForAny), nameof(PredicateTestCases))]
            public static bool Test<T>(List2D<T> list, Predicate<T> match)
                => list.TrueForAny(match);

            [TestCaseSource(typeof(TrueForAny), nameof(IPredicateTestCases))]
            public static bool Test<T, TPredicate>(List2D<T> list, TPredicate match)
                where TPredicate : struct, IPredicate<T>
                => list.TrueForAny(match);

            private static IEnumerable<TestCaseData> PredicateTestCases()
            {
                var list3x3A = List2D<int>.FromSystem2DArray(
                    new int[,] { {  2,  3,  5 },
                                 {  4,  0,  1 },
                                 { -8,  2,  3 } });
                list3x3A.IncreaseCapacity(list3x3A.Boundaries);

                var list3x3B = List2D<int>.FromSystem2DArray(
                    new int[,] { { 10, 10, 5  },
                                 {  4,  9, 10 },
                                 {  8, 10, 3  } });
                list3x3B.IncreaseCapacity(list3x3B.Boundaries);

                yield return new TestCaseData(
                    new List2D<int>(0, 0), new Predicate<int>(o => o == 42))
                    .Returns(false);
                yield return new TestCaseData(list3x3A, new Predicate<int>(o => o > 5))
                    .Returns(false);
                yield return new TestCaseData(list3x3B, new Predicate<int>(o => o == 3))
                    .Returns(true);
            }

            private static IEnumerable<TestCaseData> IPredicateTestCases()
            {
                var list3x3A = List2D<int>.FromSystem2DArray(
                    new int[,] { {  2,  3,  5 },
                                 {  4,  0,  1 },
                                 { -8,  2,  3 } });
                list3x3A.IncreaseCapacity(list3x3A.Boundaries);

                var list3x3B = List2D<int>.FromSystem2DArray(
                    new int[,] { { 10, 10, 5  },
                                 {  4,  9, 10 },
                                 {  8, 10, 3  } });
                list3x3B.IncreaseCapacity(list3x3B.Boundaries);

                yield return new TestCaseData(new List2D<int>(0, 0), new EqualsPredicate<int>(42))
                    .Returns(false);
                yield return new TestCaseData(list3x3A, new GreaterThanPredicate<int>(5))
                    .Returns(false);
                yield return new TestCaseData(list3x3B, new EqualsPredicate<int>(3))
                    .Returns(true);
            }
        }    
    }
}
