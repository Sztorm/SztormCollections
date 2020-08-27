# SztormCollections
Set of collections that are more specialized than .Net ones.
 * [License](./LICENSE)
 * [Documentation](https://sztorm.github.io/SztormCollections/)
   * [Articles](https://sztorm.github.io/SztormCollections/articles/intro.html)
   * [API](https://sztorm.github.io/SztormCollections/api/index.html)
## Features:
**Array2D&lt;T&gt;**: A two-dimensional rectangular row-major ordered array that is allocated
within single contiguous block of memory.

**List2D&lt;T&gt;**: A two-dimensional rectangular row-major ordered list allocated within single
contiguous block of memory.

All collections support methods that you will expect from a standard .Net collection like Find,
FindIndex, Exist, TrueForAll and so on. Additionally some method overloads support optimized
equivalents of standard delegates like _Predicate\<T\>_ to maximize performance for those who need
it.

## Planned types:
**ChunkedList2D&lt;T&gt;**:A two-dimensional rectangular list allocated in chunks of memory.

**Array3D&lt;T&gt;**: A three-dimensional array allocated within single contiguous block of memory.

**List3D&lt;T&gt;**: A three-dimensional list allocated within single contiguous block of memory.

## Dependencies
Main project:
 * .NET Standard 2.0 (C# 7.3)
 
Tests project:
 * .NET Core 2.2
 * Microsoft.NET.Test.Sdk 16.6.1
 * NUnit 3.12.0
 * NUnit3TestAdapter 3.17.0

## License
SztormCollections is licensed under the MIT license. SztormCollections is free for commercial and
non-commercial use.

[More about license.](./LICENSE)
