let form = document.getElementsByClassName("subMitBtn")[0];

form.addEventListener("click", submitResult);


/* global Swal */

async function submitResult(e) {
    e.preventDefault();

    let email = document.querySelector(".form-group input[type='email']").value;
    let emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    let isValid = true;
    let span = document.querySelector(".SpanError");
    if (email.length == 0 || !emailRegex.test(email)) {
        span.textContent = email.length == 0 ? "برجاء ادخال الايميل" : "برجاء ادخال ايميل صحيح";
        isValid = false;
    }
    else {
        span.textContent = "";
        isValid = true;
    }

    if (isValid) {
        try {
            const response = await fetch('/Auth/ForgetPassword', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ Email: email })
            });
            if (response.ok) {
                Swal.fire({
                    title: "إشعار!",
                    text: "تم إرسال  لينك التعيين الي البريد الاكتروني",
                    icon: "success",
                    confirmButtonText: "حسنًا"
                });
            } else {
                const errorData = await response.json();
                Swal.fire({
                    title: "خطأ",
                    text: errorData.message,
                    icon: "error",
                    confirmButtonText: "حسنًا"
                });
            }
        } catch (error) {
            Swal.fire({
                title: "خطأ في الاتصال",
                text: "حدث خطأ في الاتصال بالخادم",
                icon: "error",
                confirmButtonText: "حسنًا"
            });
        }
    }

}
