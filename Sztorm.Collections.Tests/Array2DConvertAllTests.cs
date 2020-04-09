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
        [Test]
        public static void ConvertAllThrowsExceptionIfConverterIsNull()
        {
            var array = new Array2D<int>(0, 0);
            void ConvertAllNull() => array.ConvertAll<object>(converter: null);

            Assert.Throws<ArgumentNullException>(ConvertAllNull);
        }

        [TestCaseSource(typeof(Array2DTests), nameof(ConvertAllTestCases))]
        public static void TestConvertAll<TInput, TOutput>(
            Array2D<TInput> array, Converter<TInput, TOutput> converter, Array2D<TOutput> expected)
            => CollectionAssert.AreEqual(expected, array.ConvertAll(converter));

        private static IEnumerable<TestCaseData> ConvertAllTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 0, 0 },
                                 { 0, 2, 3 } }),
                new Converter<int, bool>(o => o != 0),
                Array2D<bool>.FromSystem2DArray(
                    new bool[,] { { true, true, true },
                                  { true, false, false },
                                  { false, true, true } }));
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } }),
                new Converter<int, string>(o => o.ToString()),
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "4", "9", "1" },
                                    { "8", "2", "3" } }));
        }
    }
}
