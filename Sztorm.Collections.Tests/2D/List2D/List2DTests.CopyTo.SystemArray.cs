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
            public static class SystemArray
            {                        
                public static class ArrayInt
                {
                    [Test]
                    public static void ThrowsExceptionIfDestinationIsNull()
                        => Assert.Throws<ArgumentNullException>(
                            () => new List2D<byte>(0, 0).CopyTo(destination: null as Array, 0));

                    [TestCaseSource(typeof(ArrayInt), nameof(InvalidDestIndexTestCases))]
                    public static void ThrowsExceptionIfDestIndexIsOutOfBounds(
                        int[] destination, int destIndex)
                        => Assert.Throws<ArgumentOutOfRangeException>(
                            () => new List2D<int>(0, 0).CopyTo(destination, destIndex));

                    [TestCaseSource(typeof(ArrayInt), nameof(InvalidDestArrayTestCases))]
                    public static void ThrowsExceptionIfDestArrayCannotAccommodateAllElements(
                        List2D<int> source, int[] destination, int destIndex)
                        => Assert.Throws<ArgumentException>(
                            () => source.CopyTo(destination, destIndex));

                    [TestCaseSource(typeof(ArrayInt), nameof(TestCases))]
                    public static void Test<T>(List2D<T> src, Array dest, int destIndex, Array expected)
                    {
                        src.CopyTo(dest, destIndex);
                        CollectionAssert.AreEqual(expected, dest);
                    }

                    private static IEnumerable<TestCaseData> InvalidDestIndexTestCases()
                    {
                        yield return new TestCaseData(new int[6], -1);
                        yield return new TestCaseData(new int[6], 6);
                    }

                    private static IEnumerable<TestCaseData> InvalidDestArrayTestCases()
                    {
                        var list2x3 = List2D<int>.FromSystem2DArray(
                            new int[,] { { 1, 2, 3 },
                                         { 4, 5, 6 } });
                        list2x3.IncreaseCapacity(list2x3.Boundaries);

                        yield return new TestCaseData(list2x3, new int[5], 0);
                        yield return new TestCaseData(list2x3, new int[6], 1);
                        yield return new TestCaseData(list2x3, new int[7], 2);
                    }

                    private static IEnumerable<TestCaseData> TestCases()
                    {
                        var list0x0 = new List2D<int>(0, 0);
                        var list2x3 = List2D<int>.FromSystem2DArray(
                            new int[,] { { 1, 2, 3 },
                                         { 4, 5, 6 } });
                        list2x3.IncreaseCapacity(list2x3.Boundaries);

                        yield return new TestCaseData(list0x0, new int[1], 0, new int[1]);
                        yield return new TestCaseData(list0x0, new int[3], 2, new int[3]);
                        yield return new TestCaseData(
                            list2x3, new int[6], 0, new int[] { 1, 2, 3, 4, 5, 6 });
                        yield return new TestCaseData(
                            list2x3, new int[9], 3, new int[] { 0, 0, 0, 1, 2, 3, 4, 5, 6 });
                    }
                }
            }
        }
    }
}
