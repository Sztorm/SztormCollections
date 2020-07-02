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
        public static class TrueForAny
        {
            [Test]
            public static void ThrowsExceptionIfMatchIsNull()
                => Assert.Throws<ArgumentNullException>(
                    () => new Array2D<int>(0, 0).TrueForAny(match: null));

            [TestCaseSource(typeof(TrueForAny), nameof(PredicateTestCases))]
            public static bool Test<T>(Array2D<T> array, Predicate<T> match)
                => array.TrueForAny(match);

            [TestCaseSource(typeof(TrueForAny), nameof(IPredicateTestCases))]
            public static bool Test<T, TPredicate>(
                Array2D<T> array, TPredicate match)
                where TPredicate : struct, IPredicate<T>
                => array.TrueForAny(match);

            private static IEnumerable<TestCaseData> PredicateTestCases()
            {
                yield return new TestCaseData(
                    new Array2D<int>(0, 0), new Predicate<int>(o => o == 42))
                    .Returns(false);
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { {  2,  3,  5 },
                                     {  4,  0,  1 },
                                     { -8,  2,  3 } }),
                    new Predicate<int>(o => o > 5))
                    .Returns(false);
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 10, 10, 5  },
                                     {  4,  9, 10 },
                                     {  8, 10, 3  } }),
                    new Predicate<int>(o => o == 3))
                    .Returns(true);
            }

            private static IEnumerable<TestCaseData> IPredicateTestCases()
            {
                yield return new TestCaseData(
                    new Array2D<int>(0, 0), new EqualsPredicate<int>(42))
                    .Returns(false);
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { {  2,  3,  5 },
                                     {  4,  0,  1 },
                                     { -8,  2,  3 } }),
                    new GreaterThanPredicate<int>(5))
                    .Returns(false);
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 10, 10, 5  },
                                     {  4,  9, 10 },
                                     {  8, 10, 3  } }),
                    new EqualsPredicate<int>(3))
                    .Returns(true);
            }
        }    
    }
}
