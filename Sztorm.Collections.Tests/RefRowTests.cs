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
            => Assert.AreEqual(collection.Length2, row.Count);

        public static void IndexerThrowsExceptionIfIndexIsOutOfBounds
            <T, TRefRectangularCollection>(
            RefRow<T, TRefRectangularCollection> row, int index)
            where TRefRectangularCollection : IRefRectangularCollection<T>
        {
            TestDelegate testMethod = () => { T value = row[index]; };

            Assert.Throws<IndexOutOfRangeException>(testMethod);
        }

        public static void TestEquality<T, TRefRectangularCollection>(
            RefRow<T, TRefRectangularCollection> actual, T[] expected)
            where TRefRectangularCollection : IRefRectangularCollection<T>
            => CollectionAssert.AreEqual(expected, actual);

        public static void TestReverse<T, TRefRectangularCollection>(
            RefRow<T, TRefRectangularCollection> actual, T[] expected)
            where TRefRectangularCollection : IRefRectangularCollection<T>
        {
            actual.Reverse();
            CollectionAssert.AreEqual(expected, actual);
        }

        public static void TestFillWith<T, TRefRectangularCollection>(
            RefRow<T, TRefRectangularCollection> actual, T value, T[] expected)
            where TRefRectangularCollection : IRefRectangularCollection<T>
        {
            actual.FillWith(value);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
