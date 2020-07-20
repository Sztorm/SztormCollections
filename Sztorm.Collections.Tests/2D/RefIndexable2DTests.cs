using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public class RefIndexable2DTests
    {
        public static void TestIndexer<T, TRefIndexable>(
            TRefIndexable indexable, Index2D index, T expected)
            where TRefIndexable : IRefIndexable2D<T> 
            => Assert.AreEqual(expected, indexable[index]);

        public static void IsValidIndexReturnsFalseOnInvalidIndexArguments<T, TRefIndexable>(
            TRefIndexable indexable, Index2D index)
            where TRefIndexable : IRefIndexable2D<T>
            => Assert.False(indexable.IsValidIndex(index));
    }
}
