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

```javascriptvar
var strPrimitive = "I am a string";
typeof strPrimitive;						// "string"
strPrimitive instanceof String;					// false

var strObject = new String( "I am a string" );
ypeof strObject; 						// "object"
strObject instanceof String;					// true

// inspect the object sub-type
Object.prototype.toString.call( strObject );			// [object String]
```
We can inspect the internal sub-type by borrowing the base default toString() method, and you can see it reveals that **strObject is an object** that was in fact created by the _String_ constructor.

The primitive value "I am a string" is not an object, it's a primitive literal and immutable value. To perform operations on it, such as checking its length, accessing its individual character contents, etc, a String object is required.

Luckily, **the language automatically coerces a _"string"_ primitive to a _String_ object when necessary**, which means you almost never need to explicitly create the Object form. 

### Property vs. Method
Technically, functions never "belong" to objects, so saying that a function that just happens to be accessed on an object reference is automatically a "method" seems a bit of a stretch of semantics.

Even when you declare a function expression as part of the object-literal, that function doesn't magically belong more to the object -- still just multiple references to the same function object

Every time you access a property on an object, that is a **property access**, regardless of the type of value you get back. 

### Duplicating Objects
One subset solution is that objects which are JSON-safe (that is, can be serialized to a JSON string and then re-parsed to an object with the same structure and values) can easily be duplicated with:
```javascript
var newObj = JSON.parse( JSON.stringify( someObj ) );
```
At the same time, a shallow copy is fairly understandable and has far less issues, so ES6 has now defined **Object.assign(..)** for this task. _Object.assign(..)_ takes a target object as its first parameter, and one or more source objects as its subsequent parameters. It iterates over all the enumerable (see below), owned keys (immediately present) on the source object(s) and copies them (via = assignment only) to target. It also, helpfully, returns target, as you can see below:
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
#### getOwnPropertyDescriptor
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
#### defineProperty
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
changing configurable to false is a **one-way action, and cannot be undone!**

_configurable:false_ prevents is the ability to use the delete operator to remove an existing property.
```javascript
var myObject = {
	a: 2
};

myObject.a;				// 2
delete myObject.a;
myObject.a;				// undefined

Object.defineProperty( myObject, "a", {
	value: 2,
	writable: true,
	configurable: false,
	enumerable: true
} );

myObject.a;				// 2
delete myObject.a;
myObject.a;				// 2
```
#### Enumerable
The name probably makes it obvious, but this characteristic controls if a property will show up in certain object-property enumerations, such as the **for..in** loop. Set to _false_ to keep it from showing up in such enumerations, even though it's still completely accessible. Set to true to keep it present.

### Immutability
#### Object Constant
```javascript
var myObject = {};

Object.defineProperty( myObject, "FAVORITE_NUMBER", {
	value: 42,
	writable: false,
	configurable: false
} );
```

#### Prevent Extensions
```javascript
var myObject = {
	a: 2
};

Object.preventExtensions( myObject );

myObject.b = 3;
myObject.b; // undefined
```
#### Seal
**Object.seal(..)** creates a "sealed" object, which means it takes an existing object and essentially calls **Object.preventExtensions(..)** on it, but also marks all its existing properties as **configurable:false**.

So, not only can you not add any more properties, but you also cannot **reconfigure or delete any existing properties (though you can still modify their values)**.
```javascript
var myObject = {  a: 2 };
Object.seal(myObject);

myObject.a = 3;  
console.log(myObject.a); // 3 Can change value  myObject.

b = 4;  
console.log(myObject.b); // undefined  

Object.defineProperty( myObject, "a", {  
	value: 7,  
	writable: true,  
	configurable: false,  
	enumerable: false 
} ); 
// can't configure writable, configurable, enumerable

console.log(myObject.a); // 7
```
#### Freeze
**Object.freeze(..)** creates a frozen object, which means it takes an existing object and essentially calls **Object.seal(..)** on it, but it also marks all "data accessor" properties as writable:false, so that their **values cannot be changed**.

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

But one important result of this _[[Get]]_ operation is that if it cannot through any means come up with a value for the requested property, it instead returns the value **undefined**.

```javascript
var myObject = {
	a: 2
};

myObject.b; // undefined
```
This behavior is different from **when you reference variables by their identifier names**. If you reference a variable that cannot be resolved within the applicable lexical scope look-up, the result is not undefined as it is for object properties, but instead a **ReferenceError** is thrown.

From a value perspective, there is no difference between these two references -- they both result in undefined. However, the [[Get]] operation underneath, though subtle at a glance, potentially performed a bit more "work" for the reference myObject.b than for the reference myObject.a.
```javascript
var myObject = {
	a: undefined
};

myObject.a; // undefined

myObject.b; // undefined
```
### [[Put]]
If the property is present, the [[Put]] algorithm will roughly check:

1. Is the property an **accessor descriptor** (see "Getters & Setters" section below)? If so, call the setter, if any.
2. Is the property a data descriptor with writable of false? **If so, silently fail in non-strict mode, or throw TypeError in strict mode**.
3. Otherwise, set the value to the existing property as normal.

### Getters & Setters
ES5 introduced a way to override part of these default operations, not on an object level but a **per-property level**, through the use of getters and setters. Getters are properties which actually _call a hidden function to retrieve a value_. Setters are properties which actually _call a hidden function to set a value_.

When you define a property to have either a getter or a setter or both, its definition becomes an **"accessor descriptor"** (as opposed to a "data descriptor"). For accessor-descriptors, the _value_ and _writable_ characteristics of the descriptor are moot and ignored, and instead JS considers the set and get characteristics of the property (as well as configurable and enumerable).
```javascript
var myObject = {
	// define a getter for `a`
	get a() {
		return 2;
	}
};

Object.defineProperty(
	myObject,	// target
	"b",		// property name
	{			// descriptor
		// define a getter for `b`
		get: function(){ return this.a * 2 },

		// make sure `b` shows up as an object property
		enumerable: true
	}
);

myObject.a; // 2

myObject.b; // 4
```
Either through object-literal syntax with get a() { .. } or through explicit definition with defineProperty(..), in both cases we created a property on the object that actually doesn't hold a value, but whose access automatically results in a hidden function call to the getter function, with whatever value it returns being the result of the property access.

You will almost certainly want to always declare both getter and setter 
```javascript
var myObject = {
	// define a getter for `a`
	get a() {
		return this._a_;
	},

	// define a setter for `a`
	set a(val) {
		this._a_ = val * 2;
	}
};

myObject.a = 2;

myObject.a; // 4
```
### Existence
```javascript
var myObject = {
	a: 2
};

("a" in myObject);				// true
("b" in myObject);				// false

myObject.hasOwnProperty( "a" );	// true
myObject.hasOwnProperty( "b" );	// false
```
The **in** operator will check to see if the property is in the object, or **if it exists at any higher level of the [[Prototype]] chain object traversal**. By contrast, hasOwnProperty(..) checks to see if only myObject has the property or not, and **will not consult the [[Prototype]] chain**. 

hasOwnProperty(..) is accessible for all normal objects via delegation to Object.prototype (see Chapter 5). But it's possible to create an object that does not link to Object.prototype (via Object.create(null) -- see Chapter 5). In this case, a method call like myObject.hasOwnProperty(..) would fail.In that scenario, a more robust way of performing such a check is Object.prototype.hasOwnProperty.call(myObject,"a"), which borrows the base hasOwnProperty(..) method and uses explicit this binding (see Chapter 2) to apply it against our myObject.

### Enumeration
```javascript
var myObject = { };

Object.defineProperty(
	myObject,
	"a",
	// make `a` enumerable, as normal
	{ enumerable: true, value: 2 }
);

Object.defineProperty(
	myObject,
	"b",
	// make `b` NON-enumerable
	{ enumerable: false, value: 3 }
);

myObject.b; // 3
("b" in myObject); // true
myObject.hasOwnProperty( "b" ); // true

// .......

for (var k in myObject) {
	console.log( k, myObject[k] );
}
// "a" 2
```
:bulb:It's a good idea to use for..in loops only on objects, and traditional for loops with numeric index iteration for the values stored in arrays.
```javascript
var myObject = { };

Object.defineProperty(
	myObject,
	"a",
	// make `a` enumerable, as normal
	{ enumerable: true, value: 2 }
);

Object.defineProperty(
	myObject,
	"b",
	// make `b` non-enumerable
	{ enumerable: false, value: 3 }
);

myObject.propertyIsEnumerable( "a" ); // true
myObject.propertyIsEnumerable( "b" ); // false

Object.keys( myObject ); // ["a"]
Object.getOwnPropertyNames( myObject ); // ["a", "b"]
```
**propertyIsEnumerable(..)** tests whether the given property name exists **directly on the object** and is also enumerable:true.

**Object.keys(..)** returns an array of all enumerable properties, whereas **Object.getOwnPropertyNames(..)** returns an array of all properties, enumerable or not.

Whereas _in_ vs. _hasOwnProperty(..)_ differ in whether they consult the [[Prototype]] chain or not, Object.keys(..) and Object.getOwnPropertyNames(..) both inspect only the **direct object** specified.

### Iteration
The **for..in** loop iterates over the list of enumerable **properties** on an object (including its [[Prototype]] chain). But what if you instead want to iterate over the values?
```javascript
var myArray = [1, 2, 3];

for (var i = 0; i < myArray.length; i++) {
	console.log( myArray[i] );
}
// 1 2 3
```
**This isn't iterating over the values, though, but iterating over the indices**, where you then use the index to reference the value, as myArray[i].

#### forEach
forEach(..) will iterate over all values in the array, and ignores any callback return values. 
```javascript
var arr = [9,8.7];

arr.forEach(function (item) {
  someFn(item);
})

// ES6 Version
arr.forEach((val) => {  
    console.log(val);
})
```
https://yami.io/reduce-foreach-filter-map/

### Map
如果你希望遍歷陣列內的每個內容，然後修改原始陣列
```javascript
var arr = [1, 2, 3];

arr.map((val) => {  
    return val * 2;
})

console.log(arr); // [2, 4, 6]  
```

### Filter
這個按照字面上的意思來說就是過濾器，filter 不會修改值，但他會幫你決定要不要將這個值留在陣列裡面，要注意的是 filter 會回傳一個新的陣列，而不是修改原本的陣列。
```javascript
var arr = [1, 2, 3, 4];

var newArr = arr.filter((val) => {  
    return val > 2;
})

console.log(newArr); // [3, 4]  
```

### Reduce
如果你希望把陣列內的內容作加總並最終回傳一個結果，那麼你就可以用上 reduce
```javascript
var arr = [1, 2, 3];

var totalNum = arr.reduce((total, val) => {  
    return total + val;
}, 0);

console.log(totalNum); // 6  
```

### Every
```javascript
function isBigEnough(element, index, array) { 
  return element >= 10; 
} 

[12, 5, 8, 130, 44].every(isBigEnough);   // false 
[12, 54, 18, 130, 44].every(isBigEnough); // true
```

### Some
```javascript
function isBiggerThan10(element, index, array) {
  return element > 10;
}

[2, 5, 8, 1, 4].some(isBiggerThan10);  // false
[12, 5, 8, 1, 4].some(isBiggerThan10); // true
```

If you iterate on an object with a for..in loop, you're also only getting at the values **indirectly**, because it's actually iterating only over the **enumerable properties** of the object, leaving you to access the properties manually to get the values.

But what if you want to iterate over the values directly instead of the array indices (or object properties)? Helpfully, ES6 adds a **for..of** loop syntax for iterating over arrays (and objects, if the object defines its own custom iterator):

```javascript
var myArray = [ 1, 2, 3 ];

for (var v of myArray) {
	console.log( v );
}
// 1
// 2
// 3
```
### Manually iterate the array
```javascript
var myArray = [ 1, 2, 3 ];
var it = myArray[Symbol.iterator]();

it.next(); // { value:1, done:false }
it.next(); // { value:2, done:false }
it.next(); // { value:3, done:false }
it.next(); // { done:true }
```
:bulb:We get at the @@iterator internal property of an object using an ES6 Symbol: Symbol.iterator.
:bulb:Also, despite the name's implications, @@iterator is not the iterator object itself, but a function that returns the iterator object 
