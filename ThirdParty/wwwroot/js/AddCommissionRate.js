document.getElementById("priceForm").addEventListener("submit",async function (event) {
    event.preventDefault()
    let lowerLimit = parseFloat(document.getElementById("LowerLimit").value);
    let upperLimit = parseFloat(document.getElementById("UpperLimit").value);
    let commissionRate = parseFloat(document.getElementById("CommissionRate").value);
    let userType = document.getElementById("UserType").value;
    let valid = true;

    document.querySelectorAll(".error-message").forEach(e => e.textContent = "");

    if (isNaN(lowerLimit) || lowerLimit < 0) {
        document.getElementById("error-lower").textContent = "Lower limit must be a positive number.";
        valid = false;
    }
    if (isNaN(upperLimit) || upperLimit < lowerLimit) {
        document.getElementById("error-upper").textContent = "Upper limit must be greater than lower limit.";
        valid = false;
    }
    if (isNaN(commissionRate) || commissionRate < 0) {
        document.getElementById("error-commission").textContent = "Commission rate must be a positive number.";
        valid = false;
    }
  

    if (valid) {

        let commissionSchemeDto = {
            lowerLimit,
            upperLimit,
            commissionRate,
            userType
        }


        console.log(commissionSchemeDto)

        try {
            const response = await fetch("/Admin/AddNewCommissionSchema", {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify(commissionSchemeDto),
            });

            let data = await response.json();
            if (response.ok) {
                toastr.success(data.message || "تم اضافه البيانات بنجاح!");
                console.log(data.data)
                setTimeout(() => {
                    location.reload();
                }, 1000);
            } else {
                console.log(data);
                toastr.error(data.message || "حدث خطأ أثناء اضافه البينات.");
            }
        } catch (error) {
            console.error(error);
            toastr.error(error.message || "حدث خطأ في الاتصال.");
        }
    };
});