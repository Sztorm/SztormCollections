using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public class RefIndexableTests
    {
        public static void TestIndexer<T, TRefIndexable>(
            TRefIndexable indexable, int index, T expected)
            where TRefIndexable : IRefIndexable<T> 
            => Assert.AreEqual(expected, indexable[index]);

        public static void IsValidIndexReturnsFalseOnInvalidIndexArguments<T, TRefIndexable>(
            TRefIndexable indexable, int index)
            where TRefIndexable : IRefIndexable<T>
            => Assert.False(indexable.IsValidIndex(index));
    }
}
