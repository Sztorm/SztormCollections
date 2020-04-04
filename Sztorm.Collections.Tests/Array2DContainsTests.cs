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
        [TestCaseSource(typeof(Array2DTests), nameof(ContainsTestCases))]
        [TestCaseSource(typeof(Array2DTests), nameof(ContainsEquatableTestCases))]
        [TestCaseSource(typeof(Array2DTests), nameof(ContainsComparableTestCases))]
        public static void TestContains<T>(Array2D<T> array, T value, bool expected)
            => Assert.AreEqual(expected, array.Contains(value));

        [TestCaseSource(typeof(Array2DTests), nameof(ContainsEquatableTestCases))]
        public static void TestContainsEquatable<T>(Array2D<T> array, T value, bool expected)
            where T : IEquatable<T>
            => Assert.AreEqual(expected, array.ContainsEquatable(value));

        [TestCaseSource(typeof(Array2DTests), nameof(ContainsComparableTestCases))]
        public static void TestContainsComparable<T>(Array2D<T> array, T value, bool expected)
            where T : IComparable<T>
            => Assert.AreEqual(expected, array.ContainsComparable(value));

        private static IEnumerable<TestCaseData> ContainsTestCases()
        {
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3, 5 },                                               
                                    { 4, 9, 1 } }),  
                9,                                     
                true);
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },                      
                                    { 4, 9 },                    
                                    { 3, 6 } }),                    
                3,                      
                true);
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },                    
                                    { 4, 9 },                     
                                    { 3, 6 } }),                     
                8,                       
                false);
            yield return new TestCaseData(
                Array2D<object>.FromSystem2DArray(
                    new object[,] { { 2, 3 },                    
                                    { 4, 9 },                    
                                    { 3, 6 } }),                      
                7,                       
                false);
        }

        private static IEnumerable<TestCaseData> ContainsEquatableTestCases()
        {
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3", "5" },
                                    { "4", "9", "1" } }),
                    "9",
                    true);
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                    "3",
                    true);
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                    "8",
                    false);
            yield return new TestCaseData(
                Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "4", "9" },
                                    { "3", "6" } }),
                    "7",
                    false);
        }

        private static IEnumerable<TestCaseData> ContainsComparableTestCases()
        {
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 } }),
                    9,
                    true);
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                    3,
                    true);
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                    8,
                    false);
            yield return new TestCaseData(
                Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3 },
                                 { 4, 9 },
                                 { 3, 6 } }),
                    7,
                    false);
        }
    }
}
