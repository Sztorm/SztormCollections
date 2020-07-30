# SztormCollections
Set of collections that are more specialized than .Net ones.
## Features:
**Array2D&lt;T&gt;**: A two-dimensional rectangular row-major ordered array that is allocated within single contiguous block of memory.

**List2D&lt;T&gt;**: A two-dimensional rectangular row-major ordered list allocated within single contiguous block of memory.

## Planned types:
**ChunkedList2D&lt;T&gt;**:A two-dimensional rectangular list of specific type allocated in chunks of memory.

**Array3D&lt;T&gt;**: A three-dimensional rectangular array that is allocated within single contiguous block of memory.

**List3D&lt;T&gt;**: A three-dimensional rectangular array that is allocated within single contiguous block of memory.

## Examples
<details>    
    <summary>2D collections</summary>
In order to make every example work, the following namespaces must be imported:

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using Sztorm.Collections;
using Sztorm.Collections.Extensions;
```

Example of setting every item in the array and writing it to console

```csharp
var array = new Array2D<int>(9, 9);
int counter = 0;

for (int i = 0; i < array.Rows; i++)
{
    for (int j = 0; j < array.Columns; j++, counter++)
    {
        array[i, j] = counter;
        Console.Write($"{array[i, j]:00}, ");
    }
    Console.WriteLine();
}
```

Output

``` 
00, 01, 02, 03, 04, 05, 06, 07, 08, 
09, 10, 11, 12, 13, 14, 15, 16, 17, 
18, 19, 20, 21, 22, 23, 24, 25, 26, 
27, 28, 29, 30, 31, 32, 33, 34, 35, 
36, 37, 38, 39, 40, 41, 42, 43, 44, 
45, 46, 47, 48, 49, 50, 51, 52, 53, 
54, 55, 56, 57, 58, 59, 60, 61, 62, 
63, 64, 65, 66, 67, 68, 69, 70, 71, 
72, 73, 74, 75, 76, 77, 78, 79, 80, 
```

You can set items in many ways. Here is an example of setting up a list

```csharp
var list = List2D<int>.FromSystem2DArray(new int[,]
{
    {  0,  1,  2,  3,  4,  5,  6,  7,  8 },
    {  9, 10, 11, 12, 13, 14, 15, 16, 17 },
    { 18, 19, 20, 21, 22, 23, 24, 25, 26 },
    { 27, 28, 29, 30, 31, 32, 33, 34, 35 },
    { 36, 37, 38, 39, 40, 41, 42, 43, 44 },
    { 45, 46, 47, 48, 49, 50, 51, 52, 53 },
    { 54, 55, 56, 57, 58, 59, 60, 61, 62 },
    { 63, 64, 65, 66, 67, 68, 69, 70, 71 },
    { 72, 73, 74, 75, 76, 77, 78, 79, 80 }
});
```

If you are interested in printing specific row, you can do it this way

```csharp
foreach (int element in array.GetRow(4))
{
    Console.Write($"{element:00}, ");
}
```

Output

```
36, 37, 38, 39, 40, 41, 42, 43, 44, 
```

Analogously for columns

```csharp
foreach (int element in array.GetColumn(2))
{
    Console.Write($"{element:00}, ");
}
```

Output
```
2, 11, 20, 29, 38, 47, 56, 65, 74,
```
As columns and rows implement ``IEnumerable<T>``, you can use LINQ on them freely.

Example of searching the index of item that is equal to 42

```csharp
ItemRequestResult<Index2D> indexRequest = array.FindIndex(o => o == 42);

if (indexRequest.IsSuccess)
{
    Console.WriteLine("42 is at " + indexRequest.Item);
}
```

Output

```
42 is at (4, 6)
```

To improve performance for methods that require delegates, you can implement a struct that is treated like a delegate. In this example the equivalent of ``Predicate<T>`` is a struct which implements ``IPredicate<T>``. This way such function will be called directly or even be inlined.

Creating a predicate struct

```csharp
public struct Equals42 : IPredicate<int>
{
    public bool Invoke(int obj) => obj == 42;
}
```

And passing a new struct as argument to the method.

```csharp
ItemRequestResult<Index2D> indexRequest = array.FindIndex(new Equals42());
```

However for such basic comparisons you can use one of the predefined predicates, such as ``EqualsPredicate<T>``

```csharp
ItemRequestResult<Index2D> indexRequest = array.FindIndex(new EqualsPredicate<int>(42));
```

Example of methods dedicated to ``List2D<T>``
```csharp
var list = List2D<int>.FromSystem2DArray(new int[,]
{
    {  0,  1,  2,  3,  4,  5,  6,  7,  8 },
    {  9, 10, 11, 12, 13, 14, 15, 16, 17 },
    { 18, 19, 20, 21, 22, 23, 24, 25, 26 },
    { 27, 28, 29, 30, 31, 32, 33, 34, 35 },
    { 36, 37, 38, 39, 40, 41, 42, 43, 44 },
    { 45, 46, 47, 48, 49, 50, 51, 52, 53 },
    { 54, 55, 56, 57, 58, 59, 60, 61, 62 },
    { 63, 64, 65, 66, 67, 68, 69, 70, 71 },
    { 72, 73, 74, 75, 76, 77, 78, 79, 80 }
});

Console.WriteLine(list.Boundaries.ToValueTuple());

list.AddColumns(count: 4);

Console.WriteLine(list.Boundaries.ToValueTuple());

list.InsertRows(startIndex: 4, count: 3);

Console.WriteLine(list.Boundaries.ToValueTuple());

list.RemoveColumn(index: 2);

Console.WriteLine(list.Boundaries.ToValueTuple());
```

Output

```
(9, 9)
(9, 13)
(12, 13)
(12, 12)
```

You can define your own rectangular collection and for small expense of implementing one of rectangular collection interfaces, you get many useful methods to work with. These are extension methods defined on rectangular collection interfaces but are strongly typed thanks to generics.

This however may introduce some code bloat as types of generic methods will often need to be specified. A solution to this problem could be defining instance methods that calls the extension methods which would still reduce amount of code to write compared to implementing them manually.

There is an example of custom rectangular collection definition.

```csharp
using static RectangularCollectionUtils;

public class Matrix3x3 : IRectangularCollection<float>
{
    private readonly float[] items;

    private static readonly Bounds2D Matrix3x3Boundaries = new Bounds2D(Rows, Columns);
    public const int Rows = 3;
    public const int Columns = 3;

    public float this[Index2D index]
    {
        get
        {
            if (!IsValidIndex(index))
            {
                throw new IndexOutOfRangeException();
            }
            return items[RowMajorIndex2DToInt(index, Columns)];
        }
        set
        {
            if (!IsValidIndex(index))
            {
                throw new IndexOutOfRangeException();
            }
            items[RowMajorIndex2DToInt(index, Columns)] = value;
        }
    }

    public float this[int row, int column]
    {
        get => this[(row, column)];
        set => this[(row, column)] = value;
    }

    public Matrix3x3() => items = new float[Rows * Columns];

    public Bounds2D Boundaries => Matrix3x3Boundaries;

    public IEnumerator<float> GetEnumerator()
    {
        for (int i = 0; i < Rows * Columns; i++)
        {
            yield return items[i];
        }
    }

    public bool IsValidIndex(Index2D index)
        => index.Row >= 0 && index.Row < Rows &&
           index.Column >= 0 && index.Column < Columns;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

And it's usage

```csharp
var matrix = new Matrix3x3();
bool isTrueThatEveryNumberIsZero = matrix.TrueForAll<float, Matrix3x3>(o => o == 0);

Console.WriteLine(isTrueThatEveryNumberIsZero);
```

Output

```
True
```
</details>

## Dependencies
Main project:
 * .NET Standard 2.0 (C# 7.3)
 
Tests project:
 * .NET Core 2.2
 * Microsoft.NET.Test.Sdk 16.6.1
 * NUnit 3.12.0
 * NUnit3TestAdapter 3.17.0

## License
SztormCollections is licensed under the MIT license. SztormCollections is free for commercial and non-commercial use.

[More about license.](./LICENSE)
