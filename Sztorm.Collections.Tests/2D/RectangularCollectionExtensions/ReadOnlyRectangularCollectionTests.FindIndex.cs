using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;
using Sztorm.Collections.Extensions;

namespace Sztorm.Collections.Tests
{
    using static TestUtils;

    public partial class ReadOnlyRectangularCollectionTests
    {
        public static class FindIndex
        {
            public static class Predicate
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new ReadOnlyMatrix4x4().FindIndex<float, ReadOnlyMatrix4x4>(
                            match: null));

                [TestCaseSource(typeof(FindIndex), nameof(PredicateTestCases))]
                public static ItemRequestResult<Index2D> Test(
                    ReadOnlyMatrix4x4 matrix, Predicate<float> match)
                    => matrix.FindIndex(match);

                [Test]
                public static void Index2DBounds2DThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new ReadOnlyMatrix4x4().FindIndex<float, ReadOnlyMatrix4x4>(
                            (0, 0), new Bounds2D(), match: null));

                [TestCaseSource(typeof(FindIndex), nameof(InvalidStartIndexTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds(
                    ReadOnlyMatrix4x4 matrix, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => matrix.FindIndex<float, ReadOnlyMatrix4x4>(
                            startIndex, new Bounds2D(), o => true));

                [TestCaseSource(typeof(FindIndex), nameof(InvalidSectorTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfSectorIsOutOfBounds(
                    ReadOnlyMatrix4x4 matrix, Index2D startIndex, Bounds2D sector)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => matrix.FindIndex<float, ReadOnlyMatrix4x4>(
                            startIndex, sector, o => true));

                [TestCaseSource(typeof(FindIndex), nameof(PredicateIndex2DBounds2DTestCases))]
                public static ItemRequestResult<Index2D> Test(
                    ReadOnlyMatrix4x4 matrix,
                    Index2D startIndex,
                    Bounds2D sector,
                    Predicate<float> match)
                    => matrix.FindIndex(startIndex, sector, match);
            }

            public static class IPredicate
            {
                [TestCaseSource(typeof(FindIndex), nameof(IPredicateTestCases))]
                public static ItemRequestResult<Index2D> Test<FloatPredicate>(
                    ReadOnlyMatrix4x4 matrix, FloatPredicate match)
                    where FloatPredicate : struct, IPredicate<float>
                    => matrix.FindIndex<float, ReadOnlyMatrix4x4, FloatPredicate>(match);

                [TestCaseSource(typeof(FindIndex), nameof(InvalidStartIndexTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfStartIndexIsOutOfBounds(
                    ReadOnlyMatrix4x4 matrix, Index2D startIndex)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => matrix.FindIndex<
                            float, ReadOnlyMatrix4x4, AlwaysTruePredicate<float>>(
                            startIndex, new Bounds2D(), new AlwaysTruePredicate<float>()));

                [TestCaseSource(typeof(FindIndex), nameof(InvalidSectorTestCases))]
                public static void Index2DBounds2DThrowsExceptionIfSectorIsOutOfBounds(
                    ReadOnlyMatrix4x4 matrix, Index2D startIndex, Bounds2D sector)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => matrix.FindIndex<
                            float, ReadOnlyMatrix4x4, AlwaysTruePredicate<float>>(
                            startIndex, sector, new AlwaysTruePredicate<float>()));

                [TestCaseSource(typeof(FindIndex), nameof(IPredicateIndex2DBounds2DTestCases))]
                public static ItemRequestResult<Index2D> Test<FloatPredicate>(
                    ReadOnlyMatrix4x4 matrix,
                    Index2D startIndex,
                    Bounds2D sector,
                    FloatPredicate match)
                    where FloatPredicate : struct, IPredicate<float>
                    => matrix.FindIndex<float, ReadOnlyMatrix4x4, FloatPredicate>(
                        startIndex, sector, match);
            }

            private static IEnumerable<TestCaseData> InvalidStartIndexTestCases()
            {
                yield return new TestCaseData(new ReadOnlyMatrix4x4(), new Index2D(-1, 0));
                yield return new TestCaseData(new ReadOnlyMatrix4x4(), new Index2D(0, -1));
                yield return new TestCaseData(new ReadOnlyMatrix4x4(), new Index2D(4, 0));
                yield return new TestCaseData(new ReadOnlyMatrix4x4(), new Index2D(0, 4));
            }

            private static IEnumerable<TestCaseData> InvalidSectorTestCases()
            {
                yield return new TestCaseData(
                    new ReadOnlyMatrix4x4(), new Index2D(0, 0), new Bounds2D(5, 0));
                yield return new TestCaseData(
                    new ReadOnlyMatrix4x4(), new Index2D(0, 0), new Bounds2D(0, 5));
                yield return new TestCaseData(
                    new ReadOnlyMatrix4x4(), new Index2D(1, 1), new Bounds2D(4, 0));
                yield return new TestCaseData(
                    new ReadOnlyMatrix4x4(), new Index2D(1, 1), new Bounds2D(0, 4));
            }

            private static IEnumerable<TestCaseData> PredicateTestCases()
            {
                var matrix = ReadOnlyMatrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(matrix, new Predicate<float>(o => o > 125))
                    .Returns(new ItemRequestResult<Index2D>((3, 3)));
                yield return new TestCaseData(matrix, new Predicate<float>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }

            private static IEnumerable<TestCaseData> PredicateIndex2DBounds2DTestCases()
            {
                var matrix = ReadOnlyMatrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(
                    matrix,
                    new Index2D(0, 0),
                    new Bounds2D(4, 4),
                    new Predicate<float>(o => o > 125))
                    .Returns(new ItemRequestResult<Index2D>((3, 3)));
                yield return new TestCaseData(
                    matrix,
                    new Index2D(0, 0),
                    new Bounds2D(0, 0),
                    new Predicate<float>(o => true))
                    .Returns(ItemRequestResult<Index2D>.Fail);
                yield return new TestCaseData(
                    matrix,
                    new Index2D(2, 2),
                    new Bounds2D(2, 2),
                    new Predicate<float>(o => o == 10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }

            private static IEnumerable<TestCaseData> IPredicateTestCases()
            {
                var matrix = ReadOnlyMatrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(matrix, new GreaterThanPredicate<float>(125))
                    .Returns(new ItemRequestResult<Index2D>((3, 3)));
                yield return new TestCaseData(matrix, new EqualsPredicate<float>(10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }

            private static IEnumerable<TestCaseData> IPredicateIndex2DBounds2DTestCases()
            {
                var matrix = ReadOnlyMatrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                yield return new TestCaseData(
                    matrix,
                    new Index2D(0, 0),
                    new Bounds2D(4, 4),
                    new GreaterThanPredicate<float>(125))
                    .Returns(new ItemRequestResult<Index2D>((3, 3)));
                yield return new TestCaseData(
                    matrix,
                    new Index2D(0, 0),
                    new Bounds2D(0, 0),
                    new AlwaysTruePredicate<float>())
                    .Returns(ItemRequestResult<Index2D>.Fail);
                yield return new TestCaseData(
                    matrix,
                    new Index2D(2, 2),
                    new Bounds2D(2, 2),
                    new EqualsPredicate<float>(10))
                    .Returns(ItemRequestResult<Index2D>.Fail);
            }
        }
    }
}
