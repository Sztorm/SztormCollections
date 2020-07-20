using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    partial class Array2DTests
    {
        public static class GetSector
        {
            [TestCaseSource(typeof(GetSector), nameof(InvalidStartIndexCases))]
            public static void ThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                Array2D<T> array, Index2D startIndex)
                => Assert.Throws<ArgumentOutOfRangeException>(
                    () => array.GetSector(startIndex, new Bounds2D()));

            [TestCaseSource(typeof(GetSector), nameof(InvalidSectorTestCases))]
            public static void ThrowsExceptionIfSectorIsOutOfBounds<T>(
                Array2D<T> array, Index2D startIndex, Bounds2D sectorSize)
                => Assert.Throws<ArgumentOutOfRangeException>(
                    () => array.GetSector(startIndex, sectorSize));

            [TestCaseSource(typeof(GetSector), nameof(TestCases))]
            public static void Test<T>(
                Array2D<T> array, Index2D startIndex, Bounds2D sectorSize, Array2D<T> expected)
            {
                Array2D<T> sector = array.GetSector(startIndex, sectorSize);

                Assert.AreEqual(expected.Boundaries, sector.Boundaries,
                    $"Expected: {expected.Boundaries}\n" +
                    $"But was:  {sector.Boundaries}");
                CollectionAssert.AreEqual(expected, sector);
            }

            private static IEnumerable<TestCaseData> InvalidStartIndexCases()
            {
                yield return new TestCaseData(new Array2D<byte>(2, 3), new Index2D(2, 0));
                yield return new TestCaseData(new Array2D<byte>(2, 3), new Index2D(0, 3));
                yield return new TestCaseData(new Array2D<byte>(2, 3), new Index2D(-1, 0));
                yield return new TestCaseData(new Array2D<byte>(2, 3), new Index2D(0, -1));
                yield return new TestCaseData(new Array2D<byte>(0, 0), new Index2D(0, 0));
                yield return new TestCaseData(new Array2D<byte>(1, 0), new Index2D(0, 0));
                yield return new TestCaseData(new Array2D<byte>(0, 1), new Index2D(0, 0));
            }

            private static IEnumerable<TestCaseData> InvalidSectorTestCases()
            {
                yield return new TestCaseData(
                    new Array2D<byte>(2, 3), new Index2D(0, 0), new Bounds2D(3, 0));
                yield return new TestCaseData(
                    new Array2D<byte>(2, 3), new Index2D(0, 0), new Bounds2D(0, 4));
                yield return new TestCaseData(
                    new Array2D<byte>(2, 3), new Index2D(1, 0), new Bounds2D(2, 0));
                yield return new TestCaseData(
                    new Array2D<byte>(2, 3), new Index2D(0, 1), new Bounds2D(0, 3));
            }

            private static IEnumerable<TestCaseData> TestCases()
            {
                var array2x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 1, 2, 3 },
                                 { 4, 5, 6 } });

                yield return new TestCaseData(
                    array2x3,
                    new Index2D(1, 0),
                    new Bounds2D(1, 3),
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 4, 5, 6 } }));
                yield return new TestCaseData(
                    array2x3,
                    new Index2D(0, 1),
                    new Bounds2D(2, 1),
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 2 },
                                     { 5 } }));
                yield return new TestCaseData(
                    array2x3,
                    new Index2D(1, 2),
                    new Bounds2D(1, 1),
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 6 } }));
                yield return new TestCaseData(
                    array2x3,
                    new Index2D(0, 0),
                    new Bounds2D(2, 3),
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 1, 2, 3 },
                                     { 4, 5, 6 } }));
                yield return new TestCaseData(
                    array2x3, new Index2D(0, 0), new Bounds2D(0, 0), new Array2D<int>(0, 0));
            }
        }
    }
}
