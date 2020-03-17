using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public class Index2DTests
    {
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public static void TestRow(int row)
        {
            Index2D index = new Index2D(row, 1);

            Assert.AreEqual(index.Row, row);
            Assert.AreEqual(index.Dimension1Index, row);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public static void TestColumns(int column)
        {
            Index2D index = new Index2D(1, column);

            Assert.AreEqual(index.Column, column);
            Assert.AreEqual(index.Dimension2Index, column);
        }

        [TestCase(-3, -2)]
        [TestCase(4, -1)]
        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(5, 8)]
        public static void TestConversionToValueTuple(int row, int column)
        {
            var index = new Index2D(row, column);
            (int Row, int Column) tuple = index.ToValueTuple();

            Assert.AreEqual(index.Row, tuple.Row);
            Assert.AreEqual(index.Column, tuple.Column);
        }

        [Theory]
        [TestCaseSource(nameof(EqualityTestCases))]
        public static void EqualityDefinition(Index2D index1, Index2D index2)
        {
            Assert.That(index1.Row, Is.EqualTo(index2.Row));
            Assert.That(index1.Column, Is.EqualTo(index2.Column));
        }

        [TestCaseSource(nameof(GetItemFromTestCases))]
        public static void TestGetItemFrom<T>(Index2D index, Array2D<T> array, T expected)
            => Assert.AreEqual(index.GetItemFrom(array), expected);

        [TestCaseSource(nameof(GetItemFromInvalidTestCases))]
        public static void GetItemFromThrowsExpectionIfIndexIsOutOfBoundsOfGivenArray<T>(
            Index2D index, Array2D<T> array)
        {
            TestDelegate testMethod = () => index.GetItemFrom(array);

            Assert.Throws<IndexOutOfRangeException>(testMethod);
        }

        [TestCaseSource(nameof(TryGetValueFromTestCases))]
        public static void TestTryGetValueFrom<T>(Index2D index, Array2D<T> array, T? expected) 
            where T : struct
            => Assert.AreEqual(index.TryGetValueFrom(array), expected);

        [TestCaseSource(nameof(TryGetRefFromTestCases))]
        public static void TestTryGetRefFrom<T>(Index2D index, Array2D<T> array, T expected) 
            where T : class
            => Assert.AreEqual(index.TryGetRefFrom(array), expected);

        private static IEnumerable<TestCaseData> EqualityTestCases()
        {
            yield return new TestCaseData(
                new Index2D(1, 1),
                new Index2D(1, 1));
            yield return new TestCaseData(
                new Index2D(0, 0),
                new Index2D(0, 0));
            yield return new TestCaseData(
                new Index2D(-1, 1),
                new Index2D(-1, 1));
        }

        private static IEnumerable<TestCaseData> GetItemFromTestCases()
        {
            yield return new TestCaseData(
                new Index2D(1, 0),
                Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 4 },
                                                            { 5, 9, 1 }}),
                5);
            yield return new TestCaseData(
                new Index2D(1, 1),
                Array2D<int>.FromSystem2DArray(new int[,] { { 9, 8 },
                                                            { 2, 4 }}),
                4);
            yield return new TestCaseData(
                new Index2D(1, 1),
                Array2D<string>.FromSystem2DArray(new string[,] { { "2", "3", "4" },
                                                                  { "5", "9", "1" }}),
                "9");
            yield return new TestCaseData(
                new Index2D(1, 1),
                Array2D<string>.FromSystem2DArray(new string[,] { { "9", "8" },
                                                                  { "2", "4" }}),
                "4");
        }

        private static IEnumerable<TestCaseData> GetItemFromInvalidTestCases()
        {
            yield return new TestCaseData(
                new Index2D(2, 2),
                new Array2D<int>(2, 3));
            yield return new TestCaseData(
                new Index2D(1, 3),
                new Array2D<int>(2, 3));
            yield return new TestCaseData(
                new Index2D(-1, 0),
                new Array2D<int>(2, 3));
            yield return new TestCaseData(
                new Index2D(0, -1),
                new Array2D<int>(2, 3));
            yield return new TestCaseData(
                new Index2D(2, 2),
                new Array2D<string>(2, 3));
            yield return new TestCaseData(
                new Index2D(1, 3),
                new Array2D<string>(2, 3));
            yield return new TestCaseData(
                new Index2D(-1, 0),
                new Array2D<string>(2, 3));
            yield return new TestCaseData(
                new Index2D(0, -1),
                new Array2D<string>(2, 3));
        }

        private static IEnumerable<TestCaseData> TryGetValueFromTestCases()
        {
            yield return new TestCaseData(
                new Index2D(1, 0),
                Array2D<int>.FromSystem2DArray(new int[,] { { 2, 3, 4 },
                                                            { 5, 9, 1 }}),
                5);
            yield return new TestCaseData(
                new Index2D(1, 1),
                Array2D<int>.FromSystem2DArray(new int[,] { { 9, 8 },
                                                            { 2, 4 }}),
                4);
            yield return new TestCaseData(
                new Index2D(2, 2),
                new Array2D<int>(2, 3),
                new int?());
            yield return new TestCaseData(
                new Index2D(1, 3),
                new Array2D<int>(2, 3),
                new int?());
            yield return new TestCaseData(
                new Index2D(-1, 0),
                new Array2D<int>(2, 3),
                new int?());
            yield return new TestCaseData(
                new Index2D(0, -1),
                new Array2D<int>(2, 3),
                new int?());
        }

        private static IEnumerable<TestCaseData> TryGetRefFromTestCases()
        {
            yield return new TestCaseData(
                new Index2D(1, 1),
                Array2D<string>.FromSystem2DArray(new string[,] { { "2", "3", "4" },
                                                                  { "5", "9", "1" }}),
                "9");
            yield return new TestCaseData(
                new Index2D(1, 1),
                Array2D<string>.FromSystem2DArray(new string[,] { { "9", "8" },
                                                                  { "2", "4" }}),
                "4");
            yield return new TestCaseData(
                new Index2D(2, 2),
                new Array2D<string>(2, 3),
                null);
            yield return new TestCaseData(
                new Index2D(1, 3),
                new Array2D<string>(2, 3),
                null);
            yield return new TestCaseData(
                new Index2D(-1, 0),
                new Array2D<string>(2, 3),
                null);
            yield return new TestCaseData(
                new Index2D(0, -1),
                new Array2D<string>(2, 3),
                null);
        }
    }
}
