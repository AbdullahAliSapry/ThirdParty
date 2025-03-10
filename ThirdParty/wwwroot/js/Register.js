// Tab switching functionality
//document.querySelectorAll(".tab").forEach((tab) => {
//    tab.addEventListener("click", () => {
//        // Remove active class from all tabs and forms
//        document.querySelectorAll(".tab").forEach((t) => t.classList.remove("active"));
//        document.querySelectorAll(".form-container").forEach((f) => f.classList.remove("active"));

//        // Add active class to clicked tab and corresponding form
//        tab.classList.add("active");
//        document.getElementById(`${tab.dataset.tab}-form`).classList.add("active");
//    });
//});


// Form validation function
//function validateForm(form) {
//    let isValid = true;

//    // Reset all error states
//    form.querySelectorAll(".form-control").forEach((input) => {
//        input.classList.remove("error");
//        input.parentElement.querySelector(".error-message")?.classList.remove("visible");
//    });

//    // Validate username
//    const username = form.querySelector('[name="username"]');
//    if (!username.value || username.value.length < 3) {
//        showError(username, "اسم المستخدم يجب أن يكون 3 أحرف على الأقل");
//        isValid = false;
//    }

//    // Validate password
//    const password = form.querySelector('[name="password"]');
//    if (!password.value || password.value.length < 8) {
//        showError(password, "كلمة المرور يجب أن تكون 8 أحرف على الأقل");
//        isValid = false;
//    }

//    // Validate password confirmation
//    const confirmPassword = form.querySelector('[name="confirmPassword"]');
//    if (confirmPassword.value !== password.value) {
//        showError(confirmPassword, "كلمات المرور غير متطابقة");
//        isValid = false;
//    }

//    // Validate phone number
//    const phone = form.querySelector('[name="phone"]');
//    if (!phone.value || !/^\d{10,11}$/.test(phone.value)) {
//        showError(phone, "الرجاء إدخال رقم هاتف صحيح");
//        isValid = false;
//    }

//    // Validate verification code
//    const verificationCode = form.querySelector('[name="verificationCode"]');
//    if (!verificationCode.value || verificationCode.value.length !== 6) {
//        showError(verificationCode, "الرجاء إدخال رمز التحقق المكون من 6 أرقام");
//        isValid = false;
//    }

//    // Additional validation for business form
//    if (form.id === "businessRegistrationForm") {
//        const realName = form.querySelector('[name="realName"]');
//        if (!realName.value) {
//            showError(realName, "الرجاء إدخال اسمك الحقيقي");
//            isValid = false;
//        }

//        const companyName = form.querySelector('[name="companyName"]');
//        if (!companyName.value) {
//            showError(companyName, "الرجاء إدخال اسم الشركة");
//            isValid = false;
//        }
//    }

//    return isValid;
//}

//// Helper function to show error message
//function showError(input, message) {
//    input.classList.add("error");
//    const errorElement = input.parentElement.querySelector(".error-message") || input.parentElement.parentElement.querySelector(".error-message");
//    if (errorElement) {
//        errorElement.textContent = message;
//        errorElement.classList.add("visible");
//    }
//}

//// Form submission handlers
//document.getElementById("personalRegistrationForm").addEventListener("submit", function (e) {
//    e.preventDefault();
//    if (validateForm(this)) {
//        alert("تم إرسال نموذج الحساب الشخصي بنجاح!");
//    }
//});

//document.getElementById("businessRegistrationForm").addEventListener("submit", function (e) {
//    e.preventDefault();
//    if (validateForm(this)) {
//        alert("تم إرسال نموذج حساب الشركة بنجاح!");
//    }
//});


/* global Swal */

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



