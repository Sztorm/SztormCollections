## Use correct order when iterating through collection
The collections define how they are ordered. To improve performance during iteration, it is
important to use the correct order that will allow to utilize the CPU cache better. Accessing the
data sequentially helps to use the cache more efficiently, because cache memory is better utilized
and it leads to less cache misses.

[About CPU cache](https://en.wikipedia.org/wiki/CPU_cache)

For example the [**Array2D\<T\>**](xref:Sztorm.Collections.Array2D`1) uses row-major order which
means that the array is optimized to be accessed row by row than column by column.

Consider a 4x4 row-major array and a simplified cache that is able to fetch four elements.

The array:

| **1** | **2** | **3** | **4** |
|:-----:|:-----:|:-----:|:-----:|
| **5** | **6** | **7** | **8** |
| **9** | **10**| **11**| **12**|
| **13**| **14**| **15**| **16**|

Let us try to iterate through the first row and see how the cache behaves:

The first array row:

| <span style="color:green">**1** | <span style="color:green">**2** | <span style="color:green">**3** | <span style="color:green">**4** |
|:-:|:-:|:-:|:-:|

The cache:

| <span style="color:green">**_1_** | <span style="color:green">**_2_** | <span style="color:green">**_3_** | <span style="color:green">**_4_** |
|:-:|:-:|:-:|:-:|

Result: The whole row was fetched at one go, every item is ready to be used. In this case iterating
through the whole array would require four cache fetches.

Effect of iterating through the first column:

The first array column:

| <span style="color:green">**1** |
|:-------------------------------:|
| <span style="color:green">**5** |
| <span style="color:green">**9** |
| <span style="color:green">**13**|

The cache:

1st fetch:

| <span style="color:green">**_1_** | **2** | **3** | **4** |
|:---------------------------------:|:-----:|:-----:|:-----:|

2nd fetch:

| <span style="color:green">**_5_** | **6** | **7** | **8** |
|:---------------------------------:|:-----:|:-----:|:-----:|

3rd fetch:

| <span style="color:green">**_9_** | **10** | **11** | **12** |
|:---------------------------------:|:------:|:------:|:------:|

4th fetch:

| <span style="color:green">**_13_** | **14** | **15** | **16** |
|:----------------------------------:|:------:|:------:|:------:|

As you can see the cache contain only one element needed per fetch, so iteration through the column
only requires four cache fetches. Iteration through the whole array would require **sixteen** cache
fetches which is equal to one cache miss per element. What a horrendous waste of cache memory!

Of course today's processors have multiple levels of cache and definitely more cache memory, but it
is something to consider when you need to process enormous amounts of data. Always measure the
performance before you proceed to optimize, especially when it would hurt the readability.