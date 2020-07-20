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
        public static class TrueForAll
        {
            [Test]
            public static void ThrowsExceptionIfMatchIsNull()
                => Assert.Throws<ArgumentNullException>(
                    () => new List2D<byte>(0, 0).TrueForAll(match: null));

            [TestCaseSource(typeof(TrueForAll), nameof(PredicateTestCases))]
            public static bool Test<T>(List2D<T> list, Predicate<T> match)
                => list.TrueForAll(match);

            [TestCaseSource(typeof(TrueForAll), nameof(IPredicateTestCases))]
            public static bool Test<T, TPredicate>(List2D<T> list, TPredicate match)
                where TPredicate : struct, IPredicate<T>
                => list.TrueForAll(match);

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
                    .Returns(true);
                yield return new TestCaseData(list3x3A, new Predicate<int>(o => o <= 5))
                    .Returns(true);
                yield return new TestCaseData(list3x3B, new Predicate<int>(o => o == 10))
                    .Returns(false);
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
                    .Returns(true);
                yield return new TestCaseData(list3x3A, new LessThanOrEqualToPredicate<int>(5))
                    .Returns(true);
                yield return new TestCaseData(list3x3B, new EqualsPredicate<int>(10))
                    .Returns(false);
            }
        }    
    }
}
