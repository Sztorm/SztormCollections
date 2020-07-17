using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using NUnit.Framework;

namespace Sztorm.Collections.Tests
{
    [TestFixture]
    public class RefRowTests
    {
        public static void CountEqualsCollectionLength2<T, TRefRectangularCollection>(
            TRefRectangularCollection collection,
            RefRow<T, TRefRectangularCollection> row)
            where TRefRectangularCollection : IRefRectangularCollection<T>
            => Assert.AreEqual(collection.Boundaries.Length2, row.Count);

        public static void IndexerThrowsExceptionIfIndexIsOutOfBounds
            <T, TRefRectangularCollection>(
            RefRow<T, TRefRectangularCollection> row, int index)
            where TRefRectangularCollection : IRefRectangularCollection<T>
            => Assert.Throws<IndexOutOfRangeException>(() => { T value = row[index]; });

        public static void TestEquality<T, TRefRectangularCollection, TEnumerable>(
            RefRow<T, TRefRectangularCollection> actual, TEnumerable expected)
            where TRefRectangularCollection : IRefRectangularCollection<T>
            where TEnumerable : IEnumerable<T>
            => CollectionAssert.AreEqual(expected, actual);

        public static void TestReverse<T, TRefRectangularCollection, TEnumerable>(
            RefRow<T, TRefRectangularCollection> actual, TEnumerable expected)
            where TRefRectangularCollection : IRefRectangularCollection<T>
            where TEnumerable : IEnumerable<T>
        {
            actual.Reverse();
            CollectionAssert.AreEqual(expected, actual);
        }

        public static void TestFillWith<T, TRefRectangularCollection, TEnumerable>(
            RefRow<T, TRefRectangularCollection> actual, T value, TEnumerable expected)
            where TRefRectangularCollection : IRefRectangularCollection<T>
            where TEnumerable : IEnumerable<T>
        {
            actual.FillWith(value);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
