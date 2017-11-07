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

