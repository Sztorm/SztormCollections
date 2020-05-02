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
        public static class Enumerator
        {
            [Test]
            public static void MoveNextThrowsExceptionUponListModification()
            {
                var list = new List2D<byte>(1, 1);
                list.AddRow();
                list.AddColumn();

                Assert.Throws<InvalidOperationException>(() => 
                {
                    foreach (byte item in list)
                    {
                        list.AddColumn();
                    }
                });
            }

            [TestCaseSource(typeof(Enumerator), nameof(MoveNextTestCases))]
            public static bool TestMoveNext<T>(List2D<T> list, IEnumerable<T> expected)
            {
                var listEnumerator = list.GetEnumerator();
                var expectedEnumerator = expected.GetEnumerator();

                while (listEnumerator.MoveNext())
                {
                    if (!expectedEnumerator.MoveNext() ||
                        !listEnumerator.Current.Equals(expectedEnumerator.Current))
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
                var list3x2Int = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 }});
                list3x2Int.IncreaseCapacity(list3x2Int.Boundaries);

                var list2x3String = List2D<string>.FromSystem2DArray(
                   new string[,] { { "2", "3" },
                                   { "5", "4" },
                                   { "9", "1" }});
                list2x3String.IncreaseCapacity(list2x3String.Boundaries);

                yield return new TestCaseData(list3x2Int, new int[] { 2, 3, 5, 4, 9, 1 })
                    .Returns(true);
                yield return new TestCaseData(
                    list2x3String, new string[] { "2", "3", "5", "4", "9", "1" })
                    .Returns(true);
                yield return new TestCaseData(new List2D<byte>(), new byte[0])
                    .Returns(true);
                yield return new TestCaseData(list3x2Int, new int[] { 2, 4, 3, 9, 5, 1 })
                    .Returns(false);
                yield return new TestCaseData(list3x2Int, new int[] { 2, 3, 5 })
                    .Returns(false);
            }
        }     
    }
}
