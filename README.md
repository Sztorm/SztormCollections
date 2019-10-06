# SztormCollections
Set of collections that lacks in the .Net Framework or are more specialized than .Net ones.
### Currently this repository contains:
**Array2D&lt;T&gt;**: A two-dimensional rectangular array allocated within single contiguous block of memory.
### Examples:
#### Array2D&lt;T&gt;
Set every item in the array by for loops and write it to console.<br>
**Code**:
```csharp
var exampleArray = new Array2D<int>(9, 9);
int counter = 0;

for (int i = 0; i < exampleArray.X; i++)
{
    for (int j = 0; j < exampleArray.Y; j++)
    {
        exampleArray[i, j] = counter++;
        Console.Write($"{exampleArray[i, j]:00}, ");
    }
    Console.WriteLine();
}
```
**Output**:
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
Retrieve and print every element in X dimension subarray.<br>
**Code**:
```csharp
foreach (int element in exampleArray.GetElementsOfX(4))
{
    Console.Write($"{element:00}, ");
}
Console.WriteLine();

```
**Output**:
```
36, 37, 38, 39, 40, 41, 42, 43, 44, 
```
