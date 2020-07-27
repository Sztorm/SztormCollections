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
        public static class FindAll
        {
            public static class Items
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new RefMatrix4x4().FindAll<
                            float, List<float>, RefMatrix4x4>(match: null));

                [TestCaseSource(typeof(FindAll), nameof(ItemsTestCases))]
                public static void TestFindAll<FloatCollection>(
                    RefMatrix4x4 matrix, Predicate<float> match, FloatCollection expected)
                    where FloatCollection : ICollection<float>, new()
                    => CollectionAssert.AreEqual(expected, matrix.FindAll<
                        float, FloatCollection, RefMatrix4x4>(match));

                [TestCaseSource(typeof(FindAll), nameof(ItemsIPredicateTestCases))]
                public static void TestFindAll<FloatCollection, FloatPredicate>(
                    RefMatrix4x4 matrix, FloatPredicate match, FloatCollection expected)
                    where FloatCollection : ICollection<float>, new()
                    where FloatPredicate : struct, IPredicate<float>
                    => CollectionAssert.AreEqual(expected, matrix.FindAll<
                        float, FloatCollection, RefMatrix4x4, FloatPredicate>(match));
            }

            public static class Indices
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new RefMatrix4x4().FindAllIndices<
                            float, List<Index2D>, RefMatrix4x4>(match: null));

                [TestCaseSource(typeof(FindAll), nameof(IndicesTestCases))]
                public static void TestFindAll<Index2DCollection>(
                    RefMatrix4x4 matrix, Predicate<float> match, Index2DCollection expected)
                    where Index2DCollection : ICollection<Index2D>, new()
                    => CollectionAssert.AreEqual(expected, matrix.FindAllIndices<
                        float, Index2DCollection, RefMatrix4x4>(match));

                [TestCaseSource(typeof(FindAll), nameof(IndicesIPredicateTestCases))]
                public static void TestFindAll<Index2DCollection, FloatPredicate>(
                    RefMatrix4x4 matrix, FloatPredicate match, Index2DCollection expected)
                    where Index2DCollection : ICollection<Index2D>, new()
                    where FloatPredicate : struct, IPredicate<float>
                    => CollectionAssert.AreEqual(expected, matrix.FindAllIndices<
                        float, Index2DCollection, RefMatrix4x4, FloatPredicate>(match));
            }

            private static IEnumerable<TestCaseData> ItemsTestCases()
            {
                var matrix = RefMatrix4x4.FromSystem2DArray(
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
                var matrix = RefMatrix4x4.FromSystem2DArray(
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

            private static IEnumerable<TestCaseData> IndicesTestCases()
            {
                var matrix = RefMatrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(
                    matrix,
                    new Predicate<float>(o => o > 20),
                    new List<Index2D>() { (2, 3), (3, 1), (3, 2), (3, 3) });
                yield return new TestCaseData(
                    matrix, new Predicate<float>(o => o == 10), new List<Index2D>());
            }

            private static IEnumerable<TestCaseData> IndicesIPredicateTestCases()
            {
                var matrix = RefMatrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(
                    matrix,
                    new GreaterThanPredicate<float>(20),
                    new List<Index2D>() { (2, 3), (3, 1), (3, 2), (3, 3) });
                yield return new TestCaseData(
                    matrix, new EqualsPredicate<float>(10), new List<Index2D>());
            }
        }
    }
}
