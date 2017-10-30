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
What's missing is that we are trying to imply that each iteration of the loop "captures" its own copy of i, at the time of the iteration. But, the way scope works, all 5 of those functions, though they are defined separately in each loop iteration, all are closed over the same shared global scope, which has, in fact, only one i in it.
