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
### Hoisting (舉起)
```
var a = 2;

foo();					// works because `foo()`
						// declaration is "hoisted"

function foo() {
	a = 3;

	console.log( a );	// 3

	var a;				// declaration is "hoisted"
						// to the top of `foo()`
}

console.log( a );	// 2
```
### Scope
>Because of using let instead of var, b will belong only to the if statement and thus not to the whole foo() function's scope.
```
function foo() {
	var a = 1;

	if (a >= 1) {
		let b = 2;

		while (b < 5) {
			let c = b * 2;
			b++;

			console.log( a + c );
		}
	}
}

foo();
// 5 7 9
```
```
function foo() {
	var a = 1;

	if (a >= 1) {
		var b = 2;

		while (b < 5) {
			var c = b * 2;
			b++;
		}
	}
	console.log(b) // 5，可以讀取b的值，不同於C#
}
```
