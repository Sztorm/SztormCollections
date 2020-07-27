using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;
using Sztorm.Collections.Extensions;

namespace Sztorm.Collections.Tests
{
    public partial class RefReadOnlyRectangularCollectionTests
    {
        public static class TrueForAll
        {
            [Test]
            public static void ThrowsExceptionIfMatchIsNull()
                => Assert.Throws<ArgumentNullException>(
                    () => new RefReadOnlyMatrix4x4().TrueForAll(match: null as Predicate<float>));

            [TestCaseSource(typeof(TrueForAll), nameof(PredicateTestCases))]
            public static bool Test(RefReadOnlyMatrix4x4 matrix, Predicate<float> match)
                => matrix.TrueForAll(match);

            [TestCaseSource(typeof(TrueForAll), nameof(IPredicateTestCases))]
            public static bool Test<FloatPredicate>(
                RefReadOnlyMatrix4x4 matrix, FloatPredicate match)
                where FloatPredicate : struct, IPredicate<float>
                => matrix.TrueForAll<float, RefReadOnlyMatrix4x4, FloatPredicate>(match);

            private static IEnumerable<TestCaseData> PredicateTestCases()
            {
                var matrix = RefReadOnlyMatrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(matrix, new Predicate<float>(o => o > 0))
                    .Returns(true);
                yield return new TestCaseData(matrix, new Predicate<float>(o => o == 10))
                    .Returns(false);
            }

            private static IEnumerable<TestCaseData> IPredicateTestCases()
            {
                var matrix = RefReadOnlyMatrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(matrix, new GreaterThanPredicate<float>(0))
                    .Returns(true);
                yield return new TestCaseData(matrix, new EqualsPredicate<float>(10))
                    .Returns(false);
            }
        }    
    }
}
