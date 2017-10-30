
>two distinct actions are taken for a variable assignment: First, Compiler declares a variable (if not previously declared in the current scope), and second, when executing, Engine looks up the variable in Scope and assigns to it, if found

>"RHS" instead means "retrieve his/her source (value)", implying that RHS means "go get the value of...".
```
console.log( a );
```
>The reference to a is an RHS reference, because nothing is being assigned to a here. Instead, we're looking-up to retrieve the value of a, so that the value can be passed to console.log(..).
```
a = 2;
```
>The reference to a here is an LHS reference, because we don't actually care what the current value is, we simply want to find the variable as a target for the = 2 assignment operation.

### Hiding In Plain Scope
```
function doSomething(a) {
	b = a + doSomethingElse( a * 2 );

	console.log( b * 3 );
}

function doSomethingElse(a) {
	return a - 1;
}

var b;

doSomething( 2 ); // 15
```
>A more "proper" design would hide these private details inside the scope of doSomething(..), such as:
```
function doSomething(a) {
	function doSomethingElse(a) {
		return a - 1;
	}

	var b;

	b = a + doSomethingElse( a * 2 );

	console.log( b * 3 );
}

doSomething( 2 ); // 15
```
### Functions As Scopes
>The easiest way to distinguish declaration vs. expression is the position of the word "function" in the statement (not just a line, but a distinct statement). If "function" is the very first thing in the statement, then it's a function declaration. Otherwise, it's a function expression.
