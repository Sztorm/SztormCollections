using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    public partial class List2DTests
    {
        public static partial class CopyTo
        {
            public static class Array2D
            {
                public static class Index2DArrayBounds2DIndex2D
                {
                    [Test]
                    public static void ThrowsExceptionIfDestinationIsNull()
                        => Assert.Throws<ArgumentNullException>(
                            () => new List2D<byte>(0, 0).CopyTo(
                                new Index2D(),
                                destination: null as Array2D<byte>,
                                new Bounds2D(),
                                new Index2D()));

                    [TestCaseSource(typeof(Array2D), nameof(InvalidSourceIndexTestCases))]
                    public static void ThrowsExceptionIfSourceIndexIsOutOfBounds<T>(
                        List2D<T> src, Index2D srcIndex)
                        => Assert.Throws<ArgumentOutOfRangeException>(
                            () => src.CopyTo(
                                srcIndex,
                                new Array2D<T>(src.Rows, src.Columns),
                                new Bounds2D(),
                                new Index2D()));

                    [TestCaseSource(typeof(Array2D), nameof(InvalidDestIndexTestCases))]
                    public static void ThrowsExceptionIfDestIndexIsOutOfBounds<T>(
                        Array2D<T> destination, Index2D destIndex)
                        => Assert.Throws<ArgumentOutOfRangeException>(
                                () => new List2D<T>(0, 0).CopyTo(
                                    new Index2D(), destination, new Bounds2D(), destIndex));

                    [TestCaseSource(
                        typeof(Array2D),
                        nameof(Index2DArrayBounds2DIndex2DInvalidSectorSizeTestCases))]
                    public static void ThrowsExceptionIfSectorSizeIsOutOfBounds<T>(
                        List2D<T> src,
                        Index2D srcIndex,
                        Array2D<T> dest,
                        Bounds2D sectorSize,
                        Index2D destIndex)
                        => Assert.Throws<ArgumentOutOfRangeException>(
                            () => src.CopyTo(srcIndex, dest, sectorSize, destIndex));

                    [TestCaseSource(
                        typeof(Array2D), nameof(Index2DArrayBounds2DIndex2DTestCases))]
                    public static void Test<T>(
                        List2D<T> src,
                        Index2D srcIndex,
                        Array2D<T> dest,
                        Bounds2D sectorSize,
                        Index2D destIndex,
                        Array2D<T> expected)
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
                            () => new List2D<byte>(0, 0).CopyTo(
                                destination: null as Array2D<byte>, new Bounds2D(), new Index2D()));

                    [TestCaseSource(typeof(Array2D), nameof(InvalidDestIndexTestCases))]
                    public static void ThrowsExceptionIfDestIndexIsOutOfBounds<T>(
                        Array2D<T> destination, Index2D destIndex)
                        => Assert.Throws<ArgumentOutOfRangeException>(
                                () => new List2D<T>(0, 0).CopyTo(
                                    destination, new Bounds2D(), destIndex));

                    [TestCaseSource(
                        typeof(Array2D), nameof(Bounds2DIndex2DInvalidSectorSizeTestCases))]
                    public static void ThrowsExceptionIfSectorSizeIsOutOfBounds<T>(
                        List2D<T> source,
                        Array2D<T> destination,
                        Bounds2D sectorSize,
                        Index2D destIndex)
                        => Assert.Throws<ArgumentOutOfRangeException>(
                            () => source.CopyTo(destination, sectorSize, destIndex));

                    [TestCaseSource(typeof(Array2D), nameof(Bounds2DIndex2DTestCases))]
                    public static void Test<T>(
                        List2D<T> src,
                        Array2D<T> dest,
                        Bounds2D sectorSize,
                        Index2D destIndex,
                        Array2D<T> expected)
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
                            () => new List2D<byte>(0, 0).CopyTo(
                                destination: null as Array2D<byte>, new Bounds2D()));

                    [TestCaseSource(
                        typeof(Array2D), nameof(Bounds2DInvalidSectorSizeTestCases))]
                    public static void ThrowsExceptionIfSectorSizeIsOutOfBounds<T>(
                        List2D<T> source, Array2D<T> destination, Bounds2D sectorSize)
                        => Assert.Throws<ArgumentOutOfRangeException>(
                            () => source.CopyTo(destination, sectorSize));

                    [TestCaseSource(typeof(Array2D), nameof(Bounds2DTestCases))]
                    public static void Test<T>(
                        List2D<T> src, Array2D<T> dest, Bounds2D sectorSize, Array2D<T> expected)
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
                            () => new List2D<byte>(0, 0).CopyTo(
                                destination: null as Array2D<byte>, new Index2D()));

                    [TestCaseSource(typeof(Array2D), nameof(InvalidDestIndexTestCases))]
                    public static void ThrowsExceptionIfDestIndexIsOutOfBounds<T>(
                        Array2D<T> destination, Index2D destIndex)
                        => Assert.Throws<ArgumentOutOfRangeException>(
                                () => new List2D<T>(0, 0).CopyTo(destination, destIndex));

                    [TestCaseSource(
                        typeof(Array2D), nameof(Index2DInvalidDestArrayTestCases))]
                    public static void ThrowsExceptionIfDestArrayCannotAccommodateAllElements<T>(
                        List2D<T> source, Array2D<T> destination, Index2D destIndex)
                        => Assert.Throws<ArgumentException>(
                            () => source.CopyTo(destination, destIndex));

                    [TestCaseSource(typeof(Array2D), nameof(Index2DTestCases))]
                    public static void Test<T>(
                        List2D<T> src, Array2D<T> dest, Index2D destIndex, Array2D<T> expected)
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
                            () => new List2D<byte>(0, 0).CopyTo(destination: null as Array2D<byte>));

                    [TestCaseSource(typeof(Array2D), nameof(InvalidDestArrayTestCases))]
                    public static void ThrowsExceptionIfDestArrayCannotAccommodateAllElements<T>(
                         List2D<T> source, Array2D<T> destination)
                        => Assert.Throws<ArgumentException>(() => source.CopyTo(destination));

                    [TestCaseSource(typeof(Array2D), nameof(TestCases))]
                    public static void Test<T>(
                        List2D<T> src, Array2D<T> dest, Array2D<T> expected)
                    {
                        src.CopyTo(dest);
                        CollectionAssert.AreEqual(expected, dest);
                    }
                }

                private static IEnumerable<TestCaseData> InvalidSourceIndexTestCases()
                {
                    var list4x5 = new List2D<byte>(8, 10);
                    list4x5.AddRows(4);
                    list4x5.AddColumns(5);

                    yield return new TestCaseData(list4x5, new Index2D(0, -1));
                    yield return new TestCaseData(list4x5, new Index2D(-1, 0));
                    yield return new TestCaseData(list4x5, new Index2D(4, 0));
                    yield return new TestCaseData(list4x5, new Index2D(0, 5));
                }

                private static IEnumerable<TestCaseData> InvalidDestIndexTestCases()
                {
                    yield return new TestCaseData(new Array2D<byte>(4, 5), new Index2D(-1, 0));
                    yield return new TestCaseData(new Array2D<byte>(4, 5), new Index2D(0, -1));
                    yield return new TestCaseData(new Array2D<byte>(4, 5), new Index2D(4, 0));
                    yield return new TestCaseData(new Array2D<byte>(4, 5), new Index2D(0, 5));
                }

                private static IEnumerable<TestCaseData>
                    Index2DArrayBounds2DIndex2DInvalidSectorSizeTestCases()
                {
                    var list3x5 = new List2D<byte>(6, 10);
                    list3x5.AddRows(3);
                    list3x5.AddColumns(5);

                    yield return new TestCaseData(
                        list3x5,
                        new Index2D(0, 0),
                        new Array2D<byte>(4, 5),
                        new Bounds2D(4, 5),
                        new Index2D(0, 0));
                    yield return new TestCaseData(
                        list3x5,
                        new Index2D(0, 0),
                        new Array2D<byte>(3, 6),
                        new Bounds2D(3, 6),
                        new Index2D(0, 0));
                    yield return new TestCaseData(
                        list3x5,
                        new Index2D(0, 0),
                        new Array2D<byte>(2, 5),
                        new Bounds2D(3, 5),
                        new Index2D(0, 0));
                    yield return new TestCaseData(
                        list3x5,
                        new Index2D(0, 0),
                        new Array2D<byte>(3, 4),
                        new Bounds2D(3, 5),
                        new Index2D(0, 0));
                    yield return new TestCaseData(
                        list3x5,
                        new Index2D(1, 0),
                        new Array2D<byte>(3, 5),
                        new Bounds2D(3, 5),
                        new Index2D(0, 0));
                    yield return new TestCaseData(
                        list3x5,
                        new Index2D(0, 1),
                        new Array2D<byte>(3, 5),
                        new Bounds2D(3, 5),
                        new Index2D(0, 0));
                    yield return new TestCaseData(
                        list3x5,
                        new Index2D(0, 0),
                        new Array2D<byte>(3, 5),
                        new Bounds2D(3, 5),
                        new Index2D(1, 0));
                    yield return new TestCaseData(
                        list3x5,
                        new Index2D(0, 0),
                        new Array2D<byte>(3, 5),
                        new Bounds2D(3, 5),
                        new Index2D(0, 1));
                }

                private static IEnumerable<TestCaseData>
                    Bounds2DIndex2DInvalidSectorSizeTestCases()
                {
                    var list3x5 = new List2D<byte>(6, 10);
                    list3x5.AddRows(3);
                    list3x5.AddColumns(5);

                    yield return new TestCaseData(
                        list3x5, new Array2D<byte>(4, 5), new Bounds2D(4, 5), new Index2D(0, 0));
                    yield return new TestCaseData(
                        list3x5, new Array2D<byte>(3, 6), new Bounds2D(3, 6), new Index2D(0, 0));
                    yield return new TestCaseData(
                        list3x5, new Array2D<byte>(2, 5), new Bounds2D(3, 5), new Index2D(0, 0));
                    yield return new TestCaseData(
                        list3x5, new Array2D<byte>(3, 4), new Bounds2D(3, 5), new Index2D(0, 0));                                
                    yield return new TestCaseData(
                        list3x5, new Array2D<byte>(3, 5), new Bounds2D(3, 5), new Index2D(1, 0));
                    yield return new TestCaseData(
                        list3x5, new Array2D<byte>(3, 5), new Bounds2D(3, 5), new Index2D(0, 1));
                }

                private static IEnumerable<TestCaseData> Bounds2DInvalidSectorSizeTestCases()
                {
                    var list3x5 = new List2D<byte>(6, 10);
                    list3x5.AddRows(3);
                    list3x5.AddColumns(5);

                    yield return new TestCaseData(
                        list3x5, new Array2D<byte>(4, 5), new Bounds2D(4, 5));
                    yield return new TestCaseData(
                        list3x5, new Array2D<byte>(3, 6), new Bounds2D(3, 6));
                    yield return new TestCaseData(
                        list3x5, new Array2D<byte>(2, 5), new Bounds2D(3, 5));
                    yield return new TestCaseData(
                        list3x5, new Array2D<byte>(3, 4), new Bounds2D(3, 5));
                }

                private static IEnumerable<TestCaseData> Index2DInvalidDestArrayTestCases()
                {
                    var list3x5 = new List2D<byte>(6, 10);
                    list3x5.AddRows(3);
                    list3x5.AddColumns(5);

                    yield return new TestCaseData(
                        list3x5, new Array2D<byte>(3, 5), new Index2D(1, 0));
                    yield return new TestCaseData(
                        list3x5, new Array2D<byte>(3, 5), new Index2D(0, 1));
                    yield return new TestCaseData(
                        list3x5, new Array2D<byte>(2, 5), new Index2D(0, 0));
                    yield return new TestCaseData(
                        list3x5, new Array2D<byte>(3, 4), new Index2D(0, 0));
                }

                private static IEnumerable<TestCaseData> InvalidDestArrayTestCases()
                {
                    var list3x5 = new List2D<byte>(6, 10);
                    list3x5.AddRows(3);
                    list3x5.AddColumns(5);

                    yield return new TestCaseData(list3x5, new Array2D<byte>(2, 5));
                    yield return new TestCaseData(list3x5, new Array2D<byte>(3, 4));
                }

                private static IEnumerable<TestCaseData> Index2DArrayBounds2DIndex2DTestCases()
                {
                    var list4x5 = List2D<int>.FromSystem2DArray(
                        new int[,] { {  0,  1,  2,  3,  4 },
                                     {  5,  6,  7,  8,  9 },
                                     { 10, 11, 12, 13, 14 },
                                     { 15, 16, 17, 18, 19 }, });
                    list4x5.IncreaseCapacity(list4x5.Boundaries);

                    var list3x3 = List2D<int>.FromSystem2DArray(
                        new int[,] { { 9, 1, 2 },
                                     { 3, 4, 5 },
                                     { 6, 7, 8 }, });
                    list3x3.IncreaseCapacity(list3x3.Boundaries);

                    yield return new TestCaseData(
                        list4x5,
                        new Index2D(2, 1),
                        new Array2D<int>(3, 5),
                        new Bounds2D(2, 3),
                        new Index2D(1, 2),
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 0,  0,  0,  0,  0 },
                                         { 0,  0, 11, 12, 13 },
                                         { 0,  0, 16, 17, 18 } }));
                    yield return new TestCaseData(
                        list3x3,
                        new Index2D(0, 1),
                        new Array2D<int>(3, 3),
                        new Bounds2D(2, 1),
                        new Index2D(1, 0),
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 0, 0, 0 },
                                         { 1, 0, 0 },
                                         { 4, 0, 0 }, }));
                    yield return new TestCaseData(
                        list3x3,
                        new Index2D(0, 1),
                        new Array2D<int>(3, 4),
                        new Bounds2D(2, 1),
                        new Index2D(1, 1),
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 0, 0, 0, 0 },
                                         { 0, 1, 0, 0 },
                                         { 0, 4, 0, 0 }, }));
                }

                private static IEnumerable<TestCaseData> Bounds2DIndex2DTestCases()
                {
                    var list4x5 = List2D<int>.FromSystem2DArray(
                        new int[,] { {  0,  1,  2,  3,  4 },
                                     {  5,  6,  7,  8,  9 },
                                     { 10, 11, 12, 13, 14 },
                                     { 15, 16, 17, 18, 19 }, });
                    list4x5.IncreaseCapacity(list4x5.Boundaries);

                    var list3x3 = List2D<int>.FromSystem2DArray(
                        new int[,] { { 9, 1, 2 },
                                     { 3, 4, 5 },
                                     { 6, 7, 8 }, });
                    list3x3.IncreaseCapacity(list3x3.Boundaries);

                    yield return new TestCaseData(
                       list4x5,
                       new Array2D<int>(3, 5),
                       new Bounds2D(2, 3),
                       new Index2D(1, 2),
                       Array2D<int>.FromSystem2DArray(
                           new int[,] { { 0,  0,  0,  0,  0 },
                                        { 0,  0,  0,  1,  2, },
                                        { 0,  0,  5,  6,  7, } }));
                    yield return new TestCaseData(
                        list3x3,
                        new Array2D<int>(3, 3),
                        new Bounds2D(2, 1),
                        new Index2D(1, 0),
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 0, 0, 0 },
                                         { 9, 0, 0 },
                                         { 3, 0, 0 }, }));
                    yield return new TestCaseData(
                        list3x3,
                        new Array2D<int>(3, 4),
                        new Bounds2D(2, 1),
                        new Index2D(1, 1),
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 0, 0, 0, 0 },
                                         { 0, 9, 0, 0 },
                                         { 0, 3, 0, 0 }, }));
                }

                private static IEnumerable<TestCaseData> Bounds2DTestCases()
                {
                    var list4x5 = List2D<int>.FromSystem2DArray(
                        new int[,] { {  0,  1,  2,  3,  4 },
                                     {  5,  6,  7,  8,  9 },
                                     { 10, 11, 12, 13, 14 },
                                     { 15, 16, 17, 18, 19 }, });
                    list4x5.IncreaseCapacity(list4x5.Boundaries);

                    var list3x3 = List2D<int>.FromSystem2DArray(
                        new int[,] { { 9, 1, 2 },
                                     { 3, 4, 5 },
                                     { 6, 7, 8 }, });
                    list3x3.IncreaseCapacity(list3x3.Boundaries);

                    yield return new TestCaseData(
                        list4x5,
                        new Array2D<int>(3, 5),
                        new Bounds2D(2, 3),
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 0,  1,  2,  0,  0 },
                                         { 5,  6,  7,  0,  0, },
                                         { 0,  0,  0,  0,  0, } }));
                    yield return new TestCaseData(
                        list3x3,
                        new Array2D<int>(3, 3),
                        new Bounds2D(2, 1),
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 9, 0, 0 },
                                         { 3, 0, 0, },
                                         { 0, 0, 0, }, }));
                    yield return new TestCaseData(
                        list3x3,
                        new Array2D<int>(3, 4),
                        new Bounds2D(2, 1),
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 9, 0, 0, 0 },
                                         { 3, 0, 0, 0 },
                                         { 0, 0, 0, 0 }, }));
                }

                private static IEnumerable<TestCaseData> Index2DTestCases()
                {
                    var list4x5 = List2D<int>.FromSystem2DArray(
                        new int[,] { {  0,  1,  2,  3,  4 },
                                     {  5,  6,  7,  8,  9 },
                                     { 10, 11, 12, 13, 14 },
                                     { 15, 16, 17, 18, 19 }, });
                    list4x5.IncreaseCapacity(list4x5.Boundaries);

                    var list3x3 = List2D<int>.FromSystem2DArray(
                        new int[,] { { 9, 1, 2 },
                                     { 3, 4, 5 },
                                     { 6, 7, 8 }, });
                    list3x3.IncreaseCapacity(list3x3.Boundaries);

                    yield return new TestCaseData(
                      list4x5,
                      new Array2D<int>(5, 7),
                      new Index2D(1, 2),
                      Array2D<int>.FromSystem2DArray(
                          new int[,] { { 0,  0,  0,  0,  0,  0,  0 },
                                       { 0,  0,  0,  1,  2,  3,  4 },
                                       { 0,  0,  5,  6,  7,  8,  9 },
                                       { 0,  0, 10, 11, 12, 13, 14 },
                                       { 0,  0, 15, 16, 17, 18, 19 }}));
                    yield return new TestCaseData(
                        list3x3,
                        new Array2D<int>(4, 3),
                        new Index2D(1, 0),
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 0, 0, 0 },
                                         { 9, 1, 2 },
                                         { 3, 4, 5 },
                                         { 6, 7, 8 } }));
                }

                private static IEnumerable<TestCaseData> TestCases()
                {
                    var list2x3 = List2D<int>.FromSystem2DArray(
                        new int[,] { { 9, 1, 2 },
                                     { 3, 4, 5 } });
                    list2x3.IncreaseCapacity(list2x3.Boundaries);

                    var list3x2 = List2D<int>.FromSystem2DArray(
                        new int[,] { { 9, 1 },
                                     { 3, 4 },
                                     { 6, 7 }, });
                    list3x2.IncreaseCapacity(list3x2.Boundaries);

                    var list0x0 = new List2D<int>(list3x2.Boundaries);

                    yield return new TestCaseData(
                        list2x3,
                        new Array2D<int>(2, 3),
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 9, 1, 2 },
                                         { 3, 4, 5 } }));
                    yield return new TestCaseData(
                        list3x2,
                        new Array2D<int>(3, 2),
                        Array2D<int>.FromSystem2DArray(
                            new int[,] { { 9, 1 },
                                         { 3, 4 },
                                         { 6, 7 }, }));
                    yield return new TestCaseData(
                        list0x0, new Array2D<int>(0, 0), new Array2D<int>(0, 0));
                }
            }
        }
    }
}
