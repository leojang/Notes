```

// deps = dependencies 相依的Module  
//  impl = implementation  

var MyModules = (function Manager() {
	var modules = {};

	function define(name, deps, impl) {
		for (var i=0; i<deps.length; i++) {
		        // 取得相依模組
			deps[i] = modules[deps[i]];
		}
		modules[name] = impl.apply( impl, deps );
                // apply 傳入的impl參數沒用到          
                // 呼叫 impl 會回傳物件 (#41, #53)
	}

	function get(name) {
		return modules[name];
	}

	return {
		define: define,
		get: get
	};
})();

MyModules.define( "bar", [], function(){
	function hello(who) {
		return "Let me introduce: " + who;
	}

	return {
		hello: hello
	};
} );

MyModules.define( "foo", ["bar"], function(bar){
	var hungry = "hippo";

	function awesome() {
		console.log( bar.hello( hungry ).toUpperCase() );
	}

	return {
		awesome: awesome
	};
} );

var bar = MyModules.get( "bar" );
var foo = MyModules.get( "foo" );

console.log(
	bar.hello( "hippo" )
); // Let me introduce: hippo

foo.awesome(); // LET ME INTRODUCE: HIPPO
```
### Call Apply
https://codeplanet.io/javascript-apply-vs-call-vs-bind/
```
var person1 = {firstName: 'Jon', lastName: 'Kuperman'};
var person2 = {firstName: 'Kelly', lastName: 'King'};

function say(greeting) {
    console.log(greeting + ' ' + this.firstName + ' ' + this.lastName);
}

say.apply(person1, ['Hello']); // Hello Jon Kuperman
say.apply(person2, ['Hello']); // Hello Kelly King
```
