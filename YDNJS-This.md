
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
