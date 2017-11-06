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
var sliced = Array.prototype.slice.call( my_object, 3 ); //['three','four']
```
So this is what happens when you set an arguments object as the _this_ value of _.slice()_. Because arguments has a _.length_ property and a bunch of numeric indices, _.slice()_ just goes about its work as if it were working on a real Array.

#### Sequence Doesn't Matter

```javascript
var my_object = {  
    '3': 'zero',  
    'b': 'one',  
    'c': 'two',  
    'd': 'three',  
    'a': 'four',  
    length: 5 
};  
var sliced = Array.prototype.slice.call( my_object, 3 );  
console.log(sliced); // ['zero', undefined]
```
### Bind()
```javascript
var obj = {name:"Niladri"};

var greeting = function(a,b,c){
    return "welcome "+this.name+" to "+a+" "+b+" in "+c;
};

//creates a bound function that has same body and parameters 
var bound = greeting.bind(obj); 


console.dir(bound); ///returns a function

console.log("Output using .bind() below ");

console.log(bound("Newtown","KOLKATA","WB")); //call the bound function
```
如果某些函數，前幾個參數已經 “內定” 了，我們便可以用 bind 返回一個新的函數。也就是說，bind() 能使一個函數擁有預設的初始參數。這些參數（如果有的話）作為 bind() 的第二個參數跟在 this 後面，之後它們會被插入到目標函數的參數清單的開始位置，傳遞給綁定函數的參數會跟在它們的後面。
```javascript
function list() {
  return Array.prototype.slice.call(arguments);
}

var list1 = list(1, 2, 3); // [1, 2, 3]

// Create a function with a preset leading argument
var leadingThirtysevenList = list.bind(undefined, 37);

var list2 = leadingThirtysevenList(); // [37]
var list3 = leadingThirtysevenList(1, 2, 3); // [37, 1, 2, 3]
```
使用 bind 返回的結果還是個 function，是個 function 就可以被 new 運算子調用，那麼結果呢？:bangbang: **規範中說的很清楚了，當使用 new 操作符調用綁定函數時，bind 的第一個參數無效**。
```javascript
var boy = {firstName:'Cory'};  
function Person(lastName, age) {     
    this.name = this.firstName + ' ' + lastName;  
    this.age = age;     
    return {   
        name: this.name,   
        age: this.age  
    } 
}

var _Person = Person.bind(boy, 'Mackenson', 12 );   
var p = _Person();  
// Cory Mackenson 12   

var _Person = Person.bind(boy, 'Mackenson' );  
var p = _Person(13); 
// Cory Mackenson 13   

var _Person = Person.bind(boy);  
var p = _Person('Mackenson', 14); 
// Cory Mackenson 14    

var _Person = Person.bind(boy, 'Mackenson', 15 );  
var p = new _Person(); 
// undefined Mackenson 15   

var _Person = Person.bind(boy, 'Mackenson' );  
var p = new _Person(16); 
// undefined Mackenson 16  

var _Person = Person.bind(boy); 
var p = new _Person('Mackenson', 17); 
// undefined Mackenson 17
```
:bulb: _call_ 跟 _apply_ 會馬上呼叫方法，_bind_不會馬上呼叫可，可之後再呼叫
