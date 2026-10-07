document.getElementById("but").onclick = function () {
    let num = parseInt(document.getElementById("num").value)

    if (num % 2 == 0) {
        console.log("number is even")
    }
    else {
        console.log("number is odd")
    }
    let fact = 1;
    for (i = num; i > 0; i--) {
        fact = fact * i;
    }
    console.log("fact is="+ fact);
    console.log("multipliaction table");
    for (i = 1; i <= 10; i++) {
        console.log(num * i);

    }
}