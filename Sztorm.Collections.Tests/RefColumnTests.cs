using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public class RefColumnTests
    {
        public static void CountEqualsCollectionLength1<T, TRefRectangularCollection>(
            TRefRectangularCollection collection,
            RefColumn<T, TRefRectangularCollection> column)
            where TRefRectangularCollection : IRefRectangularCollection<T>
            => Assert.AreEqual(collection.Boundaries.Length1, column.Count);

        public static void IndexerThrowsExceptionIfIndexIsOutOfBounds
            <T, TRefRectangularCollection>(
            RefColumn<T, TRefRectangularCollection> column, int index)
            where TRefRectangularCollection : IRefRectangularCollection<T>
            => Assert.Throws<IndexOutOfRangeException>(() => { T value = column[index]; });

        public static void TestEquality<T, TRefRectangularCollection, TEnumerable>(
            RefColumn<T, TRefRectangularCollection> actual, TEnumerable expected)
            where TRefRectangularCollection : IRefRectangularCollection<T>
            where TEnumerable : IEnumerable<T>
            => CollectionAssert.AreEqual(expected, actual);

        public static void TestReverse<T, TRefRectangularCollection, TEnumerable>(
            RefColumn<T, TRefRectangularCollection> actual, TEnumerable expected)
            where TRefRectangularCollection : IRefRectangularCollection<T>
            where TEnumerable : IEnumerable<T>
        {
            actual.Reverse();
            CollectionAssert.AreEqual(expected, actual);
        }

        public static void TestFillWith<T, TRefRectangularCollection, TEnumerable>(
            RefColumn<T, TRefRectangularCollection> actual, T value, TEnumerable expected)
            where TRefRectangularCollection : IRefRectangularCollection<T>
            where TEnumerable : IEnumerable<T>
        {
            actual.FillWith(value);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
