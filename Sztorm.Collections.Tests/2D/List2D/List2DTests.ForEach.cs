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
    public partial class List2DTests
    {
        public static class ForEach
        {
            public static class Action
            {
                [Test]
                public static void ThrowsExceptionIfActionIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new List2D<byte>().ForEach(null));

                [Test]
                public static void ThrowsExceptionUponListModification()
                {
                    var list = new List2D<byte>(1, 1);
                    list.AddRow();
                    list.AddColumn();

                    Assert.Throws<InvalidOperationException>(
                        () => list.ForEach(o => list.AddColumn()));
                }

                [TestCaseSource(typeof(ForEach), nameof(SumTestCases))]
                public static int TestSum(List2D<int> list)
                {
                    int sum = 0;

                    list.ForEach(o => sum += o);

                    return sum;
                }

                [TestCaseSource(typeof(ForEach), nameof(ConcatStringsTestCases))]
                public static string TestConcatStrings(List2D<string> list)
                {
                    var sb = new StringBuilder(256);

                    list.ForEach(o => sb.Append(o));

                    return sb.ToString();
                }
            }

            public static class IAction
            {
                [Test]
                public static void ThrowsExceptionUponListModification()
                {
                    var list = new List2D<byte>(1, 1);
                    list.AddRow();
                    list.AddColumn();

                    Assert.Throws<InvalidOperationException>(
                        () => list.ForEach(new BoxedAction<byte>(o => list.AddColumn())));
                }

                [TestCaseSource(typeof(ForEach), nameof(SumTestCases))]
                public static int TestSum(List2D<int> list)
                {
                    int sum = 0;

                    list.ForEach(new BoxedAction<int>(o => sum += o));

                    return sum;
                }

                [TestCaseSource(typeof(ForEach), nameof(ConcatStringsTestCases))]
                public static string TestConcatStrings(List2D<string> list)
                {
                    var sb = new StringBuilder(256);

                    list.ForEach(new BoxedAction<string>(o => sb.Append(o)));

                    return sb.ToString();
                }
            }

            public static class IActionRef
            {
                [Test]
                public static void ThrowsExceptionUponListModification()
                {
                    var list = new List2D<byte>(1, 1);
                    list.AddRow();
                    list.AddColumn();

                    var modifyListAction = new BoxedAction<byte>(o => list.AddColumn());

                    Assert.Throws<InvalidOperationException>(
                        () => list.ForEach(ref modifyListAction));
                }

                [TestCaseSource(typeof(ForEach), nameof(SumTestCases))]
                public static int TestSum(List2D<int> list)
                {
                    var sumAction = new SumIntAction();

                    list.ForEach(ref sumAction);

                    return sumAction.Sum;
                }

                [TestCaseSource(typeof(ForEach), nameof(ConcatStringsTestCases))]
                public static string TestConcatStrings(List2D<string> list)
                {
                    var concatAction = new ConcatStringsAction(256);

                    list.ForEach(ref concatAction);

                    return concatAction.Result;
                }
            }

            private static IEnumerable<TestCaseData> SumTestCases()
            {
                var list2x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 }});
                list2x3.IncreaseCapacity(list2x3.Boundaries);

                var list2x2 = List2D<int>.FromSystem2DArray(
                   new int[,] { { 1, 3 },
                                { 3, 7 }});
                list2x2.IncreaseCapacity(list2x2.Boundaries);

                var list0x0 = new List2D<int>();
                list0x0.IncreaseCapacity(list2x3.Boundaries);

                yield return new TestCaseData(list2x3).Returns(24);
                yield return new TestCaseData(list2x2).Returns(14);
                yield return new TestCaseData(list0x0).Returns(0);
            }

            private static IEnumerable<TestCaseData> ConcatStringsTestCases()
            {
                var list3x2 = List2D<string>.FromSystem2DArray(
                    new string[,] { { "2", "3" },
                                    { "5", "4" },
                                    { "9", "1" }});
                list3x2.IncreaseCapacity(list3x2.Boundaries);

                var list2x4 = List2D<string>.FromSystem2DArray(
                    new string[,] { { "Hello", " ", "World", "! " },
                                    { "This", "Need", "4", "Strings" }});
                list2x4.IncreaseCapacity(list2x4.Boundaries);

                var list0x0 = new List2D<string>();
                list0x0.IncreaseCapacity(list2x4.Boundaries);

                yield return new TestCaseData(list3x2).Returns("235491");
                yield return new TestCaseData(list2x4).Returns("Hello World! ThisNeed4Strings");
                yield return new TestCaseData(list0x0).Returns("");
            }
        }
    }
}
