console.log("Helper loaded");

//TODO: use a real assert library..

var ok = function (value, message) {

    //if (message && message.test && message.test) {

    //}

    //console.log(this);
    if (value) return;
    var error = "Value does not evaluate to true [" + value + "]."
    if (message) error += " Message: " + message;
    throw error;
}

var equals = function (actual, expected, message) {
    if (actual === expected) return;
    var error = "Value [" + actual + "] does not equal [" + expected + "]."
    if (message) error += " Message: " + message;
    throw error;
}
