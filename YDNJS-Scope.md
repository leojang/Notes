
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

### Anonymous vs. Named
```
setTimeout( function(){
	console.log("I waited 1 second!");
}, 1000 );
```
>This is called an "anonymous function expression", because function()... has no name identifier on it. Function expressions can be anonymous, but function declarations cannot omit the name -- that would be illegal JS grammar.
>Inline function expressions are powerful and useful -- the question of anonymous vs. named doesn't detract from that. Providing a name for your function expression quite effectively addresses all these draw-backs, but has no tangible downsides. The best practice is to always name your function expressions:
```
setTimeout( function timeoutHandler(){ // <-- Look, I have a name!
	console.log( "I waited 1 second!" );
}, 1000 );
```
### Invoking Function Expressions Immediately
```
var a = 2;

(function IIFE(){

	var a = 3;
	console.log( a ); // 3

})();

console.log( a ); // 2
```
>We pass in the window object reference, but we name the parameter global, so that we have a clear stylistic delineation for global vs. non-global references. Of course, you can pass in anything from an enclosing scope you want, and you can name the parameter(s) anything that suits you. This is mostly just stylistic choice.
```
var a = 2;

(function IIFE( global ){

	var a = 3;
	console.log( a ); // 3
	console.log( global.a ); // 2

})( window );

console.log( a ); // 2
```
### Blocks As Scopes
## try catch
>It's a very little known fact that JavaScript in ES3 specified the variable declaration in the catch clause of a try/catch to be block-scoped to the catch block.
For instance:
```
try {
	undefined(); // illegal operation to force an exception!
}
catch (err) {
	console.log( err ); // works!
}

console.log( err ); // ReferenceError: `err` not found
```
