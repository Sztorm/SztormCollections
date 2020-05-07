using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    using static TestUtils;

    [TestFixture]
    partial class List2DTests
    {
        public static class GetSector
        {
            [TestCaseSource(typeof(GetSector), nameof(InvalidStartIndexCases))]
            public static void ThrowsExceptionIfStartIndexIsOutOfBounds<T>(
                List2D<T> list, Index2D startIndex)
                => Assert.Throws<ArgumentOutOfRangeException>(
                    () => list.GetSector(startIndex, new Bounds2D()));

            [TestCaseSource(typeof(GetSector), nameof(InvalidSectorTestCases))]
            public static void ThrowsExceptionIfSectorIsOutOfBounds<T>(
                List2D<T> list, Index2D startIndex, Bounds2D sectorSize)
                => Assert.Throws<ArgumentOutOfRangeException>(
                    () => list.GetSector(startIndex, sectorSize));

            [TestCaseSource(typeof(GetSector), nameof(TestCases))]
            public static void Test<T>(
                List2D<T> list, Index2D startIndex, Bounds2D sectorSize, List2D<T> expected)
            {
                List2D<T> sector = list.GetSector(startIndex, sectorSize);

                Assert.AreEqual(expected.Boundaries, sector.Boundaries,
                    $"Expected: {expected.Boundaries}\n" +
                    $"But was:  {sector.Boundaries}");
                Assert.AreEqual(sectorSize, sector.Capacity);
                CollectionAssert.AreEqual(expected, sector);
            }

            private static IEnumerable<TestCaseData> InvalidStartIndexCases()
            {
                yield return new TestCaseData(
                    CreateList2DWithBounds<byte>(2, 3), new Index2D(2, 0));
                yield return new TestCaseData(
                    CreateList2DWithBounds<byte>(2, 3), new Index2D(0, 3));
                yield return new TestCaseData(
                    CreateList2DWithBounds<byte>(2, 3), new Index2D(-1, 0));
                yield return new TestCaseData(
                    CreateList2DWithBounds<byte>(2, 3), new Index2D(0, -1));
                yield return new TestCaseData(
                    CreateList2DWithBounds<byte>(0, 0), new Index2D(0, 0));
                yield return new TestCaseData(
                    CreateList2DWithBounds<byte>(1, 0), new Index2D(0, 0));
                yield return new TestCaseData(
                    CreateList2DWithBounds<byte>(0, 1), new Index2D(0, 0));
            }

            private static IEnumerable<TestCaseData> InvalidSectorTestCases()
            {
                yield return new TestCaseData(
                    CreateList2DWithBounds<byte>(2, 3), new Index2D(0, 0), new Bounds2D(3, 0));
                yield return new TestCaseData(
                    CreateList2DWithBounds<byte>(2, 3), new Index2D(0, 0), new Bounds2D(0, 4));
                yield return new TestCaseData(
                    CreateList2DWithBounds<byte>(2, 3), new Index2D(1, 0), new Bounds2D(2, 0));
                yield return new TestCaseData(
                    CreateList2DWithBounds<byte>(2, 3), new Index2D(0, 1), new Bounds2D(0, 3));
            }

            private static IEnumerable<TestCaseData> TestCases()
            {
                var list2x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 1, 2, 3 },
                                 { 4, 5, 6 } });

                yield return new TestCaseData(
                    list2x3, new Index2D(1, 0), new Bounds2D(1, 3), List2D<int>.FromSystem2DArray(
                        new int[,] { { 4, 5, 6 } }));
                yield return new TestCaseData(
                    list2x3, new Index2D(0, 1), new Bounds2D(2, 1), List2D<int>.FromSystem2DArray(
                        new int[,] { { 2 },
                                     { 5 } }));
                yield return new TestCaseData(
                    list2x3, new Index2D(1, 2), new Bounds2D(1, 1), List2D<int>.FromSystem2DArray(
                        new int[,] { { 6 } }));
                yield return new TestCaseData(
                    list2x3, new Index2D(0, 0), new Bounds2D(2, 3), List2D<int>.FromSystem2DArray(
                        new int[,] { { 1, 2, 3 },
                                     { 4, 5, 6 } }));
                yield return new TestCaseData(
                    list2x3, new Index2D(0, 0), new Bounds2D(0, 0), new List2D<int>());
            }
        }
    }
}
