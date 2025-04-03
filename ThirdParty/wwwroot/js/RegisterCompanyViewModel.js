


let btnSend = document.querySelector(".btnEmail");



btnSend.addEventListener("click", sendCode);

async function sendCode() {
    const email = document.getElementById("emailInput").value;
    console.log(email);
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
            body: JSON.stringify({ Email: email.trim() })
        });

        if (response.ok) {
            Swal.fire({
                title: "إشعار!",
                text: "تم إرسال الكود إلى بريدك الإلكتروني",
                icon: "success",
                confirmButtonText: "حسنًا"
            });
        } else {
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


function setIsCompanyOrShop(value) {
    let companyRecordGroup = document.getElementById("CompanyRecordGroup");
    let companyRecordInput = document.getElementById("CompanyRecord");

    if (value) {
        companyRecordGroup.style.display = "block";
    } else {
        companyRecordGroup.style.display = "none";
        companyRecordInput.value = "";
    }
}

function validateForm() {
    // نحصل على قيمة الـ radio button الذي تم اختياره
    let isCompanyOrShop = document.querySelector('input[name="IsComanyOrShop"]:checked').value === "True";
    let companyRecordInput = document.getElementById("CompanyRecord");

    if (isCompanyOrShop && companyRecordInput.value.trim() === "") {
        Swal.fire({
            title: "خطأ",
            text: "يجب إدخال السجل التجاري إذا كنت شركة",
            icon: "error",
            confirmButtonText: "حسنًا"
        });

        return false;

    }
    return true;
}