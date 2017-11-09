### Syntax
_declarative (literal) form_
```javascript
var myObj = {
	key: value
	// ...
};
```
_constructed form_
```javascript
var myObj = new Object();
myObj.key = value;
```
### Type
6 primary types 
1. string
2. number
3. boolean
4. null
5. undefined
6. object

Note that the simple primitives (string, number, boolean, null, and undefined) are __not themselves objects__. null is sometimes referred to as an object type, but this misconception stems from a bug in the language which causes typeof null to return the string "object" incorrectly (and confusingly). In fact, __null is its own primitive type__.

:bulb:__It's a common mis-statement that "everything in JavaScript is an object". This is clearly not true.__

function is a sub-type of object (technically, a "callable object"). Functions in JS are said to be "first class" in that they are basically just normal objects (with callable behavior semantics bolted on), and so they can be handled like any other plain object.

### Built-in Objects
- String
- Number
- Boolean
- Object
- Function
- Array
- Date
- Reg
- ExpError
__in JS, these are actually just built-in functions, not Class like java.__ Each of these built-in functions can be used as a _constructor_ (that is, a function call with the new operator -- see Chapter 2), with the result being a _newly constructed object_ of the sub-type in question.

JS automatically coerces a "string" primitive to a String object when necessary

### Property vs. Method
Technically, functions never "belong" to objects, so saying that a function that just happens to be accessed on an object reference is automatically a "method" seems a bit of a stretch of semantics.

Even when you declare a function expression as part of the object-literal, that function doesn't magically belong more to the object -- still just multiple references to the same function object

### Duplicating Objects
One subset solution is that objects which are JSON-safe (that is, can be serialized to a JSON string and then re-parsed to an object with the same structure and values) can easily be duplicated with:
```javascript
var newObj = JSON.parse( JSON.stringify( someObj ) );
```
At the same time, a shallow copy is fairly understandable and has far less issues, so ES6 has now defined Object.assign(..) for this task. Object.assign(..) takes a target object as its first parameter, and one or more source objects as its subsequent parameters. It iterates over all the enumerable (see below), owned keys (immediately present) on the source object(s) and copies them (via = assignment only) to target. It also, helpfully, returns target, as you can see below:
```javascript
var newObj = Object.assign( {}, myObject );

newObj.a;						// 2
newObj.b === anotherObject;		// true
newObj.c === anotherArray;		// true
newObj.d === anotherFunction;	// true
```
### Property Descriptors
Prior to ES5, the JavaScript language gave no direct way for your code to inspect or draw any distinction between the characteristics of properties, such as whether the property was read-only or not.

But as of ES5, all properties are described in terms of a **property descriptor**.
```javascript
var myObject = {
	a: 2
};

Object.getOwnPropertyDescriptor( myObject, "a" );
// {
//    value: 2,
//    writable: true,
//    enumerable: true,
//    configurable: true
// }
```

```javascript
var myObject = {};

Object.defineProperty( myObject, "a", {
	value: 2,
	writable: true,
	configurable: true,
	enumerable: true
} );

myObject.a; // 2
```
#### Writable
```javascript
var myObject = {};

Object.defineProperty( myObject, "a", {
	value: 2,
	writable: false, // not writable!
	configurable: true,
	enumerable: true
} );

myObject.a = 3;

myObject.a; // 2
```

```javascript
"use strict";

var myObject = {};

Object.defineProperty( myObject, "a", {
	value: 2,
	writable: false, // not writable!
	configurable: true,
	enumerable: true
} );

myObject.a = 3; // TypeError
```
#### Configurable
```javascript
var myObject = {
	a: 2
};

myObject.a = 3;
myObject.a;					// 3

Object.defineProperty( myObject, "a", {
	value: 4,
	writable: true,
	configurable: false,	// not configurable!
	enumerable: true
} );

myObject.a;					// 4
myObject.a = 5;
myObject.a;					// 5

Object.defineProperty( myObject, "a", {
	value: 6,
	writable: true,
	configurable: true,
	enumerable: true
} ); // TypeError
```
### Prevent Extensions
```javascript
var myObject = {
	a: 2
};

Object.preventExtensions( myObject );

myObject.b = 3;
myObject.b; // undefined
```
### [[Get]]
```javascript
var myObject = {
	a: 2
};

myObject.a; // 2
```
The myObject.a is a property access, but it doesn't just look in myObject for a property of the name a, as it might seem.

According to the spec, the code above actually performs a _[[Get]]_ operation (kinda like a function call: _[[Get]]())_ on the myObject. The default built-in _[[Get]]_ operation for an object first inspects the object for a property of the requested name, and if it finds it, it will return the value accordingly.

However, the _[[Get]]_ algorithm defines other important behavior if it does not find a property of the requested name. We will examine in Chapter 5 what happens next (traversal of the _[[Prototype]]_ chain, if any).

But one important result of this _[[Get]]_ operation is that if it cannot through any means come up with a value for the requested property, it instead returns the value undefined.
