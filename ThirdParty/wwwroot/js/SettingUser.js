document.addEventListener("DOMContentLoaded", function () {
    var toastEl = document.querySelector('.toast');
    if (toastEl) {
        var toast = new bootstrap.Toast(toastEl);
        toast.show();
    }
});



const sidebarLinks = document.querySelectorAll(".profile-links a");
const sections = document.querySelectorAll(".main-content .section");




sidebarLinks.forEach((link, index) => {
    link.addEventListener("click", (e) => {

        sidebarLinks.forEach((link) => link.classList.remove("active"));
        sections.forEach((section) => section.classList.remove("active"));

        link.classList.add("active");
        sections[index].classList.add("active");
    });
});


document.getElementById("update-btn").addEventListener("click", function () {
    toggleEditMode(true);
});

document.getElementById("cancel-btn").addEventListener("click", function () {
    restoreOriginalValues();
    toggleEditMode(false);
});

function toggleEditMode(enable) {
    const fields = ["Name", "email", "PhoneNumber"];
    fields.forEach(fieldId => {
        const field = document.getElementById(fieldId);
        field.readOnly = !enable;
    });

    document.getElementById("save-btn").style.display = enable ? "inline-block" : "none";
    document.getElementById("cancel-btn").style.display = enable ? "inline-block" : "none";
    document.getElementById("update-btn").style.display = enable ? "none" : "inline-block";
}

function restoreOriginalValues() {
    document.querySelectorAll("input[data-original-value]").forEach(input => {
        input.value = input.getAttribute("data-original-value");
    });
}


document.getElementById("passwordForm").addEventListener("submit", async function (event) {
    event.preventDefault(); // منع الإرسال المبدئي

    // مسح الأخطاء السابقة
    document.getElementById("oldPasswordError").textContent = "";
    document.getElementById("newPasswordError").textContent = "";
    document.getElementById("confirmPasswordError").textContent = "";

    let isValid = true;
    let oldPassword = document.getElementById("oldpassword").value.trim();
    let newPassword = document.getElementById("newpassword").value.trim();
    let confirmPassword = document.getElementById("confirmpassword").value.trim();
    let email = document.getElementById("EmailToUser").value.trim();
    let userId = document.getElementById("UserId").value.trim();

    // التحقق من كلمة المرور القديمة
    if (oldPassword === "") {
        document.getElementById("oldPasswordError").textContent = "يجب إدخال كلمة المرور الحالية.";
        isValid = false;
    }

    let passwordRegex = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*(),.?{}|<>]).{8,}$/;

    if (!passwordRegex.test(newPassword)) {
        document.getElementById("newPasswordError").textContent =
            "الرقم السري يجب أن يحتوي على حرف كبير، حرف صغير، رقم، ورمز خاص، ويكون 8 أحرف على الأقل.";
        isValid = false;
    }

    // التحقق من تأكيد كلمة المرور
    if (confirmPassword === "") {
        document.getElementById("confirmPasswordError").textContent = "يجب إدخال تأكيد كلمة المرور.";
        isValid = false;
    } else if (confirmPassword !== newPassword) {
        document.getElementById("confirmPasswordError").textContent = "يجب أن تتطابق كلمة المرور الجديدة مع تأكيد كلمة المرور.";
        isValid = false;
    }

    // إذا كان كل شيء صحيح، يتم إرسال النموذج
    if (isValid) {

        let datasend =
        {
            newPassword,
            oldPassword,
            email
        }

        try {
            const response = await fetch(`/Account/UpdatePassword/${userId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify(
                    datasend
                ),
            });

            let data = await response.json();

            if (response.ok) {
                toastr.success(data.message || "تم تحديث  البيانات بنجاح!");

                document.getElementById("passwordForm").reset();

            } else {
                toastr.error(data.message || "حدث خطأ أثناء التحديث.");
            }
        } catch (error) {
            console.error(error);
            toastr.error(error.message || "حدث خطأ في الاتصال.");
        }


    }
});