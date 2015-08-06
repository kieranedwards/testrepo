/* bling.js see https://gist.github.com/paulirish/12fb951a8b893a454b32 

This allows:

// forEach over the qSA result, directly.
document.querySelectorAll('input').forEach(function (el) {
  // …
})

// on() rather than addEventListener()
document.body.on('dblclick', function (e) {
  // …
})

// classic $ + on()
$('p').on('click', function (e) {
  // …
})

*/

window.$ = document.querySelectorAll.bind(document);

Node.prototype.on = window.on = function (name, fn) {
    this.addEventListener(name, fn);
}

NodeList.prototype.__proto__ = Array.prototype;

NodeList.prototype.on = NodeList.prototype.addEventListener = function (name, fn) {
    this.forEach(function (elem, i) {
        elem.on(name, fn);
    });
}

