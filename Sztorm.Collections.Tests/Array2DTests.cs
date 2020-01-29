using System;
using System.Linq;
using System.Collections.Generic;
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
            var before = Incremented2DArray(rows, columns);
            var array = new Array2D<int>(rows, columns);
            var after = new int[rows, columns];

            before.CopyTo(array);
            array.CopyTo(after, 0, 0);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write(array[i, j] + ", ");
                    Assert.AreEqual(after[i, j], before[i, j]);
                }
                Console.WriteLine();
            }
        }

        private static int[,] Incremented2DArray(int rows, int columns)
        {
            var result = new int[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[i, j] = i * columns + j;
                }
            }
            return result;
        }
    }
}
