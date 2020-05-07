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
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public static void TestRows(int rows)
        {
            var list = new List2D<int>();
            list.AddRows(rows);

            Assert.AreEqual(rows, list.Rows);
            Assert.AreEqual(rows, list.Length1);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public static void TestColumns(int columns)
        {
            var list = new List2D<int>();
            list.AddColumns(columns);

            Assert.AreEqual(columns, list.Columns);
            Assert.AreEqual(columns, list.Length2);
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(4, 2)]
        [TestCase(1, 6)]
        public static void TestCount(int rows, int columns)
        {
            var list = new List2D<int>();
            list.AddRows(rows);
            list.AddColumns(columns);

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
            list.AddRows(rows);
            list.AddColumns(columns);

            Assert.AreEqual(new Bounds2D(rows, columns), list.Boundaries);
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(4, 2)]
        [TestCase(1, 6)]
        public static void IsEmptyReturnsTrueWhenListIsInitializedWithSpecificCapacity(
            int rowsCap, int colsCap)
        {
            var list = new List2D<int>(rowsCap, colsCap);

            Assert.True(list.IsEmpty);
        }

        [Test]
        public static void IsEmptyReturnsTrueWhenListIsInitializedWithParameterlessConstructor()
        {
            var list = new List2D<int>();

            Assert.True(list.IsEmpty);
        }

        [TestCaseSource(nameof(ListsOfValues))]
        [TestCaseSource(nameof(ListsOfReferences))]
        public static void IsEmptyReturnsTrueWhenListIsCleared<T>(List2D<T> nonEmptyList)
        {
            nonEmptyList.Clear();

            Assert.True(nonEmptyList.IsEmpty);
        }

        public static void CheckUnusedReferences<T>(
            List2D<T> list, Index2D firstUnusedRefIndex, Bounds2D unusedRefsQuantity)
            where T : class
        {
            for (int i = firstUnusedRefIndex.Row,
                rows = i + unusedRefsQuantity.Rows; i < rows; i++)
            {
                for (int j = firstUnusedRefIndex.Column,
                         cols = j + unusedRefsQuantity.Columns; j < cols; j++)
                {
                    ref T unusedRef = ref list.GetItemInternal(i, j);

                    Assert.AreEqual(null, unusedRef, $"Actual differs at {new Index2D(i, j)}.");
                }
            }
        }

        private static IEnumerable<TestCaseData> ListsOfValues()
        {
            var list2x3 = List2D<int>.FromSystem2DArray(
                new int[,] { { 2, 3 },
                             { 4, 9 },
                             { 8, 2 } });
            list2x3.IncreaseCapacity(list2x3.Boundaries);

            yield return new TestCaseData(list2x3);
            yield return new TestCaseData(
                List2D<int>.FromSystem2DArray(
                    new int[,] { { 1, 2, 3, 4, 5  },
                                 { 6, 7, 8, 9, 10 } }));
            yield return new TestCaseData(new List2D<byte>(3, 4));
        }

        private static IEnumerable<TestCaseData> ListsOfReferences()
        {
            var list2x3 = List2D<string>.FromSystem2DArray(
                new string[,] { { "2", "3" },
                                { "4", "9" },
                                { "8", "2" } });
            list2x3.IncreaseCapacity(list2x3.Boundaries);

            yield return new TestCaseData(list2x3);
            yield return new TestCaseData(
                List2D<string>.FromSystem2DArray(
                    new string[,] { { "1", "2", "3", "4", "5" },
                                    { "6", "7", "8", "9", "10" } }));
            yield return new TestCaseData(new List2D<object>(3, 4));
        }
    }
}
