
>`this` doesn't let a function get a reference to itself like we might have assumed.
```
function foo(num) {   
	console.log( "foo: " + num );    
	console.log(this.count); // 一開始 undefined 之後 NaN  
	
	this.count++;   
	// this 是 Windows Object  
	// JavaScript會自動建立一個 global variable count  
	// `值為 undefined`  
	// 由於 undefined + 1 = NaN 
}

foo.count = 0;
var i;

for (i=0; i<10; i++) 
{  
	if (i > 5) {   
		foo( i );     
	} 
} 
// foo: 6 
// foo: 7 
// foo: 8 
// foo: 9

// how many times was foo called? 
console.log( foo.count ); // 0 -- WTF? 
console.log( count ); // NaN
```
```
function foo() {
	foo.count = 4; // `foo` refers to itself
}

setTimeout( function(){
	// anonymous function (no name), cannot
	// refer to itself
}, 10 );
```
>In the first function, called a "named function", foo is a reference that can be used to refer to the function from inside itself.

>So another solution to our running example would have been to use the foo identifier as a function object reference in each place, and not use this at all, which works:
```
function foo(num) {
	console.log( "foo: " + num );

	// keep track of how many times `foo` is called
	foo.count++;
}

foo.count = 0;

var i;

for (i=0; i<10; i++) {
	if (i > 5) {
		foo( i );
	}
}
// foo: 6
// foo: 7
// foo: 8
// foo: 9

// how many times was `foo` called?
console.log( foo.count ); // 4
```
>Yet another way of approaching the issue is to force this to actually point at the foo function object:
```
function foo(num) {
	console.log( "foo: " + num );

	// keep track of how many times `foo` is called
	// Note: `this` IS actually `foo` now, based on
	// how `foo` is called (see below)
	this.count++;
}

foo.count = 0;

var i;

for (i=0; i<10; i++) {
	if (i > 5) {
		// using `call(..)`, we ensure the `this`
		// points at the function object (`foo`) itself
		foo.call( foo, i );
	}
}
// foo: 6
// foo: 7
// foo: 8
// foo: 9

// how many times was `foo` called?
console.log( foo.count ); // 4
```
### Its Scope
>To be clear, this does not, in any way, refer to a function's lexical scope. It is true that internally, scope is kind of like an object with properties for each of the available identifiers. But the scope "object" is not accessible to JavaScript code. It's an inner part of the Engine's implementation.

>Consider code which attempts (and fails!) to cross over the boundary and use this to implicitly refer to a function's lexical scope:
```
function foo() {
	var a = 2;
	this.bar();
}

function bar() {
	console.log( this.a );
}

foo(); //undefined
```
>An attempt is made to reference the bar() function via this.bar(). It is almost certainly an accident that it works. The most natural way to have invoked bar() would have been to omit the leading this. and just make a lexical reference to the identifier.

>However, the developer who writes such code is attempting to use this to create a bridge between the lexical scopes of foo() and bar(), so that bar() has access to the variable a in the inner scope of foo(). No such bridge is possible. You cannot use a this reference to look something up in a lexical scope. It is not possible.

>Every time you feel yourself trying to mix lexical scope look-ups with this, remind yourself: there is no bridge.
### What's `this`?
>We said earlier that this is not an author-time binding but a runtime binding. It is contextual based on the conditions of the function's invocation. this binding has nothing to do with where a function is declared, but has instead everything to do with the manner in which the function is called.

>When a function is invoked, an activation record, otherwise known as an execution context, is created. This record contains information about where the function was called from (the call-stack), how the function was invoked, what parameters were passed, etc. One of the properties of this record is the this reference which will be used for the duration of that function's execution.
```
// this指向於調用該函式之物件   
var mod1 = (function foo(num) {    
	// keep track of how many times `foo` is called  
	this.count++;    
	
	function boo()  {      
		console.log(this); // [Object Object]   
		return {a:'name'};     
	}    
	
	return {   define: boo  };   
})();   

mod1.define();  

// 如果調用函式的前方並未有物件，則函式內this就指向全域物件 
var mod2 = (function foo(num) {    
	// keep track of how many times `foo` is called  
	this.count++;    
	
	function boo()  {      
		console.log(this); // [Object Window]   
		return {a:'name'};     
	}    
	
	return boo;   
})();   

mod2();
```
