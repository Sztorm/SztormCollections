using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;
using Sztorm.Collections.Extensions;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public partial class ReadOnlyRectangularCollectionTests
    {
        public static class GetColumn
        {
            [TestCaseSource(typeof(GetColumn), nameof(GetColumnInvalidTestCases))]
            public static void GetColumnThrowExceptionIfIndexExceedsColumns(
                ReadOnlyMatrix4x4 matrix, int index)
                => Assert.Throws<ArgumentOutOfRangeException>(
                    () => matrix.GetColumn<float, ReadOnlyMatrix4x4>(index));

            [TestCaseSource(typeof(GetColumn), nameof(GetColumnTestCases))]
            public static void GetColumnTest(
                ReadOnlyMatrix4x4 matrix,
                int index,
                ReadOnlyColumn<float, ReadOnlyMatrix4x4> expected)
                => CollectionAssert.AreEqual(
                    expected, matrix.GetColumn<float, ReadOnlyMatrix4x4>(index));

            private static IEnumerable<TestCaseData> GetColumnInvalidTestCases()
            {
                yield return new TestCaseData(new ReadOnlyMatrix4x4(), -1);
                yield return new TestCaseData(new ReadOnlyMatrix4x4(), 4);
            }

            private static IEnumerable<TestCaseData> GetColumnTestCases()
            {
                var matrix = ReadOnlyMatrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                var firstColumn = new ReadOnlyColumn<float, ReadOnlyMatrix4x4>(
                    ReadOnlyMatrix4x4.FromSystem2DArray(
                        new float[,] { { 1, 0, 0, 0 },
                                       { 9, 0, 0, 0 },
                                       { 3, 0, 0, 0 },
                                       { 5, 0, 0, 0 } }), 0);

                var lastColumn = new ReadOnlyColumn<float, ReadOnlyMatrix4x4>(
                    ReadOnlyMatrix4x4.FromSystem2DArray(
                        new float[,] { { 4, 0, 0, 0 },
                                       { 6, 0, 0, 0 },
                                       { 24, 0, 0, 0 },
                                       { 625, 0, 0, 0 } }), 0);

                yield return new TestCaseData(matrix, 0, firstColumn);
                yield return new TestCaseData(matrix, 3, lastColumn);
            }
        }      
    }
}
