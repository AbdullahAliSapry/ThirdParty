

let btnSend = document.getElementById("btnEmail");

btnSend.addEventListener("click", sendCode);

async function sendCode() {
    const email = document.getElementById("emailInput").value;

    if (!email) {
        Swal.fire({
            title: "خطأ",
            text: "الرجاء إدخال البريد الإلكتروني",
            icon: "error",
            confirmButtonText: "حسنًا"
        });
        return;
    }

    try {
        const response = await fetch('/Account/verifyEmail', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ Email: email })
        });

        if (response.ok) {
            Swal.fire({
                title: "إشعار!",
                text: "تم إرسال الكود إلى بريدك الإلكتروني",
                icon: "success",
                confirmButtonText: "حسنًا"
            });
        } else {
            console.log(await response.json())
            Swal.fire({
                title: "خطأ",
                text: "حدث خطأ أثناء إرسال الكود، يرجى المحاولة لاحقًا",
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



