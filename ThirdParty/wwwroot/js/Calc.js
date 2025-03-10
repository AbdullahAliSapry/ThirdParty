

let btncalc = document.getElementById("calcbtn");



btncalc.addEventListener("click", function (event) {
    event.preventDefault(); 

    let isValid = true;

    // التحقق من الوزن
    let weightInput = document.getElementById("weight");
    if (weightInput.value.trim() === "" || parseFloat(weightInput.value) <= 0) {
        weightInput.classList.add("is-invalid");
        isValid = false;
    } else {
        weightInput.classList.remove("is-invalid");
    }



    if (isValid) {

        let prices = JSON.parse(btncalc.dataset.listprices);

        let weightvalue = parseInt(weightInput.value);
        let priceobject = null;



        prices.forEach(e => {
            if ((e.UpperLimit === Number.MAX_VALUE
                || e.UpperLimit > weightvalue) && e.LowerLimit < weightvalue) {
                priceobject = e;
            }
        });

        Swal.fire({
            title: "Result",
            text: `سعر هذا الوزن للشحن هو ${priceobject.Price * weightvalue}`,
            icon: "success",
            confirmButtonText: "OK"
        });

    }
});