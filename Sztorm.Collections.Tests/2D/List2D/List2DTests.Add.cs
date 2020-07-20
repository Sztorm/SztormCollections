using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    using static TestUtils;

    [TestFixture]
    partial class List2DTests
    {
        public static class Add
        {
            public static class Rows
            {
                [TestCaseSource(typeof(Add), nameof(InvalidCountTestCases))]
                public static void ThrowsExceptionIfCountIsInvalid<T>(List2D<T> list, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.AddRows(count));

                [TestCaseSource(typeof(Rows), nameof(RowsTestCases))]
                public static void Test<T>(List2D<T> list, int count, List2D<T> expected)
                {
                    list.AddRows(count);
                    Assert.AreEqual(expected.Boundaries, list.Boundaries,
                        $"Expected: {expected.Boundaries.ToValueTuple()}\n" +
                        $"But was:  {list.Boundaries.ToValueTuple()}");
                    Assert.That(list.Capacity.Rows >= list.Rows);
                    Assert.That(list.Capacity.Columns >= list.Columns);
                    CollectionAssert.AreEqual(expected, list);
                }

                [TestCaseSource(typeof(Rows), nameof(RowsTestCases))]
                public static void TestNoAlloc<T>(List2D<T> list, int count, List2D<T> expected)
                {
                    list.IncreaseCapacity(expected.Capacity);
                    Bounds2D expectedCapacity = list.Capacity;

                    list.AddRows(count);
                    Assert.AreEqual(expected.Boundaries, list.Boundaries);
                    Assert.AreEqual(expectedCapacity, list.Capacity);
                    CollectionAssert.AreEqual(expected, list);
                }

                [TestCaseSource(typeof(Rows), nameof(RowTestCases))]
                public static void Test<T>(List2D<T> list, List2D<T> expected)
                {
                    list.AddRow();
                    Assert.AreEqual(expected.Boundaries, list.Boundaries,
                        $"Expected: {expected.Boundaries.ToValueTuple()}\n" +
                        $"But was:  {list.Boundaries.ToValueTuple()}");
                    Assert.That(list.Capacity.Rows >= list.Rows);
                    Assert.That(list.Capacity.Columns >= list.Columns);
                    CollectionAssert.AreEqual(expected, list);
                }

                [TestCaseSource(typeof(Rows), nameof(RowTestCases))]
                public static void TestNoAlloc<T>(List2D<T> list, List2D<T> expected)
                {
                    list.IncreaseCapacity(expected.Capacity);
                    Bounds2D expectedCapacity = list.Capacity;

                    list.AddRow();
                    Assert.AreEqual(expected.Boundaries, list.Boundaries);
                    Assert.AreEqual(expectedCapacity, list.Capacity);
                    CollectionAssert.AreEqual(expected, list);
                }

                private static IEnumerable<TestCaseData> RowsTestCases()
                {
                    var expectedList2x0 = new List2D<byte>();
                    expectedList2x0.IncreaseBounds(2, 0);

                    yield return new TestCaseData(
                        List2D<string>.FromSystem2DArray(
                            new string[,] { { "2", "3", "5" },
                                        { "4", "9", "1" } }),
                        2,
                        List2D<string>.FromSystem2DArray(
                            new string[,] { { "2", "3", "5" },
                                        { "4", "9", "1" },
                                        { default, default, default },
                                        { default, default, default }}));
                    yield return new TestCaseData(new List2D<byte>(), 2, expectedList2x0);
                    yield return new TestCaseData(new List2D<byte>(), 0, new List2D<byte>());
                }

                private static IEnumerable<TestCaseData> RowTestCases()
                {
                    var expectedList1x0 = new List2D<byte>();
                    expectedList1x0.IncreaseBounds(1, 0);

                    yield return new TestCaseData(
                        List2D<string>.FromSystem2DArray(
                            new string[,] { { "2", "3", "5" },
                                        { "4", "9", "1" } }),
                        List2D<string>.FromSystem2DArray(
                            new string[,] { { "2", "3", "5" },
                                        { "4", "9", "1" },
                                        { default, default, default }}));
                    yield return new TestCaseData(new List2D<byte>(), expectedList1x0);
                }
            }

            public static class Columns
            {
                [TestCaseSource(typeof(Add), nameof(InvalidCountTestCases))]
                public static void ThrowsExceptionIfCountIsInvalid<T>(List2D<T> list, int count)
                    => Assert.Throws<ArgumentOutOfRangeException>(
                        () => list.AddColumns(count));

                [TestCaseSource(typeof(Columns), nameof(ColumnsTestCases))]
                public static void Test<T>(List2D<T> list, int count, List2D<T> expected)
                {
                    list.AddColumns(count);
                    Assert.AreEqual(expected.Boundaries, list.Boundaries,
                        $"Expected: {expected.Boundaries.ToValueTuple()}\n" +
                        $"But was:  {list.Boundaries.ToValueTuple()}");
                    Assert.That(list.Capacity.Rows >= list.Rows);
                    Assert.That(list.Capacity.Columns >= list.Columns);
                    CollectionAssert.AreEqual(expected, list);
                }

                [TestCaseSource(typeof(Columns), nameof(ColumnsTestCases))]
                public static void TestNoAlloc<T>(List2D<T> list, int count, List2D<T> expected)
                {
                    list.IncreaseCapacity(expected.Capacity);
                    Bounds2D expectedCapacity = list.Capacity;

                    list.AddColumns(count);
                    Assert.AreEqual(expected.Boundaries, list.Boundaries);
                    Assert.AreEqual(expectedCapacity, list.Capacity);
                    CollectionAssert.AreEqual(expected, list);
                }

                [TestCaseSource(typeof(Columns), nameof(ColumnTestCases))]
                public static void Test<T>(List2D<T> list, List2D<T> expected)
                {
                    list.AddColumn();
                    Assert.AreEqual(expected.Boundaries, list.Boundaries,
                        $"Expected: {expected.Boundaries.ToValueTuple()}\n" +
                        $"But was:  {list.Boundaries.ToValueTuple()}");
                    Assert.That(list.Capacity.Rows >= list.Rows);
                    Assert.That(list.Capacity.Columns >= list.Columns);
                    CollectionAssert.AreEqual(expected, list);
                }

                [TestCaseSource(typeof(Columns), nameof(ColumnTestCases))]
                public static void TestNoAlloc<T>(List2D<T> list, List2D<T> expected)
                {
                    list.IncreaseCapacity(expected.Capacity);
                    Bounds2D expectedCapacity = list.Capacity;

                    list.AddColumn();
                    Assert.AreEqual(expected.Boundaries, list.Boundaries);
                    Assert.AreEqual(expectedCapacity, list.Capacity);
                    CollectionAssert.AreEqual(expected, list);
                }

                private static IEnumerable<TestCaseData> ColumnsTestCases()
                {
                    var expectedList0x2 = new List2D<byte>(0, 2);
                    expectedList0x2.IncreaseBounds(0, 2);

                    yield return new TestCaseData(
                        List2D<string>.FromSystem2DArray(
                            new string[,] { { "2", "3", "5" },
                                        { "4", "9", "1" } }),
                        2,
                        List2D<string>.FromSystem2DArray(
                            new string[,] { { "2", "3", "5", default, default },
                                        { "4", "9", "1", default, default }}));
                    yield return new TestCaseData(new List2D<byte>(), 2, expectedList0x2);
                    yield return new TestCaseData(new List2D<byte>(), 0, new List2D<byte>());
                }

                private static IEnumerable<TestCaseData> ColumnTestCases()
                {
                    var expectedList0x1 = new List2D<byte>();
                    expectedList0x1.IncreaseBounds(0, 1);

                    yield return new TestCaseData(
                        List2D<string>.FromSystem2DArray(
                            new string[,] { { "2", "3", "5" },
                                        { "4", "9", "1" } }),
                        List2D<string>.FromSystem2DArray(
                            new string[,] { { "2", "3", "5", default },
                                        { "4", "9", "1", default }}));
                    yield return new TestCaseData(new List2D<byte>(), expectedList0x1);
                }
            }

            private static IEnumerable<TestCaseData> InvalidCountTestCases()
            {
                yield return new TestCaseData(CreateList2DWithBounds<byte>(0, 0), -1);
                yield return new TestCaseData(CreateList2DWithBounds<byte>(3, 3), -2);
            }
        }
    }
}
