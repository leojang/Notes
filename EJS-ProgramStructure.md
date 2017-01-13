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
