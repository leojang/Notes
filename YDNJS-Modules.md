>Modules require two key characteristics: 1) an outer wrapping function being invoked, to create the enclosing scope 2) the return value of the wrapping function must include reference to at least one inner function that then has closure over the private inner scope of the wrapper.
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
http://javascriptissexy.com/javascript-apply-call-and-bind-methods-are-essential-for-javascript-professionals/
https://codeplanet.io/javascript-apply-vs-call-vs-bind/
http://hangar.runway7.net/javascript/difference-call-apply
```
var person1 = {firstName: 'Jon', lastName: 'Kuperman'};
var person2 = {firstName: 'Kelly', lastName: 'King'};

function say(greeting) {
    console.log(greeting + ' ' + this.firstName + ' ' + this.lastName);
}

say.apply(person1, ['Hello']); // Hello Jon Kuperman
say.apply(person2, ['Hello']); // Hello Kelly King
```
### Apply Revisit
```
var MyModules = (function Manager() { 
	var modules = {};  
	var count = 0; 
	var obj = [{name:'John'},{name:'Tom'}];
	
 	function define(name, deps, impl) {      
		for (var i=0; i<deps.length; i++) {          
			deps[i] = modules[deps[i]];        
		}       
		modules[name] = impl.apply( obj[count], deps ); // #A
		count++;  
	}
	
  	function get(name) {   return modules[name];  }
	
  	return {   define: define,   get: get  }; 
})();

MyModules.define( "bar", [], function(){ // #B
	console.log(this); 
	// Object { name : "John" }      
	
	function hello(who) {   
		console.log(this);  // #C   
		console.log("Let me introduce: " + who + " from " + this.name);  
	}
	
  	return {   hello: hello  }; 
} );  

var bar = MyModules.get( "bar" );  
bar.hello( "hippo" );  
// Let me introduce: hippo from undefined 
// 因為 #A 行的 { name : "John" } 僅傳至 #B 的方法 
// #C 行的 this 還是 Object { hello : function hello(who) }  

var obj = [{name:'Hans'}];  
bar.hello.apply(obj[0], ['deep']);  
// 將 {name:'Hans'} 傳入 #38 的方法 
// #C 行的 this 變成 Object { name : "Hans" } 
// Let me introduce: deep from John
```
