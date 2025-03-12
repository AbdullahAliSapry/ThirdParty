console.log("Enter in sec");

document.getElementById("contact-form").addEventListener("submit", async function (event) {
    event.preventDefault();  // لمنع الإرسال التلقائي للفورم

    let name = document.getElementById("name").value.trim();
    let email = document.getElementById("email").value.trim();
    let phone = document.getElementById("PhoneNumber").value.trim();
    let subject = document.getElementById("subject").value.trim();
    let message = document.getElementById("message").value.trim();

    let isValid = true;

    // إخفاء الرسائل السابقة
    document.querySelectorAll(".error-message").forEach(span => {
        span.style.display = 'none';
    });

    // التحقق من الاسم
    if (name === "") {
        isValid = false;
        document.getElementById("name-error").innerText = "Name is required.";
        document.getElementById("name-error").style.display = 'block';
    }

    // التحقق من البريد الإلكتروني
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (phone === "") {
        isValid = false;
        document.getElementById("PhoneNumber-error").innerText = "Please enter a valid Phone .";
        document.getElementById("PhoneNumber-error").style.display = 'block';
    }
       if (email === "" || !emailRegex.test(email)) {
        isValid = false;
        document.getElementById("email-error").innerText = "Please enter a valid email address.";
        document.getElementById("email-error").style.display = 'block';
    }

    // التحقق من الموضوع
    if (subject === "") {
        isValid = false;
        document.getElementById("subject-error").innerText = "Subject is required.";
        document.getElementById("subject-error").style.display = 'block';
    }

    // التحقق من الرسالة
    if (message === "") {
        isValid = false;
        document.getElementById("message-error").innerText = "Message is required.";
        document.getElementById("message-error").style.display = 'block';
    }

    if (isValid) {
        // في حالة نجاح التحقق، إرسال البيانات إلى السيرفر
        const proplemDto = {
            name: name,
            emailToUser: email,
            typeProplem: subject,
            descriptionProplem: message,
            phoneUser: phone,
        };


        try {
            const response = await fetch("/Home/HelpCenterData", {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify(proplemDto),
            });

            let data = await response.json();

            if (response.ok) {
                toastr.success(data.message || "تم ارسال البيانات بنجاح!");
                document.getElementById("contact-form").reset();


            } else {
                toastr.error(data.message || "حدث خطأ أثناء ارسال البيانات.");
            }
        } catch (error) {
            console.error(error);
            toastr.error(error.message || "حدث خطأ في الاتصال.");
        }
    }
});

