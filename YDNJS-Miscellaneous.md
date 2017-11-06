### Array.prototype.slice.call

What happens under the hood is that when _.slice()_ is called normally, this is an Array, and then it just iterates over that Array, and does its work.

How is _this_ in the .slice() function an Array? Because when you do:
```javascript
object.method();...
```
the _object_ automatically becomes the value of this in the method(). So with:
```javascript
[1,2,3].slice()...
```
the _[1,2,3]_ Array is set as the value of _this_ in _.slice()_.

But what if you could substitute something else as the _this_ value? As long as whatever you substitute has a numeric _.length_ property, and a bunch of properties that are numeric indices, it should work. This type of object is often called an _array-like object_.

The _.call()_ and _.apply()_ methods let you manually set the value of this in a function. So if we set the value of this in _.slice()_ to an array-like object, _.slice()_ will just assume it's working with an Array, and will do its thing.

Take this plain object as an example.
```javascript
var my_object = {
    '0': 'zero',
    '1': 'one',
    '2': 'two',
    '3': 'three',
    '4': 'four',
    length: 5
};
```
This is obviously not an Array, but if you can set it as the _this_ value of _.slice()_, then it will just work, because it looks enough like an Array for _.slice()_ to work properly.
```javascript
var sliced = Array.prototype.slice.call( my_object, 3 );
```
the result is what we expect:['three','four'];So this is what happens when you set an arguments object as the _this_ value of _.slice()_. Because arguments has a _.length_ property and a bunch of numeric indices, _.slice()_ just goes about its work as if it were working on a real Array.
