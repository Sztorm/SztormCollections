using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public class Bounds2DTests
    {
        [TestCase(0, -1)]
        [TestCase(-1, 0)]
        [TestCase(-4, -3)]
        public static void 
            ConstructorThrowsExceptionIfArgumentsAreLessThanZero(int rows, int columns)
        {
            TestDelegate testMethod = () => new Bounds2D(rows, columns);

            Assert.Throws<ArgumentOutOfRangeException>(testMethod);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public static void TestRows(int rows)
        {
            Bounds2D bounds = new Bounds2D(rows, 1);

            Assert.AreEqual(bounds.Rows, rows);
            Assert.AreEqual(bounds.Length1, rows);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public static void TestColumns(int columns)
        {
            Bounds2D bounds = new Bounds2D(1, columns);

            Assert.AreEqual(bounds.Columns, columns);
            Assert.AreEqual(bounds.Length2, columns);
        }

        [TestCaseSource(nameof(IsValidIndexTrueTestCases))]
        public static void IsValidIndexReturnTrue(Bounds2D bounds, Index2D index)
            => Assert.True(bounds.IsValidIndex(index));

        [TestCaseSource(nameof(IsValidIndexFalseTestCases))]
        public static void IsValidIndexReturnFalse(Bounds2D bounds, Index2D index)
            => Assert.False(bounds.IsValidIndex(index));

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(5, 8)]
        public static void TestConversionToValueTuple(int rows, int columns)
        {
            var bounds = new Bounds2D(rows, columns);
            (int Rows, int Columns) tuple = bounds.ToValueTuple();

            Assert.AreEqual(bounds.Rows, tuple.Rows);
            Assert.AreEqual(bounds.Columns, tuple.Columns);
        }

        private static IEnumerable<TestCaseData> IsValidIndexTrueTestCases()
        {
            yield return new TestCaseData(
                new Bounds2D(2, 1),
                new Index2D(1, 0));
            yield return new TestCaseData(
                new Bounds2D(1, 1),
                new Index2D(0, 0));
        }

        private static IEnumerable<TestCaseData> IsValidIndexFalseTestCases()
        {
            yield return new TestCaseData(
                new Bounds2D(2, 1),
                new Index2D(1, 1));
            yield return new TestCaseData(
                new Bounds2D(2, 1),
                new Index2D(2, 0));
            yield return new TestCaseData(
                new Bounds2D(2, 1),
                new Index2D(-1, 0));
            yield return new TestCaseData(
                new Bounds2D(2, 1),
                new Index2D(0, -1));
            yield return new TestCaseData(
                new Bounds2D(0, 0),
                new Index2D(0, 0));
            yield return new TestCaseData(
                new Bounds2D(1, 0),
                new Index2D(0, 0));
            yield return new TestCaseData(
                new Bounds2D(0, 1),
                new Index2D(0, 0));
        }
    }
}
