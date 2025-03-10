const modal = document.getElementById('messagesModal');
const messagesBtn = document.getElementById('messagesBtn');
const closeModal = document.getElementById('closeModal');

messagesBtn.addEventListener('click', () => {
    modal.style.display = 'flex';
});

closeModal.addEventListener('click', () => {
    modal.style.display = 'none';
});

// Close modal when clicking outside the content
window.addEventListener('click', (event) => {
    if (event.target === modal) {
        modal.style.display = 'none';
    }
});


//switch

// show slide bar 
function toggleMenu() {
    let menu = document.getElementById("dropdownMenu");
    menu.style.display = menu.style.display === "block" ? "none" : "block";
}

// إغلاق القائمة عند النقر خارجها
document.addEventListener("click", function (event) {
    let menu = document.getElementById("dropdownMenu");
    let icon = document.querySelector(".menu-icon");

    if (!menu?.contains(event.target) && !icon?.contains(event.target)) {
        menu.style.display = "none";
    }
});

// show active section 
function showContent(sectionId, sectionName, clickedElement) {
    // إخفاء جميع الأقسام
    document.querySelectorAll('.content-section').forEach(section => {
        section.classList.remove('active');
    });

    // إظهار القسم المختار
    document.getElementById(sectionId).classList.add('active');

 

    // إزالة الكلاس النشط من جميع الروابط
    document.querySelectorAll('.sidebar-orders a').forEach(link => {
        link.classList.remove('active-link');
    });

    // إضافة الكلاس النشط للعنصر الذي تم النقر عليه
    clickedElement.classList.add('active-link');
}


document.addEventListener("DOMContentLoaded", function () {
    // جلب الأزرار
    const confirmBtn = document.getElementById("confirmBtn");
    const rejectBtn = document.getElementById("rejectBtn");

    // التأكد من وجود الأزرار
    if (confirmBtn && rejectBtn) {
        confirmBtn.addEventListener("click", async function () {
            await changeOrderStatus(true, confirmBtn); // تأكيد الطلب
        });

        rejectBtn.addEventListener("click", async function () {
            await changeOrderStatus(false, rejectBtn); // رفض الطلب
        });
    }

    async function changeOrderStatus(status, button) {
        try {
            const orderId = button.getAttribute("data-order-id"); // احصل على ID الطلب
            const userId = document.querySelector(".UserId").value;

            if (!orderId || !userId) {
                Swal.fire({
                    title: "خطأ",
                    text: "Order ID or User ID is missing!",
                    icon: "error",
                    confirmButtonText: "حسنًا"
                });
                return;
            }

            // تعطيل الأزرار أثناء التنفيذ
            confirmBtn.disabled = true;
            rejectBtn.disabled = true;
            button.innerText = "Processing...";

            const response = await fetch(`/Order/ChangeStatusOreder?userid=${userId}&orderId=${orderId}&status=${status}`, {
                method: "PATCH",
                headers: {
                    "Content-Type": "application/json",
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
            });

            const data = await response.json();

            if (!response.ok || data.success === false) {
                Swal.fire({
                    title: "إشعار!",
                    text: `${data.Message || "Something went wrong!"} حدث خطا:`,
                    icon: "error",
                    confirmButtonText: "حسنًا"
                });
                return;
            } else {
                Swal.fire({
                    title: "إشعار!",
                    text: `تمت العمليه بنجاح`,
                    icon: "success",
                    confirmButtonText: "حسنًا"
                });
                let actionbtns = document.querySelector(".StatusBtns");
                actionbtns.style.display = "none";

                let btnpayment = document.querySelector(`.PayMentBtn`);
                console.log(btnpayment);
                btnpayment.style.display = "block";
            }
        } catch (error) {
            console.error("Error:", error);
            Swal.fire({
                title: "إشعار!",
                text: `حدث خطا في الاتصال يرجي المحاوله لاحقا`,
                icon: "error",
                confirmButtonText: "حسنًا"
            });
        } finally {
            // إعادة تفعيل الأزرار بعد الانتهاء
            confirmBtn.disabled = false;
            rejectBtn.disabled = false;
            button.innerText = status ? "Confirm Order" : "Reject Order";
        }
    }

    //function showToast(message, type) {
    //    const toast = document.createElement("div");
    //    toast.className = `toast toast-${type}`;
    //    toast.innerText = message;
    //    document.body.appendChild(toast);

    //    setTimeout(() => {
    //        toast.remove();
    //    }, 3000);
    //}
});
