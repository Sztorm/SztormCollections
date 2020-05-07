using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    using static TestUtils;

    [TestFixture]
    partial class List2DTests
    {
        public static class Clear
        {
            [TestCaseSource(typeof(List2DTests), nameof(ListsOfValues))]
            [TestCaseSource(typeof(List2DTests), nameof(ListsOfReferences))]
            public static void Test<T>(List2D<T> list)
            {
                list.Clear();
                Assert.AreEqual(list.Boundaries, new Bounds2D(0, 0));
            }

            [TestCaseSource(typeof(List2DTests), nameof(ListsOfReferences))]
            public static void ReleasesReferences<T>(List2D<T> list) where T : class
            {
                list.Clear();
                CheckUnusedReferences(list, (0, 0), list.Capacity);
            }
        }
    }
}
