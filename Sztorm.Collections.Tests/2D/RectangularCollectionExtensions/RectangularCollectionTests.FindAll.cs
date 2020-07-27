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
        public static class FindAll
        {
            public static class Items
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Matrix4x4().FindAll<
                            float, List<float>, Matrix4x4>(match: null));

                [TestCaseSource(typeof(FindAll), nameof(ItemsTestCases))]
                public static void TestFindAll<FloatCollection>(
                    Matrix4x4 matrix, Predicate<float> match, FloatCollection expected)
                    where FloatCollection : ICollection<float>, new()
                    => CollectionAssert.AreEqual(expected, matrix.FindAll<
                        float, FloatCollection, Matrix4x4>(match));

                [TestCaseSource(typeof(FindAll), nameof(ItemsIPredicateTestCases))]
                public static void TestFindAll<FloatCollection, FloatPredicate>(
                    Matrix4x4 matrix, FloatPredicate match, FloatCollection expected)
                    where FloatCollection : ICollection<float>, new()
                    where FloatPredicate : struct, IPredicate<float>
                    => CollectionAssert.AreEqual(expected, matrix.FindAll<
                        float, FloatCollection, Matrix4x4, FloatPredicate>(match));
            }

            private static IEnumerable<TestCaseData> ItemsTestCases()
            {
                var matrix = Matrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(
                    matrix,
                    new Predicate<float>(o => o > 20),
                    new List<float>() { 24, 25, 125, 625 });
                yield return new TestCaseData(
                    matrix, new Predicate<float>(o => o == 10), new List<float>());
            }

            private static IEnumerable<TestCaseData> ItemsIPredicateTestCases()
            {
                var matrix = Matrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(
                    matrix,
                    new GreaterThanPredicate<float>(20),
                    new List<float>() { 24, 25, 125, 625 });
                yield return new TestCaseData(
                    matrix, new EqualsPredicate<float>(10), new List<float>());
            }
        }
    }
}
