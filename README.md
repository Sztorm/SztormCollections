# SztormCollections
Set of collections that .Net Framework lacks of or are more specialized than .Net ones.
### Currently this repository contains:
**Array2D&lt;T&gt;**: A two-dimensional rectangular array that is allocated within single contiguous block of memory.
### Examples:
<details>
<summary>Array2D&lt;T&gt;</summary>
    
<br>

**Set every item in the array by for loops and write it to console.**<br>

*Code:*       
```csharp
var exampleArray = new Array2D<int>(9, 9);
int counter = 0;

for (int i = 0; i < exampleArray.Rows; i++)
{
    for (int j = 0; j < exampleArray.Columns; j++)
    {
        exampleArray[i, j] = counter++;
        Console.Write($"{exampleArray[i, j]:00}, ");
    }
    Console.WriteLine();
}
```
*Output:*   
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

**Retrieve and print elements in fifth row.**<br>


*Code:*
```csharp
foreach (int element in exampleArray.GetRow(4))
{
    Console.Write($"{element:00}, ");
}
```
*Output:* 
```
36, 37, 38, 39, 40, 41, 42, 43, 44, 
```

**Find indices of a value stored in an array.**<br>


*Code:*
```csharp
int valueToSearch = 42;
Index2D? indicesFound = exampleArray.IndicesOf(valueToSearch);

if(indicesFound.HasValue)
{
    Console.WriteLine($"Indices of value {valueToSearch} are {indicesFound.Value}.");
}
```
*Output:*
```
Indices of value 42 are (4, 6).
```

</details>
