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
            => Assert.AreEqual(collection.Length1, column.Count);

        public static void IndexerThrowsExceptionIfIndexIsOutOfBounds
            <T, TRefRectangularCollection>(
            RefColumn<T, TRefRectangularCollection> column, int index)
            where TRefRectangularCollection : IRefRectangularCollection<T>
        {
            TestDelegate testMethod = () => { T value = column[index]; };

            Assert.Throws<IndexOutOfRangeException>(testMethod);
        }

        public static void TestEquality<T, TRefRectangularCollection>(
            RefColumn<T, TRefRectangularCollection> actual, T[] expected)
            where TRefRectangularCollection : IRefRectangularCollection<T>
            => CollectionAssert.AreEqual(expected, actual);

        public static void TestReverse<T, TRefRectangularCollection>(
            RefColumn<T, TRefRectangularCollection> actual, T[] expected)
            where TRefRectangularCollection : IRefRectangularCollection<T>
        {
            actual.Reverse();
            CollectionAssert.AreEqual(expected, actual);
        }

        public static void TestFillWith<T, TRefRectangularCollection>(
            RefColumn<T, TRefRectangularCollection> actual, T value, T[] expected)
            where TRefRectangularCollection : IRefRectangularCollection<T>
        {
            actual.FillWith(value);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
