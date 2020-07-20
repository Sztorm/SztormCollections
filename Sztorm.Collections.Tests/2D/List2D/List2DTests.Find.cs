using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    public partial class List2DTests
    {
        public static class Find
        {
            public static class First
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new List2D<int>(0, 0).Find(match: null));

                [TestCaseSource(typeof(Find), nameof(FirstTestCases))]
                public static ItemRequestResult<T> Test<T>(List2D<T> list, Predicate<T> match)
                    => list.Find(match);

                [TestCaseSource(typeof(Find), nameof(FirstIPredicateTestCases))]
                public static ItemRequestResult<T> Test<T, TPredicate>(
                    List2D<T> list, TPredicate match)
                    where TPredicate : struct, IPredicate<T>
                    => list.Find(match);
            }

            public static class Last
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new List2D<int>(0, 0).FindLast(match: null));

                [TestCaseSource(typeof(Find), nameof(LastTestCases))]
                public static ItemRequestResult<T> Test<T>(List2D<T> list, Predicate<T> match)
                    => list.FindLast(match);

                [TestCaseSource(typeof(Find), nameof(LastIPredicateTestCases))]
                public static ItemRequestResult<T> Test<T, TPredicate>(
                    List2D<T> list, TPredicate match)
                    where TPredicate : struct, IPredicate<T>
                    => list.FindLast(match);
            }

            private static IEnumerable<TestCaseData> FirstTestCases()
            {
                var list3x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });
                list3x3.IncreaseCapacity(list3x3.Boundaries);

                yield return new TestCaseData(list3x3, new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<int>(9));
                yield return new TestCaseData(list3x3, new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<int>.Fail);
            }

            private static IEnumerable<TestCaseData> FirstIPredicateTestCases()
            {
                var list3x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });
                list3x3.IncreaseCapacity(list3x3.Boundaries);

                yield return new TestCaseData(list3x3, new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<int>(9));
                yield return new TestCaseData(list3x3, new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<int>.Fail);
            }

            private static IEnumerable<TestCaseData> LastTestCases()
            {
                var list3x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });
                list3x3.IncreaseCapacity(list3x3.Boundaries);

                yield return new TestCaseData(list3x3, new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<int>(8));
                yield return new TestCaseData(list3x3, new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<int>.Fail);
            }

            private static IEnumerable<TestCaseData> LastIPredicateTestCases()
            {
                var list3x3 = List2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });
                list3x3.IncreaseCapacity(list3x3.Boundaries);

                yield return new TestCaseData(list3x3, new GreaterThanPredicate<int>(8))
                    .Returns(new ItemRequestResult<int>(9));
                yield return new TestCaseData(list3x3, new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<int>.Fail);
            }
        }
    }
}
