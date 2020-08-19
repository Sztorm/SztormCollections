## Array2D\<T\>

**Array2D\<T\>** is a representation of two-dimensional, fixed size rectangular collection.

 * [API](xref:Sztorm.Collections.Array2D`1)
 * [Source code](https://github.com/Sztorm/SztormCollections/blob/master/Sztorm.Collections/Implementations/2D/Array2D.cs)

### Memory layout
This is important to know that the **Array2D\<T\>** is row-major ordered which means that every item
is accessed by specyfying a row, then column.

Here is an example of how items are accessed in 3x3 array by using foreach loop. 1 is the first
element indexed, 2 is the second and so on.

| 1 | 2 | 3 |
|:-:|:-:|:-:|
| 4 | 5 | 6 |
| 7 | 8 | 9 |

However this is just a formal representation. **Array2D\<T\>** is allocated within single contiguous
block of memory and memory address space is one-dimensional, so the real memory layout of items is
presented below:

| 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 |
|:-:|:-:|:-:|:-:|:-:|:-:|:-:|:-:|:-:|

### Other notes
**Array2D\<T\>** size is fixed. If a resizable rectangular collection is needed, consider using
[**List2D\<T\>**](xref:Sztorm.Collections.List2D`1) instead.