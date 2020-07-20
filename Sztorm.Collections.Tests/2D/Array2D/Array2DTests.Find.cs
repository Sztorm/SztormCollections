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
        public static class Find
        {
            public static class First
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Array2D<int>(0, 0).Find(match: null));

                [TestCaseSource(typeof(Find), nameof(FirstTestCases))]
                public static ItemRequestResult<T> Test<T>(Array2D<T> array, Predicate<T> match)
                    => array.Find(match);

                [TestCaseSource(typeof(Find), nameof(FirstIPredicateTestCases))]
                public static ItemRequestResult<T> Test<T, TPredicate>(
                    Array2D<T> array, TPredicate match)
                    where TPredicate : struct, IPredicate<T>
                    => array.Find(match);
            }

            public static class Last
            {
                [Test]
                public static void ThrowsExceptionIfMatchIsNull()
                    => Assert.Throws<ArgumentNullException>(
                        () => new Array2D<int>(0, 0).FindLast(match: null));

                [TestCaseSource(typeof(Find), nameof(LastTestCases))]
                public static ItemRequestResult<T> Test<T>(Array2D<T> array, Predicate<T> match)
                    => array.FindLast(match);

                [TestCaseSource(typeof(Find), nameof(LastIPredicateTestCases))]
                public static ItemRequestResult<T> Test<T, TPredicate>(
                    Array2D<T> array, TPredicate match)
                    where TPredicate : struct, IPredicate<T>
                    => array.FindLast(match);
            }

            private static IEnumerable<TestCaseData> FirstTestCases()
            {
                var array3x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });

                yield return new TestCaseData(array3x3, new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<int>(9));
                yield return new TestCaseData(array3x3, new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<int>.Fail);
            }

            private static IEnumerable<TestCaseData> FirstIPredicateTestCases()
            {
                var array3x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });

                yield return new TestCaseData(array3x3, new GreaterThanPredicate<int>(5))
                    .Returns(new ItemRequestResult<int>(9));
                yield return new TestCaseData(array3x3, new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<int>.Fail);
            }

            private static IEnumerable<TestCaseData> LastTestCases()
            {
                var array3x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });

                yield return new TestCaseData(array3x3, new Predicate<int>(o => o > 5))
                    .Returns(new ItemRequestResult<int>(8));
                yield return new TestCaseData(array3x3, new Predicate<int>(o => o == 10))
                    .Returns(ItemRequestResult<int>.Fail);
            }

            private static IEnumerable<TestCaseData> LastIPredicateTestCases()
            {
                var array3x3 = Array2D<int>.FromSystem2DArray(
                    new int[,] { { 2, 3, 5 },
                                 { 4, 9, 1 },
                                 { 8, 2, 3 } });

                yield return new TestCaseData(array3x3, new GreaterThanPredicate<int>(8))
                    .Returns(new ItemRequestResult<int>(9));
                yield return new TestCaseData(array3x3, new EqualsPredicate<int>(10))
                    .Returns(ItemRequestResult<int>.Fail);
            }
        }
    }
}
