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
        public static class Increase
        {
            public static class Bounds
            {
                [TestCaseSource(typeof(Increase), nameof(InvalidSizeTestCases))]
                public static void ThrowsExceptionIfSizeIsInvalid<T>(
                    List2D<T> list, int rows, int columns)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.IncreaseBounds(rows, columns));

                [TestCaseSource(typeof(Bounds), nameof(Bounds2DTestCases))]
                public static void Test<T>(List2D<T> list, Bounds2D size, Bounds2D expected)
                {
                    list.IncreaseBounds(size);
                    Assert.AreEqual(expected, list.Boundaries, 
                        $"Expected: {expected.ToValueTuple()}\n" +
                        $"But was:  {list.Boundaries.ToValueTuple()}");
                }

                [TestCaseSource(typeof(Bounds), nameof(IntIntTestCases))]
                public static void Test<T>(
                    List2D<T> list, int rows, int columns, Bounds2D expected)
                {
                    list.IncreaseBounds(rows, columns);
                    Assert.AreEqual(expected, list.Boundaries,
                        $"Expected: {expected.ToValueTuple()}\n" +
                        $"But was:  {list.Boundaries.ToValueTuple()}");
                }

                private static IEnumerable<TestCaseData> Bounds2DTestCases()
                {
                    var list1x6 = List2D<int>.FromSystem2DArray(
                        new int[,] { { 0, 0, 0, 0, 0, 0 } });

                    yield return new TestCaseData(
                        new List2D<byte>(0, 0), new Bounds2D(2, 3), new Bounds2D(2, 3));
                    yield return new TestCaseData(list1x6, new Bounds2D(4, 2), new Bounds2D(5, 8));
                }

                private static IEnumerable<TestCaseData> IntIntTestCases()
                {
                    var list1x6 = List2D<int>.FromSystem2DArray(
                                            new int[,] { { 0, 0, 0, 0, 0, 0 } });

                    yield return new TestCaseData(new List2D<int>(0, 0), 2, 3, new Bounds2D(2, 3));
                    yield return new TestCaseData(list1x6, 4, 2, new Bounds2D(5, 8));
                }
            }

            private static IEnumerable<TestCaseData> InvalidSizeTestCases()
            {
                yield return new TestCaseData(new List2D<byte>(0, 0), -1, 0);
                yield return new TestCaseData(new List2D<byte>(0, 0), 0, -1);
            }
        }
    }
}
