using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public partial class Array2DTests
    {
        public static class Enumerator
        {
            [TestCaseSource(typeof(Enumerator), nameof(MoveNextTestCases))]
            public static bool TestMoveNext<T>(Array2D<T> array, IEnumerable<T> expected)
            {
                IEnumerator<T> arrayEnumerator = array.GetEnumerator();
                IEnumerator<T> expectedEnumerator = expected.GetEnumerator();

                while (arrayEnumerator.MoveNext())
                {
                    if (!expectedEnumerator.MoveNext() ||
                        !arrayEnumerator.Current.Equals(expectedEnumerator.Current))
                    {
                        return false;
                    }
                }
                if (expectedEnumerator.MoveNext())
                {
                    return false;
                }
                return true;
            }

            private static IEnumerable<TestCaseData> MoveNextTestCases()
            {
                var array2x3Int = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 }});
                var array3x2String = Array2D<string>.FromSystem2DArray(
                   new string[,] { { "2", "3" },
                                   { "5", "4" },
                                   { "9", "1" }});

                yield return new TestCaseData(array2x3Int, new int[] { 2, 3, 5, 4, 9, 1 })
                    .Returns(true);
                yield return new TestCaseData(
                    array3x2String, new string[] { "2", "3", "5", "4", "9", "1" })
                    .Returns(true);
                yield return new TestCaseData(new Array2D<byte>(0, 0), new byte[0])
                    .Returns(true);
                yield return new TestCaseData(array2x3Int, new int[] { 2, 4, 3, 9, 5, 1 })
                    .Returns(false);
                yield return new TestCaseData(array2x3Int, new int[] { 2, 3, 5 })
                    .Returns(false);
            }
        }     
    }
}
