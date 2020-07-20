using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    public partial class Array2DTests
    {
        public static class ConvertAll
        {
            [Test]
            public static void ThrowsExceptionIfConverterIsNull()
                => Assert.Throws<ArgumentNullException>(
                    () => new Array2D<int>(0, 0).ConvertAll<object>(converter: null));

            [TestCaseSource(typeof(ConvertAll), nameof(ConverterTestCases))]
            public static void Test<TIn, TOut>(
                Array2D<TIn> array, Converter<TIn, TOut> converter, Array2D<TOut> expected)
                => CollectionAssert.AreEqual(expected, array.ConvertAll(converter));

            [TestCaseSource(typeof(ConvertAll), nameof(IConverterTestCases))]
            public static void Test<TIn, TOut, TConverter>(
                Array2D<TIn> array, TConverter converter, Array2D<TOut> expected)
                where TConverter : struct, IConverter<TIn, TOut>
                => CollectionAssert.AreEqual(
                    expected, array.ConvertAll<TOut, TConverter>(converter));

            private static IEnumerable<TestCaseData> ConverterTestCases()
            {
                var array3x3Int = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 0, 0 },
                                 { 0, 2, 3 } });

                yield return new TestCaseData(
                    array3x3Int,
                    new Converter<int, bool>(o => o != 0),
                    Array2D<bool>.FromSystem2DArray(
                        new bool[,] { { true, true, true },
                                      { true, false, false },
                                      { false, true, true } }));
                yield return new TestCaseData(
                    array3x3Int,
                    new Converter<int, string>(o => o.ToString()),
                    Array2D<string>.FromSystem2DArray(
                        new string[,] { { "2", "3", "5" },
                                        { "4", "0", "0" },
                                        { "0", "2", "3" } }));
            }

            private static IEnumerable<TestCaseData> IConverterTestCases()
            {
                var array3x3Int = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 0, 0 },
                                 { 0, 2, 3 } });

                yield return new TestCaseData(
                    array3x3Int,
                    new IntToBoolConverter(),
                    Array2D<bool>.FromSystem2DArray(
                        new bool[,] { { true, true, true },
                                      { true, false, false },
                                      { false, true, true } }));
                yield return new TestCaseData(
                    array3x3Int,
                    new ToStringConverter<int>(),
                    Array2D<string>.FromSystem2DArray(
                        new string[,] { { "2", "3", "5" },
                                        { "4", "0", "0" },
                                        { "0", "2", "3" } }));
            }

            private struct IntToBoolConverter : IConverter<int, bool>
            {
                public bool Invoke(int input) => input != 0;
            }
        } 
    }
}
