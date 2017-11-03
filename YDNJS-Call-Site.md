### Call Site
To understand *this* binding, we have to understand the call-site: the location in code where a function is called (**not where it's declared**). We must inspect the call-site to answer the question: what's this *this* a reference to?
```javascript
function baz() {
    // call-stack is: `baz`
    // so, our call-site is in the global scope

    console.log( "baz" );
    bar(); // <-- call-site for `bar`
}

function bar() {
    // call-stack is: `baz` -> `bar`
    // so, our call-site is in `baz`

    console.log( "bar" );
    foo(); // <-- call-site for `foo`
}

function foo() {
    // call-stack is: `baz` -> `bar` -> `foo`
    // so, our call-site is in `bar`

    console.log( "foo" );
}

baz(); // <-- call-site for `baz`
```
### Rules
### Default Binding
```javascript
function foo() {
	console.log( this.a );
}

var a = 2;

foo(); // 2
```
The first thing to note, if you were not already aware, is that variables declared in the global scope, as var a = 2 is, are synonymous with global-object properties of the same name. They're not copies of each other, they are each other. Think of it as two sides of the same coin.

Secondly, we see that when foo() is called, *this.a* resolves to our global variable a. Why? Because in this case, the default binding for this applies to the function call, and so points this at the global object.

```javascript
function foo() {
	"use strict";

	console.log( this.a );
}

var a = 2;

foo(); // TypeError: `this` is `undefined`, contents run in strict mode
```
A subtle but important detail is: even though the overall this binding rules are entirely based on the call-site, **the global object is only eligible for the default binding if the contents of foo() are not running in strict mode**; the strict mode state of the call-site of foo() is irrelevant.

```javascript
function foo() {
	console.log( this.a );
}

var a = 2;

(function(){
	"use strict";

	foo(); // 2
})();
```
**Note** Don't mix `strict mode` and `non-strict mode` together 

### Implicit Binding
```javascript
function foo() {
	console.log( this.a );
}

var obj = {
	a: 2,
	foo: foo
};

obj.foo(); // 2
```
Another rule to consider is: does the call-site have a context object, also referred to as an owning or containing object, though these alternate terms could be slightly misleading.

Firstly, notice the manner in which *foo()* is declared and then later added as a reference property onto *obj*. Regardless of whether *foo()* is initially declared on *obj*, or is added as a reference later (as this snippet shows), in neither case is **the *function* really "owned" or "contained" by the _obj_ object**.

However, the call-site uses the _obj_ context to **reference** the function, so you could say that **the obj object "owns" or "contains" the _function reference_ at the time the function is called**.

Whatever you choose to call this pattern, at the point that _foo()_ is called, it's preceded by an object reference to _obj_. When there is a context object for a function reference, the implicit binding rule says that it's that object which should be used for the function call's _this_ binding.

#### Implicitly Lost
One of the most common frustrations that _this_ binding creates is when an implicitly bound function loses that binding, which usually means it falls back to the _default binding_, of either the _global object_ or _undefined_, depending on strict mode.

```javascript
function foo() {
	console.log( this.a );
}

var obj = {
	a: 2,
	foo: foo
};

var bar = obj.foo; // function reference/alias!

var a = "oops, global"; // `a` also property on global object

bar(); // "oops, global"
```
Even though bar appears to be a reference to obj.foo, in fact, **it's really just another _reference_ to foo itself**. Moreover, the call-site is what matters, and the call-site is _bar()_, which is a plain, un-decorated call and thus the default binding applies.

```javascript
function foo() {
	console.log( this.a );
}

function doFoo(fn) {
	// `fn` is just another reference to `foo`

	fn(); // <-- call-site!
}

var obj = {
	a: 2,
	foo: foo
};

var a = "oops, global"; // `a` also property on global object

doFoo( obj.foo ); // "oops, global"
```
**Parameter passing is just an implicit assignment**, and since we're passing a function, it's an implicit reference assignment, so the end result is the same as the previous snippet.

### Explicit Binding
```javascript
function foo() {
	console.log( this.a );
}

var obj = {
	a: 2
};

foo.call( obj ); // 2
```
### Hard Binding
```javascript
function foo() {
	console.log( this.a );
}

var obj = {
	a: 2
};

var bar = function() {
	foo.call( obj );
};

bar(); // 2
setTimeout( bar, 100 ); // 2

// `bar` hard binds `foo`'s `this` to `obj`
// so that it cannot be overriden
bar.call( window ); // 2
```
>Let's examine how this variation works. We create a function bar() which, internally, manually calls foo.call(obj), thereby forcibly invoking foo with obj binding for this. No matter how you later invoke the function bar, it will always manually invoke foo with obj. This binding is both explicit and strong, so we call it hard binding.

>The most typical way to wrap a function with a hard binding creates a pass-thru of any arguments passed and any return value received:
```javascript
function foo(something) {
	console.log( this.a, something );
	return this.a + something;
}

var obj = {
	a: 2
};

var bar = function() {
	return foo.apply( obj, arguments );
};

var b = bar( 3 ); // 2 3
console.log( b ); // 5
```
>Another way to express this pattern is to create a re-usable helper:
```
function foo(something) {
	console.log( this.a, something );
	return this.a + something;
}

// simple `bind` helper
function bind(fn, obj) {
	return function() {
		return fn.apply( obj, arguments );
	};
}

var obj = {
	a: 2
};

var bar = bind( foo, obj );

var b = bar( 3 ); // 2 3
console.log( b ); // 5
```
>Since hard binding is such a common pattern, it's provided with a built-in utility as of ES5: Function.prototype.bind, and it's used like this:
```
function foo(something) {
	console.log( this.a, something );
	return this.a + something;
}

var obj = {
	a: 2
};

var bar = foo.bind( obj );

var b = bar( 3 ); // 2 3
console.log( b ); // 5
```
### `new` Binding
>The fourth and final rule for this binding requires us to re-think a very common misconception about functions and objects in JavaScript.

>In traditional class-oriented languages, "constructors" are special methods attached to classes, that when the class is instantiated with a new operator, the constructor of that class is called. This usually looks something like:

```
something = new MyClass(..);
```

>JavaScript has a new operator, and the code pattern to use it looks basically identical to what we see in those class-oriented languages; most developers assume that JavaScript's mechanism is doing something similar. However, there really is no connection to class-oriented functionality implied by new usage in JS.

>First, let's re-define what a "constructor" in JavaScript is. In JS, constructors are **just functions** that happen to be called with the new operator in front of them. They are not attached to classes, nor are they instantiating a class. They are not even special types of functions. They're just regular functions that are, in essence, hijacked by the use of new in their invocation.

>So, pretty much any ol' function, including the built-in object functions like Number(..) (see Chapter 3) can be called with new in front of it, and that makes that function call a constructor call. This is an important but subtle distinction: there's really no such thing as "constructor functions", but rather construction calls of functions.

>When a function is invoked with new in front of it, otherwise known as a constructor call, the following things are done automatically:

1. a brand new object is created (aka, constructed) out of thin air
2. the newly constructed object is [[Prototype]]-linked
3. the newly constructed object is set as the this binding for that function call
4. unless the function returns its own alternate object, the new-invoked function call will automatically return the newly constructed object.
```
function foo(a) {
	this.a = a;
}

var bar = new foo( 2 );
console.log( bar.a ); // 2
```
>By calling foo(..) with new in front of it, we've constructed a new object and set that new object as the this for the call of foo(..). So new is the final way that a function call's this can be bound. We'll call this new binding.
