### Compare Values

> The proper way to characterize them is that == checks for value equality with coercion allowed, and === checks for value equality without allowing coercion; === is often called "strict equality" for this reason.

> If you can be certain about the values, and == is safe, use it! If you can't be certain about the values, use ===. It's that simple.

> You should take special note of the == and === comparison rules if you're comparing two non-primitive values, like objects (including function and array). Because those values are actually held by reference, both == and === comparisons will simply check whether the references match, not anything about the underlying values.

>For example, arrays are by default coerced to strings by simply joining all the values with commas (,) in between. You might think that two arrays with the same contents would be == equal, but they're not:

```
var a = [1,2,3];
var b = [1,2,3];
var c = "1,2,3";

a == c;		// true
b == c;		// true
a == b;		// false
```
> :bulb: there are no "strict inequality" operators that would disallow coercion the same way === "strict equality" does.
```
var a = 41;
var b = "42";
var c = "43";

a < b;		// true
b < c;		// true
```
>If both values in the < comparison are strings, as it is with b < c, the comparison is made lexicographically (aka alphabetically like a dictionary). But if one or both is not a string, as it is with a < b, then both values are coerced to be numbers, and a typical numeric comparison occurs.
```
var a = 42;
var b = "foo";

a < b;		// false
a > b;		// false
a == b;		// false
```
