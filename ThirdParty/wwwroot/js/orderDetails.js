

let btn = document.querySelector(".confirmbtn");

btn.addEventListener("click", async () => {
    let orderid = btn.dataset.orderid;
    let status = btn.dataset.status === "true" || btn.dataset.status === "True"; // تحويل النص إلى Boolean
    console.log("Current Status:", status);

    try {
        const response = await fetch(`/Admin/ChangeStatusOrder?OrderId=${orderid}&status=${!status}`, {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        });

        let data = await response.json();
        console.log("Response Data:", data);

        if (response.ok) {
            toastr.success(data.message || "Status Change Success");

            // تحديث الزر وحالته بناءً على الاستجابة
            if (data.status) {
                btn.dataset.status = "true"; // تحويل القيمة إلى نص
                btn.innerText = "Cancel confirmation";
                btn.classList.add("canceled");
            } else {
                btn.dataset.status = "false";
                btn.innerText = "Confirm Order";
                btn.classList.remove("canceled");
            }
        } else {
            toastr.error(data.message || "Error In Update Status");
        }
    } catch (error) {
        console.log(error);
        toastr.error("Error In Update Price");
    }
});



let formprice = document.querySelector(".form-shipping-price");

formprice.addEventListener("submit", async (e) => {
    e.preventDefault()
    let orderid = btn.dataset.orderid;
    let inputprice = document.getElementById("priceShipping");
    let errormessage = document.querySelector(".error-price");
    if (inputprice == null || inputprice?.value <= 0) {
        errormessage.textContent = "Please Enter Price";
        return;
    }


        try {
            const response = await fetch(`/Admin/UpdatePriceShipping`, {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify({ price: inputprice.value, orderid })
            });

            let data = await response.json();
            console.log("Response Data:", data);

            if (response.ok) {
                toastr.success(data.message || "Price Updated Success");
                let pricespan = document.querySelector(".price-shipping");
                pricespan.textContent = `${inputprice.value} SAR`;
            } else {
                toastr.error(data.message || "Error In Update Price");
            }
        } catch (error) {
            console.log(error);
            toastr.error("Error In Update");
        }
});
