
### __proto__和prototype概念區分

其實說`__proto__`並不準確，確切的說是物件的`[[prototype]]`屬性，只不過在主流的瀏覽器中，都用`__proto__`來代表`[[prototype]]`屬性，因為`[[prototype]]`只是一個標準，而針對這個標準，不同的瀏覽器有不同的實現方式。 

在ES5中用`Object.getPrototypeOf`函數獲得一個物件的`[[prototype]]`。 ES6中，使用`Object.setPrototypeOf`可以直接修改一個物件的`[[prototype]]`。 為了方便，我下面的文章用`__proto__`來代表物件的`[[prototype]]`。

而`prototype`屬性是只有函數才特有的屬性，當你創建一個函數時，js會自動為這個函數加上`prototype`屬性，值是一個空物件。 所以，函數在js中是非常特殊的，是所謂的一等公民。
