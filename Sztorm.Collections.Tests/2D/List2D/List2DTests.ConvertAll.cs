using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    using static TestUtils;

    public partial class List2DTests
    {
        public static class ConvertAll
        {
            [Test]
            public static void ThrowsExceptionIfConverterIsNull()
                => Assert.Throws<ArgumentNullException>(
                    () => new List2D<int>(0, 0).ConvertAll<object>(converter: null));

            [TestCaseSource(typeof(ConvertAll), nameof(ConverterTestCases))]
            public static void Test<TIn, TOut>(
                List2D<TIn> list, Converter<TIn, TOut> converter, List2D<TOut> expected)
            {
                List2D<TOut> actual = list.ConvertAll(converter);
                Assert.AreEqual(expected.Boundaries, actual.Capacity,
                    "expected capacity must be minimal, so actual.Capacity must be equal to " +
                    "expected.Boundaries");
                CollectionAssert.AreEqual(expected, actual);
            }

            [TestCaseSource(typeof(ConvertAll), nameof(IConverterTestCases))]
            public static void Test<TIn, TOut, TConverter>(
                List2D<TIn> list, TConverter converter, List2D<TOut> expected)
                where TConverter : struct, IConverter<TIn, TOut>
            {
                List2D<TOut> actual = list.ConvertAll<TOut, TConverter>(converter);
                Assert.AreEqual(expected.Boundaries, actual.Capacity,
                    "expected capacity must be minimal, so actual.Capacity must be equal to " +
                    "expected.Boundaries");
                CollectionAssert.AreEqual(expected, actual);
            }

            private static IEnumerable<TestCaseData> ConverterTestCases()
            {
                var list3x3Int = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 0, 0 },
                                 { 0, 2, 3 } });
                list3x3Int.IncreaseCapacity(list3x3Int.Boundaries);

                yield return new TestCaseData(
                    list3x3Int,
                    new Converter<int, bool>(o => o != 0),
                    List2D<bool>.FromSystem2DArray(
                        new bool[,] { { true, true, true },
                                      { true, false, false },
                                      { false, true, true } }));
                yield return new TestCaseData(
                    list3x3Int,
                    new Converter<int, string>(o => o.ToString()),
                    List2D<string>.FromSystem2DArray(
                        new string[,] { { "2", "3", "5" },
                                        { "4", "0", "0" },
                                        { "0", "2", "3" } }));
            }

            private static IEnumerable<TestCaseData> IConverterTestCases()
            {
                var list3x3Int = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 0, 0 },
                                 { 0, 2, 3 } });
                list3x3Int.IncreaseCapacity(list3x3Int.Boundaries);

                yield return new TestCaseData(
                    list3x3Int,
                    new IntToBoolConverter(),
                    List2D<bool>.FromSystem2DArray(
                        new bool[,] { { true, true, true },
                                      { true, false, false },
                                      { false, true, true } }));
                yield return new TestCaseData(
                    list3x3Int,
                    new ToStringConverter<int>(),
                    List2D<string>.FromSystem2DArray(
                        new string[,] { { "2", "3", "5" },
                                        { "4", "0", "0" },
                                        { "0", "2", "3" } }));
            }
        } 
    }
}
