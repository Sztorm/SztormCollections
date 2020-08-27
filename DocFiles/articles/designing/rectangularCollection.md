## RectangularCollection
You can create your own rectangular collection by implementing one of the four rectangular
collection interfaces on your type:
 * [IRectangularCollection](xref:Sztorm.Collections.IRectangularCollection`1)
 * [IReadOnlyRectangularCollection](xref:Sztorm.Collections.IReadOnlyRectangularCollection`1)
 * [IRefRectangularCollection](xref:Sztorm.Collections.IRefRectangularCollection`1)
 * [IRefReadOnlyRectangularCollection](xref:Sztorm.Collections.IRefReadOnlyRectangularCollection`1)

By doing so, your type gains many usable methods automatically such as
[Find](xref:Sztorm.Collections.Extensions.RectangularCollectionExtensions.Find*),
[Exist](xref:Sztorm.Collections.Extensions.RectangularCollectionExtensions.Exists*),
[TrueForAll](xref:Sztorm.Collections.Extensions.RectangularCollectionExtensions.TrueForAll*) and so
on. These are extension methods defined in
[Sztorm.Collections.Extensions](xref:Sztorm.Collections.Extensions) namespace, so remember to
import it if you are planning to use them.

### Example
Let us define a 3x3 matrix of floats.

Firstly, import namespaces and classes for further use.

```csharp
using Sztorm.Collections;
using Sztorm.Collections.Extensions;
using static RectangularCollectionUtils;
```

[RectangularCollectionUtils](xref:Sztorm.Collections.RectangularCollectionUtils) class can help to
implement your collection internals like indexing order.

Next step is to define the **Matrix3x3** class an choose the rectangular collection interface. The
example uses the [IRectangularCollection](xref:Sztorm.Collections.IRectangularCollection`1)
interface, but feel free to experiment with other rectangular collection interfaces.

```csharp
public class Matrix3x3 : IRectangularCollection<float>
{

}
```

Before implementing the interface, think what you already know about the **Matrix3x3** class:
 * It has exactly 3 rows and columns.
 * The type of elements is float, so the float array could be good choice for storing the items.

That is how this knowledge may be implemented in code:

```csharp
public class Matrix3x3 : IRectangularCollection<float>
{
    private readonly float[] items;
    public const int Rows = 3;
    public const int Columns = 3;
}
```

Next step is to implement the interface.
[IRectangularCollection\<float\>](xref:Sztorm.Collections.IRectangularCollection`1) requires the
following members to be implemented:
 * [IsValidIndex(Index2D)](xref:Sztorm.Collections.IIndexable2D`1.IsValidIndex(Sztorm.Collections.Index2D))
 * [this\[Index2D\]](xref:Sztorm.Collections.IIndexable2D`1.Item(Sztorm.Collections.Index2D))
 * [Boundaries](xref:Sztorm.Collections.IHasRectangularBoundaries.Boundaries)
 * GetEnumerator()

_IsValidIndex(Index2D)_

```csharp
public bool IsValidIndex(Index2D index)
    => index.Row >= 0 && index.Row < Rows &&
       index.Column >= 0 && index.Column < Columns;
```

_this\[Index2D\]_; the indexing will be row-major ordered and mapped to one-dimensional array.

 ```csharp
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
 ```

_Boundaries_; a good idea is to cache the value in static field and return it from the _Boundaries_
property getter (copying still takes place but you save
[Bounds2D](xref:Sztorm.Collections.Bounds2D) constructor call which validates arguments).

```csharp
private static readonly Bounds2D Matrix3x3Boundaries = new Bounds2D(Rows, Columns);

public Bounds2D Boundaries => Matrix3x3Boundaries;
```

_GetEnumerator()_; A simple implementation, just going through the array elements.

```csharp
public IEnumerator<float> GetEnumerator()
{
    for (int i = 0; i < Rows * Columns; i++)
    {
        yield return items[i];
    }
}

IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
```

Interface is implemented but the matrix need proper initialization, a parameterless constructor
should make the class ready to use.

```csharp
public Matrix3x3() => items = new float[Rows * Columns];
```

Additionally implementing the indexer with [Index2D](xref:Sztorm.Collections.Index2D) components as
parameters will save user from constructing the **Index2D** everytime.

```csharp
public float this[int row, int column]
{
    get => this[(row, column)];
    set => this[(row, column)] = value;
}
```

The result should look like this:


```csharp
using Sztorm.Collections;
using Sztorm.Collections.Extensions;
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

    public Bounds2D Boundaries => Matrix3x3Boundaries;

    public Matrix3x3() => items = new float[Rows * Columns];

    public bool IsValidIndex(Index2D index)
        => index.Row >= 0 && index.Row < Rows &&
           index.Column >= 0 && index.Column < Columns;

    public IEnumerator<float> GetEnumerator()
    {
        for (int i = 0; i < Rows * Columns; i++)
        {
            yield return items[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

**Matrix3x3** in action:

```csharp
var matrix = new Matrix3x3();
bool isEveryNumberEqualToZero = matrix.TrueForAll<float, Matrix3x3>(o => o == 0);

Console.WriteLine(isEveryNumberEqualToZero);
```

Output:

```
True
```

```csharp
var matrix = new Matrix3x3();
matrix[1, 2] = 3;
ItemRequestResult<Index2D> possibleIndex = matrix.FindIndex<float, Matrix3x3>(o => o == 3);

Console.WriteLine(possibleIndex.Item);
```

Output:

```
(1, 2)
```

### Summary
The extension methods are generic which make methods type-safe and calls to them direct. The
problem with them lies in possible code bloat because types of generic methods will often need to
be specified. Also these methods are not optimized for specific datatype structure, so if they
must be optimal, they still need to be implemented manually.

A solution to code bloat problem could be defining instance methods that call the extension methods
which would still reduce amount of code to write compared to implementing them from scratch.