using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public partial class List2DTests
    {
        public static class Properties
        {
            [TestCase(0)]
            [TestCase(1)]
            [TestCase(5)]
            public static void TestRows(int rows)
            {
                var list = new List2D<int>();
                list.AddRows(rows);

                Assert.AreEqual(rows, list.Rows);
            }

            [TestCase(0)]
            [TestCase(1)]
            [TestCase(5)]
            public static void TestColumns(int columns)
            {
                var list = new List2D<int>();
                list.AddColumns(columns);

                Assert.AreEqual(columns, list.Columns);
            }

            [TestCase(0, 0)]
            [TestCase(0, 1)]
            [TestCase(1, 0)]
            [TestCase(4, 2)]
            [TestCase(1, 6)]
            public static void TestCount(int rows, int columns)
            {
                var list = new List2D<int>();
                list.IncreaseBounds(rows, columns);

                Assert.AreEqual(rows * columns, list.Count);
            }

            [TestCase(0, 0)]
            [TestCase(0, 1)]
            [TestCase(1, 0)]
            [TestCase(4, 2)]
            [TestCase(1, 6)]
            public static void TestCapacity(int rows, int columns)
            {
                var capacity = new Bounds2D(rows, columns);
                var list = new List2D<int>(capacity);

                Assert.AreEqual(capacity, list.Capacity);
            }

            [TestCase(0, 0)]
            [TestCase(0, 1)]
            [TestCase(1, 0)]
            [TestCase(4, 2)]
            [TestCase(1, 6)]
            public static void TestBoundaries(int rows, int columns)
            {
                var list = new List2D<int>(rows, columns);
                list.IncreaseBounds(rows, columns);

                Assert.AreEqual(new Bounds2D(rows, columns), list.Boundaries);
            }

            [TestCase(0, 0)]
            [TestCase(0, 1)]
            [TestCase(1, 0)]
            [TestCase(4, 2)]
            [TestCase(1, 6)]
            public static void IsEmptyReturnsTrueIfListIsInitializedWithCapacity(
                int rowsCap, int colsCap)
                => Assert.That(new List2D<int>(rowsCap, colsCap).IsEmpty);           

            [Test]
            public static void IsEmptyReturnsTrueWhenListIsInitializedWithoutSpecifiedCapacity()
                => Assert.That(new List2D<int>().IsEmpty);

            [TestCaseSource(typeof(List2DTests), nameof(ListsOfValues))]
            [TestCaseSource(typeof(List2DTests), nameof(ListsOfReferences))]
            public static void IsEmptyReturnsTrueWhenListIsCleared<T>(List2D<T> list)
            {
                list.Clear();
                Assert.That(list.IsEmpty);
            }
        }
    }
}
