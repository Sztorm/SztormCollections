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
        [Test]
        public static void CopyToThrowsExceptionIfDestinationIsNull()
            {
                var array = new Array2D<int>(0, 0);
                TestDelegate copyToSys2DArrMethod = () => array.CopyTo(destination: null);
                TestDelegate copyToSys2DArrIndexMethod = ()
                    => array.CopyTo(destination: null, new Index2D());

                TestDelegate copyToSys2DArrBoundsMethod = ()
                    => array.CopyTo(destination: null, new Bounds2D());

                TestDelegate copyToSys2DArrBoundsIndexMethod = ()
                    => array.CopyTo(destination: null, new Bounds2D(), new Index2D());

                TestDelegate copyToIndexSys2DArrBoundsIndexMethod = ()
                    => array.CopyTo(
                        new Index2D(), destination: null, new Bounds2D(), new Index2D());

                Assert.Throws<ArgumentNullException>(
                    copyToSys2DArrMethod,
                    "\n In method " + TestUtils.CreateShortMethodSignature(
                        nameof(Array2D<int>.CopyTo),
                        typeof(int[,])));

                Assert.Throws<ArgumentNullException>(
                    copyToSys2DArrIndexMethod,
                    "\n In method " + TestUtils.CreateShortMethodSignature(
                        nameof(Array2D<int>.CopyTo),
                        typeof(int[,]),
                        typeof(Index2D)));

                Assert.Throws<ArgumentNullException>(
                    copyToSys2DArrBoundsMethod,
                    "\n In method " + TestUtils.CreateShortMethodSignature(
                        nameof(Array2D<int>.CopyTo),
                        typeof(int[,]),
                        typeof(Bounds2D)));

                Assert.Throws<ArgumentNullException>(
                    copyToSys2DArrBoundsIndexMethod,
                    "\n In method " + TestUtils.CreateShortMethodSignature(
                        nameof(Array2D<int>.CopyTo),
                        typeof(int[,]),
                        typeof(Bounds2D),
                        typeof(Index2D)));

                Assert.Throws<ArgumentNullException>(
                    copyToIndexSys2DArrBoundsIndexMethod,
                    "\n In method " + TestUtils.CreateShortMethodSignature(
                        nameof(Array2D<int>.CopyTo),
                        typeof(Index2D),
                        typeof(int[,]),
                        typeof(Bounds2D),
                        typeof(Index2D)));

            }

        [TestCaseSource(
            typeof(Array2DTests),
            nameof(CopyToIndexSys2DArrBoundsIndexInvalidSourceIndexTestCases))]
        public static void CopyToThrowsExceptionIfSourceIndexIsOutOfBounds(
            Array2D<int> source,
            int[,] destination,
            Index2D sourceIndex,
            Bounds2D quantity,
            Index2D destIndex)
            {
                TestDelegate copyToIndexSys2DArrBoundsIndexMethod = ()
                    => source.CopyTo(sourceIndex, destination, quantity, destIndex);

                Assert.Throws<ArgumentOutOfRangeException>(
                    copyToIndexSys2DArrBoundsIndexMethod,
                    "\n In method " + TestUtils.CreateShortMethodSignature(
                        nameof(Array2D<int>.CopyTo),
                        typeof(Index2D),
                        typeof(int[,]),
                        typeof(Bounds2D),
                        typeof(Index2D)));
            }

        [TestCaseSource(
            typeof(Array2DTests), nameof(CopyToIndexSys2DArrBoundsIndexInvalidQuantityTestCases))]
        public static void CopyToThrowsExceptionIfQuantityExceedsBounds(
            Array2D<int> source,
            int[,] destination,
            Index2D sourceIndex,
            Bounds2D quantity,
            Index2D destIndex)
            {
                TestDelegate copyToIndexSys2DArrBoundsIndexMethod = ()
                    => source.CopyTo(sourceIndex, destination, quantity, destIndex);

                TestDelegate copyToSys2DArrBoundsIndexMethod = ()
                    => source.CopyTo(destination, quantity, destIndex);

                TestDelegate copyToSys2DArrBoundsMethod = ()
                    => source.CopyTo(destination, quantity);

                Assert.Throws<ArgumentOutOfRangeException>(
                    copyToIndexSys2DArrBoundsIndexMethod,
                    "\n In method " + TestUtils.CreateShortMethodSignature(
                        nameof(Array2D<int>.CopyTo),
                        typeof(Index2D),
                        typeof(int[,]),
                        typeof(Bounds2D),
                        typeof(Index2D)));

                Assert.Throws<ArgumentOutOfRangeException>(
                    copyToSys2DArrBoundsIndexMethod,
                    "\n In method " + TestUtils.CreateShortMethodSignature(
                        nameof(Array2D<int>.CopyTo),
                        typeof(int[,]),
                        typeof(Bounds2D),
                        typeof(Index2D)));

                Assert.Throws<ArgumentOutOfRangeException>(
                    copyToSys2DArrBoundsMethod,
                    "\n In method " + TestUtils.CreateShortMethodSignature(
                        nameof(Array2D<int>.CopyTo),
                        typeof(int[,]),
                        typeof(Bounds2D)));
            }


        [TestCaseSource(
            typeof(Array2DTests), nameof(CopyToIndexSys2DArrBoundsIndexInvalidDestIndexTestCases))]
        public static void CopyToThrowsExceptionIfDestIndexExceedsBounds(
            Array2D<int> source,
            int[,] destination,
            Index2D sourceIndex,
            Bounds2D quantity,
            Index2D destIndex)
            {
                TestDelegate copyToIndexSys2DArrBoundsIndexMethod = ()
                    => source.CopyTo(sourceIndex, destination, quantity, destIndex);

                TestDelegate copyToSys2DArrBoundsIndexMethod = ()
                    => source.CopyTo(destination, quantity, destIndex);

                TestDelegate copyToSys2DArrIndexMethod = ()
                    => source.CopyTo(destination, destIndex);

                Assert.Throws<ArgumentOutOfRangeException>(
                    copyToIndexSys2DArrBoundsIndexMethod,
                    "\n In method " + TestUtils.CreateShortMethodSignature(
                        nameof(Array2D<int>.CopyTo),
                        typeof(Index2D),
                        typeof(int[,]),
                        typeof(Bounds2D),
                        typeof(Index2D)));

                Assert.Throws<ArgumentOutOfRangeException>(
                    copyToSys2DArrBoundsIndexMethod,
                    "\n In method " + TestUtils.CreateShortMethodSignature(
                        nameof(Array2D<int>.CopyTo),
                        typeof(int[,]),
                        typeof(Bounds2D),
                        typeof(Index2D)));

                Assert.Throws<ArgumentOutOfRangeException>(
                    copyToSys2DArrIndexMethod,
                    "\n In method " + TestUtils.CreateShortMethodSignature(
                        nameof(Array2D<int>.CopyTo),
                        typeof(int[,]),
                        typeof(Index2D)));
            }

        [TestCaseSource(
            typeof(Array2DTests), nameof(CopyToIndexSys2DArrBoundsIndexValidTestCases))]
        public static void TestCopyToIndexSys2DArrBoundsIndex(
            Array2D<int> sourceArray,
            Index2D sourceIndex,
            Bounds2D quantity,
            Index2D destIndex,
            int[,] expected)
            {
                var dest = new int[sourceArray.Rows, sourceArray.Columns];

                sourceArray.CopyTo(sourceIndex, dest, quantity, destIndex);
                Assert.AreEqual(expected, dest);
            }

        [TestCaseSource(
            typeof(Array2DTests), nameof(CopyToSys2DArrIndexInvalidDestArrayTestCases))]
        public static void CopyToThrowsExceptionIfDestArrayHasInvalidBoundaries(
            Array2D<int> source,
            int[,] destination,
            Index2D destIndex)
            {
                TestDelegate testMethod = ()
                    => source.CopyTo(destination, destIndex);

                Assert.Throws<ArgumentException>(testMethod,
                    "\n In method " + TestUtils.CreateShortMethodSignature(
                        nameof(Array2D<int>.CopyTo),
                        typeof(int[,])),
                        typeof(Index2D));
            }

        [TestCaseSource(typeof(Array2DTests), nameof(CopyToSys2DArrInvalidDestArrayTestCases))]
        public static void CopyToThrowsExceptionIfDestArrayHasInvalidBoundaries(
            Array2D<int> source,
            int[,] destination)
            {
                TestDelegate testMethod = ()
                    => source.CopyTo(destination);

                Assert.Throws<ArgumentException>(testMethod,
                    "\n In method " + TestUtils.CreateShortMethodSignature(
                        nameof(Array2D<int>.CopyTo),
                        typeof(int[,])));
            }

        private static IEnumerable<TestCaseData> CopyToIndexSys2DArrBoundsIndexValidTestCases()
            {
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 0,  1,  2,  3,  4  },
                                     { 5,  6,  7,  8,  9  },
                                     { 10, 11, 12, 13, 14 },
                                     { 15, 16, 17, 18, 19 }, }),
                    new Index2D(2, 1),
                    new Bounds2D(2, 3),
                    new Index2D(1, 2),
                    new int[,] { { 0,  0,  0,  0,  0,  },
                                 { 0,  0,  11, 12, 13, },
                                 { 0,  0,  16, 17, 18, },
                                 { 0,  0,  0,  0,  0,  }, });
                yield return new TestCaseData(
                    Array2D<int>.FromSystem2DArray(
                        new int[,] { { 0,  1,  2,},
                                     { 3,  4,  5,},
                                     { 6,  7,  8,}, }),
                    new Index2D(0, 1),
                    new Bounds2D(2, 1),
                    new Index2D(1, 0),
                    new int[,] { { 0,  0,  0, },
                                 { 1,  0,  0, },
                                 { 4,  0,  0, }, });
            }

        private static IEnumerable<TestCaseData> 
            CopyToIndexSys2DArrBoundsIndexInvalidSourceIndexTestCases()
            {
                // SourceIndex with negative row component.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 5),
                    new int[4, 5],
                    new Index2D(0, -1),
                    new Bounds2D(4, 5),
                    new Index2D (0, 0));

                // SourceIndex with negative column component.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 5),
                    new int[4, 5],
                    new Index2D(-1, 0),
                    new Bounds2D(4, 5),
                    new Index2D(0, 0));

                // SourceIndex exceeding row count in source array.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 5),
                    new int[4, 5],
                    new Index2D(4, 0),
                    new Bounds2D(4, 5),
                    new Index2D(0, 0));

                // SourceIndex exceeding column count in source array.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 5),
                    new int[4, 5],
                    new Index2D(0, 5),
                    new Bounds2D(4, 5),
                    new Index2D(0, 0));
            }

        private static IEnumerable<TestCaseData>
            CopyToIndexSys2DArrBoundsIndexInvalidQuantityTestCases()
            {
                // Quantity exceeding row count in source array.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 5),
                    TestUtils.IncrementedSystemInt2DArray(5, 5),
                    new Index2D(0, 0),
                    new Bounds2D(5, 5),
                    new Index2D(0, 0));

                // Quantity exceeding column count in source array.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 5),
                    TestUtils.IncrementedSystemInt2DArray(4, 6),
                    new Index2D(0, 0),
                    new Bounds2D(4, 6),
                    new Index2D(0, 0));

                // Quantity exceeding row count in destination array.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(5, 5),
                    TestUtils.IncrementedSystemInt2DArray(4, 5),
                    new Index2D(0, 0),
                    new Bounds2D(5, 5),
                    new Index2D(0, 0));

                // Quantity exceeding column count in destination array.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 6),
                    TestUtils.IncrementedSystemInt2DArray(4, 5),
                    new Index2D(0, 0),
                    new Bounds2D(4, 6), 
                    new Index2D(0, 0));
            }

        private static IEnumerable<TestCaseData>
            CopyToIndexSys2DArrBoundsIndexInvalidDestIndexTestCases()
            {
                // Destination index with negative row component.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 5),
                    new int[4, 5],
                    new Index2D(0, 0),
                    new Bounds2D(4, 5),
                    new Index2D(-1, 0));

                // Destination index with negative column component.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 5),
                    new int[4, 5],
                    new Index2D(0, 0),
                    new Bounds2D(4, 5),
                    new Index2D(0, -1));

                // Destination index exceeding row count in destination array.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 5),
                    new int[3, 5],
                    new Index2D(0, 0),
                    new Bounds2D(3, 5),
                    new Index2D(4, 0));

                // Destination index exceeding column count in destination array.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 5),
                    new int[4, 4],
                    new Index2D(0, 0),
                    new Bounds2D(4, 4),
                    new Index2D(0, 5));
            }

        private static IEnumerable<TestCaseData> CopyToSys2DArrIndexInvalidDestArrayTestCases()
            {
                // Destination array with starting index exceeding its row boundary.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 5),
                    new int[4, 5],
                    new Index2D(1, 0));

                // Destination array with starting index exceeding its column boundary.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 5),
                    new int[4, 5],
                    new Index2D(0, 1));

                // Destination array with column boundary that is lesser than source column
                // boundary.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 5),
                    new int[3, 5],
                    new Index2D(0, 0));

                // Destination array with row boundary that is lesser than source row boundary.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 5),
                    new int[4, 4],
                    new Index2D(0, 0));
            }

        private static IEnumerable<TestCaseData> CopyToSys2DArrInvalidDestArrayTestCases()
            {
                // Destination array with column boundary that is lesser than source column
                // boundary.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(3, 5),
                    new int[2, 5]);

                // Destination array with row boundary that is lesser than source row boundary.
                yield return new TestCaseData(
                    TestUtils.IncrementedIntArray2D(4, 3),
                    new int[4, 2]);
            }
    }
}
