#Values

Whenever a backslash (\) is found inside quoted text, it indicates that the character after it has a special meaning. 
This is called escaping the character.

There is only one value in JavaScript that is not equal to itself, and that is NaN

The difference in meaning between undefined and null is an accident of JavaScript’s design

type coercion
console.log(8 * null)
// → 0
console.log("5" - 1)
// → 4
console.log("5" + 1)
// → 51
console.log("five" * 2)
// → NaN
console.log(false == 0)
// → true
