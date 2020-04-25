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
        public static partial class CopyTo
        {
            public static class System2DArray
            {
                // For some reason NUnit does not allow generic array parameters in parametrized
                // tests, so all System2DArray tests involving arrays are not generic.

                public static class Index2DArrayBounds2DIndex2D
                {
                    [Test]
                    public static void ThrowsExceptionIfDestinationIsNull()
                        => Assert.Throws<ArgumentNullException>(
                            () => new Array2D<byte>(0, 0).CopyTo(
                                new Index2D(),
                                destination: null as byte[,],
                                new Bounds2D(),
                                new Index2D()));

                    [TestCaseSource(typeof(System2DArray), nameof(InvalidSourceIndexTestCases))]
                    public static void ThrowsExceptionIfSourceIndexIsOutOfBounds(
                        Array2D<byte> src, Index2D srcIndex)
                        => Assert.Throws<ArgumentOutOfRangeException>(
                            () => src.CopyTo(
                                srcIndex,
                                new byte[src.Rows, src.Columns],
                                new Bounds2D(),
                                new Index2D()));

                    [TestCaseSource(typeof(System2DArray), nameof(InvalidDestIndexTestCases))]
                    public static void ThrowsExceptionIfDestIndexIsOutOfBounds(
                        byte[,] destination, Index2D destIndex)
                        => Assert.Throws<ArgumentOutOfRangeException>(
                                () => new Array2D<byte>(0, 0).CopyTo(
                                    new Index2D(), destination, new Bounds2D(), destIndex));

                    [TestCaseSource(
                        typeof(System2DArray),
                        nameof(Index2DArrayBounds2DIndex2DInvalidSectorSizeTestCases))]
                    public static void ThrowsExceptionIfSectorSizeIsOutOfBounds(
                        Array2D<byte> src,
                        Index2D srcIndex,
                        byte[,] dest,
                        Bounds2D sectorSize,
                        Index2D destIndex)
                        => Assert.Throws<ArgumentOutOfRangeException>(
                            () => src.CopyTo(srcIndex, dest, sectorSize, destIndex));

                    [TestCaseSource(
                        typeof(System2DArray), nameof(Index2DArrayBounds2DIndex2DTestCases))]
                    public static void Test(
                        Array2D<int> src,
                        Index2D srcIndex,
                        int[,] dest,
                        Bounds2D sectorSize,
                        Index2D destIndex,
                        int[,] expected)
                    {
                        src.CopyTo(srcIndex, dest, sectorSize, destIndex);
                        CollectionAssert.AreEqual(expected, dest);
                    }
                }

                public static class ArrayBounds2DIndex2D
                {
                    [Test]
                    public static void ThrowsExceptionIfDestinationIsNull()
                        => Assert.Throws<ArgumentNullException>(
                            () => new Array2D<byte>(0, 0).CopyTo(
                                destination: null as byte[,], new Bounds2D(), new Index2D()));

                    [TestCaseSource(typeof(System2DArray), nameof(InvalidDestIndexTestCases))]
                    public static void ThrowsExceptionIfDestIndexIsOutOfBounds(
                        byte[,] destination, Index2D destIndex)
                        => Assert.Throws<ArgumentOutOfRangeException>(
                                () => new Array2D<byte>(0, 0).CopyTo(
                                    destination, new Bounds2D(), destIndex));

                    [TestCaseSource(
                        typeof(System2DArray), nameof(Bounds2DIndex2DInvalidSectorSizeTestCases))]
                    public static void ThrowsExceptionIfSectorSizeIsOutOfBounds(
                        Array2D<byte> source,
                        byte[,] destination,
                        Bounds2D sectorSize,
                        Index2D destIndex)
                        => Assert.Throws<ArgumentOutOfRangeException>(
                            () => source.CopyTo(destination, sectorSize, destIndex));

                    [TestCaseSource(typeof(System2DArray), nameof(Bounds2DIndex2DTestCases))]
                    public static void Test(
                        Array2D<int> src,
                        int[,] dest,
                        Bounds2D sectorSize,
                        Index2D destIndex,
                        int[,] expected)
                    {
                        src.CopyTo(dest, sectorSize, destIndex);
                        CollectionAssert.AreEqual(expected, dest);
                    }
                }

                public static class ArrayBounds2D
                {
                    [Test]
                    public static void ThrowsExceptionIfDestinationIsNull()
                        => Assert.Throws<ArgumentNullException>(
                            () => new Array2D<byte>(0, 0).CopyTo(
                                destination: null as byte[,], new Bounds2D()));

                    [TestCaseSource(
                        typeof(System2DArray), nameof(Bounds2DInvalidSectorSizeTestCases))]
                    public static void ThrowsExceptionIfSectorSizeIsOutOfBounds(
                        Array2D<byte> source, byte[,] destination, Bounds2D sectorSize)
                        => Assert.Throws<ArgumentOutOfRangeException>(
                            () => source.CopyTo(destination, sectorSize));

                    [TestCaseSource(typeof(System2DArray), nameof(Bounds2DTestCases))]
                    public static void Test(
                        Array2D<int> src, int[,] dest, Bounds2D sectorSize, int[,] expected)
                    {
                        src.CopyTo(dest, sectorSize);
                        CollectionAssert.AreEqual(expected, dest);
                    }
                }

                public static class ArrayIndex2D
                {
                    [Test]
                    public static void ThrowsExceptionIfDestinationIsNull()
                        => Assert.Throws<ArgumentNullException>(
                            () => new Array2D<byte>(0, 0).CopyTo(
                                destination: null as byte[,], new Index2D()));

                    [TestCaseSource(typeof(System2DArray), nameof(InvalidDestIndexTestCases))]
                    public static void ThrowsExceptionIfDestIndexIsOutOfBounds(
                        byte[,] destination, Index2D destIndex)
                        => Assert.Throws<ArgumentOutOfRangeException>(
                                () => new Array2D<byte>(0, 0).CopyTo(destination, destIndex));

                    [TestCaseSource(
                        typeof(System2DArray), nameof(Index2DInvalidDestArrayTestCases))]
                    public static void ThrowsExceptionIfDestArrayCannotAccommodateAllElements(
                        Array2D<byte> source, byte[,] destination, Index2D destIndex)
                        => Assert.Throws<ArgumentException>(
                            () => source.CopyTo(destination, destIndex));

                    [TestCaseSource(typeof(System2DArray), nameof(Index2DTestCases))]
                    public static void Test(
                        Array2D<int> src, int[,] dest, Index2D destIndex, int[,] expected)
                    {
                        src.CopyTo(dest, destIndex);
                        CollectionAssert.AreEqual(expected, dest);
                    }
                }

                public static class Array
                {
                    [Test]
                    public static void ThrowsExceptionIfDestinationIsNull()
                        => Assert.Throws<ArgumentNullException>(
                            () => new Array2D<byte>(0, 0).CopyTo(destination: null as byte[,]));

                    [TestCaseSource(typeof(System2DArray), nameof(InvalidDestArrayTestCases))]
                    public static void ThrowsExceptionIfDestArrayCannotAccommodateAllElements(
                         Array2D<byte> source, byte[,] destination)
                        => Assert.Throws<ArgumentException>(() => source.CopyTo(destination));

                    [TestCaseSource(typeof(System2DArray), nameof(TestCases))]
                    public static void Test(Array2D<int> src, int[,] dest, int[,] expected)
                    {
                        src.CopyTo(dest);
                        CollectionAssert.AreEqual(expected, dest);
                    }
                }

                private static IEnumerable<TestCaseData> InvalidSourceIndexTestCases()
                {
                    yield return new TestCaseData(new Array2D<byte>(4, 5), new Index2D(0, -1));
                    yield return new TestCaseData(new Array2D<byte>(4, 5), new Index2D(-1, 0));
                    yield return new TestCaseData(new Array2D<byte>(4, 5), new Index2D(4, 0));
                    yield return new TestCaseData(new Array2D<byte>(4, 5), new Index2D(0, 5));
                }

                private static IEnumerable<TestCaseData> InvalidDestIndexTestCases()
                {
                    yield return new TestCaseData(new byte[4, 5], new Index2D(-1, 0));
                    yield return new TestCaseData(new byte[4, 5], new Index2D(0, -1));
                    yield return new TestCaseData(new byte[4, 5], new Index2D(4, 0));
                    yield return new TestCaseData(new byte[4, 5], new Index2D(0, 5));
                }

                private static IEnumerable<TestCaseData>
                    Index2DArrayBounds2DIndex2DInvalidSectorSizeTestCases()
                {
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5),
                        new Index2D(0, 0),
                        new byte[4, 5],
                        new Bounds2D(4, 5),
                        new Index2D(0, 0));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5),
                        new Index2D(0, 0),
                        new byte[3, 6],
                        new Bounds2D(3, 6),
                        new Index2D(0, 0));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5),
                        new Index2D(0, 0),
                        new byte[2, 5],
                        new Bounds2D(3, 5),
                        new Index2D(0, 0));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5),
                        new Index2D(0, 0),
                        new byte[3, 4],
                        new Bounds2D(3, 5),
                        new Index2D(0, 0));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5),
                        new Index2D(1, 0),
                        new byte[3, 5],
                        new Bounds2D(3, 5),
                        new Index2D(0, 0));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5),
                        new Index2D(0, 1),
                        new byte[3, 5],
                        new Bounds2D(3, 5),
                        new Index2D(0, 0));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5),
                        new Index2D(0, 0),
                        new byte[3, 5],
                        new Bounds2D(3, 5),
                        new Index2D(1, 0));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5),
                        new Index2D(0, 0),
                        new byte[3, 5],
                        new Bounds2D(3, 5),
                        new Index2D(0, 1));
                }

                private static IEnumerable<TestCaseData>
                    Bounds2DIndex2DInvalidSectorSizeTestCases()
                {
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5),
                        new byte[4, 5],
                        new Bounds2D(4, 5),
                        new Index2D(0, 0));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5),
                        new byte[3, 6],
                        new Bounds2D(3, 6),
                        new Index2D(0, 0));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5),
                        new byte[2, 5],
                        new Bounds2D(3, 5),
                        new Index2D(0, 0));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5),
                        new byte[3, 4],
                        new Bounds2D(3, 5),
                        new Index2D(0, 0));                                
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5),
                        new byte[3, 5],
                        new Bounds2D(3, 5),
                        new Index2D(1, 0));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5),
                        new byte[3, 5],
                        new Bounds2D(3, 5),
                        new Index2D(0, 1));
                }

                private static IEnumerable<TestCaseData> Bounds2DInvalidSectorSizeTestCases()
                {
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5), new byte[4, 5], new Bounds2D(4, 5));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5), new byte[3, 6], new Bounds2D(3, 6));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5), new byte[2, 5], new Bounds2D(3, 5));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5), new byte[3, 4], new Bounds2D(3, 5));
                }

                private static IEnumerable<TestCaseData> Index2DInvalidDestArrayTestCases()
                {
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5), new byte[3, 5], new Index2D(1, 0));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5), new byte[3, 5], new Index2D(0, 1));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5), new byte[2, 5], new Index2D(0, 0));
                    yield return new TestCaseData(
                        new Array2D<byte>(3, 5), new byte[3, 4], new Index2D(0, 0));
                }

                private static IEnumerable<TestCaseData> InvalidDestArrayTestCases()
                {
                    yield return new TestCaseData(new Array2D<byte>(3, 5), new byte[2, 5]);
                    yield return new TestCaseData(new Array2D<byte>(3, 5), new byte[3, 4]);
                }

                private static IEnumerable<TestCaseData> Index2DArrayBounds2DIndex2DTestCases()
                {
                    yield return new TestCaseData(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { {  0,  1,  2,  3,  4 },
                                         {  5,  6,  7,  8,  9 },
                                         { 10, 11, 12, 13, 14 },
                                         { 15, 16, 17, 18, 19 }, }),
                        new Index2D(2, 1),
                        new int[3, 5],
                        new Bounds2D(2, 3),
                        new Index2D(1, 2),
                        new int[,] { { 0,  0,  0,  0,  0 },
                                     { 0,  0, 11, 12, 13 },
                                     { 0,  0, 16, 17, 18 } });
                    yield return new TestCaseData(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 9, 1, 2 },
                                         { 3, 4, 5 },
                                         { 6, 7, 8 }, }),
                        new Index2D(0, 1),
                        new int[3, 3],
                        new Bounds2D(2, 1),
                        new Index2D(1, 0),
                        new int[,] { { 0, 0, 0 },
                                     { 1, 0, 0 },
                                     { 4, 0, 0 }, });
                    yield return new TestCaseData(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 9, 1, 2 },
                                         { 3, 4, 5 },
                                         { 6, 7, 8 }, }),
                        new Index2D(0, 1),
                        new int[3, 4],
                        new Bounds2D(2, 1),
                        new Index2D(1, 1),
                        new int[,] { { 0, 0, 0, 0 },
                                     { 0, 1, 0, 0 },
                                     { 0, 4, 0, 0 }, });
                }

                private static IEnumerable<TestCaseData> Bounds2DIndex2DTestCases()
                {
                    yield return new TestCaseData(
                       Array2D<int>.FromSystem2DArray(
                           new int[,] { {  0,  1,  2,  3,  4 },
                                        {  5,  6,  7,  8,  9 },
                                        { 10, 11, 12, 13, 14 },
                                        { 15, 16, 17, 18, 19 }, }),
                       new int[3, 5],
                       new Bounds2D(2, 3),
                       new Index2D(1, 2),
                       new int[,] { { 0,  0,  0,  0,  0 },
                                    { 0,  0,  0,  1,  2, },
                                    { 0,  0,  5,  6,  7, } });
                    yield return new TestCaseData(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 9, 1, 2 },
                                         { 3, 4, 5 },
                                         { 6, 7, 8 }, }),
                        new int[3, 3],
                        new Bounds2D(2, 1),
                        new Index2D(1, 0),
                        new int[,] { { 0, 0, 0 },
                                     { 9, 0, 0 },
                                     { 3, 0, 0 }, });
                    yield return new TestCaseData(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 9, 1, 2 },
                                         { 3, 4, 5 },
                                         { 6, 7, 8 }, }),
                        new int[3, 4],
                        new Bounds2D(2, 1),
                        new Index2D(1, 1),
                        new int[,] { { 0, 0, 0, 0 },
                                     { 0, 9, 0, 0 },
                                     { 0, 3, 0, 0 }, });
                }

                private static IEnumerable<TestCaseData> Bounds2DTestCases()
                {
                    yield return new TestCaseData(
                      Array2D<int>.FromSystem2DArray(
                          new int[,] { {  0,  1,  2,  3,  4 },
                                       {  5,  6,  7,  8,  9 },
                                       { 10, 11, 12, 13, 14 },
                                       { 15, 16, 17, 18, 19 }, }),
                      new int[3, 5],
                      new Bounds2D(2, 3),
                      new int[,] { { 0,  1,  2,  0,  0 },
                                   { 5,  6,  7,  0,  0, },
                                   { 0,  0,  0,  0,  0, } });
                    yield return new TestCaseData(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 9, 1, 2 },
                                         { 3, 4, 5 },
                                         { 6, 7, 8 }, }),
                        new int[3, 3],
                        new Bounds2D(2, 1),
                        new int[,] { { 9, 0, 0 },
                                     { 3, 0, 0, },
                                     { 0, 0, 0, }, });
                    yield return new TestCaseData(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 9, 1, 2 },
                                         { 3, 4, 5 },
                                         { 6, 7, 8 }, }),
                        new int[3, 4],
                        new Bounds2D(2, 1),
                        new int[,] { { 9, 0, 0, 0 },
                                     { 3, 0, 0, 0 },
                                     { 0, 0, 0, 0 }, });
                }

                private static IEnumerable<TestCaseData> Index2DTestCases()
                {
                    yield return new TestCaseData(
                      Array2D<int>.FromSystem2DArray(
                          new int[,] { {  0,  1,  2,  3,  4 },
                                       {  5,  6,  7,  8,  9 },
                                       { 10, 11, 12, 13, 14 },
                                       { 15, 16, 17, 18, 19 } }),
                      new int[5, 7],
                      new Index2D(1, 2),
                      new int[,] { { 0,  0,  0,  0,  0,  0,  0 },
                                   { 0,  0,  0,  1,  2,  3,  4 },
                                   { 0,  0,  5,  6,  7,  8,  9 },
                                   { 0,  0, 10, 11, 12, 13, 14 },
                                   { 0,  0, 15, 16, 17, 18, 19 }});
                    yield return new TestCaseData(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 9, 1, 2 },
                                         { 3, 4, 5 },
                                         { 6, 7, 8 } }),
                        new int[4, 3],
                        new Index2D(1, 0),
                        new int[,] { { 0, 0, 0 },
                                     { 9, 1, 2 },
                                     { 3, 4, 5 },
                                     { 6, 7, 8 } });
                }

                private static IEnumerable<TestCaseData> TestCases()
                {
                    yield return new TestCaseData(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 9, 1, 2 },
                                         { 3, 4, 5 } }),
                        new int[2, 3],
                        new int[,] { { 9, 1, 2 },
                                     { 3, 4, 5 } });
                    yield return new TestCaseData(
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 9, 1 },
                                         { 3, 4 },
                                         { 6, 7 }, }),
                        new int[3, 2],
                        new int[,] { { 9, 1 },
                                     { 3, 4 },
                                     { 6, 7 }, });
                    yield return new TestCaseData(
                        Array2D<int>.FromSystem2DArray(
                            new int[0, 0]),
                        new int[0, 0],
                        new int[0, 0]);
                }
            }
        }
    }
}
