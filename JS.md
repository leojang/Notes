#Values

Whenever a backslash (\) is found inside quoted text, it indicates that the character after it has a special meaning. 
This is called escaping the character.

There is only one value in JavaScript that is not equal to itself, and that is NaN

The difference in meaning between undefined and null is an accident of JavaScript’s design

type coercion                                                                                                   
console.log(8 * null) // → 0                                                                                                   
console.log("5" - 1)  // → 4                                                                                                   
console.log("5" + 1)  // → 51                                                                                                   
console.log("five" * 2) // → NaN                                                                                                   
console.log(false == 0) // → true                                                                                                   

When null or undefined occurs on either side of the operator, it produces true only if both sides are one of null or undefined.

When you want to test whether a value has a real value instead of null or undefined, you can simply compare it to null with the == (or !=) operator.

But what if you want to test whether something refers to the precise value false? The rules for converting strings and numbers to Boolean values state that 0, NaN, and the empty string ("") count as false, while all the other values count as true. Because of this, expressions like 0 == false and "" == false are also true. For cases like this, where you do not want any automatic type conversions to happen, there are two extra operators: === and !==. The first tests whether a value is precisely equal to the other, and the second tests whether it is not precisely equal. So "" === false is false as expected.
