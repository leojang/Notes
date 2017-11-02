### Call Site
>To understand this binding, we have to understand the call-site: the location in code where a function is called (not where it's declared). We must inspect the call-site to answer the question: what's this this a reference to?
```
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
#### Default Binding
```
function foo() {
	console.log( this.a );
}

var a = 2;

foo(); // 2
```
>The first thing to note, if you were not already aware, is that variables declared in the global scope, as var a = 2 is, are synonymous with global-object properties of the same name. They're not copies of each other, they are each other. Think of it as two sides of the same coin.

>Secondly, we see that when foo() is called, this.a resolves to our global variable a. Why? Because in this case, the default binding for this applies to the function call, and so points this at the global object.

```
function foo() {
	"use strict";

	console.log( this.a );
}

var a = 2;

foo(); // TypeError: `this` is `undefined`
```
