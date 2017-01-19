#Function
In JavaScript, functions are first-class objects, i.e. they are objects and can be manipulated and passed around just like any other object. 
Specifically, they are Function objects.

#Scope
in JavaScript, functions are the only things that create a new scope. You are allowed to use free-standing blocks.

#Declaration notation
There is a slightly shorter way to say “var square = function…”.                                                                          
The function keyword can also be used at the start of a statement, as in the following:

function square(x) {
  return x * x;
}

Function declarations are not part of the regular top-to-bottom flow of control.

#Optional Arguments

JavaScript is extremely broad-minded about the number of arguments you pass to a function.                                                
If you pass too many, the extra ones are ignored.                                                                                         
If you pass too few, the missing parameters simply get assigned the value undefined.

#Closure
This feature—being able to reference a specific instance of local variables in an enclosing function—is called closure.

function wrapValue(n)                                                                                                                   
{                                                                                                                                       
    var localVariable = n;                                                                                                              
    return function() { return localVariable; };                                                                                        
}                                                                                                                                       
                                                                                                                                        
var wrap1 = wrapValue(1);                                                                                                               
var wrap2 = wrapValue(2);                                                                                                               
console.log(wrap1());// → 1                                                                                                             
console.log(wrap2());// → 2                                                                                                             
