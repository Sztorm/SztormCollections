## Avoiding virtual calls of delegates
Methods which offers delegates as paramaters, usually do offer generic object equivalents of them.
Those are methods that perform some operation on a specific object defined by delegate like find,
convert and so on. These object equivalents of delegates can be passed instead for strictly
performance reasons. For the sake of clarity, let us call them **functors**.

### How do functors improve performance
Functors are called directly or even be inlined by a good JIT compiler. Direct calls are preferred
over the virtual calls as they avoid indirection which is what their name suggests. The indirection
in case of virtual calls may lead to many cache misses for a single method call. Virtual calls are
also very often not inlined which is important in many algorithms that call function arguments in a
loop and operate on massive data.

More about virtual call and it's problems:
 * [Virtual function](https://en.wikipedia.org/wiki/Virtual_function)
 * [Cache miss](https://en.wikipedia.org/wiki/CPU_cache#Cache_miss)
 * [Dynamic binding](https://en.wikipedia.org/wiki/Late_binding)
 * [Dynamic dispatch](https://en.wikipedia.org/wiki/Dynamic_dispatch)

### When do not use functors
Do not use functors when the they need to modify itself during execution of a function that is 
using them because it is technically impossible as they would need to bind proper methods
dynamically while their method is bound statically (assuming they do not use delegates or other
virtually called functions internally). But this case almost never happens in typical collection
queries.

A more practical argument is that they can hurt readability (imagine a LINQ method chain with
delegates replaced into functors). Generic arguments would often need to be specified and basic
functors that come with library may not suffice which leads to many functors defined for every
small purpose which can effectively clutter the codebase.

Also defining a functor may be not worth for a rarely called method and is often a sign of
premature optimization.

### Examples
The following array is used for the next examples.

```csharp
var array = Array2D<int>.FromSystem2DArray(new int[,]
{
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 },
});
```

Finding the index of first item that has value of 5 by using the predicate delegate.

```csharp
ItemRequestResult<Index2D> possibleIndexOf5 = array.FindIndex(o => o == 5);

Console.WriteLine(possibleIndexOf5.Item);
```

The optimized way.

```csharp
ItemRequestResult<Index2D> possibleIndexOf5 = array.FindIndex(new EqualsPredicate<int>(5));

Console.WriteLine(possibleIndexOf5.Item);
```

Output:

```
(1, 1)
```

Consider that you want a range of ints that matches the items from 3 to 7.

By using the delegate it is simple enough:

```csharp
List<int> itemsFrom3to7 = array.FindAll<List<int>>(o => o >= 3 && o <= 7);

Console.WriteLine(string.Join(", ", itemsFrom3to7));
```

The optimized way is a bit more complicated as there is no predefined type in library that
represents the range predicate.

Firstly, define a struct which is equivalent to the delegate that is to be replaced. The delegate
is type of _Predicate\<T\>_, so the struct shall need to implement the
[_IPredicate\<T\>_](xref:Sztorm.Collections.IPredicate`1) interface. One of the generic constraints
on the delegate equivalent is that the type must be struct. The reason for this requirement is that
it makes sure the call is direct (structs do not support inheritance) and structs as arguments do
not involve managed heap allocation (assuming they do not instantiate any class types internally)
which is another optimization.

```csharp
struct RangeFrom3To7Predicate : IPredicate<int>
{
    public bool Invoke(int o) => o >= 3 && o <= 7;
}
```

As you have may noticed, the Invoke method is almost identical to the lambda expression which
makes defining own functors more convenient.

Functor in action:

```csharp
List<int> itemsFrom3to7 = array.FindAll<List<int>, RangeFrom3To7Predicate>(
    new RangeFrom3To7Predicate());

Console.WriteLine(string.Join(", ", itemsFrom3to7));
```

That is it. However repetition of _RangeFrom3To7Predicate_ may look ugly from readability stance.
For functors that offer **only** the parameterless constructor such as the one showed in the
example, you can use the **default** keyword which is an equivalent of calling the parameterless
constructor in this case.

```csharp
List<int> itemsFrom3to7 = array.FindAll<List<int>, RangeFrom3To7Predicate>(default);
```

Output:

```
3, 4, 5, 6, 7
```