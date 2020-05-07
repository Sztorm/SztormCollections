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
