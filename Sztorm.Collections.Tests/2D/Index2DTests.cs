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
    }
}
