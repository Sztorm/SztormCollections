using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;
using Sztorm.Collections.Extensions;

namespace Sztorm.Collections.Tests
{
    public partial class RectangularCollectionTests
    {
        public static class Find
        {
            public static class First
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Matrix4x4().Find(match: null as Predicate<float>));

                [TestCaseSource(typeof(Find), nameof(FirstTestCases))]
                public static ItemRequestResult<float> Test(
                    Matrix4x4 matrix, Predicate<float> match)
                    => matrix.Find(match);

                [TestCaseSource(typeof(Find), nameof(FirstIPredicateTestCases))]
                public static ItemRequestResult<float> Test<FloatPredicate>(
                    Matrix4x4 matrix, FloatPredicate match)
                    where FloatPredicate : struct, IPredicate<float>
                    => matrix.Find<float, Matrix4x4, FloatPredicate>(match);
            }

            public static class Last
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Matrix4x4().FindLast(match: null as Predicate<float>));

                [TestCaseSource(typeof(Find), nameof(LastTestCases))]
                public static ItemRequestResult<float> Test(
                    Matrix4x4 matrix, Predicate<float> match)
                    => matrix.FindLast(match);

                [TestCaseSource(typeof(Find), nameof(LastIPredicateTestCases))]
                public static ItemRequestResult<float> Test<FloatPredicate>(
                    Matrix4x4 matrix, FloatPredicate match)
                    where FloatPredicate : struct, IPredicate<float>
                    => matrix.FindLast<float, Matrix4x4, FloatPredicate>(match);
            }

            private static IEnumerable<TestCaseData> FirstTestCases()
            {
                var matrix = Matrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(matrix, new Predicate<float>(o => o > 125))
                    .Returns(new ItemRequestResult<float>(625));
                yield return new TestCaseData(matrix, new Predicate<float>(o => o == 10))
                    .Returns(ItemRequestResult<float>.Fail);
            }

            private static IEnumerable<TestCaseData> FirstIPredicateTestCases()
            {
                var matrix = Matrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(matrix, new GreaterThanPredicate<float>(125))
                    .Returns(new ItemRequestResult<float>(625));
                yield return new TestCaseData(matrix, new EqualsPredicate<float>(10))
                    .Returns(ItemRequestResult<float>.Fail);
            }

            private static IEnumerable<TestCaseData> LastTestCases()
            {
                var matrix = Matrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(matrix, new Predicate<float>(o => o < 2))
                    .Returns(new ItemRequestResult<float>(1));
                yield return new TestCaseData(matrix, new Predicate<float>(o => o == 10))
                    .Returns(ItemRequestResult<float>.Fail);
            }

            private static IEnumerable<TestCaseData> LastIPredicateTestCases()
            {
                var matrix = Matrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(matrix, new LessThanPredicate<float>(2))
                    .Returns(new ItemRequestResult<float>(1));
                yield return new TestCaseData(matrix, new EqualsPredicate<float>(10))
                    .Returns(ItemRequestResult<float>.Fail);
            }
        }
    }
}
