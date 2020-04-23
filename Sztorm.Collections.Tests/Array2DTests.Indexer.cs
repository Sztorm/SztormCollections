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
        public static class Indexer
        {
            [TestCaseSource(typeof(Indexer), nameof(Index2DInvalidTestCases))]
            public static void ThrowsExceptionIfIndexIsOutOfBounds<T>(
                Array2D<T> array, Index2D index)
                => Assert.Throws<IndexOutOfRangeException>(
                    () => { ref readonly T value = ref array[index]; });

            [TestCaseSource(typeof(Indexer), nameof(Index2DTestCases))]
            public static T Test<T>(Array2D<T> array, Index2D index) => array[index];

            [TestCaseSource(typeof(Indexer), nameof(IntInvalidTestCases))]
            public static void ThrowsExceptionIfIndexIsOutOfBounds<T>(
                Array2D<T> array, int index)
                => Assert.Throws<IndexOutOfRangeException>(
                    () => { ref readonly T value = ref array[index]; });

            [TestCaseSource(typeof(Indexer), nameof(IntTestCases))]
            public static T Test<T>(Array2D<T> array, int index) => array[index];

            private static IEnumerable<TestCaseData> Index2DTestCases()
            {
                var array3x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });

                yield return new TestCaseData(array3x3, new Index2D(1, 1)).Returns(9);
                yield return new TestCaseData(array3x3, new Index2D(2, 0)).Returns(8);
            }

            private static IEnumerable<TestCaseData> Index2DInvalidTestCases()
            {
                yield return new TestCaseData(new Array2D<byte>(3, 3), new Index2D(-1, 0));
                yield return new TestCaseData(new Array2D<byte>(3, 3), new Index2D(0, -1));
                yield return new TestCaseData(new Array2D<byte>(3, 3), new Index2D(3, 0));
                yield return new TestCaseData(new Array2D<byte>(3, 3), new Index2D(0, 3));
            }

            private static IEnumerable<TestCaseData> IntTestCases()
            {
                var array3x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });

                yield return new TestCaseData(array3x3, 4).Returns(9);
                yield return new TestCaseData(array3x3, 6).Returns(8);
            }

            private static IEnumerable<TestCaseData> IntInvalidTestCases()
            {
                yield return new TestCaseData(new Array2D<byte>(3, 3), -1);
                yield return new TestCaseData(new Array2D<byte>(3, 3), 9);
            }
        }
    }
}
