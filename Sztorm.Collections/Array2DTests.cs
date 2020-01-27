using System;
using System.Linq;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public class Array2DTests
    {
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(3, 4)]
        [TestCase(27, 99)]
        public void BasicArrayPropertiesTest(int rows, int columns)
        {
            int count = rows * columns;
            Array2D<int> array = new Array2D<int>(rows, columns);
            Assert.AreEqual(array.Rows, rows);
            Assert.AreEqual(array.Length1, rows);
            Assert.AreEqual(array.Columns, columns);
            Assert.AreEqual(array.Length2, columns);
            Assert.AreEqual(array.Count, count);
        }

        [TestCase(3, 4, 2, 1, 2)]
        public void IndicesOfTest(int rows, int columns, int rowIndex, int columnIndex, int inputValue)
        {
            Array2D<int> array = new Array2D<int>(rows, columns);
            array[rowIndex, columnIndex] = inputValue;
            Assert.AreEqual(array.IndicesOf(inputValue), new Index2D(rowIndex, columnIndex));
        }

        [TestCase(5, 4, 1, new int[] { 0, 4, 2, 0 })]
        public void ArrayRowTest(int rows, int columns, int rowIndex, int[] expected)
        {
            var array = new Array2D<int>(rows, columns);
            Array2D<int>.Row row = array.GetRow(rowIndex);

            for (int i = 0; i < row.Count; i++)
            {
                row[i] = expected[i];
            }
            Assert.IsTrue(array.GetRow(rowIndex).All(expected.Contains));
        }

        [TestCase(5, 4, 3, new int[] { 9, 8, 2, 1, 6 })]
        public void ArrayColumnTest(int rows, int columns, int columnIndex, int[] expected)
        {
            var array = new Array2D<int>(rows, columns);
            Array2D<int>.Column column = array.GetColumn(columnIndex);

            for (int i = 0; i < column.Count; i++)
            {
                column[i] = expected[i];
            }
            Assert.IsTrue(array.GetColumn(columnIndex).All(expected.Contains));
        }

        [TestCase(3, 2)]
        public void CopyToTest(int rows, int columns)
        {
            var expected = new int[,]
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            };
            int count = rows * columns;
            var array = new Array2D<int>(rows, columns);
            var actual = new int[rows, columns];

            for (int i = 0; i < array.Rows; i++)
            {
                for (int j = 0; j < array.Columns; j++)
                {
                    array[i, j] = expected[i, j];
                }
            }
            array.CopyTo(actual as int[,], 0);
            bool areElementsEqual = true;

            for (int i = 0; i < array.Rows; i++)
            {
                for (int j = 0; j < array.Columns; j++)
                {
                    areElementsEqual &= array[i, j] == expected[i, j];
                }
            }
            Assert.That(areElementsEqual);
        }

    }
}
