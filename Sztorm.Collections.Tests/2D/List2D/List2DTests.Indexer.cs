using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    using static TestUtils;

    public partial class List2DTests
    {
        public static class Indexer
        {
            [TestCaseSource(typeof(Indexer), nameof(Index2DInvalidTestCases))]
            public static void ThrowsExceptionIfIndexIsOutOfBounds<T>(
                List2D<T> list, Index2D index)
                => Assert.Throws<IndexOutOfRangeException>(
                    () => { ref readonly T value = ref list[index]; });

            [TestCaseSource(typeof(Indexer), nameof(Index2DTestCases))]
            public static T Test<T>(List2D<T> list, Index2D index) => list[index];

            private static IEnumerable<TestCaseData> Index2DTestCases()
            {
                var list3x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });
                list3x3.IncreaseCapacity(list3x3.Capacity);

                yield return new TestCaseData(list3x3, new Index2D(1, 1)).Returns(9);
                yield return new TestCaseData(list3x3, new Index2D(2, 0)).Returns(8);
            }

            private static IEnumerable<TestCaseData> Index2DInvalidTestCases()
            {
                yield return new TestCaseData(
                    CreateList2DWithBounds<byte>(3, 3), new Index2D(-1, 0));
                yield return new TestCaseData(
                    CreateList2DWithBounds<byte>(3, 3), new Index2D(0, -1));
                yield return new TestCaseData(
                    CreateList2DWithBounds<byte>(3, 3), new Index2D(3, 0));
                yield return new TestCaseData(
                    CreateList2DWithBounds<byte>(3, 3), new Index2D(0, 3));
            }
        }
    }
}
