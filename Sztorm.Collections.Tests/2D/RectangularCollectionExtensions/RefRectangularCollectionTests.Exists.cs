using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;
using Sztorm.Collections.Extensions;

namespace Sztorm.Collections.Tests
{
    public partial class RefRectangularCollectionTests
    {
        public static class Exists
        {
            [Test]
            public static void ThrowsExceptionIfMatchIsNull()
                => Assert.Throws<ArgumentNullException>(
                    () => new RefMatrix4x4().Exists(match: null as Predicate<float>));

            [TestCaseSource(typeof(Exists), nameof(PredicateTestCases))]
            public static bool Test(RefMatrix4x4 matrix, Predicate<float> match)
                => matrix.Exists(match);

            [TestCaseSource(typeof(Exists), nameof(IPredicateTestCases))]
            public static bool Test<FloatPredicate>(RefMatrix4x4 matrix, FloatPredicate match)
                where FloatPredicate : struct, IPredicate<float>
                => matrix.Exists<float, RefMatrix4x4, FloatPredicate>(match);

            private static IEnumerable<TestCaseData> PredicateTestCases()
            {
                var matrix = RefMatrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(matrix, new Predicate<float>(o => o > 125))
                    .Returns(true);
                yield return new TestCaseData(matrix, new Predicate<float>(o => o == 10))
                    .Returns(false);
            }

            private static IEnumerable<TestCaseData> IPredicateTestCases()
            {
                var matrix = RefMatrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(matrix, new GreaterThanPredicate<float>(125))
                    .Returns(true);
                yield return new TestCaseData(matrix, new EqualsPredicate<float>(10))
                    .Returns(false);
            }
        }
    }
}
