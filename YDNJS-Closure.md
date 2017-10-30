# Closure

>Closure is when a function is able to remember and access its lexical scope even when that function is executing outside its lexical scope.
```
function foo() {
	var a = 2;

	function bar() {
		console.log( a );
	}

	return bar;
}

var baz = foo();

baz(); // 2 -- Whoa, closure was just observed, man.
```
### Loops + Closure
```
for (var i=1; i<=5; i++) {
	setTimeout( function timer(){
		console.log( i );
	}, i*1000 );
}
```
>if you run this code, you get "6" printed out 5 times, at the one-second intervals.
>What's missing is that we are trying to imply that each iteration of the loop "captures" its own copy of i, at the time of the iteration. But, the way scope works, all 5 of those functions, though they are defined separately in each loop iteration, all are closed over the same shared global scope, which has, in fact, only one i in it.
```
for (var i=1; i<=5; i++) {
	(function(){
		setTimeout( function timer(){
			console.log( i );
		}, i*1000 );
	})();
}
// Not Working, Still Print five 6
```
>It's not enough to have a scope to close over if that scope is empty. Look closely. Our IIFE is just an empty do-nothing scope. It needs something in it to be useful to us.

```
// All Working
for (var i=1; i<=5; i++) {
	(function(){
		var j = i;
		setTimeout( function timer(){
			console.log( j );
		}, j*1000 );
	})();
}

for (var i=1; i<=5; i++) {
	(function(j){
		setTimeout( function timer(){
			console.log( j );
		}, j*1000 );
	})( i );
}

for (var i=1; i<=5; i++) {
	let j = i; // yay, block-scope for closure!
	setTimeout( function timer(){
		console.log( j );
	}, j*1000 );
}
```
>There's a special behavior defined for let declarations used in the head of a for-loop. This behavior says that the variable will be declared not just once for the loop, but each iteration. And, it will, helpfully, be initialized at each subsequent iteration with the value from the end of the previous iteration.
```
for (let i=1; i<=5; i++) {
	setTimeout( function timer(){
		console.log( i );
	}, i*1000 );
}
```
### Modules
```
function CoolModule() {
	var something = "cool";
	var another = [1, 2, 3];

	function doSomething() {
		console.log( something );
	}

	function doAnother() {
		console.log( another.join( " ! " ) );
	}

	return {
		doSomething: doSomething,
		doAnother: doAnother
	};
}

var foo = CoolModule();

foo.doSomething(); // cool
foo.doAnother(); // 1 ! 2 ! 3
```
>An object with a function property on it alone is not really a module. An object which is returned from a function invocation which only has data properties on it and no closured functions is not really a module, in the observable sense.

>The code snippet above shows a standalone module creator called CoolModule() which can be invoked any number of times, each time creating a new module instance. A slight variation on this pattern is when you only care to have one instance, a "singleton" of sorts:
```
var foo = (function CoolModule() {
	var something = "cool";
	var another = [1, 2, 3];

	function doSomething() {
		console.log( something );
	}

	function doAnother() {
		console.log( another.join( " ! " ) );
	}

	return {
		doSomething: doSomething,
		doAnother: doAnother
	};
})();

foo.doSomething(); // cool
foo.doAnother(); // 1 ! 2 ! 3
```
>Another slight but powerful variation on the module pattern is to name the object you are returning as your public API:
```
var foo = (function CoolModule(id) {
	function change() {
		// modifying the public API
		publicAPI.identify = identify2;
	}

	function identify1() {
		console.log( id );
	}

	function identify2() {
		console.log( id.toUpperCase() );
	}

	var publicAPI = {
		change: change,
		identify: identify1
	};

	return publicAPI;
})( "foo module" );

foo.identify(); // foo module
foo.change();
foo.identify(); // FOO MODULE
```
