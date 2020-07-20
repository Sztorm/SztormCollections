using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public partial class Array2DTests
    {
        public static class Properties
        {
            [TestCase(0)]
            [TestCase(1)]
            [TestCase(5)]
            public static void TestRows(int rows)
            {
                var array = new Array2D<int>(rows, 1);

                Assert.AreEqual(rows, array.Rows);
                Assert.AreEqual(rows, array.Length1);
            }

            [TestCase(0)]
            [TestCase(1)]
            [TestCase(5)]
            public static void TestColumns(int columns)
            {
                var array = new Array2D<int>(1, columns);

                Assert.AreEqual(columns, array.Columns);
                Assert.AreEqual(columns, array.Length2);
            }

            [TestCase(0, 0)]
            [TestCase(0, 1)]
            [TestCase(1, 0)]
            [TestCase(4, 2)]
            [TestCase(1, 6)]
            public static void TestCount(int rows, int columns)
                => Assert.AreEqual(rows * columns, new Array2D<int>(rows, columns).Count);

            [TestCase(0, 0)]
            [TestCase(0, 1)]
            [TestCase(1, 0)]
            [TestCase(4, 2)]
            [TestCase(1, 6)]
            public static void TestBoundaries(int rows, int columns)
                => Assert.AreEqual(
                    new Bounds2D(rows, columns), new Array2D<int>(rows, columns).Boundaries);
        }
    }
}
