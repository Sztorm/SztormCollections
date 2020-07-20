using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    using static TestUtils;

    [TestFixture]
    public partial class Array2DTests
    {
        public static class ForEach
        {
            public static class Action
            {
                [Test]
                public static void ThrowsExceptionIfActionIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Array2D<byte>(0, 0).ForEach(null));

                [TestCaseSource(typeof(ForEach), nameof(SumTestCases))]
                public static int TestSum(Array2D<int> array)
                {
                    int sum = 0;

                    array.ForEach(o => sum += o);
                    return sum;
                }

                [TestCaseSource(typeof(ForEach), nameof(ConcatStringsTestCases))]
                public static string TestConcatStrings(Array2D<string> array)
                {
                    var sb = new StringBuilder(256);

                    array.ForEach(o => sb.Append(o));
                    return sb.ToString();
                }
            }

            public static class IAction
            {
                [TestCaseSource(typeof(ForEach), nameof(SumTestCases))]
                public static int TestSum(Array2D<int> array)
                {
                    int sum = 0;

                    array.ForEach(new BoxedAction<int>(o => sum += o));
                    return sum;
                }

                [TestCaseSource(typeof(ForEach), nameof(ConcatStringsTestCases))]
                public static string TestConcatStrings(Array2D<string> array)
                {
                    var sb = new StringBuilder(256);

                    array.ForEach(new BoxedAction<string>(o => sb.Append(o)));
                    return sb.ToString();
                }
            }

            public static class IActionRef
            {
                [TestCaseSource(typeof(ForEach), nameof(SumTestCases))]
                public static int TestSum(Array2D<int> array)
                {
                    var sumAction = new SumIntAction();

                    array.ForEach(ref sumAction);
                    return sumAction.Sum;
                }

                [TestCaseSource(typeof(ForEach), nameof(ConcatStringsTestCases))]
                public static string TestConcatStrings(Array2D<string> array)
                {
                    var concatAction = new ConcatStringsAction(256);

                    array.ForEach(ref concatAction);
                    return concatAction.Result;
                }
            }

            private static IEnumerable<TestCaseData> SumTestCases()
            {
                yield return new TestCaseData(Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 }})).Returns(24);
                yield return new TestCaseData(Array2D<int>.FromSystem2DArray(
                    new int[,] { { 1, 3 },
                                 { 3, 7 }})).Returns(14);
                yield return new TestCaseData(new Array2D<int>(0, 0)).Returns(0);
            }

            private static IEnumerable<TestCaseData> ConcatStringsTestCases()
            {
                yield return new TestCaseData(Array2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "5", "4" },
                                    { "9", "1" }})).Returns("235491");
                yield return new TestCaseData(Array2D<string>.FromSystem2DArray(
                    new string[,] { { "Hello", " ", "World", "! " },
                                    { "This", "Need", "4", "Strings" }}))
                    .Returns("Hello World! ThisNeed4Strings");
                yield return new TestCaseData(new Array2D<string>(0, 0)).Returns("");
            }
        }
    }
}
