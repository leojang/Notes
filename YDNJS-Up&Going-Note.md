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
>目前看起來 Scope 取決於 function 範圍
### Strict Mode
>ES5 added a "strict mode" to the language, which tightens the rules for certain behaviors.
```
// "use strict" 也可以放在外面
function foo() {
	"use strict";	// turn on strict mode
	a = 1;			// `var` missing, ReferenceError
}

foo();
```
### Functions As Values
>So far, we've discussed functions as the primary mechanism of scope in JavaScript. You recall typical function declaration syntax as follows:
```
function foo() {
	// ..
}
```
>Though it may not seem obvious from that syntax, foo is basically just a variable in the outer enclosing scope that's given a reference to the function being declared. That is, the function itself is a value, just like 42 or [1,2,3] would be.This may sound like a strange concept at first, so take a moment to ponder it. Not only can you pass a value (argument) to a function, but a function itself can be a value that's assigned to variables, or passed to or returned from other functions.As such, a function value should be thought of as an expression, much like any other value or expression.
```
var foo = function() {
	// ..
};

var x = function bar(){
	// ..
};
```
>The first function expression assigned to the foo variable is called anonymous because it has no name.The second function expression is named (bar), even as a reference to it is also assigned to the x variable. Named function expressions are generally more preferable, though anonymous function expressions are still extremely common.
### Closure
>You can think of closure as a way to "remember" and continue to access a function's scope (its variables) even once the function has finished running.
```
function makeAdder(x) {
	// parameter `x` is an inner variable

	// inner function `add()` uses `x`, so
	// it has a "closure" over it
	function add(y) {
		return y + x;
	};

	return add;
}
```
```
// `plusOne` gets a reference to the inner `add(..)`
// function with closure over the `x` parameter of
// the outer `makeAdder(..)`
var plusOne = makeAdder( 1 );

// `plusTen` gets a reference to the inner `add(..)`
// function with closure over the `x` parameter of
// the outer `makeAdder(..)`
var plusTen = makeAdder( 10 );

plusOne( 3 );		// 4  <-- 1 + 3
plusOne( 41 );		// 42 <-- 1 + 41

plusTen( 13 );		// 23 <-- 10 + 13
```
### Modules
>The most common usage of closure in JavaScript is the module pattern. Modules let you define private implementation details (variables, functions) that are hidden from the outside world, as well as a public API that is accessible from the outside.
```
function User(){
	var username, password;

	function doLogin(user,pw) {
		username = user;
		password = pw;

		// do the rest of the login work
	}

	var publicAPI = {
		login: doLogin
	};

	return publicAPI;
}

// create a `User` module instance
var fred = User();

fred.login( "fred", "12Battery34!" );
```
```
function User(){
	var username, password;

	function doLogin(user,pw) {
		username = user;
		password = pw;

		// do the rest of the login work
	}

	var publicAPI = {
		login: doLogin
	};

	return publicAPI;
}

// create a `User` module instance
var fred = User();

fred.login( "fred", "12Battery34!" );
```
