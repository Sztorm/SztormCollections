﻿using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;
using Sztorm.Collections.Extensions;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public partial class RefReadOnlyRectangularCollectionTests
    {
        public static class GetRow
        {
            [TestCaseSource(typeof(GetRow), nameof(GetRowInvalidTestCases))]
            public static void GetRowThrowExceptionIfIndexExceedsColumns(
                RefReadOnlyMatrix4x4 matrix, int index)
                => Assert.Throws<ArgumentOutOfRangeException>(
                    () => matrix.GetRow<float, RefReadOnlyMatrix4x4>(index));

            [TestCaseSource(typeof(GetRow), nameof(GetRowTestCases))]
            public static void GetRowTest(
                RefReadOnlyMatrix4x4 matrix,
                int index,
                RefReadOnlyRow<float, RefReadOnlyMatrix4x4> expected)
                => CollectionAssert.AreEqual(
                    expected, matrix.GetRow<float, RefReadOnlyMatrix4x4>(index));

            private static IEnumerable<TestCaseData> GetRowInvalidTestCases()
            {
                yield return new TestCaseData(new RefReadOnlyMatrix4x4(), -1);
                yield return new TestCaseData(new RefReadOnlyMatrix4x4(), 4);
            }

            private static IEnumerable<TestCaseData> GetRowTestCases()
            {
                var matrix = RefReadOnlyMatrix4x4.FromSystem2DArray(
                    new float[,] { { 1, 2, 3, 4 },
                                   { 9, 8, 7, 6 },
                                   { 3, 6, 12, 24 },
                                   { 5, 25, 125, 625 } });

                var firstRow = new RefReadOnlyRow<float, RefReadOnlyMatrix4x4>(
                    RefReadOnlyMatrix4x4.FromSystem2DArray(
                        new float[,] { { 1, 2, 3, 4 },
                                       { 0, 0, 0, 0 },
                                       { 0, 0, 0, 0 },
                                       { 0, 0, 0, 0 } }), 0);

                var lastRow = new RefReadOnlyRow<float, RefReadOnlyMatrix4x4>(
                    RefReadOnlyMatrix4x4.FromSystem2DArray(
                        new float[,] { { 5, 25, 125, 625 },
                                       { 0, 0, 0, 0 },
                                       { 0, 0, 0, 0 },
                                       { 0, 0, 0, 0 } }), 0);

                yield return new TestCaseData(matrix, 0, firstRow);
                yield return new TestCaseData(matrix, 3, lastRow);
            }
        }      
    }
}
