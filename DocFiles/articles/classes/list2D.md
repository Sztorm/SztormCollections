## List2D\<T\>
**List2D\<T\>** is a representation of two-dimensional, row-major ordered, resizable rectangular
collection.

 * [API](xref:Sztorm.Collections.List2D`1)
 * [Source code](https://github.com/Sztorm/SztormCollections/blob/master/Sztorm.Collections/Implementations/2D/List2D.cs)

### Initialization
**List2D\<T\>** can be initialized with [parameterless constructor](xref:Sztorm.Collections.List2D`1.%23ctor):

```csharp
var list = new List2D<T>();
```

However if the collection size is more or less known, it is recommended to specify initial capacity
in order to prevent unnecessary memory copying and improve performance.

```csharp
int predictedMaxRows = 5;
int predictedMaxColumns = 7;
var list = new List2D<T>(predictedMaxRows, predictedMaxColumns);
```
or
```csharp
var predictedMaxBoundaries = new Bounds2D(5, 7);
var list = new List2D<T>(predictedMaxBoundaries);
```

Note that the list **capacity** is not the list **boundaries**. You can specify initial capacity
but the created list will have **zero** rows and columns. For more information read about resizing
below.

### Resizing
**List2D\<T\>** can be resized in many ways. Rows and columns can be added, inserted, and removed.
Before proceeding further, you should distinguish between **capacity** and **boundaries**.
 * [**capacity**](xref:Sztorm.Collections.List2D`1.Capacity) is the number of rows and columns the
 list **may** have before the resizing happens. When more rows or columns are added to the list (or
 inserted into) and they exceed existing capacity, the list capacity and memory usage is increased
 automatically.
 * [**boundaries**](xref:Sztorm.Collections.List2D`1.Boundaries) are the rows and columns the list
 **currently** have. Resizing always affect the boundaries (assuming you are adding, removing or
 inserting more than zero rows or columns) but not always affect the list capacity.

Generally you should not bother how the list works internally but knowing the difference between
capacity and boundaries can help you make fewer mistakes like initializing list with specified
capacity and expect it to be the list boundaries.

Resizing methods overview

Group of methods that increases the list boundaries:
 * [AddColumn](xref:Sztorm.Collections.List2D`1.AddColumn)
 * [AddColumns(Int32)](xref:Sztorm.Collections.List2D`1.AddColumns(System.Int32))
 * [AddRow](xref:Sztorm.Collections.List2D`1.AddRow)
 * [AddRows(Int32)](xref:Sztorm.Collections.List2D`1.AddRows(System.Int32))
 * [IncreaseBounds(Int32, Int32)](xref:Sztorm.Collections.List2D`1.IncreaseBounds(System.Int32,System.Int32))
 * [IncreaseBounds(Bounds2D)](xref:Sztorm.Collections.List2D`1.IncreaseBounds(Sztorm.Collections.Bounds2D))
 * [InsertColumn(Int32)](xref:Sztorm.Collections.List2D`1.InsertColumn(System.Int32))
 * [InsertColumns(Int32, Int32)](xref:Sztorm.Collections.List2D`1.InsertColumns(System.Int32,System.Int32))
 * [InsertRow(Int32)](xref:Sztorm.Collections.List2D`1.InsertRow(System.Int32))
 * [InsertRows(Int32, Int32)](xref:Sztorm.Collections.List2D`1.InsertRows(System.Int32,System.Int32))

Group of methods that shrinks the list boundaries:
 * [Clear](xref:Sztorm.Collections.List2D`1.Clear)
 * [RemoveColumn(Int32)](xref:Sztorm.Collections.List2D`1.RemoveColumn(System.Int32))
 * [RemoveColumns(Int32, Int32)](xref:Sztorm.Collections.List2D`1.RemoveColumns(System.Int32,System.Int32))
 * [RemoveRow(Int32)](xref:Sztorm.Collections.List2D`1.RemoveRow(System.Int32))
 * [RemoveRows(Int32, Int32)](xref:Sztorm.Collections.List2D`1.RemoveRows(System.Int32,System.Int32))

For better view what happens with list after performing some of these methods on it, see the
examples:

Consider that you have a 3x3 list of various ints that can be presented as below:

| **1** | **2** | **3** |
|:-----:|:-----:|:-----:|
| **4** | **5** | **6** |
| **7** | **8** | **9** |

Which can be coded like that:

```csharp
var list = List2D<int>.FromSystem2DArray(new int[,] 
{
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 },
});
```

Assume that the above piece of code is ran before every method example.

_AddColumn_

```csharp
list.AddColumn();
```

Result: A 3x4 list with the last column that is filled with default value.

| **1** | **2** | **3** | **_0_** |
|:-----:|:-----:|:-----:|:-------:|
| **4** | **5** | **6** | **_0_** |
| **7** | **8** | **9** | **_0_** |

_AddRows_

```csharp
list.AddRows(count: 2);
```

Result: A 5x3 list with the last two rows that are filled with default value.

|  **1**  |  **2**  |  **3**  |
|:-------:|:-------:|:-------:|
|  **4**  |  **5**  |  **6**  |
|  **7**  |  **8**  |  **9**  |
| **_0_** | **_0_** | **_0_** |
| **_0_** | **_0_** | **_0_** |

You can use both _AddRows_ and _AddColumns_ one after another but it's better to use
_IncreaseBounds_ instead for better performance and readability.

```csharp
list.IncreaseBounds(rows: 1, columns: 3);
```

Result: A 4x6 list with new rows and columns that are filled with default value.

|  **1**  |  **2**  |  **3**  | **_0_** | **_0_** | **_0_** |
|:-------:|:-------:|:-------:|:-------:|:-------:|:-------:|
|  **4**  |  **5**  |  **6**  | **_0_** | **_0_** | **_0_** |
|  **7**  |  **8**  |  **9**  | **_0_** | **_0_** | **_0_** |
| **_0_** | **_0_** | **_0_** | **_0_** | **_0_** | **_0_** |

_InsertColumns_

```csharp
list.InsertColumns(startIndex: 1, count: 2)
```

Result: A 3x5 list with two columns at index of 1 that are filled with default value.

| **1** | **_0_** | **_0_** | **2** | **3** |
|:-----:|:-------:|:-------:|:-----:|:-----:|
| **4** | **_0_** | **_0_** | **5** | **6** |
| **7** | **_0_** | **_0_** | **8** | **9** |

That should clear things up a bit, so let us proceed to shrinking methods.

_Clear_

```csharp
list.Clear();
```

Result: As expected, _Clear_ clears out the list so it is empty and has boundaries of 0x0.

_RemoveColumn_

```csharp
list.RemoveColumn(index: 0);
```

Result: A 3x2 list that has the first column just removed.

_RemoveRows_

```csharp
list.RemoveRows(startIndex: 1, count: 2);
```

Result: A 1x3 list that has the last two rows removed.

### Memory layout
Like [**Array2D\<T\>**](xref:Sztorm.Collections.Array2D`1), **List2D\<T\>** is row-major ordered
which means that every item is accessed by specyfying a row, then column.

A visual example is the best way to explain how the items are stored in the list. Consider that you
have a list with capacity of 3x3 and boundaries of 3x2, the numbers other than zero show the order
of access to to each next item in the foreach loop:

Representation:

| **1** | **2** | **0** |
|:-----:|:-----:|:-----:|
| **3** | **4** | **0** |
| **5** | **6** | **0** |

Memory layout:

| **1** | **2** | **0** | **3** | **4** | **0** | **5** | **6** | **0** |
|:-----:|:-----:|:-----:|:-----:|:-----:|:-----:|:-----:|:-----:|:-----:|

Memory layout shows that **List2D\<T\>** is allocated within contiguous memory block, but the
layout itself may contain gaps between the items.